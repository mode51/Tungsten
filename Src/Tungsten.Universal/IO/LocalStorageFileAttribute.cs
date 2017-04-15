using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib.Reflection;

namespace lib.IO
{
    public partial class LocalStorage
    {
        [Newtonsoft.Json.JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)]
        public class LocalStorageFileAttribute : Attribute
        {
            public string Filename { get; set; }
            public LocalStorageFileAttribute() { }
            public LocalStorageFileAttribute(string filename = "")
            {
                if (string.IsNullOrEmpty(filename))
                    Filename = System.Windows.Forms.Application.ProductName + ".settings";
                else
                    Filename = filename;
            }
            public LocalStorageFileAttribute(System.Environment.SpecialFolder folder, string filename = "")
            {
                if (string.IsNullOrEmpty(filename))
                    Filename = System.IO.Path.Combine(System.Environment.GetFolderPath(folder), System.Windows.Forms.Application.ProductName + ".settings");
                else
                    Filename = filename;
            }
        }
    }
}
