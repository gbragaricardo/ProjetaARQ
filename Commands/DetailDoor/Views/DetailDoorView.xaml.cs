using ProjetaARQ.Commands.DetailDoor.ViewModels;
using Wpf.Ui.Controls;

namespace ProjetaARQ.Commands.DetailDoor.Views
{
    public partial class DetailDoorView : FluentWindow
    {
        // O construtor recebe a ViewModel via Injeção de Dependência do nosso contêiner Scoped
        public DetailDoorView(DetailDoorViewModel viewModel)
        {
            InitializeComponent();

            // Define o DataContext para o MVVM funcionar de forma limpa
            DataContext = viewModel;
            viewModel.RequestClose += () => this.DialogResult = true;
        }
    }
}
