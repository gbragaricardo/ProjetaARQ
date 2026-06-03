using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ProjetaARQ.Commands.WordExport.Services
{
    internal class FileServices
    {
        private readonly string _rootPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Docs", "mmd.docx");
        private string _newPath;
        internal string GetSavePath(string title, string defaultFileName, string defaultExtension)
        {
            var dialog = new CommonSaveFileDialog
            {
                //Title = "Exportar para",
                //DefaultFileName = "MMD-XXXXX-EXE-ARQ-0101-REV0X.docx",
                //DefaultExtension = ".docx",
                //EnsurePathExists = true

                Title = title,
                DefaultFileName = defaultFileName,
                DefaultExtension = defaultExtension,
                EnsurePathExists = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                _newPath = dialog.FileName;
                return dialog.FileName;
            }
                
            else
                return null;
            
        }

        internal bool CreateNewFile(string newPath)
        {
            if (newPath == null || File.Exists(_rootPath) == false)
            {
                return false;
            }
            

            try
            {
                File.Copy(_rootPath, newPath, overwrite: true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        internal bool OpenFile(string filePath)
        {
            if (File.Exists(filePath) == false) 
                return false;

            try
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true});
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
    }
}
