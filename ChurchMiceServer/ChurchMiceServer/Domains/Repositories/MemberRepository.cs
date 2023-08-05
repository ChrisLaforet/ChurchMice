using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Persistence;

namespace ChurchMiceServer.Domains.Repositories;

public class MemberRepository : Repository<Member>, IMemberRepository
{
    public MemberRepository(ChurchMiceContext context) : base(context) {}
}