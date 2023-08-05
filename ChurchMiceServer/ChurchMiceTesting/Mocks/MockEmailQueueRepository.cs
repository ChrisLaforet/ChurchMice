using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;

namespace ChurchMiceTesting.Mocks;

public class MockEmailQueueRepository : MockRepository<EmailQueue, string>, IEmailQueueRepository
{
}