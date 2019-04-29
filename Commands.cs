using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cars.bg
{
    public static class Commands
    {
        public static User signIn()
        {
            Console.WriteLine("Username: ");
            string username = Console.ReadLine();

            //Check wheater there is such username
            while (!Commands.isInDatabase("username", username))
            {
                Console.WriteLine("There is not such username. Please try again!");
                username = Console.ReadLine();
            }

            //User enters password
            SecureString pass = Commands.maskInputString();
            string Password = new System.Net.NetworkCredential(string.Empty, pass).Password;
            Console.WriteLine();

            var encryptedPassword = Commands.getData("Users", "username", "password", username);

            var splitPassSalt = encryptedPassword.Split(' ');

            //Check wheater this is the right password for the username
            while (!Commands.isInDatabase("password", encryptPassword(Password, splitPassSalt[1])))
            {
                Console.WriteLine("Wrong password! Try again.");
                SecureString pass2 = Commands.maskInputString();
                Password = new System.Net.NetworkCredential(string.Empty, pass2).Password;
                Console.WriteLine();
            }

            //Gets data from the Database
            string messages = Commands.getData("Users", "username", "messages", username);

            string type = Commands.getData("Users", "username", "type", username);

            string cars = Commands.getData("Users", "username", "cars", username);

            string gender = Commands.getData("Users", "username", "gender", username);

            //Gets how many unread messages the user has 
            string[] output = messages.Split('\n');
            Console.WriteLine($"You have {output.GetLength(0) - 1} unread messages!");

            //Gets how many views the user's cars have
            string[] carIds = cars.Split(',');
            for (int i = 1; i < carIds.GetLength(0); i++)
            {
                Console.WriteLine($"{carIds[i]} : {Commands.getData("Cars", "carId", "views", carIds[i])}");
            }

            if (type == "regular user")
            {
                User user = new RegularUser(username, Password, cars, messages, gender, type);
                return user;
            }
            else if(type == "dealer")
            {
                User user = new Dealer(username, Password, cars, messages, gender, type);
                return user;
            }
            else
            {
                User user = new Administrator(username, Password, messages, gender, type);
                return user;
            }

        }

        public static void signUp(string username)
        {
            Console.WriteLine($"Your username is: {username}");
            //Input to show '*'
            SecureString pass = Commands.maskInputString();
            string Password = new System.Net.NetworkCredential(string.Empty, pass).Password;
            Console.WriteLine();
            SecureString pass2 = Commands.maskInputString();
            string Password2 = new System.Net.NetworkCredential(string.Empty, pass2).Password;

            //User should try again in the two passwords does not match
            while (Password != Password2)
            {
                Console.WriteLine();
                Console.WriteLine("The passwords does not match. Try again!");
                SecureString pass3 = Commands.maskInputString();
                Password2 = new System.Net.NetworkCredential(string.Empty, pass3).Password;
            }

            //User enters its gender
            Console.WriteLine();
            Console.WriteLine("Enter gender: ");
            string gender = Console.ReadLine();

            //User must choose type of user
            Console.WriteLine("Choose your role: (regular user, dealer)");
            string type = Console.ReadLine();


            //Connection to the Database
            if (type == "regular user")
            {
                User user = new RegularUser(username, Password, gender, type);
                Commands.insertUser(user);
            }
            else
            {
                User user = new Dealer(username, Password, gender, type);
                Commands.insertUser(user);
            }
            
        }

        public static void seeAllNewMessages(string username)
        {
            Console.WriteLine(Commands.getData("Users", "username", "messages", username));

            using (SqlConnection connection = new SqlConnection(Helper.CnnString("Cars.bg")))
            {
                connection.Open();

                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    SqlCommand cmd = new SqlCommand($"UPDATE Users SET messages = '' WHERE username = '{username}'", connection);

                    adapter.UpdateCommand = new SqlCommand($"UPDATE Users SET messages = '' WHERE username = '{username}'", connection);
                    adapter.UpdateCommand.ExecuteNonQuery();

                    cmd.Dispose();

                }
                catch (Exception)
                {
                    Console.WriteLine("There is not user with this username!");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static void writeMessage(string currentUsername)
        {
            Console.WriteLine("Enter username you want to send message to: ");
            string username = Console.ReadLine();

            while (!Commands.isInDatabase("username", username))
            {
                Console.WriteLine("There is not such username. Try again!");
                username = Console.ReadLine();
            }

            Console.WriteLine($"Write your message to {username}: ");
            string message = Console.ReadLine();
            Commands.insertData("messages", message, username, currentUsername);
        }

        public static void addCar(string currentUser)
        {
            Console.WriteLine("Brand: ");
            string brand = Console.ReadLine();

            Console.WriteLine("Model: ");
            string model = Console.ReadLine();

            Console.WriteLine("Year: ");
            int year = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Engine: ");
            string engine = Console.ReadLine();

            Console.WriteLine("Horse Power: ");
            int horsePower = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Transmission: ");
            string transmission = Console.ReadLine();

            Car car = new Car(0, brand, model, engine, horsePower, year, transmission);
            Commands.insertCar(car);

            using (SqlConnection connection = new SqlConnection(Helper.CnnString("Cars.bg")))
            {

                connection.Open();
                SqlCommand cmd = new SqlCommand($"SELECT carId FROM Cars WHERE brand = '{brand}' AND model = '{model}' AND engine = '{engine}' AND horsePowers = '{horsePower}' AND year = '{year}' AND transmission = '{transmission}'", connection);

                SqlDataReader reader = cmd.ExecuteReader();

                int carId = 0;
                while(reader.Read())
                {
                    carId = Int32.Parse(reader["carId"].ToString());
                }

                cmd.Dispose();
                reader.Close();

                SqlCommand command = new SqlCommand($"UPDATE Users SET cars = cars + ',' + '{carId}' WHERE username = '{currentUser}'", connection);
                SqlDataAdapter adapter = new SqlDataAdapter();

                adapter.UpdateCommand = new SqlCommand($"UPDATE Users SET cars = cars + ',' + '{carId}' WHERE username = '{currentUser}'", connection);

                adapter.UpdateCommand.ExecuteNonQuery();

                command.Dispose();

                connection.Close();
            }
        }

        public static Car showCar(string carId)
        {
            string brand = getData("Cars", "carId", "brand", carId);
            string model = getData("Cars", "carId", "model", carId);
            string engine = getData("Cars", "carId", "engine", carId);
            int horsePowers = Int32.Parse(getData("Cars", "carId", "horsePowers", carId));
            int year = Int32.Parse(getData("Cars", "carId", "year", carId));
            string transmission = getData("Cars", "carId", "transmission", carId);

            Car car = new Car(Int32.Parse(carId), brand, model, engine, horsePowers, year, transmission);

            return car;
        }

        public static User showUser(string username)
        {
            string gender = getData("Users", "username", "gender", username);
            string cars = getData("Users", "username", "cars", username);
            string type = getData("Users", "username", "type", username);

            if(type == "regular user")
            {
                User user = new RegularUser(username, cars, type);
                return user;
            }
            else if(type == "dealer")
            {
                User user = new Dealer(username, cars, type);
                return user;
            }
            else
            {
                User user = new Administrator();
                return user;
            }
        }

        public static void deleteCar(int carId)
        {
            using (SqlConnection connection = new SqlConnection(Helper.CnnString("Cars.bg")))
            {
                connection.Open();

                try
                {

                    SqlCommand cmd = new SqlCommand($"DELETE FROM Cars WHERE carId = {carId}", connection);

                    cmd.ExecuteNonQuery();

                    cmd.Dispose();

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.UpdateCommand = new SqlCommand($"UPDATE Users SET cars = (SELECT REPLACE(cars, ',{carId}', '') FROM Users WHERE username = 'vasko') WHERE username = 'vasko'", connection);
                    adapter.UpdateCommand.ExecuteNonQuery();

                
                }
                catch(Exception)
                {
                    Console.WriteLine("There is not car with this ID!");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static void deleteProfile(string username)
        {
            using (SqlConnection connection = new SqlConnection(Helper.CnnString("Cars.bg")))
            {
                connection.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand($"DELETE FROM Users WHERE username = '{username}'", connection);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    Console.WriteLine("There is not user with this username!");
                }
                finally
                {
                    connection.Close();
                }

            }
        }

        public static string getData(string table, string attribute, string column, string key)
        {
            string output = null;
            using (SqlConnection connection = new SqlConnection(Helper.CnnString("Cars.bg")))
            {
                connection.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand($"SELECT {column} FROM {table} WHERE {attribute} = '{key}'", connection);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        output = reader[column].ToString();
                    }
                }
                catch(Exception)
                {
                    Console.WriteLine("Bad data!");
                }
                finally
                {
                    
                    connection.Close();           
                }

                return output;
            }
        }

        public static List<Car> getDetails(string table, string column, string firstKey, string secondKey)
        {
            List<Car> cars = new List<Car>();

            using (SqlConnection connection = new SqlConnection(Helper.CnnString("Cars.bg")))
            {
                connection.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand($"SELECT {column} FROM {table} WHERE brand = '{firstKey}' AND model = '{secondKey}'", connection);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cars.Add(new Car
                        {
                            carId = Int32.Parse(reader["carId"].ToString()),
                            brand = firstKey,
                            model = secondKey,
                            engine = reader["engine"].ToString(),
                            horsePowers = Int32.Parse(reader["horsePowers"].ToString()),
                            year = Int32.Parse(reader["year"].ToString()),
                            transmission = reader["transmission"].ToString(),
                            views = Int32.Parse(reader["views"].ToString())
                        });
                    }
                }
                catch(Exception)
                {
                    Console.WriteLine("Bad data!");
                }
                finally
                {
                    connection.Close();
                }

                return cars;
            }
        }

        private static void insertData(string type, string input, string username, string currentUser)
        {
            using (SqlConnection connection = new SqlConnection(Helper.CnnString("Cars.bg")))
            {
                connection.Open();

                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    SqlCommand cmd = new SqlCommand($"UPDATE Users SET {type} = {type} + CHAR(10) + '{input}' + '    (from {currentUser})' WHERE username = '{username}'", connection);

                    adapter.UpdateCommand = new SqlCommand($"UPDATE Users SET {type} = {type} + CHAR(10) + '{input}' + '    (from {currentUser})' WHERE username = '{username}'", connection);
                    adapter.UpdateCommand.ExecuteNonQuery();

                    cmd.Dispose();
                }
                catch(Exception)
                {
                    Console.WriteLine("The data wasn't added!");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static void insertCar(Car car)
        {
            using (SqlConnection connection = new SqlConnection(Helper.CnnString("Cars.bg")))
            {
                //Connection opens here
                connection.Open();

                try
                {
                    //Sql query
                    SqlCommand cmd = new SqlCommand("INSERT INTO Cars ([brand], [model], [engine], [horsePowers], [year], [transmission], [views]) VALUES (@brand, @model, @engine, @horsePowers, @year, @transmission, @views)", connection);

                    //Add parameters to the command in order to update the Database
                    cmd.Parameters.AddWithValue("@brand", car.brand);
                    cmd.Parameters.AddWithValue("@model", car.model);
                    cmd.Parameters.AddWithValue("@engine", car.engine);
                    cmd.Parameters.AddWithValue("@horsePowers", car.horsePowers);
                    cmd.Parameters.AddWithValue("@year", car.year);
                    cmd.Parameters.AddWithValue("@transmission", car.transmission);
                    cmd.Parameters.AddWithValue("@views", car.views);


                    int rows = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    Console.WriteLine("The data wasn't inserted!");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static void insertUser(User user)
        {
            string salt = Commands.getSalt(10);

            string password = Commands.encryptPassword(user.password, salt);

            using (SqlConnection connection = new SqlConnection(Helper.CnnString("Cars.bg")))
            {
                //Connection opens here
                connection.Open();

                try
                {

                    //Sql query
                    SqlCommand cmd = new SqlCommand("INSERT INTO Users ([username], [password], [cars], [messages], [gender], [type]) VALUES (@username, @password, @cars, @messages, @gender, @type)", connection);

                    //Add parameters to the command in order to update the Database
                    cmd.Parameters.AddWithValue("@username", user.username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@cars", "");
                    cmd.Parameters.AddWithValue("@messages", "");
                    cmd.Parameters.AddWithValue("@gender", user.gender);
                    cmd.Parameters.AddWithValue("@type", user.type);
                    

                    int rows = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    Console.WriteLine("The data wasn't inserted!");
                }
                finally
                {
                    connection.Close();
                } 
            }    
        }
  
        private static bool isInDatabase(string type, string input)
        {
            bool isValid = false;

            using (SqlConnection connection = new SqlConnection(Helper.CnnString("Cars.bg")))
            {
                connection.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand($"SELECT {type} From Users", connection);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<string> list = new List<string>();

                    while (reader.Read())
                    { 
                        list.Add(reader[type].ToString());
                    }

                    isValid = list.Contains(input);
                    
                }
                catch(Exception)
                {
                    Console.WriteLine("Bad data!");
                }
                finally
                {
                    connection.Close();
                }
            }

            return isValid;
        }

        private static SecureString maskInputString()
        {
            Console.WriteLine("Enter password: ");
            SecureString pass = new SecureString();
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);
                if (!char.IsControl(keyInfo.KeyChar))
                {
                    pass.AppendChar(keyInfo.KeyChar);
                    Console.Write("*");
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    pass.RemoveAt(pass.Length - 1);
                    Console.Write("\b \b");
                }
            }
            while (keyInfo.Key != ConsoleKey.Enter);
            {
                return pass;
            }
        }

        private static string getSalt(int size)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            var buff = new byte[size];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        private static string encryptPassword(string password, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            var sha256HashString = new System.Security.Cryptography.SHA256Managed();

            byte[] hash = sha256HashString.ComputeHash(bytes);

            return Convert.ToBase64String(hash) + " " + salt;
        }  
    }   
}
