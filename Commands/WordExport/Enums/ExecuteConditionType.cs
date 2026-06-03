using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Commands.WordExport.Enums
{
    public enum ExecuteConditionType
    {
        [Description("Executar Sempre")]
        AlwaysExecute,
        [Description("Sim/Não do Usuário")]
        CheckBox,

    }
}
