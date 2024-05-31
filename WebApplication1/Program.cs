public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("/webhook", async context =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();
                var message = body.ToString();
                Console.WriteLine(message);
                string mess_id = message.Substring(0, 19);
                string chat_id = message.Substring(19, 19);
                string username = message.Substring(38);
                Console.WriteLine(mess_id);
                Console.WriteLine(username);
                if (username == "deadgaffer") // Укажите имя пользователя, которого вы отслеживаете
                {
                    var client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Put,
                        $"https://discord.com/api/v9/channels/{chat_id}/messages/{mess_id}/reactions/%F0%9F%91%8D/%40me?location=Message&type=0");
                    request.Headers.Add("Host", "discord.com");
                    request.Headers.Add("Connection", "keep-alive");
                    request.Headers.Add("Referer", "https://discord.com/channels/@me/1245967394761871381");
                    request.Headers.Add("Authorization",
                        "MTIyMjI0MjY1NDg2ODIwOTc3NQ.Gk6XWw.2H3z83H61Ia0Tn9lvlQ5bmvs4_NME9yWmESUAk");//вторым значением укажите свой токен
                    request.Headers.Add("Cookie",
                        "__cfruid=6e665a5ac458e0ecc81134985c607b05cf39d545-1717133465; __dcfduid=f9c210061f0e11ef85962222dbdcba35; __sdcfduid=f9c210061f0e11ef85962222dbdcba3559d3f61a1e061afcf7ea120a5dfa93ff9afa8d86eafc62db5aed9fdbff5ea159; _cfuvid=2xH1Mz7lg1gw04lHpICGdG3ta4TobHDnSiLBrWxGpcA-1717133465264-0.0.1.1-604800000");
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }

                context.Response.StatusCode = StatusCodes.Status200OK;
            });
        });
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
            .Build()
            .Run();
    }
}

public class DiscordMessage
{
    public string ID { get; set; }
    public string ChannelId { get; set; }
}