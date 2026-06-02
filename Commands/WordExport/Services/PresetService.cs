using ProjetaARQ.Commands.WordExport.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;


namespace ProjetaARQ.Commands.WordExport.Services
{
    internal class PresetService
    {
        private readonly string _presetsFolderPath = "G:\\Other computers\\Meu laptop\\Projeta\\Disciplinas\\Arquitetura\\Plugin\\";

        public void SavePreset(PresetModel preset, string filePath)
        {
            // Opções para que o JSON fique formatado e fácil de ler (indentado)
            var options = new JsonSerializerOptions { WriteIndented = true };

            // Converte o objeto C# para uma string JSON
            string jsonString = JsonSerializer.Serialize(preset, options);

            // Salva a string no arquivo
            File.WriteAllText(filePath, jsonString);
        }

        public PresetModel LoadPreset(string filePath)
        {
            if (!File.Exists(filePath))
                return null;


            // Lê todo o conteúdo do arquivo
            string jsonString = File.ReadAllText(filePath);

            // Converte a string JSON de volta para um objeto C#
            return JsonSerializer.Deserialize<PresetModel>(jsonString);
        }

        public List<string> GetAllPresetPaths()
        {
            // Procura por todos os arquivos que terminam em .json na nossa pasta de presets
            return Directory.GetFiles(_presetsFolderPath, "*.Json").ToList();
        }
    }
}
