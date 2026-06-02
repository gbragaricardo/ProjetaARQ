using System;
using ProjetaARQ.Commands.WordExport.Models;
using ProjetaARQ.UI.Core;

namespace ProjetaARQ.Commands.WordExport.MVVM.ViewModels
{
    internal class WordConfigViewModel : ObservableObject
    {
        #region Properties

        private Object _currentView;
        public Object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private bool _isMenuExpanded = false;
        public bool IsMenuExpanded
        {
            get => _isMenuExpanded;
            set
            {

                if (_isMenuExpanded != value)
                {
                    _isMenuExpanded = value;
                    OnPropertyChanged();
                }
            }
        }

        public RuleEditorViewModel RuleEditorVM { get; set; }
        public PresetsListViewModel PresetsListVM { get; set; }




        public RelayCommand RuleEditorViewCommand { get; }
        public RelayCommand PresetsListViewCommand { get; }

        public RelayCommand ToggleMenuCommand { get; }
        #endregion

        public WordConfigViewModel()
        {
            RuleEditorVM = new RuleEditorViewModel();
            PresetsListVM = new PresetsListViewModel();

            RuleEditorViewCommand = new RelayCommand(o =>
            {
                CurrentView = RuleEditorVM;
            });

            PresetsListViewCommand = new RelayCommand(o =>
            {
                CurrentView = PresetsListVM;
            });

            ToggleMenuCommand = new RelayCommand(x => ToggleMenu());

            ShowPresetListView();
        }

        private void ToggleMenu()
        {
            IsMenuExpanded = !IsMenuExpanded;
        }

        private void ShowPresetListView()
        {
            PresetsListVM.EditPresetRequested += OnPresetEditRequested;
            PresetsListVM.CreateNewPresetRequested += OnPresetCreateRequested;

            //CurrentView = presetListVM;
        }

        // Este método é o "ouvinte" do evento de edição
        private void OnPresetEditRequested(object sender, PresetModel presetToEdit)
        {
            if (presetToEdit == null) return;

            var ruleCardEditorVM = new RuleEditorViewModel();
            ruleCardEditorVM.LoadPreset(presetToEdit);
            CurrentView = ruleCardEditorVM;

            OnPropertyChanged(nameof(CurrentView));
        }

        private void OnPresetCreateRequested(object sender, EventArgs e)
        {
            var newPreset = new PresetModel { PresetName = "Novo Preset" };
            OnPresetEditRequested(this, newPreset);
        }
    }
}
