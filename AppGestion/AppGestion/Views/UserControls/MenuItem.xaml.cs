using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AppGestion.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MenuItem.xaml
    /// </summary>
    public partial class MenuItem : UserControl
    {
        public MenuItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The icon to display in the menu
        /// </summary>
        public PathGeometry Icon
        {
            get { return (PathGeometry)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary>
        /// The icon to display in the menu
        /// </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon",
            typeof(PathGeometry),
            typeof(MenuItem)
        );

        /// <summary>
        /// The width of the icon
        /// </summary>
        public int IconWidth
        {
            get { return (int)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        /// <summary>
        /// The width of the icon
        /// </summary>
        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(
            "IconWidth",
            typeof(int),
            typeof(MenuItem)
        );

        /// <summary>
        /// The brush for the Indicator
        /// </summary>
        public SolidColorBrush IndicatorBrush
        {
            get { return (SolidColorBrush)GetValue(IndicatorBrushProperty); }
            set { SetValue(IndicatorBrushProperty, value); }
        }

        /// <summary>
        /// The brush for the Indicator
        /// </summary>
        public static readonly DependencyProperty IndicatorBrushProperty =
            DependencyProperty.Register(
                "IndicatorBrush",
                typeof(SolidColorBrush),
                typeof(MenuItem)
            );

        /// <summary>
        /// The corner radius of the indicator
        /// </summary>
        public int IndicatorIndicatorCornerRadius
        {
            get { return (int)GetValue(IndicatorIndicatorCornerRadiusProperty); }
            set { SetValue(IndicatorIndicatorCornerRadiusProperty, value); }
        }

        /// <summary>
        /// The corner radius of the indicator
        /// </summary>
        public static readonly DependencyProperty IndicatorIndicatorCornerRadiusProperty =
            DependencyProperty.Register(
                "IndicatorIndicatorCornerRadius",
                typeof(int),
                typeof(MenuItem)
            );

        /// <summary>
        /// The display text of the menu item
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// The display text of the menu item
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(MenuItem)
        );

        /// <summary>
        /// the padding of the menu item
        /// </summary>
        public new Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        /// <summary>
        /// the padding of the menu item
        /// </summary>
        public static new readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            "Padding",
            typeof(Thickness),
            typeof(MenuItem)
        );

        /// <summary>
        /// Check if the menu item is selected
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Check if the menu item is selected
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected",
            typeof(bool),
            typeof(MenuItem)
        );

        /// <summary>
        /// The group name of the menu item
        /// </summary>
        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        /// <summary>
        /// The group name of the menu item
        /// </summary>
        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(
            "GroupName",
            typeof(string),
            typeof(MenuItem)
        );
    }
}
