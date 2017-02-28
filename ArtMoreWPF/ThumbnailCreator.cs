namespace ArtMoreWPF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;

    public static class ThumbnailCreator
    {
        //public BitmapImage GetThumbnail(string imagePath, int maxWidth, int maxHeight)
        //{
        //    BitmapImage img = new BitmapImage();
        //    img.BeginInit();
        //    img.UriSource = new Uri(imagePath);


        //    uint width = 0;
        //    uint height = 0;
        //    if (decoder.PixelWidth < 120 && decoder.PixelHeight < 120)
        //    {
        //        width = decoder.PixelWidth;
        //        height = decoder.PixelHeight;
        //    }
        //    else if (decoder.PixelHeight == decoder.PixelWidth)
        //    {
        //        width = 120;
        //        height = 120;
        //    }
        //    else if (decoder.PixelHeight > decoder.PixelWidth)
        //    {
        //        var ratio = (float)decoder.PixelWidth / decoder.PixelHeight;
        //        height = 120;
        //        width = (uint)(120 * ratio);
        //    }
        //    else
        //    {
        //        var ratio = (float)decoder.PixelHeight / decoder.PixelWidth;
        //        width = 120;
        //        height = (uint)(120 * ratio);
        //    }

        //    bmp = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);

        //    await bmp.SetSourceAsync(stream);
        //    bmp = bmp.Resize((int)width, (int)height, WriteableBitmapExtensions.Interpolation.NearestNeighbor);


        //    Thumbnail = bmp;
        //}
    }
}
