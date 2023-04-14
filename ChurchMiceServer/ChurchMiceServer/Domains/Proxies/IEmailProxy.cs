namespace ChurchMiceServer.Domains.Proxies;

public interface IEmailProxy
{
    void SendMessageTo(string to, string subject, string body);
}