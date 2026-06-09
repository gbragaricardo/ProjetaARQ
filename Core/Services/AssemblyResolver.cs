using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Reflection;

namespace ProjetaARQ.Core.Services
{
    public static class AssemblyResolver
    {
        public static void Register(UIControlledApplication app, string pluginFolderName)
        {
            string revitVersion = app.ControlledApplication.VersionNumber;

            string addinPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Autodesk",
                "Revit",
                "Addins",
                revitVersion,
                pluginFolderName
            );

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string assemblyName = new AssemblyName(args.Name).Name;
                string assemblyPath = Path.Combine(addinPath, $"{assemblyName}.dll");

                return File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
            };
        }
    }
}