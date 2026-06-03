using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Commands.WordExport.Enums
{
    public enum DataSourceType
    {
        [Description("")]
        Void,

        [Description("Bloco de Texto")]
        TextBlock,

        [Description("Escrever Texto")]
        WriteText,

        [Description("Parâmetro Revit")]
        RevitParameter,

    }
}
