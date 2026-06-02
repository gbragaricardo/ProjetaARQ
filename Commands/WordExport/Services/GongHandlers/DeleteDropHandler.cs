using System;
using System.Collections.ObjectModel;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using ProjetaARQ.Commands.WordExport.MVVM.ViewModels;
using ProjetaARQ.Commands.WordExport.Services.UndoableCommands;
using ProjetaARQ.UI;

namespace ProjetaARQ.Commands.WordExport.Services.GongHandlers
{
    internal class DeleteDropHandler : ObservableObject, IDropTarget
    {
        // Uma referência à lista de regras principal
        private readonly ObservableCollection<RuleCardViewModel> _collection;
        private readonly UndoRedoManager _undoRedoManager;

        internal DeleteDropHandler(ObservableCollection<RuleCardViewModel> collection, UndoRedoManager undoRedoManager)
        {
            _collection = collection;
            _undoRedoManager = undoRedoManager;
        }

        #region TargetInterface
        public void DragEnter(IDropInfo dropInfo)
        {

        }

        public void DragLeave(IDropInfo dropInfo)
        {
            
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is RuleCardViewModel)
                dropInfo.Effects = DragDropEffects.Move;

        }

        public void Drop(IDropInfo dropInfo)
        {
            // Quando o item é solto aqui, ele é removido da coleção.
            RuleCardViewModel itemToRemove = dropInfo.Data as RuleCardViewModel;

            if (itemToRemove == null)
                return;
            
            var command = new DeleteRuleCommand(_collection, itemToRemove);
            _undoRedoManager.Do(command);
        }

        public void DropHint(IDropHintInfo dropHintInfo)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
