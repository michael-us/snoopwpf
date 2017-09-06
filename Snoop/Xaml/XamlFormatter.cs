using System.Windows;
using System.Windows.Documents;
using System.Xml;

namespace Snoop.Xaml
{
    public static class XamlFormatter
    {
        /// <summary>
        /// Fill flow document paragraph with syntax colored XAML.
        /// </summary>
        /// <param name="paragraph">paragraph to be filled</param>
        /// <param name="xaml">xaml to colorize</param>
        /// <param name="styles">styles used for syntax coloring</param>
        public static void FillParagraph(this Paragraph paragraph, string xaml, XamlStyles styles)
        {
            using (var reader = new XmlTextReader(xaml, XmlNodeType.Document, null))
            {
                var indent = 0;
                var tagOpened = false;
                while (reader.Read())
                {
                    if (reader.IsStartElement()) // opening tag, e.g. <Button
                    {
                        if (tagOpened)
                        {
                            // new line for child element
                            paragraph.Inlines.Add(new LineBreak());
                        }

                        // indentation
                        paragraph.AddRun(new string(' ', indent * 4), styles.TextStyle);

                        paragraph.AddRun("<", styles.DelimiterStyle);
                        paragraph.AddRun(reader.Name, styles.NameStyle);
                        if (reader.HasAttributes)
                        {
                            // write tag attributes
                            while (reader.MoveToNextAttribute())
                            {
                                paragraph.AddRun(" " + reader.Name, styles.AttributeStyle);
                                paragraph.AddRun("=", styles.DelimiterStyle);
                                paragraph.AddRun("\"", styles.QuotesStyle);
                                string value;
                                if (reader.Name == "TargetType") // target type fix - should use the Type MarkupExtension
                                {
                                    value = "{x:Type " + reader.Value + "}";
                                }
                                else
                                {
                                    value = reader.Value;
                                }
                                paragraph.AddAttributeValue(value, styles);
                                paragraph.AddRun("\"", styles.QuotesStyle);
                            }
                            reader.MoveToElement();
                        }
                        if (reader.IsEmptyElement) // empty tag, e.g. <Button />
                        {
                            paragraph.AddRun(" />", styles.DelimiterStyle);
                            paragraph.Inlines.Add(new LineBreak());
                            tagOpened = false;
                            --indent;
                        }
                        else // non-empty tag, e.g. <Button>
                        {
                            paragraph.AddRun(">", styles.DelimiterStyle);
                            tagOpened = true;
                        }

                        ++indent;
                    }
                    else // closing tag, e.g. </Button>
                    {
                        --indent;

                        // text content of a tag, e.g. the text "Do This" in <Button>Do This</Button>
                        if (reader.NodeType == XmlNodeType.Text)
                        {
                            var content = reader.ReadContentAsString();
                            if (content.IndexOfAny(new[] { '\r', '\n' }) >= 0)
                            {
                                // indentation
                                paragraph.AddRun(new string(' ', indent * 4), styles.TextStyle);
                            }
                            paragraph.AddRun(content, styles.TextStyle);
                        }
                        else
                        {
                            // indentation
                            paragraph.AddRun(new string(' ', indent * 4), styles.TextStyle);
                        }

                        paragraph.AddRun("</", styles.DelimiterStyle);
                        paragraph.AddRun(reader.Name, styles.NameStyle);
                        paragraph.AddRun(">", styles.DelimiterStyle);
                        paragraph.Inlines.Add(new LineBreak());
                        tagOpened = false;
                    }
                }
            }
        }

        /// <summary>
        /// Adds a span with the specified text and style.
        /// </summary>
        public static void AddRun(this Paragraph paragraph, string text, Style style)
        {
            paragraph.Inlines.Add(new Run(text) { Style = style });
        }

        private static void AddAttributeValue(this Paragraph paragraph, string value, XamlStyles styles)
        {
            if (value.StartsWith("{}"))
                paragraph.AddRun(value, styles.TextStyle);
            else if (value.StartsWith("{") && value.EndsWith("}"))
                paragraph.AddMarkupValue(value, styles);
            else
                paragraph.AddRun(value, styles.AttributeValueStyle);
        }

        private static void AddMarkupValue(this Paragraph paragraph, string value, XamlStyles styles)
        {
            var markupString = value.Substring(1, value.Length - 2);
            var indexOfSpace = markupString.IndexOf(' ');
            var markupType = indexOfSpace < 0 ? markupString : markupString.Substring(0, indexOfSpace);
            paragraph.AddRun("{", styles.DelimiterStyle);
            paragraph.AddRun(markupType, styles.NameStyle);
            if (indexOfSpace >= 0)
                paragraph.AddMarkupBody(markupString.Substring(indexOfSpace), styles);
            paragraph.AddRun("}", styles.DelimiterStyle);
        }

        private static void AddMarkupBody(this Paragraph paragraph, string value, XamlStyles styles)
        {
            var indexOfNonspace = FindNonspaceInMarkup(value);
            var spaceText = indexOfNonspace < 0 ? value : value.Substring(0, indexOfNonspace);
            paragraph.AddRun(spaceText, styles.AttributeValueStyle);
            if (indexOfNonspace < 0)
                return;
            value = value.Substring(indexOfNonspace);
            if (value.StartsWith("{}"))
            {
                var indexOfComma = FindCommaInMarkup(value);
                var text = indexOfComma < 0 ? value : value.Substring(0, indexOfComma);
                paragraph.AddRun(text, styles.TextStyle);
                if (indexOfComma < 0)
                    return;
                value = value.Substring(indexOfComma);
            }
            else if (value.StartsWith("{"))
            {
                var indexOfClosedBrace = FindClosedBraceInMarkup(value);
                if (indexOfClosedBrace < 0)
                {
                    paragraph.AddRun(value, styles.ErrorStyle);
                    return;
                }
                paragraph.AddMarkupValue(value.Substring(0, indexOfClosedBrace), styles);
                value = value.Substring(indexOfClosedBrace + 1);
            }
            // TODO:
            // Path=Title, StringFormat={}{0}AVS, ElementName=window
            paragraph.AddRun(value, styles.AttributeValueStyle);
        }

        private static int FindNonspaceInMarkup(string markup)
        {
            for (var i = 0; i < markup.Length; i++)
                if (!char.IsWhiteSpace(markup[i]))
                    return i;
            return -1;
        }

        private static int FindCommaInMarkup(string markup)
        {
            var isEscaped = false;
            for (var i = 0; i < markup.Length; i++)
            {
                var ch = markup[i];
                if (isEscaped)
                    isEscaped = false;
                else if (ch == '\\')
                    isEscaped = true;
                else if (ch == ',')
                    return i;
            }
            return -1;
        }

        private static int FindClosedBraceInMarkup(string markup)
        {
            var isEscaped = false;
            var level = 0;
            for (var i = 0; i < markup.Length; i++)
            {
                var ch = markup[i];
                if (isEscaped)
                {
                    isEscaped = false;
                }
                else if (ch == '\\')
                {
                    isEscaped = true;
                }
                else if (ch == '{')
                {
                    level++;
                }
                else if (ch == '}')
                {
                    level--;
                    if (level == 0)
                        return i;
                }
            }
            return -1;
        }
    }
}