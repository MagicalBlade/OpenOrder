using OpenOrder.Infrastructure.Commands;
using OpenOrder.Models;
using OpenOrder.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using OpenOrder.Views.Windows;
using OpenOrder.Data;
using System.Windows.Media;

namespace OpenOrder.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Список папок
        /// <summary>Список папок</summary>
        private ObservableCollection<Folder> _Folders;

        /// <summary>Список папок</summary>
        public ObservableCollection<Folder> Folders
        {
            get => _Folders;
            set => Set(ref _Folders, value);
        }
        #endregion

        #region Строка состояния
        /// <summary>Строка состояния</summary>
        private string _StatusBar;

        /// <summary>Строка состояния</summary>
        public string StatusBar
        {
            get => _StatusBar;
            set => Set(ref _StatusBar, value);
        }
        #endregion

        #region Выбранная папка
        /// <summary>Выбранная папка</summary>
        private Folder _SelectFolder;

        /// <summary>Выбранная папка</summary>
        public Folder SelectFolder
        {
            get => _SelectFolder;
            set => Set(ref _SelectFolder, value);
        }
        #endregion

        #region Папка по умолчанию
        /// <summary>Папка по умолчанию</summary>
        private Folder _DefaultFolder;

        /// <summary>Папка по умолчанию</summary>
        public Folder DefaultFolder
        {
            get => _DefaultFolder;
            set => Set(ref _DefaultFolder, value);
        }
        #endregion

        #region Заказ для поиска
        /// <summary>Заказ для поиска</summary>
        private string _SearhOrder;

        /// <summary>Заказ для поиска</summary>
        public string SearhOrder
        {
            get => _SearhOrder;
            set => Set(ref _SearhOrder, value);
        }
        #endregion

        #region Команды

        #region OpenFolderAddWindow - Открытие окна добавления папки
        /// <summary>Открытие окна добавления папки</summary>
        public ICommand OpenFolderAddWindow { get; }

        private bool CanOpenFolderAddWindowCommandExecute(object p) => true;
        private void OnOpenFolderAddWindowCommandExecute(object p)
        {
            var view_model = new FolderEditorWindowViewModel(Folders);
            var view = new FolderEditorWindow
            {
                DataContext = view_model,
                Owner = App.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            view.ShowDialog();
        }
        #endregion

        #region OpenFolderEditorWindow - Открытие окна редактирования папки
        /// <summary>Открытие окна редактирования папки</summary>
        public ICommand OpenFolderEditorWindow { get; }

        private bool CanOpenFolderEditorWindowCommandExecute(object p) => SelectFolder != null;
        private void OnOpenFolderEditorWindowCommandExecute(object p)
        {
            var view_model = new FolderEditorWindowViewModel(SelectFolder, Folders);
            var view = new FolderEditorWindow
            {
                DataContext = view_model,
                Owner = App.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            view.ShowDialog();
        }
        #endregion

        #region DeletFolder - Удаление выделенной папки
        /// <summary>Удаление выделенной папки</summary>
        public ICommand DeletFolder { get; }

        private bool CanDeletFolderCommandExecute(object p) => SelectFolder != null;
        private void OnDeletFolderCommandExecute(object p)
        {
            Folders.Remove(SelectFolder);
            Data.Data.SaveData(Folders);
        }
        #endregion

        #region OpenSearchFolder - Открытие заказа
        /// <summary>Открытие заказа</summary>
        public ICommand OpenSearchFolder { get; }

        private bool CanOpenFolderCommandExecute(object p) => SelectFolder != null && SearhOrder != null;
        private void OnOpenFolderCommandExecute(object p)
        {
            StatusBar = SelectFolder.Open(SearhOrder);
        }
        #endregion

        #region OpenSearchDefaultFolder - Открытие заказа из папки по умолчанию
        /// <summary>Открытие заказа из папки по умолчанию</summary>
        public ICommand OpenSearchDefaultFolder { get; }

        private bool CanOpenSearchDefaultFolderCommandExecute(object p) => DefaultFolder != null && SearhOrder != null && SearhOrder != "";
        private void OnOpenSearchDefaultFolderCommandExecute(object p)
        {
            StatusBar = DefaultFolder.Open(SearhOrder);
        }
        #endregion

        #region SetDefaultFolder - Сделать папку, папкой по умолчанию
        /// <summary>Сделать папку, папкой по умолчанию</summary>
        public ICommand SetDefaultFolder { get; }

        private bool CanSetDefaultFolderCommandExecute(object p) => SelectFolder != null;
        private void OnSetDefaultFolderCommandExecute(object p)
        {
            ObservableCollection<Folder> folder = Folders;
            Folder selectfolder = SelectFolder;

            foreach (Folder item in folder)
            {
                item.DefaultFolder = null;
            }
            selectfolder.DefaultFolder = Brushes.LightSkyBlue;
            DefaultFolder = selectfolder;
            Folders = null;
            SelectFolder = null;
            Folders = folder;
            SelectFolder = selectfolder;
            Data.Data.SaveData(Folders);
        }
        #endregion

        #region SaveProperties - Сохранение настроек
        /// <summary>Сохранение настроек</summary>
        public ICommand SaveProperties { get; }

        private bool CanSavePropertiesCommandExecute(object p) => true;
        private void OnSavePropertiesCommandExecute(object p)
        {
            Data.Data.SaveData(Folders);
        }
        #endregion

        #endregion


        public MainWindowViewModel()
        {
            #region Команды

            OpenFolderAddWindow = new LambdaCommand(OnOpenFolderAddWindowCommandExecute, CanOpenFolderAddWindowCommandExecute);

            OpenFolderEditorWindow = new LambdaCommand(OnOpenFolderEditorWindowCommandExecute, CanOpenFolderEditorWindowCommandExecute);

            DeletFolder = new LambdaCommand(OnDeletFolderCommandExecute, CanDeletFolderCommandExecute);

            SetDefaultFolder = new LambdaCommand(OnSetDefaultFolderCommandExecute, CanSetDefaultFolderCommandExecute);

            OpenSearchFolder = new LambdaCommand(OnOpenFolderCommandExecute, CanOpenFolderCommandExecute);

            OpenSearchDefaultFolder = new LambdaCommand(OnOpenSearchDefaultFolderCommandExecute, CanOpenSearchDefaultFolderCommandExecute);

            SaveProperties = new LambdaCommand(OnSavePropertiesCommandExecute, CanSavePropertiesCommandExecute);

            #endregion

            Data.Data.LoadData();

            Folders = new ObservableCollection<Folder>(Data.Data.Folders);

            foreach (Folder item in Folders)
            {
                if (item.DefaultFolder != null)
                {
                    DefaultFolder = item;
                }
            }
        }
    }
}
