using OpenOrder.Infrastructure.Commands;
using OpenOrder.Models;
using OpenOrder.ViewModels.Base;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OpenOrder.ViewModels
{
    class FolderEditorWindowViewModel : ViewModel
    {

        #region Название кнопки подтверждения
        /// <summary>Название кнопки подтверждения</summary>
        private string _ButtonNameOK;

        /// <summary>Название кнопки подтверждения</summary>
        public string ButtonNameOK
        {
            get => _ButtonNameOK;
            set => Set(ref _ButtonNameOK, value);
        }
        #endregion

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

        #region Имя папки
        /// <summary>Имя папки</summary>
        private string _Name;

        /// <summary>Имя папки</summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }
        #endregion

        #region Путь к папке
        /// <summary>Путь к папке</summary>
        private string _Dir;

        /// <summary>Путь к папке</summary>
        public string Dir
        {
            get => _Dir;
            set => Set(ref _Dir, value);
        }
        #endregion

        #region Папка по усмолчанию
        /// <summary>Папка по усмолчанию</summary>
        private Brush _DefaultFolder;

        /// <summary>Папка по усмолчанию</summary>
        public Brush DefaultFolder
        {
            get => _DefaultFolder;
            set => Set(ref _DefaultFolder, value);
        }
        #endregion

        #region Команды

        #region AddFolder - Добавление папки и редактирование папки
        /// <summary>Добавление папки и редактирование папки</summary>
        public ICommand AddFolder { get; }

        private bool CanAddFolderCommandExecute(object p) => Name != null && Dir != null && Name != "" && Dir != "";
        private void OnAddFolderCommandExecute(object p)
        {
            Folder folder = new Folder
            {
                Name = Name,
                Dir = Dir
            };
            Folders.Add(folder);
        }

        /// <summary>Редактирование папки</summary>
        private void OnEditFolderCommandExecute(object p)
        {
            int index = Folders.IndexOf(SelectFolder);
            Folders.Remove(SelectFolder);
            Folder folder = new Folder { Name = Name, Dir = Dir, DefaultFolder = DefaultFolder };
            Folders.Insert(index, folder);
            Close(p);
        }
        #endregion

        #region CloseApplication - Закрыть окно
        /// <summary>Закрыть окно</summary>
        public ICommand CloseApplication { get; }

        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecute(object p)
        {
            Close(p);
        }
        #endregion

        #endregion

        public void Close(object p)
        {
            Data.Data.SaveData(Folders);
            Window win = p as Window;
            win.Close();
        }
        public FolderEditorWindowViewModel(ObservableCollection<Folder> folders)
        {
            #region Команды

            AddFolder = new LambdaCommand(OnAddFolderCommandExecute, CanAddFolderCommandExecute);

            CloseApplication = new LambdaCommand(OnCloseApplicationCommandExecute, CanCloseApplicationCommandExecute);

            #endregion
            Folders = folders;
            ButtonNameOK = "Добавить";
        }
        public FolderEditorWindowViewModel(Folder selectfolder, ObservableCollection<Folder> folders)
        {
            #region Команды

            AddFolder = new LambdaCommand(OnEditFolderCommandExecute, CanAddFolderCommandExecute);

            CloseApplication = new LambdaCommand(OnCloseApplicationCommandExecute, CanCloseApplicationCommandExecute);

            #endregion

            Folders = folders;
            SelectFolder = selectfolder;
            Name = selectfolder.Name;
            Dir = selectfolder.Dir;
            DefaultFolder = selectfolder.DefaultFolder;
            ButtonNameOK = "Применить";
        }
    }
}