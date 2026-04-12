using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace BLL
{
    public class ImageConverter
    {
        /// <summary>
        /// Pretvara sliku u niz bajtova (PNG format).
        /// Koristi se za spremanje slike u bazu podataka.
        /// </summary>
        /// <param name="image">Slika koja se pretvara</param>
        /// <returns>Niz bajtova slike</returns>
        public byte[] ImageToBytes(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                byte[] bytes = ms.ToArray();
                return bytes;
            }
        }
        /// <summary>
        /// Pretvara niz bajtova u sliku.
        /// Koristi se za prikaz slike dohvaćene iz baze.
        /// </summary>
        /// <param name="bytes">Niz bajtova slike</param>
        /// <returns>Slika dobivena iz bajtova</returns>
        public Image BytesToImage(byte[] bytes){
            using (var ms = new MemoryStream(bytes))
            {
                var image = Image.FromStream(ms);
                return new Bitmap(image);
            }
        }
        /// <summary>
        /// Učitava sliku s diska i prikazuje je u PictureBox kontroli.
        /// Provjerava da veličina slike ne prelazi dopušteni limit.
        /// </summary>
        /// <param name="path">Putanja do slike</param>
        /// <param name="pictureBox">PictureBox u koji se slika učitava</param>
        public void LoadImage(string path, PictureBox pictureBox)
        {
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            var tmp = Image.FromStream(fs);
            if(ImageToBytes(tmp).Length > 600000)
            {
                MessageBox.Show("The selected image is too large. Please choose an image smaller than 600KB.", "Image Size Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (pictureBox.Image != null)
            {
                var old = pictureBox.Image;
                pictureBox.Image = null;
                old.Dispose();
            }
            pictureBox.Image = new Bitmap(tmp);
        }
    }
}
