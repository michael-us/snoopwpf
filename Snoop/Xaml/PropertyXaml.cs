using System.Windows;
using System.Windows.Documents;

namespace Snoop.Xaml
{
    public class PropertyXaml : DependencyObject
    {
        private readonly string _name;

        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document", typeof(FlowDocument), typeof(PropertyXaml), new PropertyMetadata(default(FlowDocument)));

        public PropertyXaml(string name)
        {
            _name = name;
        }


        public string Name { get { return _name; } }

        public FlowDocument Document
        {
            get { return (FlowDocument)GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }
    }
}