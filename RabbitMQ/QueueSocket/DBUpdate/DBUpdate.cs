using System.Configuration;
using Newtonsoft.Json;
using Npgsql;

namespace DBUpdate
{
    public class Concert
    {
        public DateTime perf_date;
        public string perf_name;
        public string main_actor_name;
        public int main_actor_age;
        public int main_actor_experience;
        public string genre_name;
        public string genre_birthplace;
        public string director_name;
        public DateTime director_birthdate;
        public string theatr_name;
        public string theatr_address;
        public string theatr_city;

        public Concert(DateTime perf_date, string perf_name, string main_actor_name, int main_actor_age, int main_actor_experience, string genre_name, string genre_birthplace, string director_name, DateTime director_birthdate, string theatr_name, string theatr_address, string theatr_city)
        {
            this.perf_date = perf_date;
            this.perf_name = perf_name;
            this.main_actor_name = main_actor_name;
            this.main_actor_age = main_actor_age;
            this.main_actor_experience = main_actor_experience;
            this.genre_name = genre_name;
            this.genre_birthplace = genre_birthplace;
            this.director_name = director_name;
            this.director_birthdate = director_birthdate;
            this.theatr_name = theatr_name;
            this.theatr_address = theatr_address;
            this.theatr_city = theatr_city;
        }
    }

    public class DBUpdate
    {
        private string rawJson;
        private Concert ConcertString;
        private static string connStrPostgres = new NpgsqlConnectionStringBuilder
        {
            Host = ConfigDB.Host,
            Port = ConfigDB.Port,
            Database = ConfigDB.Database,
            Username = ConfigDB.Username,
            Password = ConfigDB.Password
        }.ConnectionString;

        public DBUpdate(string rawJson)
        {
            this.rawJson = rawJson;
            ParseRaw();
        }

        public void ParseRaw()
        {
            ConcertString = JsonConvert.DeserializeObject<Concert>(rawJson);
        }

        public void Insert_String()
        {
            // ID вставленных объектов для вставки в performance
            Int32 actor_id;
            Int32 director_id;
            Int32 genre_id;
            Int32 theatr_id;
            Int32 performace_id;

            using (var PostGreConn = new NpgsqlConnection(connStrPostgres))
            {
                PostGreConn.Open();

                // Вставка актера (или получение id существующего)
                using (var sqlCommand = new NpgsqlCommand
                {
                    Connection = PostGreConn,
                    CommandText = @"INSERT 
                                            INTO actor (name, experience, age)
                                            VALUES (@name, @experience, @age)
                                            RETURNING ID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("name", ConcertString.main_actor_name);
                    sqlCommand.Parameters.AddWithValue("experience", ConcertString.main_actor_experience);
                    sqlCommand.Parameters.AddWithValue("age", ConcertString.main_actor_age);

                    try
                    {
                        actor_id = (Int32)sqlCommand.ExecuteScalar();
                    }
                    catch (Exception e)
                    {
                        using (var sqlCommand_find = new NpgsqlCommand
                        {
                            Connection = PostGreConn,
                            CommandText = @"SELECT
                                                    id FROM actor
                                                    WHERE
                                                    age = @age and
                                                    experience = @experience and
                                                    name = @name"
                        })
                        {
                            sqlCommand_find.Parameters.AddWithValue("age", ConcertString.main_actor_age);
                            sqlCommand_find.Parameters.AddWithValue("experience", ConcertString.main_actor_experience);
                            sqlCommand_find.Parameters.AddWithValue("name", ConcertString.main_actor_name);

                            using (var reader_finder = sqlCommand_find.ExecuteReader())
                            {
                                reader_finder.Read();
                                actor_id = (Int32)reader_finder["id"];
                            }
                        }
                    }
                }
                // Вставка жанра (или получение id существующего)
                using (var sqlCommand = new NpgsqlCommand
                {
                    Connection = PostGreConn,
                    CommandText = @"INSERT 
                                            INTO genre (name, birthplace)
                                            VALUES (@name, @birthplace)
                                            RETURNING ID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("name", ConcertString.genre_name);
                    sqlCommand.Parameters.AddWithValue("birthplace", ConcertString.genre_birthplace);

                    try
                    {
                        genre_id = (Int32)sqlCommand.ExecuteScalar();
                    }
                    catch (Exception e)
                    {
                        using (var sqlCommand_find = new NpgsqlCommand
                        {
                            Connection = PostGreConn,
                            CommandText = @"SELECT
                                                    id FROM genre
                                                    WHERE
                                                    name = @name and
                                                    birthplace = @birthplace"
                        })
                        {
                            sqlCommand_find.Parameters.AddWithValue("name", ConcertString.genre_name);
                            sqlCommand_find.Parameters.AddWithValue("birthplace", ConcertString.genre_birthplace);

                            using (var reader_finder = sqlCommand_find.ExecuteReader())
                            {
                                reader_finder.Read();
                                genre_id = (Int32)reader_finder["id"];
                            }
                        }
                    }
                }

                // Вставка режисера (или получение id существующего)
                using (var sqlCommand = new NpgsqlCommand
                {
                    Connection = PostGreConn,
                    CommandText = @"INSERT 
                                            INTO director (name, birthdate)
                                            VALUES (@name, @birthdate)
                                            RETURNING ID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("name", ConcertString.director_name);
                    sqlCommand.Parameters.AddWithValue("birthdate", ConcertString.director_birthdate);

                    try
                    {
                        director_id = (Int32)sqlCommand.ExecuteScalar();
                    }
                    catch (Exception e)
                    {
                        using (var sqlCommand_find = new NpgsqlCommand
                        {
                            Connection = PostGreConn,
                            CommandText = @"SELECT
                                                    id FROM director
                                                    WHERE
                                                    name = @name and
                                                    birthdate = @birthdate"
                        })
                        {
                            sqlCommand_find.Parameters.AddWithValue("name", ConcertString.director_name);
                            sqlCommand_find.Parameters.AddWithValue("birthdate", ConcertString.director_birthdate);

                            using (var reader_finder = sqlCommand_find.ExecuteReader())
                            {
                                reader_finder.Read();
                                director_id = (Int32)reader_finder["id"];
                            }
                        }
                    }
                }

                // Вставка театра (или получение id существующего)
                using (var sqlCommand = new NpgsqlCommand
                {
                    Connection = PostGreConn,
                    CommandText = @"INSERT 
                                            INTO theatr (name, address, city)
                                            VALUES (@name, @address, @city)
                                            RETURNING ID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("name", ConcertString.theatr_name);
                    sqlCommand.Parameters.AddWithValue("address", ConcertString.theatr_address);
                    sqlCommand.Parameters.AddWithValue("city", ConcertString.theatr_city);

                    try
                    {
                        theatr_id = (Int32)sqlCommand.ExecuteScalar();
                    }
                    catch (Exception e)
                    {
                        using (var sqlCommand_find = new NpgsqlCommand
                        {
                            Connection = PostGreConn,
                            CommandText = @"SELECT
                                                    id FROM theatr
                                                    WHERE
                                                    name = @name and
                                                    address = @address and
                                                    city = @city"
                        })
                        {
                            sqlCommand_find.Parameters.AddWithValue("name", ConcertString.theatr_name);
                            sqlCommand_find.Parameters.AddWithValue("address", ConcertString.theatr_address);
                            sqlCommand_find.Parameters.AddWithValue("city", ConcertString.theatr_city);

                            using (var reader_finder = sqlCommand_find.ExecuteReader())
                            {
                                reader_finder.Read();
                                theatr_id = (Int32)reader_finder["id"];
                            }
                        }
                    }
                }

                // Вставка выступления 
                using (var sqlCommand = new NpgsqlCommand
                {
                    Connection = PostGreConn,
                    CommandText = @"INSERT 
                                            INTO performance (main_actor_id, genre_id, director_id, theatr_id, perf_date, name)
                                            VALUES (@actor_id, @genre_id, @director_id, @theatr_id, @perf_date, @perf_name)
                                            RETURNING ID"
                })
                {
                    sqlCommand.Parameters.AddWithValue("actor_id", actor_id);
                    sqlCommand.Parameters.AddWithValue("genre_id", genre_id);
                    sqlCommand.Parameters.AddWithValue("director_id", director_id);
                    sqlCommand.Parameters.AddWithValue("theatr_id", theatr_id);
                    sqlCommand.Parameters.AddWithValue("perf_date", ConcertString.perf_date);
                    sqlCommand.Parameters.AddWithValue("perf_name", ConcertString.perf_name);

                    try
                    {
                        performace_id = (Int32)sqlCommand.ExecuteScalar();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {

        }
    }
}
