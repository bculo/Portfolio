namespace Notification.Hub.Extensions
{
    public static class SignalRConfiguration
    {
        public static void ConfigureSignalR(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("SignlarROptions:UseRedis"))
            {
                string connectionString = configuration["SignlarROptions:RedisConnection"];

                services.AddSignalR().AddStackExchangeRedis(connectionString, options => 
                {
                      options.Configuration.ChannelPrefix = "SignlarROptions:AppPrefix";
                });
            }
            else
            {
                services.AddSignalR();
            }
        }
    }
}
