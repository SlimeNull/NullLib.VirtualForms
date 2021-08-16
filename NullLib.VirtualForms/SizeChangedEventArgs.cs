using System;
using System.Drawing;

namespace NullLib.VirtualForms
{
    public class SizeChangedEventArgs : PropertyChangedEventArgs<Size>
    {
        public SizeChangedEventArgs(Size oldSize, Size newSize) : base(oldSize, newSize) { }
    }
}
