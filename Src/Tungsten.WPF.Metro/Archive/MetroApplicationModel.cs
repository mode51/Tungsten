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

namespace W.WPF.Models
{
    /// <summary>
    /// Base class for a WPF application model
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class MetroApplicationModel<TModel> : W.WPF.Models.ApplicationModel<TModel> where TModel : MetroApplicationModel<TModel>, new()
    {
        //public Action<MetroApplicationModel<TModel>, EAccent, EAccent> AccentChanged { get; set; }
        //public Action<MetroApplicationModel<TModel>, ETheme, ETheme> ThemeChanged { get; set; }

        #region Accent/Theme Enumerations
        /// <summary>
        /// MahApps Accents
        /// </summary>
        //public static CollectionProperty<string> Accents { get; } = new CollectionProperty<string>(
        //    "Red",
        //    "Green",
        //    "Blue",
        //    "Purple",
        //    "Orange",
        //    "Lime",
        //    "Emerald",
        //    "Teal",
        //    "Cyan",
        //    "Cobalt",
        //    "Indigo",
        //    "Violet",
        //    "Pink",
        //    "Magenta",
        //    "Crimson",
        //    "Amber",
        //    "Yellow",
        //    "Brown",
        //    "Olive",
        //    "Steel",
        //    "Mauve",
        //    "Taupe",
        //    "Sienna"
        //);
        /// <summary>
        /// MahApps Themes
        /// </summary>
        //public static CollectionProperty<string> Themes = new CollectionProperty<string>(
        //    "BaseLight",
        //    "BaseDark"
        //);
        //public enum EAccent
        //{
        //    Red,
        //    Green,
        //    Blue,
        //    Purple,
        //    Orange,
        //    Lime,
        //    Emerald,
        //    Teal,
        //    Cyan,
        //    Cobalt,
        //    Indigo,
        //    Violet,
        //    Pink,
        //    Magenta,
        //    Crimson,
        //    Amber,
        //    Yellow,
        //    Brown,
        //    Olive,
        //    Steel,
        //    Mauve,
        //    Taupe,
        //    Sienna
        //}
        //public enum ETheme
        //{
        //    BaseLight,
        //    BaseDark
        //}
        #endregion

        #region Theme Support
        ///// <summary>
        ///// Get or set the current MahApps Accent
        ///// </summary>
        //public Property<ApplicationModel<TModel>, EAccent> CurrentAccent { get; } = new Property<ApplicationModel<TModel>, EAccent>(null, EAccent.Blue, (owner, oldValue, newValue) =>
        //{
        //    if (oldValue != newValue)
        //    {
        //        var vm = owner.As<MetroApplicationModel<TModel>>();
        //        vm.CurrentAccentName.Value = GetAccentName(newValue);
        //        vm.OnAccentChanged(oldValue, newValue);
        //        vm.AccentChanged?.Invoke(vm, oldValue, newValue);
        //    }
        //    //if (!string.IsNullOrEmpty(newValue))
        //    //{
        //    //    var vm = owner as MetroApplicationModel<TModel>;
        //    //    mah.ThemeManager.DetectAppStyle(Application.Current);
        //    //    mah.ThemeManager.ChangeAppStyle(Application.Current, mah.ThemeManager.GetAccent(newValue), mah.ThemeManager.GetAppTheme(vm.CurrentThemeName.Value));
        //    //}
        //});
        public static Property<ApplicationModel<TModel>, string> CurrentAccentName { get; } = new Property<ApplicationModel<TModel>, string>((owner, oldValue, newValue) =>
        {
            if (newValue != oldValue)
            {
                var vm = owner.As<MetroApplicationModel<TModel>>();
                //vm.CurrentAccent.Value = GetAccent(newValue);
                SetAccentAndTheme(newValue, vm.CurrentThemeName.Value);
            }
        });
        //public Property<ApplicationModel<TModel>, List<string>> AllAccents { get; } = new Property<ApplicationModel<TModel>, List<string>>(new List<string>());
        ///// <summary>
        ///// Get or set the current MahApps Theme
        ///// </summary>
        //public Property<ApplicationModel<TModel>, ETheme> CurrentTheme { get; } = new Property<ApplicationModel<TModel>, ETheme>(null, ETheme.BaseLight, (owner, oldValue, newValue) =>
        //{
        //    if (oldValue != newValue)
        //    {
        //        var vm = owner.As<MetroApplicationModel<TModel>>();
        //        vm.CurrentThemeName.Value = GetThemeName(newValue);
        //        vm.OnThemeChanged(oldValue, newValue);
        //        vm.ThemeChanged?.Invoke(vm, oldValue, newValue);
        //    }
        //    //var vm = owner as MetroApplicationModel<TModel>;
        //    //mah.ThemeManager.DetectAppStyle(Application.Current);
        //    //mah.ThemeManager.ChangeAppStyle(Application.Current, mah.ThemeManager.GetAccent(vm.CurrentAccent.Value), mah.ThemeManager.GetAppTheme(newValue));
        //});
        public Property<ApplicationModel<TModel>, string> CurrentThemeName { get; } = new Property<ApplicationModel<TModel>, string>((owner, oldValue, newValue) =>
        {
            if (newValue != oldValue)
            {
                var vm = owner.As<MetroApplicationModel<TModel>>();
                //vm.CurrentTheme.Value = GetTheme(newValue);
                SetAccentAndTheme(vm.CurrentAccentName.Value, newValue);
            }
        });
        //public Property<ApplicationModel<TModel>, List<string>> AllThemes { get; } = new Property<ApplicationModel<TModel>, List<string>>(new List<string>());
        #endregion

        /// <summary>
        /// Constructs a new ApplicationModel object
        /// </summary>
        public MetroApplicationModel() : base()
        {
            CurrentAccentName.Value = "Blue";
            CurrentThemeName.Value = "BaseLight";
        }
    }
}
