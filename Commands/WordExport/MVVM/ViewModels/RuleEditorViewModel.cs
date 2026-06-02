using GongSolutions.Wpf.DragDrop;
using ProjetaARQ.UI.Core;
using ProjetaARQ.Commands.WordExport.Models;
using ProjetaARQ.Commands.WordExport.MVVM.ViewModels.Actions;
using ProjetaARQ.Commands.WordExport.Services;
using ProjetaARQ.Commands.WordExport.Services.GongHandlers;
using ProjetaARQ.Commands.WordExport.Services.UndoableCommands;
using System;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.IO;
using System.Linq;

namespace ProjetaARQ.Commands.WordExport.MVVM.ViewModels
{
    internal class RuleEditorViewModel : ObservableObject
    {
        public string _templatePath;
        private readonly PresetService _presetService = new PresetService();
        private readonly PresetModel _presetModel;

        public IDropTarget DeleteDropHandler { get; }
        public IDragSource RuleDragHandler { get; }
        public IDropTarget RuleDropHandler { get; }



        private readonly UndoRedoManager _undoRedoManager = new UndoRedoManager();
        public RelayCommand UndoCommand { get; }
        public RelayCommand RedoCommand { get; }

        public ObservableCollection<RuleCardViewModel> RuleCardList { get; set; } = new ObservableCollection<RuleCardViewModel>();
        public RelayCommand AddRuleCommand { get; }
        public RelayCommand ExportCommand { get; }
        public RelayCommand SaveCommand { get; }



        public RuleEditorViewModel()
        {
            DeleteDropHandler = new DeleteDropHandler(RuleCardList, _undoRedoManager);
            RuleDragHandler = new RuleDragHandler(this);
            RuleDropHandler = new RuleDropHandler(_undoRedoManager);

            UndoCommand = new RelayCommand(p => _undoRedoManager.Undo());
            RedoCommand = new RelayCommand(p => _undoRedoManager.Redo());

            AddRuleCommand = new RelayCommand(x => AddRule());
            ExportCommand = new RelayCommand(x => ExportWord());
            SaveCommand = new RelayCommand(x => SaveCurrentPreset());

            RuleCardList.Add(new RuleCardViewModel(_undoRedoManager));
        }

        private void AddRule()
        {
            var ruleToAdd = new RuleCardViewModel(_undoRedoManager);
            var command = new AddRuleCommand(RuleCardList, ruleToAdd);
            _undoRedoManager.Do(command);
        }

        private void ExportWord()
        {
            //FileHandler fileHandler = new FileHandler();

            //string savePath = fileHandler.GetSavePath();
            //fileHandler.CreateNewFile(savePath);

            //using (WordEditor editor = new WordEditor(savePath))
            //{
            //    foreach (var ruleCard in RulesList)
            //    {
            //        var ruleCardViewModel = ruleCard.CurrentActionViewModel as ReplaceTextViewModel;

            //        editor.ReplaceTextInContentControl(ruleCardViewModel.ContentTag, ruleCardViewModel.NewText);
            //    }
            //}


            //fileHandler.OpenFile(savePath);
        }


        private void SaveCurrentPreset()
        {
            FileServices fileHandler = new FileServices();
            string filePath = fileHandler.GetSavePath("Salvar em", "{Name}", ".json");
            if (string.IsNullOrEmpty(filePath))
                return; // O usuário cancelou
            

            var presetToSave = new PresetModel
            {
                PresetName = Path.GetFileNameWithoutExtension(filePath),
                WordTemplatePath = _templatePath 
            };

            foreach (var ruleCardVM in RuleCardList)
            {
                // Cria um novo RuleModel para ser salvo
                var ruleModel = new RuleCardModel
                {
                    // Mapeia as propriedades universais
                    CardName = ruleCardVM.RuleCardName,
                    CardAction = ruleCardVM.SelectedAction,
                };
                switch (ruleCardVM.CurrentActionViewModel)
                {
                    // Se for um ViewModel de "Substituir Texto"...
                    case ReplaceTextViewModel replaceVM:

                        ruleModel.RuleCardValues = new ReplaceTextActionModel
                        {
                            ExecuteCondition = replaceVM.SelectedCondition,
                            CheckBoxConditionText = replaceVM.CheckBoxConditionText,
                            TargetTag = replaceVM.ContentTag,
                            EditMode = replaceVM.SelectedEditMode,
                            TextToReplace = replaceVM.TextBoxToReplace,
                            DataSource = replaceVM.SelectedDataSource,
                            ReplacementValue = replaceVM.ReplacementText,
                        };
                        break;
                }

                // Adiciona a regra completa (com a sua ação específica) à lista do preset
                presetToSave.RuleCards.Add(ruleModel);
            }

            _presetService.SavePreset(presetToSave, filePath);
        }

        public void LoadPreset(PresetModel presetToEdit)
        {
            foreach (var ruleCardModel in presetToEdit.RuleCards)
            {
                var ruleCardVM = new RuleCardViewModel(_undoRedoManager)
                {
                    RuleCardName = ruleCardModel.CardName,
                    SelectedAction = ruleCardModel.CardAction
                };

                OnPropertyChanged(nameof(ruleCardVM.RuleCardName));
                OnPropertyChanged(nameof(ruleCardVM.SelectedAction));

                switch (ruleCardVM.SelectedAction)
                {
                    case Enums.RuleActionType.ReplaceText:
                        var replaceTextModel = ruleCardModel.RuleCardValues as ReplaceTextActionModel;
                        var replaceTextVM = new ReplaceTextViewModel(_undoRedoManager)
                        {
                            SelectedCondition = replaceTextModel.ExecuteCondition,
                            CheckBoxConditionText = replaceTextModel.CheckBoxConditionText,
                            ContentTag = replaceTextModel.TargetTag,
                            SelectedEditMode = replaceTextModel.EditMode,
                            TextBoxToReplace = replaceTextModel.TextToReplace,
                            SelectedDataSource = replaceTextModel.DataSource,
                            ReplacementText = replaceTextModel.ReplacementValue
                        };

                        ruleCardVM.CurrentActionViewModel = replaceTextVM;

                        break;

                }
                RuleCardList.Add(ruleCardVM);
            }
        }
    }
}
