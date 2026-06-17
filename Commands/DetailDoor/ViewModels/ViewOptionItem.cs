using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using ProjetaARQ.Commands.Shared.Models;

namespace ProjetaARQ.Commands.DetailDoor.ViewModels
{
    public partial class ViewOptionItem : ObservableObject
    {
        [ObservableProperty]
        private string _viewOptionName;

        [ObservableProperty]
        private string _tag;

        [ObservableProperty]
        private bool _isChecked;

        [ObservableProperty]
        private RevitViewTemplateItem _selectedViewTemplate;

        [ObservableProperty]
        private bool _isViewOption3D = false;

        [ObservableProperty]
        private AssemblyDetailViewOrientation _viewOrientation;
    }
}
