using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProjetaARQ.Services
{
    internal class BitmapConverter
    {
        /// <summary>
        /// Carrega uma imagem da pasta "Icons" embutida no Assembly
        /// </summary>
        public static BitmapImage GetIcon(string fileName)
            => LoadImage("Resources.Icons", fileName);
        

        /// <summary>
        /// Carrega uma imagem da pasta "Images" embutida no Assembly
        /// </summary>
        public static BitmapImage GetResource(string fileName)
            => LoadImage("Resources.Images", fileName);
        

        /// <summary>
        /// Método genérico para carregar uma imagem de qualquer pasta do Assembly
        /// </summary>
        private static BitmapImage LoadImage(string folder, string fileName)
        {
            string resourcePath = AssemblyServices.GetNamespace() + $"{folder}.{fileName}";

            using (Stream stream = AssemblyServices.GetAssembly().GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                    throw new FileNotFoundException($"Recurso {resourcePath} não encontrado.");

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();

                return image;
            }
        }
    }
}
