using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NullLib.VirtualForms
{
    public class VLabel : VAutoSizeControl
    {
        private string text;
        public string Text
        {
            get => text; set
            {
                TextChangedEventArgs args = new(text, value);
                text = value;
                OnTextChanged(args);
            }
        }

        public event EventHandler<TextChangedEventArgs> TextChanged;
        private void OnTextChanged(TextChangedEventArgs args)
        {
            ForceProcessPaint(true);
            TextChanged?.Invoke(this, args);
        }

        public static readonly Font DefaultTextFont = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Regular);
        public static readonly Brush DefaultTextBrush = new SolidBrush(Color.Black);

        public Font TextFont { get; set; } = DefaultTextFont;
        public Brush TextBrush { get; set; } = DefaultTextBrush;

        protected override Size CalcAutoSize()
        {
            return GetTempCanvas().MeasureString(Text, TextFont).ToSize();
        }

        protected override void RenderContentAndNotify(PaintEventArgs args)
        {
            base.RenderContentAndNotify(args);
            args.Canvas.DrawString(Text, TextFont, TextBrush, Point.Empty);
        }
    }
}
