using ChurchMiceServer.Domains.Interfaces;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Persistence;

namespace ChurchMiceServer.Domains.Repositories;

public class MemberEditorRepository : Repository<MemberEditor, int>, IMemberEditorRepository
{
	public MemberEditorRepository(ChurchMiceContext context) : base(context) {}
	
}