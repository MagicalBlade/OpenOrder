using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace OpenOrder.Models
{
    internal class Folder
    {
        public string Name { get; set; }

        public string Dir { get; set; }

        public Brush DefaultFolder { get; set; }

        internal string Open(string SearchForder)
        {
            #region Проверки на допустимые символы и т.п. а так же открытие папки если ячейка с заказом пустая.
            if (!Directory.Exists(Dir))
            {
                return "Проверьте правильность пути к папке.";
            }

            if (SearchForder == "" || SearchForder == null)
            {
                Process.Start(Dir);
                return "";
            }
            if (SearchForder.Intersect("\\/:*?<>|\"").Any())
            {
                return "Не допустимые символы в названии заказа.";
            }
            if (SearchForder.Length < 4)
            {
                return "Номер заказа должен состоять из минимум 4 цифр.";
            }
            #endregion

            string[] dirs = Directory.GetDirectories(Dir, "*" + "З.з.№" + SearchForder + ". " + "*");

            if (dirs.Length == 0)
            {
                return "Заказ не найден.";
            }

            foreach (var dir_item in dirs)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = dir_item
                };
                Process.Start(processStartInfo);
            }
            return "";
        }
    }
}
