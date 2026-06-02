using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using ProjetaARQ.Commands.WordExport.MVVM.ViewModels;
using ProjetaARQ.Commands.WordExport.Services.UndoableCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjetaARQ.Commands.WordExport.Services.GongHandlers
{
    internal class RuleDropHandler : IDropTarget
    {
        #region Properties
        private readonly UndoRedoManager _undoRedoManager;

        #endregion
        public RuleDropHandler(UndoRedoManager undoRedoManager)
        {
            _undoRedoManager = undoRedoManager;
        }
        #region TargetInterface
        public void Drop(IDropInfo dropInfo)
        {
            // Pega o item que foi arrastado e sua coleção de origem
            var sourceItem = dropInfo.Data as RuleCardViewModel;
            var sourceCollection = dropInfo.DragInfo.SourceCollection.TryGetList() as ObservableCollection<RuleCardViewModel>;

            if (sourceItem == null || sourceCollection == null)
                return;

            
            // Pega o índice onde o item seria inserido
            int newIndex = dropInfo.InsertIndex;
            int oldIndex = sourceCollection.IndexOf(sourceItem);

            // Ajusta o índice se estiver movendo para baixo na mesma lista
            if (oldIndex < newIndex)
                newIndex--;
            

            // Não executa o comando se a posição não mudou
            if (oldIndex == newIndex)
                return;

            var command = new MoveRuleCommand(sourceCollection, oldIndex, newIndex);
            _undoRedoManager.Do(command);

        }

        public void DragOver(IDropInfo dropInfo)
        {
            // 1. Verifica se o que está sendo arrastado é do tipo correto
            if (dropInfo.Data is RuleCardViewModel)
            {
                // 2. Define o feedback visual como uma linha de inserção
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;

                // 3. Garante que o efeito visual do cursor seja de "Mover"
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        public void DragEnter(IDropInfo dropInfo) { }
        public void DragLeave(IDropInfo dropInfo) { }
        public void DropHint(IDropHintInfo dropHintInfo) { }
        #endregion
    }
}
