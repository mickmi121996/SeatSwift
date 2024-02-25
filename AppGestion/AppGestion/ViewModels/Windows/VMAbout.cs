using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppGestion.ViewModels.Windows
{
    public partial class VMAbout : ObservableObject
    {
        #region Properties

        /// <summary>
        /// Version of the application
        /// </summary>
        [ObservableProperty]
        private string? _version;

        /// <summary>
        /// Release date of the version
        /// </summary>
        [ObservableProperty]
        private DateOnly _releaseDate;

        /// <summary>
        /// Phone number of the company
        /// </summary>
        [ObservableProperty]
        private string _phoneNumber;

        /// <summary>
        /// Email of the company
        /// </summary>
        [ObservableProperty]
        private string _email;

        /// <summary>
        /// Adress of the company
        /// </summary>
        [ObservableProperty]
        private string _adress;

        #endregion



        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VMAbout()
        {
            // Get the version and set it
            var _version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (_version is not null)
            {
                this.Version = _version.ToString();
            }

            // Set the release date
            this._releaseDate = new DateOnly(2024, 03, 22);

            // Set the phone number
            this._phoneNumber = "(418) 944-1603";

            // Set the email
            this._email = "1336289@etu.cchic.ca";

            // Set the adress
            this._adress = "1910, route 170, Sagard, QC";
        }

        #endregion


        #region Command



        #endregion


        #region Method



        #endregion
    }
}
