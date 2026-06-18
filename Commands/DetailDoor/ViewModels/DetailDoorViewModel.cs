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

        [ObservableProperty] private RevitPhaseItem _selectedPhase;

        public DetailDoorViewModel(IRevitContext revitContext, IViewTemplateService viewTemplateService, IDoorService doorService)
        {
            _revitContext = revitContext;
            _viewTemplateService = viewTemplateService;
            _doorService = doorService;

            LoadAvailablePhases();
            LoadAvailableViewTemplates();
            CreateViewOptions();
        }

        public IList<ViewOptionItem> ViewOptions => ViewOptionItems.Where(vo => vo.IsChecked == true).ToList();
        public RevitPhaseItem PhaseItem => SelectedPhase;

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
            ViewOptionItems.Add(new ViewOptionItem
            {
                ViewOptionName = "Ortogonal 3D",
                IsChecked = true,
                IsViewOption3D = true,
                Tag = "3D", 
            });

            ViewOptionItems.Add(new ViewOptionItem 
            { 
                ViewOptionName = "Planta da Porta", 
                IsChecked = true, 
                ViewOrientation = AssemblyDetailViewOrientation.HorizontalDetail,
                Tag = "PLANTA",
            });

            ViewOptionItems.Add(new ViewOptionItem 
            { 
                ViewOptionName = "Corte A", 
                IsChecked = true,
                ViewOrientation = AssemblyDetailViewOrientation.DetailSectionA,
                Tag = "CORTE",
            });

            ViewOptionItems.Add(new ViewOptionItem 
            { 
                ViewOptionName = "Elevação Frontal", 
                IsChecked = true, 
                ViewOrientation = AssemblyDetailViewOrientation.ElevationFront,
                Tag = "VISTA FRONTAL",
            });

            ViewOptionItems.Add(new ViewOptionItem 
            { 
                ViewOptionName = "Corte B", 
                ViewOrientation = AssemblyDetailViewOrientation.DetailSectionB,
                Tag = "CORTE B",
            });

            ViewOptionItems.Add(new ViewOptionItem
            { 
                ViewOptionName = "Elevação Superior",
                ViewOrientation = AssemblyDetailViewOrientation.ElevationTop,
                Tag = "VISTA SUPERIOR",
            });

            ViewOptionItems.Add(new ViewOptionItem 
            {
                ViewOptionName = "Elevação Inferior",
                ViewOrientation = AssemblyDetailViewOrientation.ElevationBottom,
                Tag = "VISTA INFERIOR",
            });

            ViewOptionItems.Add(new ViewOptionItem
            { 
                ViewOptionName = "Elevação Esquerda", 
                ViewOrientation = AssemblyDetailViewOrientation.ElevationLeft,
                Tag = "VISTA LATERAL ESQUERDA",
            });

            ViewOptionItems.Add(new ViewOptionItem 
            { 
                ViewOptionName = "Elevação Direita",
                ViewOrientation = AssemblyDetailViewOrientation.ElevationRight,
                Tag = "VISTA LATERAL DIREITA",
            });

            ViewOptionItems.Add(new ViewOptionItem 
            { 
                ViewOptionName = "Elevação Posterior",
                ViewOrientation = AssemblyDetailViewOrientation.ElevationBack,
                Tag = "VISTA POSTERIOR",
            });
        }

        private bool CanCreateAssemblies()
        {
            if (SelectedPhase == null)
                return false;

            if (ViewOptionItems.Any(vo => vo.IsChecked && vo.SelectedViewTemplate == null))
                return false;

            if (!ViewOptionItems.Any(vo => vo.IsChecked))
                return false;

            return true;
        }

        [RelayCommand]
        public void CreateAssemblies()
        {
            if (CanCreateAssemblies() == false) 
                return;

            RequestClose?.Invoke();
        }
    }
}
