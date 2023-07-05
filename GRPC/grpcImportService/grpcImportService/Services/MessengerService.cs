using Grpc.Core;
using ImportService;  // пространство имен сервиса MessengerService
using Google.Protobuf.WellKnownTypes;

public class MessengerService : Messenger.MessengerBase
{
    public override async Task<Response> ClientDataStream(IAsyncStreamReader<Request> requestStream, ServerCallContext context)
    {
        Concert TempConcert;
        await foreach (Request request in requestStream.ReadAllAsync())
        {
            Console.WriteLine(request.Perfname);
            TempConcert = new Concert
            (
                request.Perfdate.ToDateTime(),
                request.Perfname,
                request.Mainactorname,
                request.Mainactorage,
                request.Mainactorexperience,
                request.Genrename,
                request.Genrebirthplace,
                request.Directorname,
                request.Directorbirthdate.ToDateTime(),
                request.Theatrname,
                request.Theatraddress,
                request.Theatrcity
            );

            WritePostgres.InsertString(TempConcert);
        }
        Console.WriteLine("Все данные получены...");
        return new Response { Content = "все данные получены" };
    }
}