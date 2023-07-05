using System;
using Grpc.Net.Client;
using System.Configuration;
using Google.Protobuf.WellKnownTypes;

namespace gprcExportClient
{
    class Program
    {   
        public static void Main(string[] args)
        {
            Run();
            Console.ReadKey();
        }

        async static void Run()
        {



            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            using (var channel = GrpcChannel.ForAddress("https://" + Config.Host + ':' + Config.Port, new GrpcChannelOptions { HttpHandler = httpHandler }))
            {
                var client = new Messenger.MessengerClient(channel);
                var call = client.ClientDataStream();

                List<Concert> Concerts = ReadAccess.ReadConcert();

                foreach (var concert in Concerts)
                {
                    await call.RequestStream.WriteAsync(new Request 
                    {
                        Content = "",
                        Perfdate = Timestamp.FromDateTimeOffset(concert.perf_date),
                        Perfname = concert.perf_name,
                        Mainactorname = concert.main_actor_name,
                        Mainactorage = concert.main_actor_age,
                        Mainactorexperience = concert.main_actor_experience,
                        Genrename = concert.genre_name,
                        Genrebirthplace = concert.genre_birthplace,
                        Directorname = concert.director_name,
                        Directorbirthdate = Timestamp.FromDateTimeOffset(concert.director_birthdate),
                        Theatrname = concert.theatr_name,
                        Theatraddress = concert.theatr_address,
                        Theatrcity = concert.theatr_city,
                    });
                }

                await call.RequestStream.CompleteAsync();
                Response response = await call.ResponseAsync;
                Console.WriteLine($"Ответ сервера: {response.Content}");
            } 
        }
    }
}