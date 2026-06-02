using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Commands.WordExport.Enums
{
    public enum RuleActionType
    {
        [Description("")]
        Void,

        [Description("Substituir Texto")]
        ReplaceText,

        [Description("Substituir Imagem")]
        ReplaceImage,

        [Description("Deletar Seção")]
        DeleteSection,
        // Adicione aqui novas ações no futuro
    }
}
