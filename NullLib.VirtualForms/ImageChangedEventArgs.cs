using System.Drawing;

namespace NullLib.VirtualForms
{
    public class ImageChangedEventArgs : PropertyChangedEventArgs<Image>
    {
        public ImageChangedEventArgs(Image oldImage, Image newImage) : base(oldImage, newImage)
        {
            
        }
    }
}