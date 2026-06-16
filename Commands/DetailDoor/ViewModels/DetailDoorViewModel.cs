using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using ProjetaARQ.Commands.DetailDoor.Services;
using ProjetaARQ.Commands.Shared.Models;
using ProjetaARQ.Commands.Shared.Services;
using ProjetaARQ.Core.Services;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ProjetaARQ.Commands.DetailDoor.ViewModels
{
    public partial class DetailDoorViewModel : ObservableObject, IDetailDoorResult
    {
        private readonly IRevitContext _revitContext;
        private readonly IViewTemplateService _viewTemplateService;
        private readonly IDoorService _doorService;
        public ObservableCollection<RevitViewTemplateItem> AvailableViewTemplates { get; set; } = [];
        public ObservableCollection<RevitPhaseItem> AvailablePhases { get; set; } = [];
        public ObservableCollection<ViewOptionItem> ViewOptionItems { get; } = [];

        [ObservableProperty] private ElementId _selectedPhaseId;

        public DetailDoorViewModel(IRevitContext revitContext, IViewTemplateService viewTemplateService, IDoorService doorService)
        {
            _revitContext = revitContext;
            _viewTemplateService = viewTemplateService;
            _doorService = doorService;

            LoadAvailablePhases();
            LoadAvailableViewTemplates();
            CreateViewOptions();
        }

        public IList<ViewOptionItem> ViewOptions => ViewOptionItems.ToList();
        public ElementId PhaseId => SelectedPhaseId;

        private void LoadAvailablePhases()
        {
            var phases = _doorService.GetPhasesFromDoors(_revitContext.Doc);

            AvailablePhases.Clear();

            foreach (var phase in phases)
                AvailablePhases.Add(phase);
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
