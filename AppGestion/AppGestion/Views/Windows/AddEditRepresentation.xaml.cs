﻿using AppGestion.Tools;
using AppGestion.ViewModels.Windows;
using SeatSwiftDLL;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace AppGestion.Views.Windows
{
    /// <summary>
    /// Interaction logic for AddEditRepresentation.xaml
    /// </summary>
    public partial class AddEditRepresentation : Window
    {
        public AddEditRepresentation(Show show)
        {
            InitializeComponent();
            VMAddEditRepresentation vMAddEditRepresentation = new VMAddEditRepresentation(show);
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
