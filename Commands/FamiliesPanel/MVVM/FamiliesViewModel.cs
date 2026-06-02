using Autodesk.Revit.UI;
using ControlzEx.Theming;
using Microsoft.WindowsAPICodePack.Dialogs;
using ProjetaARQ.Commands.FamiliesPanel.Events;
using ProjetaARQ.Services;
using ProjetaARQ.UI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace ProjetaARQ.Commands.FamiliesPanel.MVVM
{
    internal class FamiliesViewModel : ObservableObject
    {
        public ObservableCollection<FolderItem> SubFolders { get; set; } = new ObservableCollection<FolderItem>();
        public ObservableCollection<FolderItem> SelectedSubFolders { get; set; } = new ObservableCollection<FolderItem>();
        public ObservableCollection<FamilyItem> FolderFamilies { get; set; } = new ObservableCollection<FamilyItem>();

        private readonly DownloadFamilyEvent _downloadHandler;

        private readonly ExternalEvent _downloadEvent;


        private FamiliesView _familiesWindow;
        public FamiliesView FamiliesWindow
        {
            get => _familiesWindow;
            set
            {
                if (_familiesWindow != value)
                {
                    _familiesWindow = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _rootPath;

        private string _currentPath;
        public string CurrentPath
        {
            get => _currentPath;
            set
            {
                if (_currentPath != value)
                {
                    _currentPath = value;
                    OnPropertyChanged();
                    GetDirectories();
                }
            }
        }

        private bool _isDarkTheme;
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (_isDarkTheme != value)
                {
                    _isDarkTheme = value;
                    OnPropertyChanged();

                    string theme = value ? "Dark.Crimson" : "Light.Crimson";
                    ThemeManager.Current.ChangeTheme(_familiesWindow, theme);

                    _eggIcon =  IsDarkTheme ? "eggprojeta-darktheme.png" : "eggprojeta.png";
                    OnPropertyChanged(nameof(EggIcon));
                }
            }
        }

        private string _eggIcon = "eggprojeta.png";
        public string EggIcon
        {
            get => _eggIcon;
            set
            {
                if (_eggIcon != value)
                {
                    _eggIcon = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _search;
        public string Search
        {
            get => _search;
            set
            {
                if (_search != value)
                {
                    _search = value.ToUpper();
                    OnPropertyChanged();
                    LoadFamilies(_search);
                }
            }
        }

        public RelayCommand ChangeRootPathCommand { get; }
        public RelayCommand ChangeCurrentPathCommand { get; }
        public RelayCommand BackToPreviousPathCommand { get; }
        public RelayCommand GoToHomeCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand DownloadCommand { get; }




        public FamiliesViewModel()
        {
            _rootPath = GetRootPath();
            Console.WriteLine(_rootPath);

            _downloadHandler = new DownloadFamilyEvent();
            _downloadEvent = ExternalEvent.Create(_downloadHandler);

            CurrentPath = _rootPath;
            ChangeRootPathCommand = new RelayCommand(_ => ChangeRootPath(_rootPath));
            ChangeCurrentPathCommand = new RelayCommand(parameter => ChangeCurrentPath(CurrentPath, parameter));
            BackToPreviousPathCommand = new RelayCommand(x => BackToPreviousPath());
            GoToHomeCommand = new RelayCommand(x => GoToHome());
            UpdateCommand = new RelayCommand(x => Update());
            DownloadCommand = new RelayCommand(x => DownloadFamily(x));

            Update();
        }

        private string GetRootPath()
        {
            string projetaPath = @"\\192.168.0.250\GrupoProjeta$\Engenharia\QUALIDADE\11 - ARQUIVOS BASE DAS DISCIPLINAS\SETOR DE BIM\FAMÍLIAS BIM";
            if (Directory.Exists(projetaPath))
                return projetaPath;

            projetaPath = @"\\10.0.0.251\GrupoProjeta$\Engenharia\QUALIDADE\11 - ARQUIVOS BASE DAS DISCIPLINAS\SETOR DE BIM\FAMÍLIAS BIM";
            if (Directory.Exists(projetaPath))
                return projetaPath;

            return null;
        }
        private string ChangeRootPath(string rootPath)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                InitialDirectory = rootPath,
                Title = "Selecione a pasta raiz",
                EnsurePathExists = true,
                AllowNonFileSystemItems = false,
                AddToMostRecentlyUsedList = false,
                ShowPlacesList = true,
                NavigateToShortcut = true,
                // Aqui está a chave: desabilita o botão "Nova Pasta"
                AllowPropertyEditing = false
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                _rootPath = dialog.FileName;
                CurrentPath = dialog.FileName;
                Update();
                return dialog.FileName;
            }

            return null;
        }

        private void ChangeCurrentPath(string currentPath, object parameter)
        {
            CurrentPath = Path.Combine(currentPath, parameter as string);
            Update();
        }
        private void Update()
        {
            GetDirectories();
            GetSelectedDirectories();
            LoadFamilies();
        }

        private void GoToHome()
        {
            CurrentPath = _rootPath;
            Update();
        }

        private void BackToPreviousPath()
        {
            if (string.IsNullOrWhiteSpace(CurrentPath))
                return;
            if (CurrentPath == _rootPath)
                return;

            var previousDirectory = System.IO.Directory.GetParent(CurrentPath);
            CurrentPath = previousDirectory.FullName;

            Update();
        }

        private void GetDirectories()
        {
            if (Directory.Exists(CurrentPath))
            {
                string[] dirs = Directory.GetDirectories(CurrentPath);
                SubFolders.Clear();

                foreach (string dir in dirs)
                {
                    if (Path.GetFileName(dir).ToUpper() == "ICONS")
                        continue;

                    FolderItem folder = new FolderItem
                    {
                        Name = Regex.Replace(Path.GetFileName(dir), @"^\d+\.\s*", "").ToUpper(),
                        Path = Path.GetFileName(dir),
                        FullPath = dir
                    };

                    SubFolders.Add(folder);
                }
                    
            }
            OnPropertyChanged(nameof(SubFolders));
        }

        private void GetSelectedDirectories()
        {
            SelectedSubFolders.Clear();
            if (string.IsNullOrWhiteSpace(CurrentPath) || CurrentPath == _rootPath)
                return;
                
            string relativePath = CurrentPath;
            while (relativePath != _rootPath)
            {
                FolderItem folder = new FolderItem
                {
                    Name = Regex.Replace(Path.GetFileName(relativePath), @"^\d+\.\s*", "").ToUpper(),
                    Path = Path.GetFileName(relativePath),
                    FullPath = relativePath
                };
                SelectedSubFolders.Insert(0, folder);

                relativePath = Directory.GetParent(relativePath).FullName;
            }
        }

        private void LoadFamilies()
        {
            FolderFamilies.Clear();

            if (!Directory.Exists(CurrentPath))
                return;

            var files = Directory.GetFiles(CurrentPath, "*.rfa");

            foreach (var file in files)
            {
                FamilyItem family = new FamilyItem(file);
                family.ThumbnailPath = WindowsThumbServices.GetRevitFileThumbnail(family.FilePath);
                if (family.ThumbnailPath == null) family.ThumbnailPath = BackPropagateIcons(family);
                FolderFamilies.Add(family);
            }

            OnPropertyChanged(nameof(FolderFamilies));
        }

        private void LoadFamilies(string search)
        {
            FolderFamilies.Clear();

            if (!Directory.Exists(CurrentPath))
                return;

            var files = Directory.GetFiles(CurrentPath, "*.rfa");

            // Filtra só os arquivos cujo caminho (ou nome) contenha o 'search'
            var filteredFiles = files.Where(file => file.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);

            foreach (var file in filteredFiles)
            {
                FamilyItem family = new FamilyItem(file);
                family.ThumbnailPath = WindowsThumbServices.GetRevitFileThumbnail(family.FilePath);
                if (family.ThumbnailPath == null) family.ThumbnailPath = BackPropagateIcons(family);
                FolderFamilies.Add(family);
            }
        }

        private void DownloadFamily(object parameter)
        {
            FamilyItem selectedFamily = parameter as FamilyItem;
            string familyPath = selectedFamily.FilePath;

            _downloadHandler.SetFamilyPath(selectedFamily.Name, familyPath);
            _downloadEvent.Raise();
        }

        private BitmapSource BackPropagateIcons(FamilyItem family )
        {
            string relativePath = CurrentPath;
            string iconPath;
            string defaultIconPath;

            while (relativePath != _rootPath)
            {
                string iconsDirectory = Path.Combine(relativePath, "Icons");

                if (Directory.Exists(iconsDirectory))
                {
                    iconPath = Path.Combine(iconsDirectory, family.Name + ".png");
                    if (File.Exists(iconPath))
                        return WindowsThumbServices.LoadBitmapSourceFromFile(iconPath);

                    defaultIconPath = Path.Combine(iconsDirectory, "default.png");
                    if (File.Exists(defaultIconPath))
                        return WindowsThumbServices.LoadBitmapSourceFromFile(defaultIconPath);
                }
                relativePath = Directory.GetParent(relativePath).FullName;
            }
            return null;
        }
    }
}
