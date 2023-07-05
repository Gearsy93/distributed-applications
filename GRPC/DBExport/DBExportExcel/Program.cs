using Npgsql;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DBExportExcel
{
    internal class Program
    {
        private static string connStr = new NpgsqlConnectionStringBuilder
        {
            Host = "localhost",
            Port = 5432,
            Database = "theatr",
            Username = "postgres",
            Password = "4276"
        }.ConnectionString;

        public class Actor
        {
            public Int16 ID;
            public int age;
            public string name;
            public int experience;

            public Actor(Int16 ID, int age, string name, int experience)
            {
                this.ID = ID;
                this.age = age;
                this.name = name;
                this.experience = experience;
            }
        }

        public class Director
        {
            public Int16 ID;
            public string name;
            public DateTime birthdate;

            public Director(Int16 ID, string name, DateTime birthdate)
            {
                this.ID = ID;
                this.name = name;
                this.birthdate = birthdate;
            }
        }

        public class Genre
        {
            public Int16 ID;
            public string name;
            public string birthplace;

            public Genre(Int16 ID, string name, string birthplace)
            {
                this.ID = ID;
                this.name = name;
                this.birthplace = birthplace;
            }
        }

        public class Performance
        {
            public Int16 ID;
            public Int16 main_actor_ID;
            public Int16 genre_ID;
            public Int16 director_ID;
            public Int16 theatr_ID;
            public string name;
            public DateTime perf_date;

            public Performance(Int16 iD, Int16 main_actor_ID, Int16 genre_ID, Int16 director_ID, Int16 theatr_ID, string name, DateTime perf_date)
            {
                this.ID = iD;
                this.main_actor_ID = main_actor_ID;
                this.genre_ID = genre_ID;
                this.director_ID = director_ID;
                this.theatr_ID = theatr_ID;
                this.name = name;
                this.perf_date = perf_date;
            }
        }

        public class Theatr
        {
            public Int16 ID;
            public string name;
            public string address;
            public string city;

            public Theatr(short ID, string name, string address, string city)
            {
                this.ID = ID;
                this.name = name;
                this.address = address;
                this.city = city;
            }
        }

        public class Export
        {
            public List<Actor> actors;
            public List<Theatr> theatrs;
            public List<Director> directors;
            public List<Genre> genres;
            public List<Performance> performances;

            public Export()
            {
                actors = new List<Actor>();
                theatrs = new List<Theatr>();
                directors = new List<Director>();
                genres = new List<Genre>();
                performances = new List<Performance>();
            }
        }

        static void Main(string[] args)
        {
            Export export = new Export();

            // Жанр
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                using (var sqlCommand = new NpgsqlCommand
                {
                    Connection = conn,
                    CommandText = @"SELECT * FROM actor"
                })
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Int16 id = (Int16)reader["id"];
                            string name = (string)reader["name"];
                            Int16 age = (Int16)reader["age"];
                            Int16 experience = (Int16)reader["experience"];

                            Actor actor = new Actor(id, age, name, experience);
                            export.actors.Add(actor);
                        }
                    }
                }
            }

            // Режисер
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                using (var sqlCommand = new NpgsqlCommand
                {
                    Connection = conn,
                    CommandText = @"SELECT * FROM director"
                })
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Int16 id = (Int16)reader["id"];
                            string name = (string)reader["name"];
                            DateTime birthdate = (DateTime)reader["birthdate"];

                            Director director = new Director(id, name, birthdate);
                            export.directors.Add(director);
                        }
                    }
                }
            }

            // Жанр 
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                using (var sqlCommand = new NpgsqlCommand
                {
                    Connection = conn,
                    CommandText = @"SELECT * FROM genre"
                })
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Int16 id = (Int16)reader["id"];
                            string name = (string)reader["name"];
                            string birthplace = (string)reader["birthplace"];

                            Genre genre = new Genre(id, name, birthplace);
                            export.genres.Add(genre);
                        }
                    }
                }
            }

            // Выступление
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                using (var sqlCommand = new NpgsqlCommand
                {
                    Connection = conn,
                    CommandText = @"SELECT * FROM performance"
                })
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Int16 id = (Int16)reader["id"];
                            string name = (string)reader["name"];
                            DateTime perf_date = (DateTime)reader["perf_date"];
                            Int16 main_actor_id = (Int16)reader["main_actor_id"];
                            Int16 genre_id = (Int16)reader["genre_id"];
                            Int16 director_id = (Int16)reader["director_id"];
                            Int16 theatr_id = (Int16)reader["theatr_id"];

                            Performance performance = new Performance(id, main_actor_id, genre_id, director_id, theatr_id, name, perf_date);
                            export.performances.Add(performance);
                        }
                    }
                }
            }

            // Театр
            using (var conn = new NpgsqlConnection(connStr))
            {
                conn.Open();

                using (var sqlCommand = new NpgsqlCommand
                {
                    Connection = conn,
                    CommandText = @"SELECT * FROM theatr"
                })
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Int16 id = (Int16)reader["id"];
                            string name = (string)reader["name"];
                            string address = (string)reader["address"];
                            string city = (string)reader["city"];

                            Theatr theatr = new Theatr(id, name, address, city);
                            export.theatrs.Add(theatr);
                        }
                    }
                }
            }
            // Сериализация в строку
            string serialized_string = JsonConvert.SerializeObject(export).ToString();

            // Запуск интерпретатора Python 3.9.2
            string file_source = @"C:\Users\Pists\PycharmProjects\ExcellFill\main.py";
            string interpreterer = @"C:\Users\Pists\PycharmProjects\ExcellFill\venv\Scripts\python.exe";
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = interpreterer;

            // При передачи строки пропадают символы " и ' ', меняю на другие, затем кодирую обратно
            string coded = serialized_string.Replace('"', '*');
            coded = coded.Replace(' ', '?');
            start.Arguments = string.Format("{0} {1}", file_source, coded);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }

            Console.WriteLine("Enter для выхода...");
            Console.ReadLine();
        }
    }
}
