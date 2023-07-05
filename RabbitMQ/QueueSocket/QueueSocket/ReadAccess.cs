using System;
using Npgsql;
using Newtonsoft.Json;
using System.Data.OleDb;

namespace QueueSocketClient
{
    public class ReadAccess
    {
        private static string connStrAccess = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=theatr.mdb;";
        private static OleDbConnection myConnectionAccess;

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


        public static List<string> GetStringByString()
        {
            List<string> Strings = new List<string>();

            myConnectionAccess = new OleDbConnection(connStrAccess);
            myConnectionAccess.Open();

            string query = "SELECT * FROM концерт";

            OleDbCommand command = new OleDbCommand(query, myConnectionAccess);

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

                    Concert currentConcert = new Concert(perf_date, perf_name, main_actor_name, main_actor_age, main_actor_experience, genre_name, genre_birthplace, director_name, director_birthdate, theatr_name, theatr_address, theatr_city);
                    Strings.Add(JsonConvert.SerializeObject(currentConcert).ToString());
                }
            }

            return Strings;
        }
    }
}
