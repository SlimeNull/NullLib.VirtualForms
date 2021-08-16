using System;
using System.Drawing;

namespace NullLib.VirtualForms
{
    public class AbsBoundsChangedEventArgs : PropertyChangedEventArgs<Rectangle>
    {
        public AbsBoundsChangedEventArgs(Rectangle oldBounds, Rectangle newBounds) : base(oldBounds, newBounds) { }
    }
}
