using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL
{
    /// <summary>
    /// The order class
    /// </summary>
    /// <remarks>
    /// This class is used to store the order data
    /// </remarks>
    public class Order : ICloneable
    {
        # region Properties

        /// <summary>
        /// The Id of the order in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// If the order is active in the database
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The number of the order
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// The date of the order
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// The total price of the order
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// The client of the order
        /// </summary>
        public Client? Client { get; set; }
        

        # endregion


        #region Constructor

        /// <summary>
        /// The default constructor of the order
        /// </summary>
        public Order()
        {
            Id = default;
            IsActive = true;
            OrderNumber = string.Empty;
            OrderDate = DateTime.Now;
            TotalPrice = default;
            Client = null;
        }

        /// <summary>
        /// The constructor of the order
        /// </summary>
        /// <param name="id">The Id of the order in the database</param>
        /// <param name="isActive">If the order is active in the database</param>
        /// <param name="orderNumber">The number of the order</param>
        /// <param name="orderDate">The date of the order</param>
        /// <param name="totalPrice">The total price of the order</param>
        /// <param name="client">The client of the order</param>
        public Order(int id, bool isActive, string orderNumber, DateTime orderDate, decimal totalPrice, Client client)
        {
            if(client == null) { throw new ArgumentNullException(nameof(client)); }

            this.Id = id;
            this.IsActive = isActive;
            this.OrderNumber = orderNumber;
            this.OrderDate = orderDate;
            this.TotalPrice = totalPrice;
            this.Client = client;
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
