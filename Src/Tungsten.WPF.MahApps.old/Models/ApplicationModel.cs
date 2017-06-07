using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using mah = MahApps.Metro;
using W;
using W.WPF.Core;

namespace W.WPF.MahApps.Models
{
    /// <summary>
    /// Base class for a WPF application model
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ApplicationModel<TModel> : W.WPF.Models.ApplicationModel<TModel> where TModel : ApplicationModel<TModel>, new()
    {
        #region Accent/Theme Enumerations
        /// <summary>
        /// MahApps Accents
        /// </summary>
        public static CollectionProperty<string> Accents { get; } = new CollectionProperty<string>(
            "Red",
            "Green",
            "Blue",
            "Purple",
            "Orange",
            "Lime",
            "Emerald",
            "Teal",
            "Cyan",
            "Cobalt",
            "Indigo",
            "Violet",
            "Pink",
            "Magenta",
            "Crimson",
            "Amber",
            "Yellow",
            "Brown",
            "Olive",
            "Steel",
            "Mauve",
            "Taupe",
            "Sienna"
        );
        /// <summary>
        /// MahApps Themes
        /// </summary>
        public static CollectionProperty<string> Themes = new CollectionProperty<string>(
            "BaseLight",
            "BaseDark"
        );
        #endregion

        #region Theme Support
        /// <summary>
        /// Get or set the current MahApps Accent
        /// </summary>
        public Property<ApplicationModel<TModel>, string> CurrentAccent { get; } = new Property<ApplicationModel<TModel>, string>((owner, oldValue, newValue) =>
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                mah.ThemeManager.DetectAppStyle(Application.Current);
                mah.ThemeManager.ChangeAppStyle(Application.Current, mah.ThemeManager.GetAccent(newValue), mah.ThemeManager.GetAppTheme(owner.CurrentTheme.Value));
            }
        });
        //public Property<ApplicationModel<TModel>, List<string>> AllAccents { get; } = new Property<ApplicationModel<TModel>, List<string>>(new List<string>());
        /// <summary>
        /// Get or set the current MahApps Theme
        /// </summary>
        public Property<ApplicationModel<TModel>, string> CurrentTheme { get; } = new Property<ApplicationModel<TModel>, string>((owner, oldValue, newValue) =>
        {
            mah.ThemeManager.DetectAppStyle(Application.Current);
            mah.ThemeManager.ChangeAppStyle(Application.Current, mah.ThemeManager.GetAccent(owner.CurrentAccent.Value), mah.ThemeManager.GetAppTheme(newValue));
        });
        //public Property<ApplicationModel<TModel>, List<string>> AllThemes { get; } = new Property<ApplicationModel<TModel>, List<string>>(new List<string>());
        #endregion

        /// <summary>
        /// Constructs a new ApplicationModel object
        /// </summary>
        public ApplicationModel() : base()
        {
            CurrentAccent.Value = Accents.Value[2];
            CurrentTheme.Value = Themes.Value[0];
        }
    }
}
