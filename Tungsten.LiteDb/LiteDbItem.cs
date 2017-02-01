using System.Windows;

namespace W.LiteDb
{
    public interface ILiteDbItem
    {
        int _id { get; set; }
    }
    public class LiteDbItem : ILiteDbItem //: DependencyObject
    {
        //[Newtonsoft.Json.JsonIgnore]
        //public static LiteDbInformationAttribute LiteDbInformation = null; //public for extension methods

        public int _id { get; set; }

        public LiteDbItem()
        {
            //if (LiteDbInformation == null)
            //    LiteDbInformation = this.GetAttribute<LiteDbInformationAttribute>();
        }
    }
}