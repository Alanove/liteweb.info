using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace lw.GraphicUtils
{
    public class ImageUtils
    {
        public const int JpegQuality = 60;

        public ImageUtils()
        {
        }

        /// <summary>
        /// Creates a thumbnail version of the image
        /// The resize will be proporpotional and no cropping will occur
        /// If you want to crop the image and resize to the exact width and height use <seealso cref="CreateThumb"/> width aded parameter _FixedSize
        /// </summary>
        /// <param name="pathFrom">Srouce Path</param>
        /// <param name="pathTo">Destinaton Path</param>
        /// <param name="width">Resize Width</param>
        /// <param name="height">Resize Height</param>
        public static void CreateThumb(string pathFrom, string pathTo, int width, int height)
        {
            CreateThumb(pathFrom, pathTo, width, height, false, ImageFormat.Jpeg);
        }

        /// <summary>
        /// Creates a thumbnail version of the image
        /// </summary>
        /// <param name="pathFrom">Srouce Path</param>
        /// <param name="pathTo">Destinaton Path</param>
        /// <param name="width">Resize Width</param>
        /// <param name="height">Resize Height</param>
        /// <param name="_FixedSize">if true: the image will be cropped and the result will be of the exact passed width and height, if false: the image will be resized proportionally</param>
        public static void CreateThumb(string pathFrom, string pathTo, int width, int height, bool _FixedSize)
        {
            CreateThumb(pathFrom, pathTo, width, height, _FixedSize, ImageFormat.Jpeg);
        }

        /// <summary>
        /// Creates a thumbnail version of the image
        /// </summary>
        /// <param name="pathFrom">Srouce Path</param>
        /// <param name="pathTo">Destinaton Path</param>
        /// <param name="width">Resize Width</param>
        /// <param name="height">Resize Height</param>
        /// <param name="_FixedSize">if true: the image will be cropped and the result will be of the exact passed width and height, if false: the image will be resized proportionally</param>
        /// <param name="format">Defines the fomrat of the image (default is JPEG)</param>
        public static void CreateThumb(string pathFrom, string pathTo, int width, int height, bool _FixedSize, ImageFormat format)
        {
            string pathRead = pathFrom;
            if (pathFrom == pathTo)
            {
                var fi = new FileInfo(pathFrom);
                pathRead = Path.Combine(fi.Directory.FullName, System.Guid.NewGuid().ToString() + Path.GetExtension(pathFrom));
                fi.MoveTo(pathRead);
            }

            System.Drawing.Image im = System.Drawing.Image.FromFile(pathRead);
            System.Drawing.Image dest;
            if (_FixedSize)
                dest = Crop(im, width, height, AnchorPosition.Default);
            else
                dest = FixedSize(im, width, height);
            im.Dispose();

            if (format == ImageFormat.Jpeg)
                SaveJpeg(pathTo, dest, JpegQuality);
            else
                dest.Save(pathTo, format);

            dest.Dispose();

            if (pathRead != pathFrom)
                File.Delete(pathRead);
        }

        public static void Resize(string pathFrom, string pathTo, int width, int height)
        {
            Resize(pathFrom, pathTo, width, height, ImageFormat.Jpeg);
        }
        public static void Resize(string pathFrom, string pathTo, int width, int height, ImageFormat format)
        {
            RotateImageByExifOrientationData(pathFrom, true);

            string pathRead = pathFrom;
            if (pathFrom == pathTo)
            {
                FileInfo fi = new FileInfo(pathFrom);
                pathRead = Path.Combine(fi.Directory.FullName, System.Guid.NewGuid().ToString() + Path.GetExtension(pathFrom));
                fi.MoveTo(pathRead);
            }


            System.Drawing.Image im = System.Drawing.Image.FromFile(pathRead);
            System.Drawing.Image dest;
            if (im.Width == width && im.Height == height)
            {
                /* it will keep the image with the guid name and stay working in the process
                 * 
                 *
                 */
                if (pathFrom != pathTo)
                    System.IO.File.Copy(pathFrom, pathTo);

                if (pathFrom == pathTo)
                {
                    System.IO.File.Copy(pathRead, pathTo);
                    im.Dispose();
                    File.Delete(pathRead);
                }

                return;
            }
            dest = FixedSize(im, width, height);
            im.Dispose();

            if (pathFrom == pathTo && System.IO.File.Exists(pathFrom))
            {
                try
                {
                    System.IO.File.Delete(pathFrom);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (format == ImageFormat.Jpeg)
                SaveJpeg(pathTo, dest, JpegQuality);
            else
                dest.Save(pathTo, format);
            dest.Dispose();

            if (pathRead != pathFrom)
                File.Delete(pathRead);
        }

        public static void CropImage(string pathFrom, string destinationPath, int Width, int Height, AnchorPosition anchor, CropOptions cropOptions)
        {
            if (cropOptions != null)
                CropImage(pathFrom, destinationPath, Width, Height, anchor, ImageFormat.Jpeg, cropOptions);
            else
                CropImage(pathFrom, destinationPath, Width, Height, AnchorPosition.Custom, ImageFormat.Jpeg);
        }

        public static void CropImage(string pathFrom, string destinationPath, int Width, int Height, AnchorPosition anchor)
        {
            CropImage(pathFrom, destinationPath, Width, Height, anchor, ImageFormat.Jpeg);
        }
        public static void CropImage(string pathFrom, string destinationPath, int Width, int Height, AnchorPosition anchor, ImageFormat format)
        {
            CropImage(pathFrom, destinationPath, Width, Height, anchor, format, null, null);
        }
        public static void CropImage(string pathFrom, string destinationPath, int Width, int Height,
            AnchorPosition anchor, ImageFormat format, int? Top, int? Left)
        {
            

            string pathRead = pathFrom;

            if (pathFrom == destinationPath)
            {
                FileInfo fi = new FileInfo(pathFrom);
                pathRead = Path.Combine(fi.Directory.FullName, System.Guid.NewGuid().ToString() + Path.GetExtension(pathFrom));
                fi.MoveTo(pathRead);
            }

            System.Drawing.Image im = System.Drawing.Image.FromFile(pathRead);
            System.Drawing.Image dest = Crop(im, Width, Height, anchor, Top, Left);

            if (format == ImageFormat.Jpeg)
                SaveJpeg(destinationPath, dest, JpegQuality);
            else
                dest.Save(destinationPath, format);

            im.Dispose();
            dest.Dispose();

            if (pathRead != pathFrom)
                File.Delete(pathRead);

            RotateImageByExifOrientationData(destinationPath, true);
        }

        public static void CropImage(string pathFrom, string destinationPath, int Width, int Height,
           AnchorPosition anchor, ImageFormat format, CropOptions cropOptions)
        {


            string pathRead = pathFrom;

            if (pathFrom == destinationPath)
            {
                FileInfo fi = new FileInfo(pathFrom);
                pathRead = Path.Combine(fi.Directory.FullName, System.Guid.NewGuid().ToString() + Path.GetExtension(pathFrom));
                fi.MoveTo(pathRead);
            }

            System.Drawing.Image im = System.Drawing.Image.FromFile(pathRead);
            System.Drawing.Image dest = Crop(im, Width, Height, cropOptions);

            if (format == ImageFormat.Jpeg)
                SaveJpeg(destinationPath, dest, JpegQuality);
            else
                dest.Save(destinationPath, format);

            im.Dispose();
            dest.Dispose();

            if (pathRead != pathFrom)
                File.Delete(pathRead);

            RotateImageByExifOrientationData(destinationPath, true);
        }
        public static System.Drawing.Image ScaleByPercent(System.Drawing.Image imgPhoto, int Percent)
        {
            float nPercent = ((float)Percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight,
                PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                imgPhoto.VerticalResolution);

            System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static System.Drawing.Image FixedSize(System.Drawing.Image imgPhoto,
            int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            if (sourceWidth <= Width && sourceHeight <= Height)
            {
                Bitmap _bmPhoto = new Bitmap(sourceWidth, sourceHeight,
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                _bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                    imgPhoto.VerticalResolution);

                System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(_bmPhoto);

                grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                grPhoto.Clear(System.Drawing.Color.White);

                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(-2, -2, sourceWidth + 4, sourceHeight + 4),
                    new Rectangle(-2, -2, sourceWidth + 4, sourceHeight + 4),
                    GraphicsUnit.Pixel);

                grPhoto.Dispose();
                return _bmPhoto;
            }

            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;


            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Width = Width > destWidth ? destWidth : Width;
            Height = Height > destHeight ? destHeight : Height;

            Bitmap bmPhoto = new Bitmap(Width, Height,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                imgPhoto.VerticalResolution);

            System.Drawing.Graphics _grPhoto = System.Drawing.Graphics.FromImage(bmPhoto);

            _grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            _grPhoto.Clear(System.Drawing.Color.White);

            _grPhoto.DrawImage(imgPhoto,
                new Rectangle(-2, -2, destWidth + 4, destHeight + 4),
                new Rectangle(-2, -2, sourceWidth + 4, sourceHeight + 4),
                GraphicsUnit.Pixel);

            _grPhoto.Dispose();
            return bmPhoto;
        }

        public static System.Drawing.Image FixedSize(System.Drawing.Image imgPhoto,
            int Width, int Height, System.Drawing.Color FillColor)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                    (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                    (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                imgPhoto.VerticalResolution);

            System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto);
            grPhoto.Clear(FillColor);
            grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        /// <summary>
        /// Crops the image imgPhoto
        /// If Top and Left are specified the crop will start at those coordinates
        /// Otherwise it will follow the parameter <seealso cref="AnchorPosition"/>
        /// </summary>
        /// <param name="imgPhoto">Input Image</param>
        /// <param name="Width">Destination Width</param>
        /// <param name="Height">Destination Height</param>
        /// <param name="Anchor">Anchor Poisition <seealso cref="AnchorPosition"/></param>
        /// <returns>Output Image</returns>
        public static System.Drawing.Image Crop(System.Drawing.Image imgPhoto, int Width,
            int Height, AnchorPosition Anchor)
        {
            return Crop(imgPhoto, Width, Height, Anchor, null, null);
        }

        /// <summary>
        /// Crops the image imgPhoto
        /// If Top and Left are specified the crop will start at those coordinates
        /// Otherwise it will follow the parameter <seealso cref="AnchorPosition"/>
        /// </summary>
        /// <param name="imgPhoto">Input Image</param>
        /// <param name="Width">Destination Width</param>
        /// <param name="Height">Destination Height</param>
        /// <param name="Anchor">Anchor Poisition <seealso cref="AnchorPosition"/></param>
        /// <param name="Top">Start Top used with AnchorPosition.Custom</param>
        /// <param name="Left">Start Left used with AnchorPosition.Custom</param>
        /// <returns>Output Image</returns>
        public static System.Drawing.Image Crop(System.Drawing.Image imgPhoto, int Width,
            int Height, AnchorPosition Anchor, int? Top, int? Left)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            if (sourceWidth <= Width && sourceHeight <= Height)
            {
                Bitmap _bmPhoto = new Bitmap(sourceWidth, sourceHeight,
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                _bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                    imgPhoto.VerticalResolution);

                System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(_bmPhoto);

                grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                grPhoto.Clear(System.Drawing.Color.White);

                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(0, 0, sourceWidth, sourceHeight + 2),
                    new Rectangle(0, 0, sourceWidth + 2, sourceHeight + 2),
                    GraphicsUnit.Pixel);

                grPhoto.Dispose();
                return _bmPhoto;
            }

            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;
            int destWidth = 0;
            int destHeight = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            Bitmap bmPhoto;

            //If position is custom, the cutting can start on
            //specific positions in the image and not only top and buttom.
            if (Anchor == AnchorPosition.Custom)
            {
                if (Top != null && Left != null) //Check if Top and Left values are submitted.
                {
                    destY = Top.Value;
                    destX = Left.Value;
                }
                else
                {
                    destY = 0;
                    destX = 0;
                }
                destHeight = Height;
                destWidth = Width;

                //Setting a new image bitmap constructor
                //Setting the resolution of the bitmap image in order to be always clear after resizing.
                bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

                System.Drawing.Graphics _grPhoto = System.Drawing.Graphics.FromImage(bmPhoto);
                _grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                _grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                _grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
                _grPhoto.Clear(System.Drawing.Color.White);

                //Drawing the new Image with the specified dimentions and at the specified position
                _grPhoto.DrawImage(imgPhoto,
                    new Rectangle(0, 0, destWidth, destHeight), 
                    destX, destY, destWidth, destHeight,
                    GraphicsUnit.Pixel);

                //Disposing the image
                _grPhoto.Dispose();
            }
            else
            {

                nPercentW = ((float)Width / (float)sourceWidth);
                nPercentH = ((float)Height / (float)sourceHeight);

                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentW;
                    switch (Anchor)
                    {
                        case AnchorPosition.Top:
                            destY = 0;
                            break;
                        case AnchorPosition.Bottom:
                            destY = (int)(Height - (sourceHeight * nPercent));
                            break;
                        case AnchorPosition.Custom:
                            destY = Top.Value;
                            break;
                        default:
                            destY = (int)((Height - (sourceHeight * nPercent)) / 2) - 1;
                            break;
                    }
                }
                else
                {
                    nPercent = nPercentH;
                    switch (Anchor)
                    {
                        case AnchorPosition.Left:
                            destX = 0;
                            break;
                        case AnchorPosition.Right:
                            destX = (int)(Width - (sourceWidth * nPercent));
                            break;
                        case AnchorPosition.Custom:
                            destX = Left.Value;
                            break;
                        default:
                            destX = (int)((Width - Math.Round(sourceWidth * nPercent)) / 2);
                            break;
                    }
                }
                destWidth = (int)Math.Round(sourceWidth * nPercent);
                destHeight = (int)Math.Round(sourceHeight * nPercent);

                bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

                System.Drawing.Graphics _grPhoto = System.Drawing.Graphics.FromImage(bmPhoto);
                _grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                _grPhoto.Clear(System.Drawing.Color.White);

                _grPhoto.DrawImage(imgPhoto,
                    new Rectangle(destX - 0, destY - 0, destWidth, destHeight),
                    new Rectangle(sourceX - 0, sourceY - 0, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);

                _grPhoto.Dispose();
            }
            return bmPhoto;
        }


        public static System.Drawing.Image Crop(System.Drawing.Image imgPhoto, int Width,
          int Height, CropOptions cropOptions)
        {
            Rectangle cropRect = new Rectangle(
                cropOptions.x,
                cropOptions.y,
                cropOptions.width,
                cropOptions.height);

            Bitmap target = new Bitmap(Width, Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(imgPhoto, 
                    new Rectangle(0, 0, target.Width, target.Height),
                    cropRect,
                    GraphicsUnit.Pixel);
            }
            return target;
        }


        /// <summary> 
        /// Saves an image as a jpeg image, with the given quality 
        /// </summary> 
        /// <param name="path">Path to which the image would be saved.</param> 
        // <param name="quality">A number from 0 to 100, with 100 being the 
        /// highest quality</param> 
        public static void SaveJpeg(string path, Image img, long quality)
        {
            img.Save(path, ImageFormat.Jpeg);

            quality = Math.Min(100, quality);
            quality = Math.Max(0, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            EncoderParameters encodingParameters = new EncoderParameters(3);
            encodingParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            encodingParameters.Param[1] = new EncoderParameter(Encoder.ScanMethod, (int)EncoderValue.ScanMethodInterlaced);
            encodingParameters.Param[2] = new EncoderParameter(Encoder.RenderMethod, (int)EncoderValue.RenderProgressive);

            img.Save(path, jpegCodec, encodingParameters);
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == "image/jpeg")
                    return codec;
            }
            return null;
        }

        public enum AnchorPosition
        {
            Top, Bottom, Left, Right, Default, Custom
        }


        public enum WaterMarkStyle
        {
            Center
        }

        public static void AddWaterMark(string Source, string Destination, string WaterMarkImage, WaterMarkStyle style)
        {
            if (Source == Destination)
            {
                string backup = Path.GetDirectoryName(Source) + "\\" + Path.GetFileNameWithoutExtension(Source) + "_original.jpg";
                File.Copy(Source, backup);
                Source = backup;
            }
            using (Image image = Image.FromFile(Source))
            using (Image watermarkImage = Image.FromFile(WaterMarkImage))
            using (Graphics imageGraphics = Graphics.FromImage(image))
            using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
            {
                int x = (image.Width / 2 - watermarkImage.Width / 2);
                int y = (image.Height / 2 - watermarkImage.Height / 2);
                watermarkBrush.TranslateTransform(x, y);
                imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width + 1, watermarkImage.Height)));
                image.Save(Destination);
            }

        }


        public static void SmartCrop(string SourceFile, string DestinationFile, int Width, int Height)
        {
			lw.GraphicUtils.SmartCrop.SmartCrop sc = new lw.GraphicUtils.SmartCrop.SmartCrop();
			sc.Crop(SourceFile, DestinationFile, Width, Height);
		}


        /// <summary>
        /// Rotate the given image file according to Exif Orientation data
        /// </summary>
        /// <param name="sourceFilePath">path of source file</param>
        /// <param name="targetFilePath">path of target file</param>
        /// <param name="targetFormat">target format</param>
        /// <param name="updateExifData">set it to TRUE to update image Exif data after rotation (default is TRUE)</param>
        /// <returns>The RotateFlipType value corresponding to the applied rotation. If no rotation occurred, RotateFlipType.RotateNoneFlipNone will be returned.</returns>
        public static RotateFlipType RotateImageByExifOrientationData(string sourceFilePath, bool updateExifData = true)
        {
            // Rotate the image according to EXIF data
            var bmp = new Bitmap(sourceFilePath);
            RotateFlipType fType = RotateImageByExifOrientationData(bmp, updateExifData);
            if (fType != RotateFlipType.RotateNoneFlipNone)
            {
                //Delete the file so the new image can be saved
                System.IO.File.Delete(sourceFilePath);

                //save the image out to the file
                bmp.Save(sourceFilePath);
            }

            //release image file
            bmp.Dispose();
            return fType;
        }

        /// <summary>
        /// Rotate the given bitmap according to Exif Orientation data
        /// </summary>
        /// <param name="img">source image</param>
        /// <param name="updateExifData">set it to TRUE to update image Exif data after rotation (default is TRUE)</param>
        /// <returns>The RotateFlipType value corresponding to the applied rotation. If no rotation occurred, RotateFlipType.RotateNoneFlipNone will be returned.</returns>
        public static RotateFlipType RotateImageByExifOrientationData(Image img, bool updateExifData = true)
        {
            int orientationId = 0x0112;
            var fType = RotateFlipType.RotateNoneFlipNone;

            foreach (var prop in img.PropertyItems)
            {
                if (prop.Id == orientationId) //value of EXIF
                {
                    var pItem = img.GetPropertyItem(orientationId);
                    fType = GetRotateFlipTypeByExifOrientationData(pItem.Value[0]);
                    if (fType != RotateFlipType.RotateNoneFlipNone)
                    {
                        img.RotateFlip(fType);
                        if (updateExifData) img.RemovePropertyItem(orientationId); // Remove Exif orientation tag
                    }
                    break;
                }
            }

            //if (img.PropertyIdList.Contains(orientationId))
            //{
            //    var pItem = img.GetPropertyItem(orientationId);
            //    fType = GetRotateFlipTypeByExifOrientationData(pItem.Value[0]);
            //    if (fType != RotateFlipType.RotateNoneFlipNone)
            //    {
            //        img.RotateFlip(fType);
            //        if (updateExifData) img.RemovePropertyItem(orientationId); // Remove Exif orientation tag
            //    }
            //}
            return fType;
        }



        /// <summary>
        /// Return the proper System.Drawing.RotateFlipType according to given orientation EXIF metadata
        /// </summary>
        /// <param name="orientation">Exif "Orientation"</param>
        /// <returns>the corresponding System.Drawing.RotateFlipType enum value</returns>
        public static RotateFlipType GetRotateFlipTypeByExifOrientationData(int orientation)
        {
            switch (orientation)
            {
                case 1:
                default:
                    return RotateFlipType.RotateNoneFlipNone;
                case 2:
                    return RotateFlipType.RotateNoneFlipX;
                case 3:
                    return RotateFlipType.Rotate180FlipNone;
                case 4:
                    return RotateFlipType.Rotate180FlipX;
                case 5:
                    return RotateFlipType.Rotate90FlipX;
                case 6:
                    return RotateFlipType.Rotate90FlipNone;
                case 7:
                    return RotateFlipType.Rotate270FlipX;
                case 8:
                    return RotateFlipType.Rotate270FlipNone;
            }
        }
    }
}