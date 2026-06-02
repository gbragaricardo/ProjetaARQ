using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using ProjetaARQ.Core;

namespace ProjetaARQ.Services
{
    internal class AssemblyServices
    {
        /// <summary>
        /// Obtém o Assembly atual (DLL onde os recursos estão embutidos)
        /// </summary>
        public static Assembly GetAssembly() => Assembly.GetExecutingAssembly();

        /// <summary>
        /// Obtém o namespace base onde os recursos estão armazenados
        /// </summary>
        public static string GetNamespace() => "ProjetaARQ.";
    }
}

