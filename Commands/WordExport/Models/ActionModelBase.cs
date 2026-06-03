using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ProjetaARQ.Commands.WordExport.Enums;

namespace ProjetaARQ.Commands.WordExport.Models
{

    [JsonDerivedType(typeof(ReplaceTextActionModel), typeDiscriminator: "ReplaceText")]
    public abstract class ActionModelBase
    {
        public string TargetTag { get; set; }
        public string CheckBoxConditionText { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ExecuteConditionType ExecuteCondition { get; set; }
    }
}
