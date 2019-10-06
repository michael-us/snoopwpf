﻿// (c) Copyright Cory Plotts.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Snoop
{
	using System.Windows.Interop;

	class VisualCaptureUtil
	{
		public static void SaveVisual(Visual visual, int dpi, string filename)
		{
			// sometimes RenderTargetBitmap doesn't render the Visual or doesn't render the Visual properly
			// below i am using the trick that jamie rodriguez posted on his blog
			// where he wraps the Visual inside of a VisualBrush and then renders it.
			// http://blogs.msdn.com/b/jaimer/archive/2009/07/03/rendertargetbitmap-tips.aspx

			if (visual == null || !IsSafeToVisualize(visual))
				return;

			Rect bounds;
			UIElement uiElement = visual as UIElement;
			if (uiElement != null)
			{
				bounds = new Rect(new Size((int)uiElement.RenderSize.Width, (int)uiElement.RenderSize.Height));
			}
			else
			{
				bounds = VisualTreeHelper.GetDescendantBounds(visual);
			}

			var scale = dpi / BaseDpi;
            var finalImageSize = new Size((int)(bounds.Width * scale), (int)(bounds.Height * scale));

			RenderTargetBitmap rtb =
				new RenderTargetBitmap
				(
                    (int)finalImageSize.Width,
                    (int)finalImageSize.Height,
					dpi,
					dpi,
					PixelFormats.Pbgra32
				);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(visual);
                ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }
            rtb.Render(dv);

            SaveRTBAsPNG(rtb, filename);
		}

		public static VisualBrush CreateVisualBrushSafe(Visual visual)
		{
			return IsSafeToVisualize(visual) ? new VisualBrush(visual) : null;
		}

		public static bool IsSafeToVisualize(Visual visual)
		{
			if (visual is Window window) {
				var source = PresentationSource.FromVisual(visual) as HwndSource;
				return source?.CompositionTarget != null;
			}

			return true;
		}

		private static void SaveRTBAsPNG(RenderTargetBitmap bitmap, string filename)
		{
			var pngBitmapEncoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
			pngBitmapEncoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmap));

			using (var fileStream = System.IO.File.Create(filename))
				pngBitmapEncoder.Save(fileStream);
		}

		private const double BaseDpi = 96;
	}
}
