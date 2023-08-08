using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Persistence;

namespace ChurchMiceServer.Domains.Repositories;

public class MemberImageRepository : Repository<MemberImage, int>, IMemberImageRepository
{
    public MemberImageRepository(ChurchMiceContext context) : base(context) {}
}