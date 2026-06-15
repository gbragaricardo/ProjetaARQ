using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ProjetaARQ.Commands.DetailDoor
{
    public partial class DetailDoorViewModel : ObservableObject
    {
        public ObservableCollection<ViewOptionItem> ViewOptionItems { get; } = new();

        public DetailDoorViewModel()
        {
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Ortogonal 3D" });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Planta da Porta" });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Corte A" });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Corte B" });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação de Topo" });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Inferior" });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Esquerda" });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Direita" });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Frontal" });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Posterior" });
        }
    }
}
