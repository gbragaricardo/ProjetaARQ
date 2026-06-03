using MahApps.Metro.Controls;
using ProjetaARQ.Commands.WordExport.MVVM.ViewModels;

namespace ProjetaARQ.Commands.WordExport.MVVM.Views
{
    /// <summary>
    /// Interaction logic for DevView.xaml
    /// </summary>
    public partial class WordConfigView : MetroWindow
    {
        internal WordConfigView(WordConfigViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

    }
}
