using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProjetaARQ.Commands.FamiliesPanel.MVVM
{
    internal class FamilyItem
    {
        public string Name { get; set; }
        public string UpperName { get; set; }

        public string FilePath { get; set; }
        public BitmapSource ThumbnailPath { get; set; }

        public FamilyItem(string filePath)
        {
            FilePath = filePath;
            Name = Path.GetFileNameWithoutExtension(filePath);
            UpperName = Name.ToUpper();

            // MODO ANTIGO
            //string thumbPath = Path.Combine(thumbsDirectory, Name + ".png");
            //ThumbnailPath = File.Exists(thumbPath) ? thumbPath : Path.Combine(thumbsDirectory, "default.png");
        }
    }
}
