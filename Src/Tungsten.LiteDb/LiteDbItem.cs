using System.Windows;

namespace W.LiteDb
{
    /// <summary>
    /// Used by the Save methods in LiteDbMethods
    /// </summary>
    public interface ILiteDbItem
    {
        /// <summary>
        /// The unique Id for this LiteDb collection item
        /// </summary>
        int _id { get; set; }
    }

    /// <summary>
    /// The base class for POCO class which translates to a LiteDb collection
    /// </summary>
    public class LiteDbItem : ILiteDbItem //: DependencyObject
    {
        //[Newtonsoft.Json.JsonIgnore]
        //public static LiteDbInformationAttribute LiteDbInformation = null; //public for extension methods

        /// <summary>
        /// The unique Id for this LiteDb collection item
        /// </summary>
        public int _id { get; set; }

        /// <summary>
        /// Constructs a LiteDbItem
        /// </summary>
        public LiteDbItem()
        {
            //if (LiteDbInformation == null)
            //    LiteDbInformation = this.GetAttribute<LiteDbInformationAttribute>();
        }
    }
}