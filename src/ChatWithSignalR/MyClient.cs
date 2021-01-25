using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatWithSignalR
{
    public class MyClient
    {
        private HubConnection connection;
        private string user;

        public MyClient()
        {
            StartConnectionAsync().Wait();
            InitChat().Wait();
            ChatingAsync().Wait();
        }

        private async Task StartConnectionAsync()
        {
            connection = new HubConnectionBuilder()
               .WithUrl("http://localhost:5000/chat")
               .Build();

            await connection.StartAsync();

            while (connection.State != HubConnectionState.Connected)
                Console.WriteLine(connection.State);

            connection.On<string, string>("ReceiveMessage", (fromuser, message) =>
            {
                if (!fromuser.Equals(user))
                    Console.WriteLine($"{fromuser} diz: {message}");
            });

            connection.Closed += async (error) =>
            {
                Console.WriteLine("Conexão perdida.");
                Environment.Exit(0);
                await Task.CompletedTask;
            };
        }

        private Task InitChat()
        {
            user = string.Empty;
            while (string.IsNullOrWhiteSpace(user))
            {
                Console.WriteLine("Informe seu nome:");
                user = Console.ReadLine();
                Console.Clear();
            }

            Console.WriteLine("###################################################");
            Console.WriteLine($"Olá {user}, bem vindo(a) ao chat!");
            Console.WriteLine("###################################################");

            return connection?.InvokeAsync("SendMessage", "SISTEMA", $"{user} entrou no chat");
        }

        private async Task ChatingAsync()
        {
            var message = string.Empty;
            while (string.IsNullOrWhiteSpace(message))
            {
                message = Console.ReadLine();

                if (connection.State != HubConnectionState.Connected)
                {
                    Console.WriteLine("Not connected");
                    message = string.Empty;
                }

                if (message == "sair")
                    Environment.Exit(0);

                if (!string.IsNullOrWhiteSpace(message))
                {
                    await connection?.InvokeAsync("SendMessage", user, message);
                    message = string.Empty;
                }
            }
        }
    }
}
