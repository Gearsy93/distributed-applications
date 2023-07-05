using System;

namespace ImportService
{
    class Program
    {
        public static void Main(string[] args)
        {
            Run(args);
        }

        async public static void Run(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpc();

            var app = builder.Build();

            // встраиваем MessengerService в обработку запроса
            app.MapGrpcService<MessengerService>();
            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}


