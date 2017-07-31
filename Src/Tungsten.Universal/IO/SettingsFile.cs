using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W.Logging;

namespace W.IO
{
    public class SettingsProperties
    {
        
    }
    public class SettingsFile<T> : W.IO.LocalStorage.LocalStorageFile where T : SettingsFile<T>, new()
    {
        public static Property<string> SettingsFilenameAndPath { get; } = new Property<string>(System.IO.Path.ChangeExtension(System.Environment.GetCommandLineArgs()[0], ".settings.json"));

        public new bool Load()
        {
            return Load(SettingsFilenameAndPath.Value);
        }

        public new bool Save()
        {
            return Save(SettingsFilenameAndPath.Value);
        }
        public SettingsFile()
        {
        }
        ~SettingsFile()
        {
            try
            {
                Save();
            }
            catch (Exception e)
            {
                Log.e(e);
            }
        }
    }
}
