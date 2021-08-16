using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NullLib.VirtualForms
{
    public partial class VImageView : VAutoSizeControl
    {
        private Image image;

        public Image Image
        {
            get => image; set
            {
                ImageChangedEventArgs args = new ImageChangedEventArgs(image, value);
                image = value;
                OnImageChanged(args);
            }
        }

        public event EventHandler<ImageChangedEventArgs> ImageChanged;
        private void OnImageChanged(ImageChangedEventArgs args)
        {
            RequestRender();
            ImageChanged?.Invoke(this, args);
        }

        public ImageSizingMode ImageSizingMode { get; set; }

        protected override Size CalcAutoSize()
        {
            return Image != null ? Image.Size : Size;
        }
        protected override void RenderContentAndNotify(PaintEventArgs args)
        {
            base.RenderContentAndNotify(args);
            if (Image is Image img)
            {
                Size imgSize = img.Size;
                Rectangle descRect = CalcImageAbsBounds(imgSize.Width, imgSize.Height);
                args.Canvas.DrawImage(img, descRect, new Rectangle(Point.Empty, img.Size), GraphicsUnit.Pixel);
            }
        }
    }
    public partial class VImageView
    {
        private Rectangle CalcImageAbsBounds(int imgWidth, int imgHeight)
        {
            int
                left, width, top, height;

            (left, width, top, height) = ImageSizingMode switch
            {
                ImageSizingMode.None => (0, imgWidth, 0, imgHeight),
                ImageSizingMode.Fill => (0, Width, 0, Height),
                ImageSizingMode.Uniform => CalcUniformBounds(imgWidth, imgHeight, Width, Height),
                ImageSizingMode.UniformToFill => CalcUniformToFillBounds(imgWidth, imgHeight, Width, Height),

                _ => (0, imgWidth, 0, imgHeight)
            };

            return new Rectangle(left, top, width, height);

            (int, int, int, int) CalcUniformBounds(int imgWidth, int imgHeight, int parWidth, int parHeight)
            {
                int testHeight = parWidth * imgHeight / imgWidth;
                if (testHeight >= parHeight)
                    return (0, parWidth, (parHeight - testHeight) / 2, testHeight);
                int testWidth = parHeight * imgWidth / imgHeight;
                return ((parWidth - testWidth) / 2, testWidth, 0, parHeight);
            }
            (int, int, int, int) CalcUniformToFillBounds(int imgWidth, int imgHeight, int parWidth, int parHeight)
            {
                int testHeight = parWidth * imgHeight / imgWidth;
                if (testHeight <= parHeight)
                    return (0, parWidth, (parHeight - testHeight) / 2, testHeight);
                int testWidth = parHeight * imgWidth / imgHeight;
                return ((parWidth - testWidth) / 2, testWidth, 0, parHeight);
            }
        }
    }
}
