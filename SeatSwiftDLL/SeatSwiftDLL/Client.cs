using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL
{
    /// <summary>
    /// The client class
    /// </summary>
    /// <remarks>
    /// This class is used to store the client data
    /// </remarks>
    public class Client : ICloneable
    {
        # region Properties

        /// <summary>
        /// The Id of the client in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// If the client is active in the database
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The first name of the client
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the client
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The email of the client
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The password Hash of the client
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// The password Salt of the client
        /// </summary>
        public byte[] PasswordSalt { get; set; }

        /// <summary>
        /// The nullable phone number of the client
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// The nullable city of the client
        /// </summary>
        public string? City { get; set; }

        # endregion


        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public Client()
        {
            Id = default;
            IsActive = true;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PasswordHash = Array.Empty<byte>();
            PasswordSalt = Array.Empty<byte>();
            PhoneNumber = string.Empty;
            City = string.Empty;
        }

        /// <summary>
        /// The constructor with parameters
        /// </summary>
        /// <param name="id">The Id of the client in the database</param>
        /// <param name="isActive">If the client is active in the database</param>
        /// <param name="firstName">The first name of the client</param>
        /// <param name="lastName">The last name of the client</param>
        /// <param name="email">The email of the client</param>
        /// <param name="phoneNumber">The nullable phone number of the client</param>
        /// <param name="city">The nullable city of the client</param>
        public Client(int id, bool isActive, string firstName, string lastName, string email, string? phoneNumber, string? city)
        {
            this.Id = id;
            this.IsActive = isActive;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.City = city;
            this.PasswordHash = Array.Empty<byte>();
            this.PasswordSalt = Array.Empty<byte>();
        }

        #endregion


        # region Interface methods

        public object Clone()
        {
            return new Client
            {
                Id = this.Id,
                IsActive = this.IsActive,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                PasswordHash = this.PasswordHash,
                PasswordSalt = this.PasswordSalt,
                PhoneNumber = this.PhoneNumber,
                City = this.City
            };
        }

        # endregion
    }
}
