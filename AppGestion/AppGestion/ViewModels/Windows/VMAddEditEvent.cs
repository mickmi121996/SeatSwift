using AppGestion.DataAccessLayer;
using AppGestion.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AppGestion.ViewModels.Windows
{
    public partial class VMAddEditEvent : ObservableObject
    {
        #region properties

        /// <summary>
        /// The is modified property
        /// </summary>
        [ObservableProperty]
        private bool _isModified;

        /// <summary>
        /// The title
        /// </summary>
        [ObservableProperty]
        private string? _title;

        /// <summary>
        /// The title of the window
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _name;

        /// <summary>
        /// The artist of the show
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _artist;

        /// <summary>
        /// The base price of the show
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private decimal _basePrice;

        /// <summary>
        /// The max tickets by client
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private int _maxTicketsByClient;

        /// <summary>
        /// The selected image path
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _selectedImagePath;

        /// <summary>
        /// The event description
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _eventDescription;

        /// <summary>
        /// The list of show type
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _showTypes;

        /// <summary>
        /// The selected show type
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private string _selectedShowType;

        /// <summary>
        /// The show
        /// </summary>
        [ObservableProperty]
        private Show _show;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor for Add
        /// </summary>
        public VMAddEditEvent()
        {
            _name = string.Empty;
            _artist = string.Empty;
            _basePrice = 0;
            _maxTicketsByClient = 0;
            _selectedImagePath = string.Empty;
            _title = "Ajouter un événement";
            _eventDescription = string.Empty;
            _showTypes = new ObservableCollection<string>(Enum.GetNames(typeof(ShowType)));
            _selectedShowType = _showTypes.FirstOrDefault();
            _isModified = false;
            _show = new Show();
        }

        /// <summary>
        /// Constructor for Edit
        /// </summary>
        /// <param name="show">The show to edit</param>
        public VMAddEditEvent(Show show)
        {
            _name = show.Name;
            _artist = show.Artist;
            _basePrice = show.BasePrice;
            _maxTicketsByClient = show.MaxTicketsByClient;
            _selectedImagePath = show.ImageUrl;
            _eventDescription = show.Description;
            _showTypes = new ObservableCollection<string>(Enum.GetNames(typeof(ShowType)));
            _selectedShowType = show.ShowType.ToString();
            _isModified = true;
            if (show is not null)
            {
                _show = show;
                _title = $"Modifier {Show.Name}";
            }
        }

        #endregion


        #region commands

        [RelayCommand]
        private async Task BrowseImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Images files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*",
                Title = "Select an event image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedImagePath = openFileDialog.FileName;
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Command to confirm the event
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanConfirm))]
        private async Task Confirm()
        {
            try
            {
                if (IsModified)
                {
                    if (Show is not null)
                    {
                        // Edit the show
                        Show.Name = Name;
                        Show.Artist = Artist;
                        Show.BasePrice = BasePrice;
                        Show.MaxTicketsByClient = MaxTicketsByClient;
                        Show.ImageUrl = SelectedImagePath;
                        Show.Description = EventDescription;
                        Show.ShowType = (ShowType)Enum.Parse(typeof(ShowType), SelectedShowType);
                        await DAL.ShowFactory.UpdateAsync(Show);

                        Application.Current.Windows
                            .OfType<AddEditEvent>()
                            .FirstOrDefault()
                            ?.Close();
                    }
                    else
                    {
                        // pop a message box
                        System.Windows.MessageBox.Show(
                            "The show is null",
                            "Error",
                            System.Windows.MessageBoxButton.OK,
                            System.Windows.MessageBoxImage.Error
                        );
                    }
                }
                else
                {
                    // Add the show
                    Show = new Show
                    {
                        Name = Name,
                        Artist = Artist,
                        BasePrice = BasePrice,
                        MaxTicketsByClient = MaxTicketsByClient,
                        ImageUrl = SelectedImagePath,
                        Description = EventDescription,
                        ShowType = (ShowType)Enum.Parse(typeof(ShowType), SelectedShowType),
                        User = VMMainWindow.Instance.User
                    };
                    await DAL.ShowFactory.CreateAsync(Show);

                    Application.Current.Windows.OfType<AddEditEvent>().FirstOrDefault()?.Close();
                }
            }
            catch (Exception ex)
            {
                // Pop a message box with the exception message
                System.Windows.MessageBox.Show(
                    ex.Message,
                    "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error
                );
            }
        }

        /// <summary>
        /// Command to check if the confirm command can be executed
        /// </summary>
        /// <returns>True if the command can be executed, otherwise false</returns>
        private bool CanConfirm()
        {
            if (
                string.IsNullOrWhiteSpace(Name)
                || string.IsNullOrWhiteSpace(Artist)
                || string.IsNullOrWhiteSpace(SelectedShowType)
                || string.IsNullOrWhiteSpace(SelectedImagePath)
                || string.IsNullOrWhiteSpace(EventDescription)
                || BasePrice <= 0
                || MaxTicketsByClient <= 0
                || string.IsNullOrWhiteSpace(SelectedShowType)
            )
            {
                return false;
            }

            return true;
        }

        #endregion


        #region methods



        #endregion
    }
}
