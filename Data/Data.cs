using Newtonsoft.Json;
using OpenOrder.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenOrder.Data
{
    static class Data
    {
        internal static void SaveData(ObservableCollection<Folder> folder)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(@"folders.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, folder);
            }
        }

        internal static void LoadData()
        {
            if (File.Exists("folders.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StreamReader sw = new StreamReader(@"folders.json"))
                using (JsonReader writer = new JsonTextReader(sw))
                {
                    Folders = serializer.Deserialize<List<Folder>>(writer);
                }
            }
            else
            {
                Folders = new List<Folder>() { new Folder { Dir = "123", Name = "asd" } };
            }
        }

        private static List<Folder> _Folders;

        internal static List<Folder> Folders
        {
            get => _Folders;
            set => _Folders = value;
        }
    }
}
