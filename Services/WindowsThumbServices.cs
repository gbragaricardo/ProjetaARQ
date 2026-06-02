using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

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
                using (ShellFile shellFile = ShellFile.FromFilePath(filePath))
                {
                    shellFile.Thumbnail.FormatOption = ShellThumbnailFormatOption.ThumbnailOnly;

                    // Pega o Bitmap nativo do pacote 1.1.5
                    using (var shellBitmap = shellFile.Thumbnail.LargeBitmap)
                    {
                        // Cria um stream de memória para salvar a imagem
                        using (var memoryStream = new MemoryStream())
                        {
                            // Esse é o método correto e real da classe Bitmap!
                            shellBitmap.Save(memoryStream, ImageFormat.Png);
                            memoryStream.Position = 0; // Volta o ponteiro para o início da leitura

                            // Converte o Stream para o formato que o WPF (MahApps) entende
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = memoryStream;
                            bitmapImage.EndInit();
                            bitmapImage.Freeze(); // Otimização para UI

                            return bitmapImage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao obter thumbnail para {filePath}: {ex.Message}");
                return null;
            }
        }
    }
}
