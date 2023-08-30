using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;

namespace ChurchMiceTesting.Mocks;

public class MockConfigurationRepository : MockRepository<Configuration, string>, IConfigurationRepository
{
}