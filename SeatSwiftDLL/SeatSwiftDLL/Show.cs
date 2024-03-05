namespace SeatSwiftDLL
{
    /// <summary>
    /// The show class
    /// </summary>
    /// <remarks>
    /// This class is used to store the show data
    /// </remarks>
    public class Show : ICloneable
    {
        # region Properties

        /// <summary>
        /// The Id of the show in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// If the show is active in the database
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The name of the show
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The artist of the show
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// The description of the show
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The type of the show    
        /// </summary>
        public ShowType ShowType { get; set; }

        /// <summary>
        /// The show status
        /// </summary>
        public ShowStatus ShowStatus { get; set; }

        /// <summary>
        /// The Image url of the show
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The number of tickets max by client
        /// </summary>
        public int MaxTicketsByClient { get; set; }

        /// <summary>
        /// The base price of the show
        /// </summary>
        public double BasePrice { get; set; }

        /// <summary>
        /// The user who created the show
        /// </summary>
        public User User { get; set; }
        

        # endregion


        # constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public Show()
        {
            Id = default;
            IsActive = true;
            Name = string.empty;
            Artist = string.empty;
            Description = string.empty;
            ShowType = default;
            ShowStatus = default;
            ImageUrl = string.empty;
            MaxTicketsByClient = default;
            BasePrice = default;
            User = new User();
        }

        /// <summary>
        /// The constructor with parameters
        /// </summary>
        /// <param name="id">The Id of the show in the database</param>
        /// <param name="isActive">If the show is active in the database</param>
        /// <param name="name">The name of the show</param>
        /// <param name="artist">The artist of the show</param>
        /// <param name="description">The description of the show</param>
        /// <param name="showType">The type of the show</param>
        /// <param name="showStatus">The status of the show</param>
        /// <param name="imageUrl">The image url of the show</param>
        /// <param name="maxTicketsByClient">The number of tickets max by client</param>
        /// <param name="basePrice">The base price of the show</param>
        /// <param name="user">The user who created the show</param>
        public Show(int id, bool isActive, string name, string artist, string description, ShowType showType, ShowStatus showStatus, string imageUrl, int maxTicketsByClient, double basePrice, User user)
        {
            this.Id = id;
            this.IsActive = isActive;
            this.Name = name;
            this.Artist = artist;
            this.Description = description;
            this.ShowType = showType;
            this.ShowStatus = showStatus;
            this.ImageUrl = imageUrl;
            this.MaxTicketsByClient = maxTicketsByClient;
            this.BasePrice = basePrice;
            this.User = user;
        }

        #endregion


        # region Interface methods

        public object Clone()
        {
            throw new NotImplementedException();
        }

        # endregion
    }
}