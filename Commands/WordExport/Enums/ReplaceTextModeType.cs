using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Commands.WordExport.Enums
{
    public enum ReplaceTextModeType
    {
        [Description("")]
        Void,

        [Description("Substituir tudo")]
        ReplaceAll,

        [Description("Substituir parte")]
        ReplaceIn
    }
}
