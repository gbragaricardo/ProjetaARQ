using ProjetaARQ.Commands.WordExport.Models;
using ProjetaARQ.Commands.WordExport.Services;
using System;
using System.Collections.ObjectModel;
using ProjetaARQ.Core.UI;

namespace ProjetaARQ.Commands.WordExport.MVVM.ViewModels
{
    internal class PresetsListViewModel : ObservableObject
    {
        public event EventHandler<PresetModel> EditPresetRequested;
        public event EventHandler CreateNewPresetRequested;

        private readonly PresetService _presetService;
        public ObservableCollection<PresetModel> PresetsList { get; }


        private PresetModel _selectedPreset;
        public PresetModel SelectedPreset
        {
            get => _selectedPreset;
            set => SetProperty(ref _selectedPreset, value);
        }

        public RelayCommand EditPresetCommand { get; }
        public RelayCommand CreateNewPresetCommand { get; }

        public PresetsListViewModel()
        {
            _presetService = new PresetService();
            PresetsList = new ObservableCollection<PresetModel>();

            //PresetsList = new ObservableCollection<PresetModel>
            //{
            //    new PresetModel { PresetName = "Preset de Exemplo 1" },
            //    new PresetModel { PresetName = "Memorial Padrão (Exemplo)" },
            //    new PresetModel { PresetName = "Preset de Teste 3" }
            //};

            EditPresetCommand = new RelayCommand(commandParameter => OnEditPresetRequested(commandParameter as PresetModel), p => true);

            CreateNewPresetCommand = new RelayCommand(p => OnCreateNewPresetRequested());

            LoadAllPresets();
        }


        public void LoadAllPresets()
        {
            PresetsList.Clear();

            var presetPaths = _presetService.GetAllPresetPaths();

            foreach (var path in presetPaths)
            {
                var preset = _presetService.LoadPreset(path);

                if (preset != null)
                    PresetsList.Add(preset);
            }

            //SelectedPreset = PresetsList.FirstOrDefault();
        }

        protected virtual void OnEditPresetRequested(PresetModel preset)
        {
            EditPresetRequested?.Invoke(this, preset);
        }

        protected virtual void OnCreateNewPresetRequested()
        {
            CreateNewPresetRequested?.Invoke(this, EventArgs.Empty);
        }

        private void DeleteSelected()
        {
            // AQUI, você usaria o PresetService para apagar o ficheiro .json
            // e depois removeria o item da coleção 'Presets'.
            // Ex: _presetService.DeletePreset(SelectedPreset);
            //      Presets.Remove(SelectedPreset);
        }
    }
}
