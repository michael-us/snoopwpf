// (c) Copyright Cory Plotts.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

namespace Snoop
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Snoop.Infrastructure;

    public enum WindowFinderType
    {
        Snoop,
        Magnify
    }

    public partial class WindowFinder
    {
        private static readonly Point cursorHotSpot = new Point(16, 20);
        private readonly Cursor crosshairsCursor;
        private Point _startPoint;
        private WindowInfo currentWindowInfo;
        private readonly LowLevelMouseHook lowLevelMouseHook;

        private Cursor currentWindowInfoCursor;
        private Cursor lastWindowInfoCursor;

        public WindowFinder()
        {
            this.InitializeComponent();

            this.crosshairsCursor = ConvertToCursor(this.WindowInfoControl, cursorHotSpot);

            this.lowLevelMouseHook = new LowLevelMouseHook();
            this.lowLevelMouseHook.LowLevelMouseMove += this.LowLevelMouseMove;
        }

        public WindowFinderType WindowFinderType { get; set; }

        public WindowInfoControl WindowInfoControl { get; } = new WindowInfoControl();

        /// <inheritdoc />
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this._startPoint = e.GetPosition(null);
            this.StartSnoopTargetsSearch();
            e.Handled = true;

            base.OnPreviewMouseLeftButtonDown(e);
        }

        /// <inheritdoc />
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var currentPosition = e.GetPosition(null);
            var diff = this._startPoint - currentPosition;

            if (e.LeftButton == MouseButtonState.Pressed
                && (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                this.lowLevelMouseHook.Start();
            }
        }

        /// <inheritdoc />
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            this.StopSnoopTargetsSearch();

            if (this.currentWindowInfo != null
                && this.currentWindowInfo.IsValidProcess)
            {
                if (this.WindowFinderType == WindowFinderType.Snoop)
                {
                    this.AttachSnoop();
                }
                else if (this.WindowFinderType == WindowFinderType.Magnify)
                {
                    this.AttachMagnify();
                }
            }
        }

        /// <inheritdoc />
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.StopSnoopTargetsSearch();
            }

            base.OnPreviewKeyDown(e);
        }

        private void LowLevelMouseMove(object sender, LowLevelMouseHook.LowLevelMouseMoveEventArgs e)
        {
            var windowUnderCursor = NativeMethods.GetWindowUnderMouse();

            // the window under the cursor has changed
            if (windowUnderCursor != this.currentWindowInfo?.HWnd)
            {
                this.currentWindowInfo = new WindowInfo(windowUnderCursor);
                this.WindowInfoControl.DataContext = this.currentWindowInfo;

                Trace.WriteLine($"Window under cursor: {this.currentWindowInfo.TraceInfo}");

                this.UpdateCursor();
            }
        }

        private void StartSnoopTargetsSearch()
        {
            this.CaptureMouse();
            Keyboard.Focus(this.btnStartWindowsSearch);

            this.snoopCrosshairsImage.Visibility = Visibility.Hidden;
            this.currentWindowInfo = null;

            this.lowLevelMouseHook.Start();

            this.UpdateCursor();
        }

        private void StopSnoopTargetsSearch()
        {
            this.lowLevelMouseHook.Stop();

            this.ReleaseMouseCapture();

            this.snoopCrosshairsImage.Visibility = Visibility.Visible;

            // clear out cached process info to make the force refresh do the process check over again.
            WindowInfo.ClearCachedWindowHandleInfo();

            this.UpdateCursor();
        }

        private void UpdateCursor()
        {
            if (this.currentWindowInfo?.IsValidProcess == true)
            {
                this.lastWindowInfoCursor = this.currentWindowInfoCursor;

                this.currentWindowInfoCursor = ConvertToCursor(this.WindowInfoControl, cursorHotSpot);
                this.Cursor = this.currentWindowInfoCursor;
            }
            else if (this.lowLevelMouseHook.IsRunning)
            {
                this.Cursor = this.crosshairsCursor;
            }
            else
            {
                this.Cursor = null;
            }

            this.lastWindowInfoCursor?.Dispose();
            this.lastWindowInfoCursor = null;
        }

        private void AttachSnoop()
        {
            new AttachFailedHandler(this.currentWindowInfo);
            this.currentWindowInfo.Snoop();
        }

        private void AttachMagnify()
        {
            new AttachFailedHandler(this.currentWindowInfo);
            this.currentWindowInfo.Magnify();
        }

        // https://stackoverflow.com/a/27077188/122048
        private static Cursor ConvertToCursor(UIElement control, Point hotSpot = default(Point))
        {
            // convert FrameworkElement to PNG stream
            using (var pngStream = new MemoryStream())
            {
                control.InvalidateMeasure();
                control.InvalidateArrange();
                control.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                
                var rect = new Rect(0, 0, control.DesiredSize.Width, control.DesiredSize.Height);
                
                control.Arrange(rect);
                control.UpdateLayout();

                var rtb = new RenderTargetBitmap((int)control.DesiredSize.Width, (int)control.DesiredSize.Height, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(control);

                var png = new PngBitmapEncoder();
                png.Frames.Add(BitmapFrame.Create(rtb));
                png.Save(pngStream);

                // write cursor header info
                using (var cursorStream = new MemoryStream())
                {
                    cursorStream.Write(new byte[2]
                                       {
                                           0x00,
                                           0x00
                                       }, 0, 2); // ICONDIR: Reserved. Must always be 0.
                    cursorStream.Write(new byte[2]
                                       {
                                           0x02,
                                           0x00
                                       }, 0, 2); // ICONDIR: Specifies image type: 1 for icon (.ICO) image, 2 for cursor (.CUR) image. Other values are invalid
                    cursorStream.Write(new byte[2]
                                       {
                                           0x01,
                                           0x00
                                       }, 0, 2); // ICONDIR: Specifies number of images in the file.
                    cursorStream.Write(new byte[1]
                                       {
                                           (byte)control.DesiredSize.Width
                                       }, 0, 1); // ICONDIRENTRY: Specifies image width in pixels. Can be any number between 0 and 255. Value 0 means image width is 256 pixels.
                    cursorStream.Write(new byte[1]
                                       {
                                           (byte)control.DesiredSize.Height
                                       }, 0, 1); // ICONDIRENTRY: Specifies image height in pixels. Can be any number between 0 and 255. Value 0 means image height is 256 pixels.
                    cursorStream.Write(new byte[1]
                                       {
                                           0x00
                                       }, 0, 1); // ICONDIRENTRY: Specifies number of colors in the color palette. Should be 0 if the image does not use a color palette.
                    cursorStream.Write(new byte[1]
                                       {
                                           0x00
                                       }, 0, 1); // ICONDIRENTRY: Reserved. Should be 0.
                    cursorStream.Write(new byte[2]
                                       {
                                           (byte)hotSpot.X,
                                           0x00
                                       }, 0, 2); // ICONDIRENTRY: Specifies the horizontal coordinates of the hotspot in number of pixels from the left.
                    cursorStream.Write(new byte[2]
                                       {
                                           (byte)hotSpot.Y,
                                           0x00
                                       }, 0, 2); // ICONDIRENTRY: Specifies the vertical coordinates of the hotspot in number of pixels from the top.
                    cursorStream.Write(new byte[4]
                                       {
                                           // ICONDIRENTRY: Specifies the size of the image's data in bytes
                                           (byte)(pngStream.Length & 0x000000FF),
                                           (byte)((pngStream.Length & 0x0000FF00) >> 0x08),
                                           (byte)((pngStream.Length & 0x00FF0000) >> 0x10),
                                           (byte)((pngStream.Length & 0xFF000000) >> 0x18)
                                       }, 0, 4);
                    cursorStream.Write(new byte[4]
                                       {
                                           // ICONDIRENTRY: Specifies the offset of BMP or PNG data from the beginning of the ICO/CUR file
                                           0x16,
                                           0x00,
                                           0x00,
                                           0x00
                                       }, 0, 4);

                    // copy PNG stream to cursor stream
                    pngStream.Seek(0, SeekOrigin.Begin);
                    pngStream.CopyTo(cursorStream);

                    // return cursor stream
                    cursorStream.Seek(0, SeekOrigin.Begin);
                    return new Cursor(cursorStream);
                }
            }
        }
    }
}