using CommunityToolkit.Mvvm.ComponentModel;
using ProjetaARQ.Commands.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private bool _isChecked;

        [ObservableProperty]
        private RevitViewTemplateItem _selectedViewTemplate;
    }
}
