using ChurchMiceServer.Controllers.Models;
using ChurchMiceServer.CQS.CommandHandlers;
using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.QueryHandlers;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;
using ChurchMiceServer.Security;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMiceServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MemberController : ControllerBase
{
    private readonly IMemberProxy memberProxy;
    private readonly ILogger<UserController> logger;
    
    private readonly MemberQueryHandler memberQueryHandler;
    private readonly CreateMemberCommandHandler createMemberCommandHandler;
    
    public MemberController(IServiceProvider serviceProvider, IMemberProxy userProxy, ILogger<UserController> logger)
    {
        this.memberProxy = memberProxy;
        this.logger = logger;
        
        this.memberQueryHandler = ActivatorUtilities.CreateInstance<MemberQueryHandler>(serviceProvider);
        this.createMemberCommandHandler = ActivatorUtilities.CreateInstance<CreateMemberCommandHandler>(serviceProvider);
    }

    [HttpGet("getMemberById")]
    [Authorize]
    public MemberResponse GetMemberById(string id)
    {
        try
        {
            return memberQueryHandler.Handle(new MemberQuery(id));
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Debug, "Error retrieving member record", ex);
        }

        return null;
    }

    [HttpPost("create")]
    [Authorize]
    public MemberResponse CreateMember(MemberRequest request)
    {
        var identity = this;
        return null;
        //return createMemberCommandHandler.Handle(new CreateMemberCommand(request));
    }
}