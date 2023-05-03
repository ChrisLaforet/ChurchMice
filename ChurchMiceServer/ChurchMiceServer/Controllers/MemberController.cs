using ChurchMiceServer.Controllers.Models;
using ChurchMiceServer.CQS.CommandHandlers;
using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.QueryHandlers;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Security;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMiceServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MemberController : ControllerBase
{
    private readonly ILogger<UserController> logger;
    
    private readonly MemberQueryHandler memberQueryHandler;
    private readonly CreateMemberCommandHandler createMemberCommandHandler;
    private readonly UpdateMemberCommandHandler updateMemberCommandHandler;

    public MemberController(IServiceProvider serviceProvider, ILogger<UserController> logger)
    {
        this.logger = logger;
        
        this.memberQueryHandler = ActivatorUtilities.CreateInstance<MemberQueryHandler>(serviceProvider);
        this.createMemberCommandHandler = ActivatorUtilities.CreateInstance<CreateMemberCommandHandler>(serviceProvider);
        this.updateMemberCommandHandler = ActivatorUtilities.CreateInstance<UpdateMemberCommandHandler>(serviceProvider);
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
        return createMemberCommandHandler.Handle(new CreateMemberCommand(request,  HttpContext.User.Identity.Name));
    }
    
    [HttpPut("update")]
    [Authorize]
    public MemberResponse UpdateMember(MemberRequest request)
    {
        return updateMemberCommandHandler.Handle(new UpdateMemberCommand(request,  HttpContext.User.Identity.Name));
    }
}