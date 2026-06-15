using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Commands.DetailDoor
{
    public partial class ViewOptionItem : ObservableObject
    {
        [ObservableProperty]
        private string _viewOptionName;

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private string _selectedTemplate;
    }
}
