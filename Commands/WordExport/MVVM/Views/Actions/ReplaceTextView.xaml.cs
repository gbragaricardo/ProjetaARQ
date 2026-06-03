using System.Windows.Controls;
using System.Windows.Input;

namespace ProjetaARQ.Commands.WordExport.MVVM.Views.Actions
{
    /// <summary>
    /// Interaction logic for ReplaceTextView.xaml
    /// </summary>
    public partial class ReplaceTextView : UserControl
    {
        public ReplaceTextView()
        {
            InitializeComponent();
        }

        private void ComboBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Converte o 'sender' para um ComboBox
            if (sender is ComboBox comboBox)
            {
                // A LÓGICA PRINCIPAL:
                // Se a lista suspensa NÃO estiver aberta...
                if (!comboBox.IsDropDownOpen)
                {
                    // ...marca o evento como "manipulado".
                    // Isso impede que o ComboBox processe a rolagem,
                    // mas permite que o evento continue a "subir" para um
                    // ScrollViewer pai, se houver.
                    e.Handled = true;
                }
            }
        }
    }
}
