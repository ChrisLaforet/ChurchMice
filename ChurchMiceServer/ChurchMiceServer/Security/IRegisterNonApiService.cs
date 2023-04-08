namespace ChurchMiceServer.Security;

public interface IRegisterNonApiService
{
    void registerRoute(String route);
    bool containsRoute(String route);
}