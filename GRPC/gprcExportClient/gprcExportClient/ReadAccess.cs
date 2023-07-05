using System.Data.OleDb;

namespace gprcExportClient
{
    public static class ReadAccess
    {
        private static string connStrAccess = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=theatr.mdb;";
        private static OleDbConnection myConnectionAccess;

        public static List<Concert> ReadConcert()
        {
            List <Concert> Strings = new List <Concert>(); 
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
                    Strings.Add(currentConcert);
                }
            }

            return Strings;
        }
    }
}
