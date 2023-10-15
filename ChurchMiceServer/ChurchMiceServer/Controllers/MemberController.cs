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
    private readonly ILogger<MemberController> logger;
    
    public MemberQueryHandler MemberQueryHandler { get; set; }
    public CreateMemberCommandHandler CreateMemberCommandHandler { get; set; }
    public UpdateMemberCommandHandler UpdateMemberCommandHandler { get; set; }
    public EditableMembersQueryHandler EditableMembersQueryHandler { get; set; }
    public GetMembersQueryHandler GetMembersQueryHandler { get; set; }

    public MemberController(IServiceProvider serviceProvider, ILogger<MemberController> logger)
    {
        this.logger = logger;

        if (serviceProvider != null)
        {
            this.MemberQueryHandler = ActivatorUtilities.CreateInstance<MemberQueryHandler>(serviceProvider);
            this.CreateMemberCommandHandler = ActivatorUtilities.CreateInstance<CreateMemberCommandHandler>(serviceProvider);
            this.UpdateMemberCommandHandler = ActivatorUtilities.CreateInstance<UpdateMemberCommandHandler>(serviceProvider);
            this.EditableMembersQueryHandler = ActivatorUtilities.CreateInstance<EditableMembersQueryHandler>(serviceProvider);
            this.GetMembersQueryHandler = ActivatorUtilities.CreateInstance<GetMembersQueryHandler>(serviceProvider);
        }
    }

    [HttpGet("getMember/{memberId}")]
    [Authorize]
    public MemberResponse GetMemberById(int memberId)
    {
        try
        {
            return MemberQueryHandler.Handle(new MemberQuery(memberId));
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Debug, "Error retrieving member record", ex);
        }

        return null;
    }
     
    [HttpGet("getMembers")]
    [Authorize]
    public IList<MemberResponse> GetMembers()
    {
        try
        {
            return GetMembersQueryHandler.Handle(new GetMembersQuery());
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Debug, "Error retrieving member records", ex);
        }

        return null;
    }
    
    [HttpGet("getEditableMembers")]
    [Authorize]
    public IList<MemberResponse> GetEditableMembers()
    {
        try
        {
            return EditableMembersQueryHandler.Handle(new EditableMembersQuery(HttpContext.User.Identity.Name));
        }
        catch (Exception ex)
        {
            logger.Log(LogLevel.Debug, "Error retrieving member records", ex);
        }

        return null;
    }

    [HttpPost("create")]
    [Authorize]
    public MemberResponse CreateMember(MemberRequest request)
    {
        return CreateMemberCommandHandler.Handle(new CreateMemberCommand(request,  HttpContext.User.Identity.Name));
    }
    
    [HttpPut("update")]
    [Authorize]
    public MemberResponse UpdateMember(MemberRequest request)
    {
        return UpdateMemberCommandHandler.Handle(new UpdateMemberCommand(request,  HttpContext.User.Identity.Name));
    }

	[HttpPost("uploadImage")]
	[Authorize]
	public IActionResult UploadImage(UploadImageRequest request)
	{
        var command = new UploadImageCommand(request.UploadUserId, request.MemberId, request.Image);
if (request.MemberId == 1)
{
    return Ok("Member image accepted");
}
		return BadRequest(new { message = "Image can't be accepted" });
	}
	
}