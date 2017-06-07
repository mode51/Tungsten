using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using W.WPF.Core;

namespace W.WPF
{
    public class MetroStyle
    {
        private Window _window;
        public Property<MetroStyle, string> Accent { get; } = new Property<MetroStyle, string>((owner, oldValue, newValue) =>
        {
            SetAccentAndTheme(newValue, owner.Theme.Value, owner._window);
        });
        public Property<MetroStyle, string> Theme { get; } = new Property<MetroStyle, string>((owner, oldValue, newValue) =>
        {
            SetAccentAndTheme(owner.Accent.Value, newValue, owner._window);
        });

        public static CollectionProperty<string> AllAccents { get; } = new CollectionProperty<string>();
        public static CollectionProperty<string> AllThemes { get; } = new CollectionProperty<string>();
        public static void SetApplicationAccentAndTheme(string accent, string theme)
        {
            SetAccentAndTheme(accent, theme, null);
        }
        public static void SetAccentAndTheme(string accent, string theme, Window window)
        {
            if (window != null)
            {
                MahApps.Metro.ThemeManager.DetectAppStyle(window);
                MahApps.Metro.ThemeManager.ChangeAppStyle(window, MahApps.Metro.ThemeManager.GetAccent(accent), MahApps.Metro.ThemeManager.GetAppTheme(theme));
            }
            else
            {
                MahApps.Metro.ThemeManager.DetectAppStyle(Application.Current);
                MahApps.Metro.ThemeManager.ChangeAppStyle(Application.Current, MahApps.Metro.ThemeManager.GetAccent(accent), MahApps.Metro.ThemeManager.GetAppTheme(theme));
            }
        }

        static MetroStyle()
        {
            foreach (var accent in MahApps.Metro.ThemeManager.Accents.OrderBy(a => a.Name))
                AllAccents.Value.Add(accent.Name);
            foreach (var Theme in MahApps.Metro.ThemeManager.AppThemes)
                AllThemes.Value.Add(Theme.Name);
        }

        public MetroStyle(string defaultAccent = "Blue", string defaultTheme = "BaseLight") : this(null, defaultAccent, defaultTheme) { }
        public MetroStyle(Window window, string defaultAccent = "Blue", string defaultTheme = "BaseLight")
        {
            this.InitializeProperties();
            _window = window;
            Accent.LoadValue(defaultAccent);
            Theme.LoadValue(defaultTheme);
            SetAccentAndTheme(defaultAccent, defaultTheme, _window);
        }
    }
}
