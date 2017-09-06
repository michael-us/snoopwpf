using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Snoop.Xaml
{
    /// <summary>
    /// Interaction logic for XamlView.xaml
    /// </summary>
    public partial class XamlView : UserControl
    {
        private static readonly Type[] AcceptableTypes = { typeof(FrameworkTemplate), typeof(Style) };

        private readonly XamlStyles _styles;
        private readonly IList<PropertyXaml> _tabs;

        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(object), typeof(XamlView), new PropertyMetadata(default(object), OnTargetChanged));
        public static readonly DependencyProperty SelectedTabProperty =
            DependencyProperty.Register("SelectedTab", typeof(PropertyXaml), typeof(XamlView), new PropertyMetadata(default(PropertyXaml)));

        public XamlView()
        {
            InitializeComponent();

            _styles = new XamlStyles
                {
                    DelimiterStyle = new Style(typeof(Run)) { Setters = { new Setter(TextElement.ForegroundProperty, new SolidColorBrush(Color.FromRgb(240, 241, 242))) } },
                    NameStyle = new Style(typeof(Run)) { Setters = { new Setter(TextElement.ForegroundProperty, new SolidColorBrush(Color.FromRgb(103, 139, 176))) } },
                    QuotesStyle = new Style(typeof(Run)) { Setters = { new Setter(TextElement.ForegroundProperty, new SolidColorBrush(Color.FromRgb(235, 118, 0))) } },
                    TextStyle = new Style(typeof(Run)) { Setters = { new Setter(TextElement.ForegroundProperty, new SolidColorBrush(Colors.White)) } },
                    AttributeStyle = new Style(typeof(Run)) { Setters = { new Setter(TextElement.ForegroundProperty, new SolidColorBrush(Color.FromRgb(184, 201, 218))) } },
                    AttributeValueStyle = new Style(typeof(Run)) { Setters = { new Setter(TextElement.ForegroundProperty, new SolidColorBrush(Color.FromRgb(235, 118, 0))) } },
                    ConstantValueStyle = new Style(typeof(Run)) { Setters = { new Setter(TextElement.ForegroundProperty, new SolidColorBrush(Color.FromRgb(254, 204, 34))) } },
                    ErrorStyle = new Style(typeof(Run)) { Setters = { new Setter(TextElement.ForegroundProperty, new SolidColorBrush(Color.FromRgb(254, 51, 51))) } },
                    GeneralStyle = new Style(typeof(Paragraph))
                        {
                            Setters =
                            {
                                new Setter(TextElement.BackgroundProperty, new SolidColorBrush(Color.FromRgb(34, 40, 42))),
                                new Setter(Block.TextAlignmentProperty, TextAlignment.Left),
                                new Setter(TextElement.FontFamilyProperty, new FontFamily("Consolas"))
                            }
                        }
                };
            _tabs = new ObservableCollection<PropertyXaml>();
            Tabs.ItemsSource = _tabs;
            Tabs.SetBinding(Selector.SelectedItemProperty,
                new Binding(SelectedTabProperty.Name) { Source = this, Mode = BindingMode.TwoWay });
            DocView.SetBinding(FlowDocumentScrollViewer.DocumentProperty,
                new Binding(SelectedTabProperty.Name + "." + PropertyXaml.DocumentProperty.Name) { Source = this, FallbackValue = null });
        }


        public object Target
        {
            get { return GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        public PropertyXaml SelectedTab
        {
            get { return (PropertyXaml)GetValue(SelectedTabProperty); }
            set { SetValue(SelectedTabProperty, value); }
        }


        private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = (XamlView)d;
            var target = e.NewValue;
            var styles = me._styles;
            var tabs = me._tabs;

            if (target != null)
            {
                var tabsToRemove = new HashSet<string>(tabs.Select(o => o.Name));
                foreach (var propertyInfo in target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (!propertyInfo.CanRead || propertyInfo.GetIndexParameters().Length > 0)
                        continue;
                    if (AcceptableTypes.All(o => !o.IsAssignableFrom(propertyInfo.PropertyType)))
                        continue;

                    var propertyXaml = tabs.FirstOrDefault(o => o.Name == propertyInfo.Name);
                    if (propertyXaml == null)
                    {
                        propertyXaml = new PropertyXaml(propertyInfo.Name);
                        var insertIndex = tabs.Count;
                        for (var i = 0; i < tabs.Count; i++)
                        {
                            if (string.CompareOrdinal(propertyInfo.Name, tabs[i].Name) < 0)
                            {
                                insertIndex = i;
                                break;
                            }
                        }
                        tabs.Insert(insertIndex, propertyXaml);
                    }
                    else
                    {
                        tabsToRemove.Remove(propertyInfo.Name);
                    }

                    var value = propertyInfo.GetValue(target, null);
                    if (value != null)
                    {
                        try
                        {
                            string serializedStyle;
                            using (var stringWriter = new StringWriter())
                            using (var xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented })
                            {
                                XamlWriter.Save(value, xmlTextWriter);
                                serializedStyle = stringWriter.ToString();
                            }
                            var paragraph = new Paragraph { Style = styles.GeneralStyle };
                            paragraph.FillParagraph(serializedStyle, styles);
                            propertyXaml.Document = new FlowDocument { Blocks = { paragraph } };
                        }
                        catch (Exception ex)
                        {
                            var message = "[Exception]" + Environment.NewLine + Environment.NewLine + ex;
                            var paragraph = new Paragraph { Style = styles.GeneralStyle };
                            paragraph.AddRun(message, styles.ErrorStyle);
                            propertyXaml.Document = new FlowDocument { Blocks = { paragraph } };
                        }
                    }
                    else
                    {
                        var paragraph = new Paragraph { Style = styles.GeneralStyle };
                        paragraph.AddRun("(NULL)", styles.TextStyle);
                        propertyXaml.Document = new FlowDocument { Blocks = { paragraph } };
                    }
                }
                foreach (var tabName in tabsToRemove)
                {
                    var tab = tabs.FirstOrDefault(o => o.Name == tabName);
                    if (tab != null)
                        tabs.Remove(tab);
                }
            }
            else
            {
                tabs.Clear();
            }
        }
    }
}
