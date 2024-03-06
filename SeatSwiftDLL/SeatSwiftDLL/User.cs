using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeatSwiftDLL.Enums;

namespace SeatSwiftDLL
{
    public class User : ICloneable
    {
        # region Properties

        /// <summary>
        /// The Id of the user in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// If the user is active in the database
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The first name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The type of the user
        /// </summary>
        public EmployeeType Type { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The employee number of the user
        /// </summary>
        public string EmployeeNumber { get; set; }

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The phone number of the user
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The password Hash of the user
        /// </summary>  
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// The password Salt of the user
        /// </summary>
        public byte[] PasswordSalt { get; set; }

        # endregion


        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>
        public User()
        {
            Id = default;
            IsActive = true;
            FirstName = string.Empty;
            LastName = string.Empty;
            Type = default;
            Email = string.Empty;
            EmployeeNumber = string.Empty;
            PhoneNumber = string.Empty;
            PasswordHash = Array.Empty<byte>();
            PasswordSalt = Array.Empty<byte>();
        }

        /// <summary>
        /// The constructor of the user
        /// </summary>
        /// <param name="id">The Id of the user in the database</param>
        /// <param name="isActive">If the user is active in the database</param>
        /// <param name="firstName">The first name of the user</param>
        /// <param name="lastName">The last name of the user</param>
        /// <param name="email">The email of the user</param>
        /// <param name="phoneNumber">The phone number of the user</param>
        /// <param name="passwordHash">The password Hash of the user</param>
        /// <param name="passwordSalt">The password Salt of the user</param>
        public User(int id, bool isActive, string firstName, string lastName, string employeeNumber, EmployeeType type,string email, string phoneNumber)
        {
            this.Id = id;
            this.IsActive = isActive;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EmployeeNumber = employeeNumber;
            this.Type = type;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.PasswordHash = Array.Empty<byte>();
            this.PasswordSalt = Array.Empty<byte>();
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
