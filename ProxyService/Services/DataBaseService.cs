using MySql.Data.MySqlClient;
using OpenQA.Selenium;
using ProxyServer.Models;
using ProxyService.Interfaces;

namespace ProxyService.Services
{
    public class DataBaseService : IDataBaseService
    {
        private static string connectionString
        = "server=localhost;port=3306;database=users_base;user=root;password=12344321;";

        public void CreateUser(User user)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO users_base.users (email, first_name, last_name, avatar, url, text) VALUES (@Email, @FirstName, @LastName, @Avatar)";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Avatar", user.Avatar);
                    

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<User> GetAllUsers()
        {
            var usersList = new List<User>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users_base.users";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var userObj = new User()
                            {
                                Id = int.Parse(reader["id"].ToString()),
                                Email = reader["email"].ToString(),
                                FirstName = reader["first_name"].ToString(),
                                LastName = reader["last_name"].ToString(),
                                Avatar = reader["avatar"].ToString()
                            };

                            usersList.Add(userObj);
                        }
                    }
                }
            }

            return usersList;
        }


        public User GetUserById(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users_base.users WHERE id = @Id";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var userObj = new User()
                            {
                                Id = int.Parse(reader["id"].ToString()),
                                Email = reader["email"].ToString(),
                                FirstName = reader["first_name"].ToString(),
                                LastName = reader["last_name"].ToString(),
                                Avatar = reader["avatar"].ToString()
                            };

                            return userObj;
                        }
                    }
                }
            }
            return null;
        }



        public void UpdateUser(User user)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE users_base.users SET email = @Email, first_name = @FirstName, last_name = @LastName, avatar = @Avatar WHERE id = @Id";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Avatar", user.Avatar);
                    

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM users_base.users WHERE id = @Id";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int MaxId()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT id FROM users_base.users ORDER BY id DESC LIMIT 1", connection);

                var result = command.ExecuteScalar();

                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public bool UniqueCheck(User user)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM users_base.users WHERE email = @Email";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Email", user.Email);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count == 0; 
                }
            }
        }
    }
}
