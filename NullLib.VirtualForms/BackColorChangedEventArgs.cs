using System.Drawing;

namespace NullLib.VirtualForms
{
    public class BackColorChangedEventArgs : PropertyChangedEventArgs<Color>
    {
        public BackColorChangedEventArgs(Color oldColor, Color newColor) : base(oldColor, newColor)
        {
        }
    }
    public class ForeColorChangedEventArgs : PropertyChangedEventArgs<Color>
    {
        public ForeColorChangedEventArgs(Color oldColor, Color newColor) : base(oldColor, newColor)
        {
        }
    }
}