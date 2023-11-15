using MySqlConnection = MySql.Data.MySqlClient.MySqlConnection;
using MySqlDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using MySqlCommand = MySql.Data.MySqlClient.MySqlCommand;
using MySqlParameter = MySql.Data.MySqlClient.MySqlParameter;
using MySqlConnectionStringBuilder = MySql.Data.MySqlClient.MySqlConnectionStringBuilder;
using ConnectionState = System.Data.ConnectionState;
using JSONWebToken = PoCs.Classes.Security.JSONWebToken;
using NaClLibrary = PoCs.Classes.Security.NaClLibrary;
using TOTP = OtpNet.Totp;
using PoCs.Functions;

namespace PoCs.Classes.TestCases
{

    public class User(string username, byte[] password, byte[] salt, byte[] jwt, byte[] totp)
    {

        // This connection is just for PoC purposes, don't use this credentials on a production project
        private static readonly MySqlConnection mysqlconn = new (new MySqlConnectionStringBuilder() {
            Database = "UsersDatabase",
            UserID = "root",
            Password = "hwuab+2@",
            Port = 3306,
            Server = "localhost"
        }.ConnectionString);
        private const string SPCreateUser = "sp_create_user";
        private const string VWReadUser = "vw_read_user";
        public string Username = username;
        public byte[] Password = password;
        public byte[] Salt = salt;
        public byte[] Password_jwt = jwt;
        public byte[] Secret_totp = totp;

        public static readonly User Empty = new ();

        private User() : this(string.Empty, [], [], [], []) {}

        public bool Login(string password, out string jwt) {
            if (NaClLibrary.VerifyHash(password, Salt, Password)) {
                jwt = JSONWebToken.GenerateToken(Username, Password_jwt);
                return true;
            }
            jwt = string.Empty;
            return false;
        }

        public static bool SignUp(string username, string password, out User user) {
            byte[] salt = NaClLibrary.CreateSalt();
            user = new User(
                username,
                NaClLibrary.HashPassword(password, salt),
                salt,
                JSONWebToken.GenerateKey(),
                NaClLibrary.CreateSalt(32)
            );
            if (user.SaveInDB()) {
                return true;
            }
            user = Empty;
            return false;
        }
        public bool ValidateTOTP(string totp) {
            TOTP token = new (Secret_totp);
            if (token.ValidateTOTP(totp)) {
                return true;
            }
            return false;
        }

        public bool ValidateTOTP(uint totp) {
            TOTP token = new (Secret_totp);
            if (token.ValidateTOTP(totp)) {
                return true;
            }
            return false;
        }

        public string? ValidateJWT(string jwt) {
            if (JSONWebToken.ValidateCurrentToken(jwt, Password_jwt)) {
                return JSONWebToken.GenerateToken(Username, Password_jwt);
            }
            return null;
        }

        public bool SaveInDB() {
            try {
                // Prepare SQL Query
                string sql = "CALL `@UserStoredProcedure`(@chvUser, @chvPassword, @chvSalt, @chvPasswordJWT, @chvTOTP);";
                MySqlCommand mysqlcomm = new(sql);
                mysqlcomm.Parameters.Add(new MySqlParameter("@UserStoredProcedure", SPCreateUser));
                mysqlcomm.Parameters.Add(new MySqlParameter("@chvUser", Username));
                mysqlcomm.Parameters.Add(new MySqlParameter("@chvPassword", Convert.ToHexString(Password)));
                mysqlcomm.Parameters.Add(new MySqlParameter("@chvSalt", Convert.ToHexString(Salt)));
                mysqlcomm.Parameters.Add(new MySqlParameter("@chvPasswordJWT", Convert.ToHexString(Password_jwt)));
                mysqlcomm.Parameters.Add(new MySqlParameter("@chvTOTP", Convert.ToHexString(Secret_totp)));
                mysqlcomm.CommandText = SPCreateUser;
                mysqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                
                // Connect and execute SQL Query
                mysqlconn.Dispose();
                MySqlDataReader myReader;
                if (mysqlconn.State != ConnectionState.Open) {
                    mysqlconn.Open();
                }
                mysqlcomm.Connection = mysqlconn;
                mysqlcomm.Prepare();
                myReader = mysqlcomm.ExecuteReader();

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            } finally {
                mysqlconn.Close();
            }
            return true;
        }

        public static bool ReadFromDB(string username, out User user) {
            try {
                // Prepare SQL Query
                string sql = "SELECT `Username`, `Password`, `Salt`, `Password_jwt`, `Secret_totp` FROM `" + VWReadUser + "` WHERE `Username` = @chvUser LIMIT 1";
                MySqlCommand mysqlcomm = new(sql);
                mysqlcomm.Parameters.Add(new MySqlParameter("@chvUser", username));

                // Connect and execute SQL Query
                mysqlconn.Dispose();
                MySqlDataReader myReader;
                if (mysqlconn.State != ConnectionState.Open) {
                    mysqlconn.Open();
                }
                mysqlcomm.Connection = mysqlconn;
                mysqlcomm.Prepare();
                myReader = mysqlcomm.ExecuteReader();

                // Read and save SQL Query
                myReader.Read();

                if (myReader.GetName(0) == "Username") {
                    user = new User(
                        myReader.GetString("Username"),
                        Convert.FromHexString(myReader.GetString("Password")),
                        Convert.FromHexString(myReader.GetString("Salt")),
                        Convert.FromHexString(myReader.GetString("Password_jwt")),
                        Convert.FromHexString(myReader.GetString("Secret_totp"))
                    );
                    return true;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
                mysqlconn.Close();
            }
            user = Empty;
            return false;
        }

        public override string ToString()
        {
            return @$"User = {Username}, Password = {BitConverter.ToString(Password).Replace("-", string.Empty)}, Salt = {BitConverter.ToString(Salt).Replace("-", string.Empty)}, Password_jwt = {BitConverter.ToString(Password_jwt).Replace("-", string.Empty)}, Secret_totp = {BitConverter.ToString(Secret_totp).Replace("-", string.Empty)}";
        }
    }
}