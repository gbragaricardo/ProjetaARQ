using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Shell;
using System.Windows.Media.Imaging;
using System.IO;

namespace ProjetaARQ.Services
{
    internal class WindowsThumbServices
    {
        public static BitmapSource LoadBitmapSourceFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            try
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(filePath, UriKind.Absolute);
                image.CacheOption = BitmapCacheOption.OnLoad; // Garante que a imagem seja carregada completamente
                image.EndInit();
                image.Freeze(); // Otimização para uso em threads de UI
                return image;
            }
            catch (Exception ex)
            {
                // Log do erro, se necessário
                System.Diagnostics.Debug.WriteLine($"Erro ao carregar imagem de {filePath}: {ex.Message}");
                return null;
            }
        }

        public static BitmapSource GetRevitFileThumbnail(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            try
            {
                // Usa a biblioteca para carregar o arquivo como um objeto do Shell
                ShellFile shellFile = ShellFile.FromFilePath(filePath);

                // Pede ao Shell para gerar a miniatura em um tamanho médio
                shellFile.Thumbnail.FormatOption = ShellThumbnailFormatOption.ThumbnailOnly;
                var shellBitmap = shellFile.Thumbnail.LargeBitmap; // Você pode escolher outros tamanhos como Large, Small, etc.

                // Converte o Bitmap do Shell para um BitmapSource que o WPF entende
                // (Este método de conversão é necessário)
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    shellBitmap.GetHbitmap(),
                    System.IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (System.Exception ex)
            {
                // Pode falhar se o arquivo estiver corrompido ou se o handler do Revit não estiver presente
                System.Diagnostics.Debug.WriteLine($"Erro ao obter thumbnail para {filePath}: {ex.Message}");
                return null;
            }
        }
    }
}
