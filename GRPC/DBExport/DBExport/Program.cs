using System;
using Npgsql;
using System.Data.OleDb;

namespace DBExport
{
    public class Program
    {
        private static string connStrAccess = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=theatr.mdb;";
        private static string connStrPostgres = new NpgsqlConnectionStringBuilder
        {
            Host = "localhost",
            Port = 5432,
            Database = "theatr",
            Username = "postgres",
            Password = "4276"
        }.ConnectionString;

        private static OleDbConnection myConnectionAccess;
        static void Main(string[] args)
        {

                myConnectionAccess = new OleDbConnection(connStrAccess);
            myConnectionAccess.Open();

            string query = "SELECT * FROM концерт";

            OleDbCommand command = new OleDbCommand(query, myConnectionAccess);


            // Добавить обработку ошибок
            // Обработка нулевых значений

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Выступление
                    DateTime perf_date = (DateTime)reader["дата_выступления"];
                    string perf_name = (string)reader["название_концерта"];

                    // Актер
                    string main_actor_name = (string)reader["имя_главного_актера"];
                    int main_actor_age = (int)reader["возраст_главного_актера"];
                    int main_actor_experience = (int)reader["опыт_главного_актера"];

                    // Жанр
                    string genre_name = (string)reader["название_жанра"];
                    string genre_birthplace = (string)reader["родина_жанра"];

                    // Режисер
                    string director_name = (string)reader["имя_режисера"];
                    DateTime director_birthdate = (DateTime)reader["дата_рождения_режисера"];

                    // Театр
                    string theatr_name = (string)reader["название_театра"];
                    string theatr_address = (string)reader["адрес_театра"];
                    string theatr_city = (string)reader["город_театра"];

                    // ID вставленных объектов для вставки в performance
                    Int16 actor_id;
                    Int16 director_id;
                    Int16 genre_id;
                    Int16 theatr_id;
                    Int16 performace_id;

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
                            sqlCommand.Parameters.AddWithValue("name", main_actor_name);
                            sqlCommand.Parameters.AddWithValue("experience", main_actor_experience);
                            sqlCommand.Parameters.AddWithValue("age", main_actor_age);

                            try
                            {
                                actor_id = (Int16)sqlCommand.ExecuteScalar();
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
                                    sqlCommand_find.Parameters.AddWithValue("age", main_actor_age);
                                    sqlCommand_find.Parameters.AddWithValue("experience", main_actor_experience);
                                    sqlCommand_find.Parameters.AddWithValue("name", main_actor_name);

                                    using (var reader_finder = sqlCommand_find.ExecuteReader())
                                    {
                                        reader_finder.Read();
                                        actor_id = (Int16)reader_finder["id"];
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
                            sqlCommand.Parameters.AddWithValue("name", genre_name);
                            sqlCommand.Parameters.AddWithValue("birthplace", genre_birthplace);

                            try
                            {
                                genre_id = (Int16)sqlCommand.ExecuteScalar();
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
                                    sqlCommand_find.Parameters.AddWithValue("name", genre_name);
                                    sqlCommand_find.Parameters.AddWithValue("birthplace", genre_birthplace);

                                    using (var reader_finder = sqlCommand_find.ExecuteReader())
                                    {
                                        reader_finder.Read();
                                        genre_id = (Int16)reader_finder["id"];
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
                            sqlCommand.Parameters.AddWithValue("name", director_name);
                            sqlCommand.Parameters.AddWithValue("birthdate", director_birthdate);

                            try
                            {
                                director_id = (Int16)sqlCommand.ExecuteScalar();
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
                                    sqlCommand_find.Parameters.AddWithValue("name", director_name);
                                    sqlCommand_find.Parameters.AddWithValue("birthdate", director_birthdate);

                                    using (var reader_finder = sqlCommand_find.ExecuteReader())
                                    {
                                        reader_finder.Read();
                                        director_id = (Int16)reader_finder["id"];
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
                            sqlCommand.Parameters.AddWithValue("name", theatr_name);
                            sqlCommand.Parameters.AddWithValue("address", theatr_address);
                            sqlCommand.Parameters.AddWithValue("city", theatr_city);

                            try
                            {
                                theatr_id = (Int16)sqlCommand.ExecuteScalar();
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
                                    sqlCommand_find.Parameters.AddWithValue("name", theatr_name);
                                    sqlCommand_find.Parameters.AddWithValue("address", theatr_address);
                                    sqlCommand_find.Parameters.AddWithValue("city", theatr_city);

                                    using (var reader_finder = sqlCommand_find.ExecuteReader())
                                    {
                                        reader_finder.Read();
                                        theatr_id = (Int16)reader_finder["id"];
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
                            sqlCommand.Parameters.AddWithValue("perf_date", perf_date);
                            sqlCommand.Parameters.AddWithValue("perf_name", perf_name);

                            performace_id = (Int16)sqlCommand.ExecuteScalar();
                        }
                    }
                }
            }

            Console.WriteLine("nter для выхода...");
            Console.ReadLine();
        }
    }
}
