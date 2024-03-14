using GuichetAutonome.Tools;
using MySql.Data.MySqlClient;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuichetAutonome.DataAccessLayer.Factories
{
    /// <summary>
    /// Factory class for order
    /// </summary>
    /// <remarks>
    /// This class is a factory for the order class. It is used
    /// to create order objects from data read from a database.
    /// </remarks>
    public class OrderFactory : Base.FactoryBase
    {
        #region Create methods

        /// <summary>
        /// Create order from data reader
        /// </summary>
        /// <param name="dataReader">Data reader</param>
        /// <returns>Order</returns>
        public async Task<Order> CreateFromReaderAsync(MySqlDataReader dataReader)
        {
            int id = dataReader.GetInt32("Id");
            bool isActive = dataReader.GetBoolean("IsActive");
            DateTime orderDate = dataReader.GetDateTime("OrderDate");
            string orderNumber = dataReader.GetString("OrderNumber");
            int clientId = dataReader.GetInt32("ClientId");
            decimal totalPrice = dataReader.GetDecimal("TotalPrice");

            // Get the user with the specified id
            Client client = await new ClientFactory().GetByIdAsync(clientId);
            if (client is null)
            {
                throw new Exception("The client with the specified id does not exist");
            }

            // Create the order
            return new Order(id, isActive, orderNumber, orderDate, totalPrice, client);
        }

        /// <summary>
        /// Create order from a data row
        /// </summary>
        /// <param name="dataRow">Data row</param>
        /// <returns>Order</returns>
        public async Task<Order> CreateFromRowAsync(DataRow dataRow)
        {
            int id = dataRow.Field<int>("Id");
            bool isActive = dataRow.Field<bool>("IsActive");
            DateTime orderDate = dataRow.Field<DateTime>("OrderDate");
            string orderNumber = dataRow.Field<string>("OrderNumber")
                ?? throw new Exception("The order number is null");
            int clientId = dataRow.Field<int>("ClientId");
            decimal totalPrice = dataRow.Field<decimal>("TotalPrice");

            // Get the user with the specified id
            Client client = await new ClientFactory().GetByIdAsync(clientId);
            if (client is null)
            {
                throw new Exception("The client with the specified id does not exist");
            }

            // Create the order
            return new Order(id, isActive, orderNumber, orderDate, totalPrice, client);
        }

        #endregion


        #region Factory methods

        /// <summary>
        /// Get order by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Order</returns>
        public async Task<Order> GetByIdAsync(int id)
        {
            try
            {
                // Get the show with the given id
                using (
                    DataTable result = await DataBaseTools.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM orders WHERE Id = @id;",
                    new MySqlParameter("@id", id)
                    )
                )
                {
                    // If no show is found, throw an exception
                    if (result.Rows.Count == 0)
                    {
                        throw new KeyNotFoundException("No show with the id " + id + " was found");
                    }

                    // Create the Show object
                    return await CreateFromRowAsync(result.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the show with the given id", ex);
            }
        }

        /// <summary>
        /// Get all active orders
        /// </summary>
        /// <returns>Orders</returns>
        public async Task<List<Order>> GetAllAsync()
        {
            try
            {
                // Get all shows
                using (
                    DataTable result = await DataBaseTools.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM orders WHERE IsActive = 1;")
                )
                {
                    // Create the list of shows
                    List<Order> orders = new List<Order>();

                    // Create the shows
                    foreach (DataRow row in result.Rows)
                    {
                        orders.Add(await CreateFromRowAsync(row));
                    }

                    return orders;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting all shows", ex);
            }
        }

        /// <summary>
        /// Get by order number
        /// </summary>
        /// <param name="orderNumber">Order number</param>
        /// <returns>Order</returns>
        /// <exception cref="KeyNotFoundException">If no order with the specified order number is found</exception>
        /// <exception cref="Exception">If an error occurred while getting the order</exception>
        public async Task<Order> GetByOrderNumberAsync(string orderNumber)
        {
            try
            {
                // Get the show with the given id
                using (
                    DataTable result = await DataBaseTools.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM orders WHERE OrderNumber = @orderNumber;",
                    new MySqlParameter("@orderNumber", orderNumber)
                    )
                )
                {
                    // If no show is found, throw an exception
                    if (result.Rows.Count == 0)
                    {
                        throw new KeyNotFoundException("No order with the order number " + orderNumber + " was found");
                    }

                    // Create the Show object
                    return await CreateFromRowAsync(result.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the order with the given order number", ex);
            }
        }

        /// <summary>
        /// Get all active orders of a client
        /// </summary>
        /// <param name="clientId">Client id</param>
        /// <returns>Orders</returns>
        /// <exception cref="KeyNotFoundException">If no client with the specified id is found</exception>
        /// <exception cref="Exception">If an error occurred while getting the orders</exception>
        public async Task<List<Order>> GetByClientIdAsync(int clientId)
        {
            try
            {
                // Get the client with the given id
                Client client = await new ClientFactory().GetByIdAsync(clientId);
                if (client is null)
                {
                    throw new KeyNotFoundException("No client with the id " + clientId + " was found");
                }

                // Get all shows
                using (
                    DataTable result = await DataBaseTools.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM orders WHERE IsActive = 1 AND ClientId = @clientId;",
                    new MySqlParameter("@clientId", clientId)
                    )
                )
                {
                    // Create the list of shows
                    List<Order> orders = new List<Order>();

                    // Create the shows
                    foreach (DataRow row in result.Rows)
                    {
                        orders.Add(await CreateFromRowAsync(row));
                    }

                    return orders;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the orders of the client with the given id", ex);
            }
        }

        /// <summary>
        /// Get all active orders for a date
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>Orders</returns>
        /// <exception cref="Exception">If an error occurred while getting the orders</exception>
        /// <exception cref="KeyNotFoundException">If no order is found for the specified date</exception>
        public async Task<List<Order>> GetByDateAsync(DateTime date)
        {
            try
            {
                // Get all shows
                using (
                    DataTable result = await DataBaseTools.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM orders WHERE IsActive = 1 AND OrderDate = @date;",
                    new MySqlParameter("@date", date)
                    )
                )
                {
                    // Create the list of shows
                    List<Order> orders = new List<Order>();

                    // Create the shows
                    foreach (DataRow row in result.Rows)
                    {
                        orders.Add(await CreateFromRowAsync(row));
                    }

                    return orders;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the orders of the client with the given id", ex);
            }
        }

        /// <summary>
        /// Get all active orders for a mount and year
        /// </summary>
        /// <param name="month">Month</param>
        /// <param name="year">Year</param>
        /// <returns>Orders</returns>
        /// <exception cref="Exception">If an error occurred while getting the orders</exception>
        /// <exception cref="KeyNotFoundException">If no order is found for the specified month and year</exception>
        public async Task<List<Order>> GetByMonthAndYearAsync(int month, int year)
        {
            try
            {
                // Get all shows
                using (
                    DataTable result = await DataBaseTools.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM orders WHERE IsActive = 1 AND MONTH(OrderDate) = @month AND YEAR(OrderDate) = @year;",
                    new MySqlParameter("@month", month),
                    new MySqlParameter("@year", year)
                    )
                )
                {
                    // Create the list of shows
                    List<Order> orders = new List<Order>();

                    // Create the shows
                    foreach (DataRow row in result.Rows)
                    {
                        orders.Add(await CreateFromRowAsync(row));
                    }

                    return orders;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the orders of the client with the given id", ex);
            }
        }

        /// <summary>
        /// Create a new order for a client
        /// </summary>
        /// <param name="order">Order</param>
        /// <exception cref="Exception">If an error occurred while creating the order</exception>
        /// <exception cref="ArgumentNullException">If the order is null</exception>
        public async Task CreateAsync(Order order)
        {
            // Check if the order is null
            if (order is null)
            {
                throw new ArgumentNullException("The order is null");
            }

            // Check if the client exists
            if (order.Client is null)
            {
                throw new ArgumentNullException("The client is null");
            }

            try
            {
                // Create the order
                await DataBaseTools.ExecuteNonQueryAsync
                (this.ConnectionString,
                "INSERT INTO orders (IsActive, OrderDate, OrderNumber, ClientId, TotalPrice) VALUES (@isActive, @orderDate, @orderNumber, @clientId, @totalPrice);",
                new MySqlParameter("@isActive", order.IsActive),
                new MySqlParameter("@orderDate", order.OrderDate),
                new MySqlParameter("@orderNumber", order.OrderNumber),
                new MySqlParameter("@clientId", order.Client.Id),
                new MySqlParameter("@totalPrice", order.TotalPrice)
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the order", ex);
            }
        }

        /// <summary>
        /// Get the count of tickets sold for each show type
        /// </summary>
        /// <returns>List of tuples containing show type (as enum) and count</returns>
        /// <exception cref="Exception">If an error occurred while getting the count</exception>
        public async Task<List<Tuple<ShowType, int>>> GetCountByShowTypeAsync()
        {
            try
            {
                var countsByShowType = new List<Tuple<ShowType, int>>();

                string query = @"
                SELECT s.ShowType, COUNT(t.Id) AS TicketsSold
                FROM shows s
                JOIN representation r ON s.Id = r.ShowId
                JOIN ticket t ON r.Id = t.RepresentationId
                WHERE t.IsActive = 1 AND t.TicketStatus = 'Purchased'
                GROUP BY s.ShowType;
            "
                ;

                using (DataTable result = await DataBaseTools.GetDataTableFromQueryAsync(this.ConnectionString, query))
                {
                    foreach (DataRow row in result.Rows)
                    {
                        ShowType showType;
                        if (Enum.TryParse(row["ShowType"].ToString(), out showType))
                        {
                            var count = Convert.ToInt32(row["TicketsSold"]);
                            countsByShowType.Add(Tuple.Create(showType, count));
                        }
                    }
                }

                if (countsByShowType.Count == 0)
                {
                    // Handle the case where no tickets are found
                    throw new KeyNotFoundException("No tickets were found for any show type");
                }

                return countsByShowType;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the counts of tickets for show types", ex);
            }
        }

        /// <summary>
        /// Create a tuple list of mount and sell for the current year
        /// </summary>
        /// <returns>List of tuples containing month and sell if 0 sell, return the mount and 0</returns>
        /// <exception cref="Exception">If an error occurred while getting the sell</exception>
        /// <exception cref="KeyNotFoundException">If no sell is found for the current year</exception>
        public async Task<List<Tuple<string, int>>> GetSellByMonthAsync()
        {
            try
            {
                var sellByMonth = new List<Tuple<string, int>>();

                string query = @"
                SELECT MONTH(OrderDate) AS Month, SUM(TotalPrice) AS Sell
                FROM orders
                WHERE YEAR(OrderDate) = YEAR(CURDATE())
                GROUP BY MONTH(OrderDate);
            ";

                using (DataTable result = await DataBaseTools.GetDataTableFromQueryAsync(this.ConnectionString, query))
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        var month = i.ToString("00");
                        var row = result.AsEnumerable().FirstOrDefault(r => r.Field<int>("Month") == i);
                        if (row is null)
                        {
                            sellByMonth.Add(Tuple.Create(month, 0));
                        }
                        else
                        {
                            sellByMonth.Add(Tuple.Create(month, Convert.ToInt32(row["Sell"])));
                        }
                    }
                }

                if (sellByMonth.Count == 0)
                {
                    // Handle the case where no sell is found
                    throw new KeyNotFoundException("No sell was found for the current year");
                }

                return sellByMonth;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting the sell by month", ex);
            }
        }

        /// <summary>
        /// Check if the order number is unique
        /// </summary>
        /// <param name="orderNumber">Order number</param>
        /// <returns>True if the order number is unique, false otherwise</returns>
        /// <exception cref="Exception">If an error occurred while checking the order number</exception>
        /// <exception cref="ArgumentNullException">If the order number is null</exception>
        public async Task<bool> IsOrderNumberUniqueAsync(string orderNumber)
        {
            // Check if the order number is null
            if (orderNumber is null)
            {
                throw new ArgumentNullException("The order number is null");
            }

            try
            {
                // Check if the order number is unique
                using (
                    DataTable result = await DataBaseTools.GetDataTableFromQueryAsync
                    (this.ConnectionString,
                    "SELECT * FROM orders WHERE OrderNumber = @orderNumber;",
                    new MySqlParameter("@orderNumber", orderNumber)
                    )
                )
                {
                    return result.Rows.Count == 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while checking if the order number is unique", ex);
            }
        }

        #endregion
    }
}
