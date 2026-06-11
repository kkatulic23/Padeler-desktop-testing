using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace BLL
{
    public class ImageConverter // Kristian Katulić
    {
        private const int MaxImageSizeBytes = 600000; // 600 KB
        /// <summary>
        /// Pretvara sliku u niz bajtova (PNG format).
        /// Koristi se za spremanje slike u bazu podataka.
        /// </summary>
        /// <param name="image">Slika koja se pretvara</param>
        /// <returns>Niz bajtova slike</returns>
        public byte[] ImageToBytes(Image image)
        {
            if(image == null)
            {
                return null;
            }
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
        /// <summary>
        /// Pretvara niz bajtova u sliku.
        /// Koristi se za prikaz slike dohvaćene iz baze.
        /// </summary>
        /// <param name="bytes">Niz bajtova slike</param>
        /// <returns>Slika dobivena iz bajtova</returns>
        public Image BytesToImage(byte[] bytes){
            if(bytes == null || bytes.Length == 0)
            {
                return null;
            }
            using (var ms = new MemoryStream(bytes))
            using (var image = Image.FromStream(ms))
            {
                return new Bitmap(image);
            }
        }
        /// <summary>
        /// Učitava sliku s diska i prikazuje je u PictureBox kontroli.
        /// Provjerava da veličina slike ne prelazi dopušteni limit.
        /// </summary>
        /// <param name="path">Putanja do slike</param>
        /// <param name="pictureBox">PictureBox u koji se slika učitava</param>
        public Image LoadImage(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new Exception("Image path is required.");

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var tmp = Image.FromStream(fs))
            using (var bitmap = new Bitmap(tmp))
            {
                var bytes = ImageToBytes(bitmap);

                if (bytes.Length > MaxImageSizeBytes)
                    throw new Exception("The selected image is too large. Please choose an image smaller than 600KB.");

                return new Bitmap(bitmap);
            }
        }
    }
}
