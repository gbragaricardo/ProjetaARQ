using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ProjetaARQ.Commands.WordExport.Enums;

namespace ProjetaARQ.Commands.WordExport.Models
{
    internal class ReplaceTextActionModel : ActionModelBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ReplaceTextModeType EditMode { get; set; }

        public string TextToReplace { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DataSourceType DataSource { get; set; }

        public string ReplacementValue { get; set; }
    }
}
