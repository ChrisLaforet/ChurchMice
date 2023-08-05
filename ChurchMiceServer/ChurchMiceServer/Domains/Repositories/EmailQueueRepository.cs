using System.Collections;
using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Persistence;

namespace ChurchMiceServer.Domains.Repositories;

public class EmailQueueRepository : Repository<EmailQueue, string>, IEmailQueueRepository
{
    public EmailQueueRepository(ChurchMiceContext context) : base(context) {}
}