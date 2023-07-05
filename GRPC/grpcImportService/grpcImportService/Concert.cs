namespace ImportService
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
}
