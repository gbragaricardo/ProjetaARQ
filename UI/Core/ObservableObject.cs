using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjetaARQ.UI.Core
{
    internal class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            // Se o valor antigo e o novo forem iguais, não faz nada.
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            // Usa a referência (ref) para atualizar o campo privado original.
            backingStore = value;

            // Dispara a notificação para a UI.
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}
