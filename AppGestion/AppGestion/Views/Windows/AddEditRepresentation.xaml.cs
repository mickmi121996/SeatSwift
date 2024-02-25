using AppGestion.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AppGestion.Tools;
using System.Windows.Shapes;

namespace AppGestion.Views.Windows
{
    /// <summary>
    /// Interaction logic for AddEditRepresentation.xaml
    /// </summary>
    public partial class AddEditRepresentation : Window
    {
        public AddEditRepresentation()
        {
            InitializeComponent();
            VMAddEditRepresentation vMAddEditRepresentation = new VMAddEditRepresentation();
            this.DataContext = vMAddEditRepresentation;

            // Get the culture from the config file and set it
            string cultureFromConfig = CultureTools.GetConfiguredCulture();

            // Set the culture
            var customCulture = new System.Globalization.CultureInfo(cultureFromConfig);

            // Set the first day of the week to Sunday
            customCulture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Sunday;

            // Set the calendar to Gregorian
            calendar.Language = XmlLanguage.GetLanguage(customCulture.IetfLanguageTag);

            // Set the culture
            Thread.CurrentThread.CurrentCulture = customCulture;
            Thread.CurrentThread.CurrentUICulture = customCulture;
        }

        #region Button Close | Restore | Minimize 

        /// <summary>
        /// Close the application
        /// </summary>
        private void AlertColor_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Drag the window
        /// </summary>
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        #endregion
    }
}
