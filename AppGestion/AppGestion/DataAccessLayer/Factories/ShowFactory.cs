using AppGestion.Tools;
using MySql.Data.MySqlClient;
using SeatSwiftDLL;
using SeatSwiftDLL.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AppGestion.DataAccessLayer.Factories
{
    /// <summary>
    /// Factory class for Show
    /// </summary>
    /// <remarks>
    /// This class is a factory for the Show class. It is used
    /// to create Show objects from data read from a database.
    /// </remarks>
    public class ShowFactory : Base.FactoryBase
    {
        #region Create methods

        /// <summary>
        /// Create a new Show object from a data reader
        /// </summary>
        /// <param name="dataReader">The data reader to create the Show object from</param>
        /// <returns>The newly created Show object</returns>
        public async Task<Show> CreateFromReaderAsync(MySqlDataReader dataReader)
        {
            // Get the data from the data reader
            int id = dataReader.GetInt32("Id");
            bool isActive = dataReader.GetBoolean("IsActive");
            string name =
                dataReader.GetString("ShowName")
                ?? throw new InvalidOperationException("The ShowName field is null");
            string artist = dataReader.GetString("Artiste");
            string description = dataReader.GetString("Description");
            string showType = dataReader.GetString("ShowType");
            string image = dataReader.GetString("ImageUrl");
            int maxTicketsByUser = dataReader.GetInt32("NumberOfTicketsMaxByClient");
            decimal baseTicketPrice = dataReader.GetDecimal("BaseTicketPrice");
            int userId = dataReader.GetInt32("UserId");

            // Get the enum values
            ShowType showTypeEnum = (ShowType)Enum.Parse(typeof(ShowType), showType);

            // Get the user with the userId
            User user = await new UserFactory().GetByIdAsync(userId);
            if (user is null)
            {
                throw new KeyNotFoundException(
                    "The user with the id " + userId + " does not exist"
                );
            }

            // Create the Show object
            Show show = new Show(
                id,
                isActive,
                name,
                artist,
                description,
                showTypeEnum,
                image,
                maxTicketsByUser,
                baseTicketPrice,
                user
            );

            // Return the Show object
            return show;
        }

        /// <summary>
        /// Create a new Show object from a data row
        /// </summary>
        /// <param name="dataRow">The data row to create the Show object from</param>
        /// <returns>The newly created Show object</returns>
        public async Task<Show> CreateFromRowAsync(DataRow dataRow)
        {
            // Get the data from the data row
            int id = dataRow.Field<int>("Id");
            bool isActive = dataRow.Field<bool>("IsActive");
            string name =
                dataRow.Field<string>("ShowName")
                ?? throw new InvalidOperationException("The ShowName field is null");
            string artist =
                dataRow.Field<string>("Artiste")
                ?? throw new InvalidOperationException("The Artist field is null");
            string description =
                dataRow.Field<string>("Description")
                ?? throw new InvalidOperationException("The Description field is null");
            string showType =
                dataRow.Field<string>("ShowType")
                ?? throw new InvalidOperationException("The ShowType field is null");
            string image =
                dataRow.Field<string>("ImageUrl")
                ?? throw new InvalidOperationException("The ImageUrl field is null");
            int maxTicketsByUser = dataRow.Field<int>("NumberOfTicketsMaxByClient");
            decimal baseTicketPrice = dataRow.Field<decimal>("BaseTicketPrice");
            int userId = dataRow.Field<int>("UserId");

            // Get the enum values
            ShowType showTypeEnum = (ShowType)Enum.Parse(typeof(ShowType), showType);

            // Get the user with the userId
            User user = await new UserFactory().GetByIdAsync(userId);
            if (user is null)
            {
                throw new KeyNotFoundException(
                    "The user with the id " + userId + " does not exist"
                );
            }

            // Create the Show object
            Show show = new Show(
                id,
                isActive,
                name,
                artist,
                description,
                showTypeEnum,
                image,
                maxTicketsByUser,
                baseTicketPrice,
                user
            );

            // Return the Show object
            return show;
        }

        #endregion


        #region Factory methods

        /// <summary>
        /// Get all active shows
        /// </summary>
        /// <returns>A list of all active shows</returns>
        /// <exception cref="Exception">If an error occurs while executing the query</exception>
        public async Task<List<Show>> GetAllActiveAsync()
        {
            try
            {
                // Get all active shows
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM Shows WHERE IsActive = 1"
                    )
                )
                {
                    // Create a list of Show objects
                    List<Show> shows = new List<Show>();

                    // Create a Show object for each row in the result
                    foreach (DataRow row in result.Rows)
                    {
                        shows.Add(await CreateFromRowAsync(row));
                    }

                    // Return the list of Show objects
                    return shows;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting all active shows", ex);
            }
        }

        /// <summary>
        /// Get all shows
        /// </summary>
        /// <returns>A list of all shows</returns>
        /// <exception cref="Exception">If an error occurs while executing the query</exception>
        public async Task<List<Show>> GetAllAsync()
        {
            try
            {
                // Get all shows
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM Shows"
                    )
                )
                {
                    // Create a list of Show objects
                    List<Show> shows = new List<Show>();

                    // Create a Show object for each row in the result
                    foreach (DataRow row in result.Rows)
                    {
                        shows.Add(await CreateFromRowAsync(row));
                    }

                    // Return the list of Show objects
                    return shows;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting all shows", ex);
            }
        }

        /// <summary>
        /// Get all active shows create by a user
        /// </summary>
        /// <param name="userId">The id of the user to get the shows of</param>
        /// <returns>A list of all shows of the user with the given id</returns>
        /// <exception cref="Exception">If an error occurs while executing the query</exception>
        /// <exception cref="KeyNotFoundException">If no user with the given id is found</exception>
        public async Task<ICollection<Show>> GetAllActiveByUserIdAsync(int userId)
        {
            try
            {
                // Get the user with the given id
                User user = await new UserFactory().GetByIdAsync(userId);
                if (user is null)
                {
                    throw new KeyNotFoundException("No user with the id " + userId + " was found");
                }

                // Get all shows of the user with the given id
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM Shows WHERE UserId = @userId AND IsActive = 1",
                        new MySqlParameter("@userId", userId)
                    )
                )
                {
                    // Create a list of Show objects
                    List<Show> shows = new List<Show>();

                    // Create a Show object for each row in the result
                    foreach (DataRow row in result.Rows)
                    {
                        shows.Add(await CreateFromRowAsync(row));
                    }

                    // Return the list of Show objects order by name
                    return shows.OrderBy(s => s.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while getting all shows of the user with the given id",
                    ex
                );
            }
        }

        /// <summary>
        /// Get count of active shows by type
        /// </summary>
        /// <returns>List of tuples containing show type and count</returns>
        /// <exception cref="Exception">If an error occurs while executing the query</exception>
        public async Task<List<Tuple<ShowType, int>>> GetCountActiveByTypeAsync()
        {
            try
            {
                var showCounts = new List<Tuple<ShowType, int>>();

                string query =
                    @"
            SELECT ShowType, COUNT(*) AS Count
            FROM Shows
            WHERE IsActive = 1
            GROUP BY ShowType;
        ";

                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        query
                    )
                )
                {
                    foreach (DataRow row in result.Rows)
                    {
                        ShowType showType = (ShowType)
                            Enum.Parse(
                                typeof(ShowType),
                                row["ShowType"].ToString()
                                    ?? throw new InvalidOperationException("ShowType is null")
                            );
                        int count = Convert.ToInt32(row["Count"]);
                        showCounts.Add(Tuple.Create(showType, count));
                    }
                }

                // Retourne la liste vide si aucun spectacle actif n'est trouvé, au lieu de lancer une exception
                return showCounts;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while getting the count of active shows by type",
                    ex
                );
            }
        }


        /// <summary>
        /// Get a show by its id
        /// </summary>
        /// <param name="id">The id of the show to get</param>
        /// <returns>The show with the given id</returns>
        /// <exception cref="Exception">If an error occurs while executing the query</exception>
        /// <exception cref="KeyNotFoundException">If no show with the given id is found</exception>
        public async Task<Show> GetByIdAsync(int id)
        {
            try
            {
                // Get the show with the given id
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM Shows WHERE Id = @id",
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
                throw new Exception(
                    "An error occurred while getting the show with the given id",
                    ex
                );
            }
        }

        /// <summary>
        /// Get a show by its name
        /// </summary>
        /// <param name="showName">The name of the show to get</param>
        /// <returns>The show with the given name</returns>
        /// <exception cref="Exception">If an error occurs while executing the query</exception>
        /// <exception cref="KeyNotFoundException">If no show with the given name is found</exception>
        public async Task<Show> GetByNameAsync(string showName)
        {
            try
            {
                // Get the show with the given name
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT * FROM Shows WHERE ShowName = @showName",
                        new MySqlParameter("@showName", showName)
                    )
                )
                {
                    // If no show is found, throw an exception
                    if (result.Rows.Count == 0)
                    {
                        throw new KeyNotFoundException(
                            "No show with the name " + showName + " was found"
                        );
                    }

                    // Create the Show object
                    return await CreateFromRowAsync(result.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while getting the show with the given name",
                    ex
                );
            }
        }

        /// <summary>
        /// Get the number of active shows created by a user
        /// </summary>
        /// <param name="userId">The id of the user to get the number of shows of</param>
        /// <returns>The number of shows of the user with the given id</returns>
        /// <exception cref="Exception">If an error occurs while executing the query</exception>
        /// <exception cref="KeyNotFoundException">If no user with the given id is found</exception>
        public async Task<int> GetCountActiveByUserIdAsync(int userId)
        {
            try
            {
                // Get the user with the given id
                User user = await new UserFactory().GetByIdAsync(userId);
                if (user is null)
                {
                    throw new KeyNotFoundException("No user with the id " + userId + " was found");
                }

                // Get the number of shows of the user with the given id
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT COUNT(*) FROM Shows WHERE UserId = @userId AND IsActive = 1",
                        new MySqlParameter("@userId", userId)
                    )
                )
                {
                    // Return the number of shows
                    return result.Rows[0].Field<int>(0);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while getting the number of shows of the user with the given id",
                    ex
                );
            }
        }

        /// <summary>
        /// Create a new show
        /// </summary>
        /// <param name="show">The show to create</param>
        /// <exception cref="Exception">If an error occurs while executing the query</exception>
        /// <exception cref="InvalidOperationException">If the show already exists</exception>
        public async Task CreateAsync(Show show)
        {
            try
            {
                // Check if a show with the same name already exists
                if (await ExistsAsync(show.Name))
                {
                    throw new InvalidOperationException(
                        "A show with the name " + show.Name + " already exists"
                    );
                }

                // Check if the show is null
                if (show is null)
                {
                    throw new ArgumentNullException(nameof(show));
                }

                // Check if the user is null
                if (show.User is null)
                {
                    throw new ArgumentNullException(nameof(show.User));
                }

                // Create the show
                await DataBaseTool.ExecuteNonQueryAsync(
                    this.ConnectionString,
                    "INSERT INTO Shows (IsActive, ShowName, Artiste, Description, ShowType, ImageUrl, NumberOfTicketsMaxByClient, BaseTicketPrice, UserId) "
                        + "VALUES (@isActive, @showName, @artist, @description, @showType, @imageUrl, @maxTicketsByClient, @baseTicketPrice, @userId)",
                    new MySqlParameter("@isActive", show.IsActive),
                    new MySqlParameter("@showName", show.Name),
                    new MySqlParameter("@artist", show.Artist),
                    new MySqlParameter("@description", show.Description),
                    new MySqlParameter("@showType", show.ShowType.ToString()),
                    new MySqlParameter("@imageUrl", show.ImageUrl),
                    new MySqlParameter("@maxTicketsByClient", show.MaxTicketsByClient),
                    new MySqlParameter("@baseTicketPrice", show.BasePrice),
                    new MySqlParameter("@userId", show.User.Id)
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the show", ex);
            }
        }

        /// <summary>
        /// Update a show
        /// </summary>
        /// <param name="show">The show to update</param>
        /// <exception cref="Exception">If an error occurs while executing the query</exception>
        /// <exception cref="KeyNotFoundException">If no show with the given id is found</exception>
        public async Task UpdateAsync(Show show)
        {
            // Check if the show is null
            if (show is null)
            {
                throw new ArgumentNullException(nameof(show));
            }

            // Check if the user is null
            if (show.User is null)
            {
                throw new ArgumentNullException(nameof(show.User));
            }

            try
            {
                // Update the show
                await DataBaseTool.ExecuteNonQueryAsync(
                    this.ConnectionString,
                    "UPDATE Shows SET IsActive = @isActive, ShowName = @showName, Artiste = @artist, Description = @description, ShowType = @showType, ImageUrl = @imageUrl, NumberOfTicketsMaxByClient = @maxTicketsByClient, BaseTicketPrice = @baseTicketPrice WHERE Id = @id",
                    new MySqlParameter("@isActive", show.IsActive),
                    new MySqlParameter("@showName", show.Name),
                    new MySqlParameter("@artist", show.Artist),
                    new MySqlParameter("@description", show.Description),
                    new MySqlParameter("@showType", show.ShowType.ToString()),
                    new MySqlParameter("@imageUrl", show.ImageUrl),
                    new MySqlParameter("@maxTicketsByClient", show.MaxTicketsByClient),
                    new MySqlParameter("@baseTicketPrice", show.BasePrice),
                    new MySqlParameter("@id", show.Id)
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the show", ex);
            }
        }

        /// <summary>
        /// Set a show as inactive
        /// </summary>
        /// <param name="showId">The id of the show to set as inactive</param>
        /// <exception cref="Exception">If an error occurs while executing the query</exception>
        /// <exception cref="KeyNotFoundException">If no show with the given id is found</exception>
        public async Task SetInactiveAsync(int showId)
        {
            try
            {
                // Set the show as inactive
                await DataBaseTool.ExecuteNonQueryAsync(
                    this.ConnectionString,
                    "UPDATE Shows SET IsActive = 0 WHERE Id = @id",
                    new MySqlParameter("@id", showId)
                );
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while setting the show as inactive", ex);
            }
        }

        #endregion


        #region methods

        /// <summary>
        /// Check if a show exists with the given name
        /// </summary>
        /// <param name="showName">The name of the show to check for</param>
        /// <returns>True if a show exists with the given name, false otherwise</returns>
        /// <exception cref="Exception">If an error occurs while executing the query</exception>
        public async Task<bool> ExistsAsync(string showName)
        {
            try
            {
                // Get the total number of shows with the given name
                using (
                    DataTable result = await DataBaseTool.GetDataTableFromQueryAsync(
                        this.ConnectionString,
                        "SELECT COUNT(*) FROM Shows WHERE ShowName = @showName",
                        new MySqlParameter("@showName", showName)
                    )
                )
                {
                    // Return true if the total number of shows is greater than 0
                    return result.Rows[0].Field<long>(0) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "An error occurred while checking if a show exists with the given name",
                    ex
                );
            }
        }

        #endregion
    }
}
