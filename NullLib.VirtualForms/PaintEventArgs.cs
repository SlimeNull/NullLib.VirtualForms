using System.Drawing;

namespace NullLib.VirtualForms
{
    public class PaintEventArgs : RouteEventArgs
    {
        public PaintEventArgs(Graphics canvas, Graphics parentCanvas, Rectangle clip, Rectangle selfBounds) => (Canvas, ParentCanvas, Clip, SelfBounds) = (canvas, parentCanvas, clip, selfBounds);
        public Graphics Canvas { get; }
        public Graphics ParentCanvas { get; }
        public Rectangle Clip { get; }
        public Rectangle SelfBounds { get; }
    }
}