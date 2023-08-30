using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Persistence;

namespace ChurchMiceServer.Domains.Repositories;

public class ConfigurationRepository: Repository<Models.Configuration, string>, IConfigurationRepository
{
	public ConfigurationRepository(ChurchMiceContext context) : base(context) {}
}