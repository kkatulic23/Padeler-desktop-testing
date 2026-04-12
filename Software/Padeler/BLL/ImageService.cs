using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace BLL
{
    public class ImageService
    {
        public Image ConvertUserImage(UserImageDto userImage, Image fallback)
        {
            if(userImage == null || !userImage.Success || string.IsNullOrWhiteSpace(userImage.ImageBase64)){
                return fallback;
            }

            try
            {
                byte[] bytes = Convert.FromBase64String(userImage.ImageBase64);

                using (var ms = new MemoryStream(bytes))
                {
                    return Image.FromStream(ms);
                }
            }
            catch
            {
                return fallback;
            }
        }
    }
}
