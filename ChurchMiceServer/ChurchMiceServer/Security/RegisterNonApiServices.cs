using Microsoft.VisualBasic;

namespace ChurchMiceServer.Security;

public class RegisterNonApiServices : IRegisterNonApiService
{
    private readonly List<String> nonApiServiceRoutes = new List<string>();
    //private readonly ILogger<RegisterNonApiServices> logger;

    // public RegisterNonApiServices(ILogger<RegisterNonApiServices> logger)
    // {
    //     this.logger = logger;
    // }
    
    public RegisterNonApiServices() {}

    public void registerRoute(string route)
    {
        if (!nonApiServiceRoutes.Contains(route))
        {
            //logger.Log(LogLevel.Trace,"Adding route " + route);
            nonApiServiceRoutes.Add(route);
        }
    }

    public bool containsRoute(string route)
    {
        foreach (var nonApiRoute in nonApiServiceRoutes)
        {
            if (route.Equals(nonApiRoute))
            {
                return true;
            }
        }
        return false;
    }
}