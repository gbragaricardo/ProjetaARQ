using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetaARQ.Commands.DetailDoor.Services;
using ProjetaARQ.Commands.Shared.Models;
using ProjetaARQ.Commands.Shared.Services;
using ProjetaARQ.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProjetaARQ.Commands.DetailDoor.ViewModels
{
    public partial class DetailDoorViewModel : ObservableObject, IDetailDoorResult
    {
        private readonly IRevitContext _revitContext;
        private readonly IViewTemplateService _viewTemplateService;
        private readonly IDoorService _doorService;
        public event Action RequestClose;
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
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Planta da Porta", IsChecked = true, ViewOrientation = AssemblyDetailViewOrientation.HorizontalDetail });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Corte A", IsChecked = true, ViewOrientation = AssemblyDetailViewOrientation.DetailSectionA });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Frontal", IsChecked = true, ViewOrientation = AssemblyDetailViewOrientation.ElevationFront });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Corte B",  ViewOrientation = AssemblyDetailViewOrientation.DetailSectionB });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação de Topo",  ViewOrientation = AssemblyDetailViewOrientation.ElevationTop });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Inferior", ViewOrientation = AssemblyDetailViewOrientation.ElevationBottom });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Esquerda", ViewOrientation = AssemblyDetailViewOrientation.ElevationLeft });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Direita", ViewOrientation = AssemblyDetailViewOrientation.ElevationRight });
            ViewOptionItems.Add(new ViewOptionItem { ViewOptionName = "Elevação Posterior", ViewOrientation = AssemblyDetailViewOrientation.ElevationBack });
        }

        [RelayCommand]
        public void GenerateAssemblies()
        {
            RequestClose?.Invoke();
        }
    }
}
