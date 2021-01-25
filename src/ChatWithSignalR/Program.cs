using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ChatWithSignalR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "server")
                CreateHostBuilder(args).Build().Run();
            else
                new MyClient();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<MyServer>();
                });
    }
}
