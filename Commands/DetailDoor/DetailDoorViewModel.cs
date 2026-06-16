using CommunityToolkit.Mvvm.ComponentModel;
using ProjetaARQ.Commands.Shared.Models;
using ProjetaARQ.Commands.Shared.Services;
using ProjetaARQ.Core.Services;
using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace ProjetaARQ.Commands.DetailDoor
{
    public partial class DetailDoorViewModel : ObservableObject
    {
        private readonly IRevitContext _revitContext;
        private readonly IViewTemplateService _viewTemplateService;
        public ObservableCollection<ViewOptionItem> ViewOptionItems { get; } = new();
        public ObservableCollection<RevitViewTemplateItem> AvailableViewTemplates { get; set; } = new();

        public DetailDoorViewModel(IRevitContext revitContext, IViewTemplateService viewTemplateService)
        {
            _revitContext = revitContext;
            _viewTemplateService = viewTemplateService;
            LoadAvailableViewTemplates();
            CreateViewOptions();
        }

        private void LoadAvailableViewTemplates()
        {
            var templates = _viewTemplateService.GetTemplates(_revitContext.Doc);

            AvailableViewTemplates.Clear();

            foreach (var template in templates)
                AvailableViewTemplates.Add(template);
        }
        private void CreateViewOptions()
        {
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Ortogonal 3D", IsChecked = true });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Planta da Porta", IsChecked = true });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Corte A", IsChecked = true });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Corte B" });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação de Topo" });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Inferior" });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Esquerda" });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Direita" });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Frontal", IsChecked = true });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Posterior" });
        }
    }
}
