using ChurchMiceServer.Controllers.Models;
using ChurchMiceServer.CQS.CommandHandlers;
using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.QueryHandlers;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Models;
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
    public DeleteMemberCommandHandler DeleteMemberCommandHandler { get; set; }
    public UploadImageCommandHandler UploadImageCommandHandler { get; set; }
    public GetMemberImagesQueryHandler GetMemberImagesQueryHandler { get; set; }

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
            this.DeleteMemberCommandHandler = ActivatorUtilities.CreateInstance<DeleteMemberCommandHandler>(serviceProvider);
            this.UploadImageCommandHandler = ActivatorUtilities.CreateInstance<UploadImageCommandHandler>(serviceProvider);
            this.GetMemberImagesQueryHandler = ActivatorUtilities.CreateInstance<GetMemberImagesQueryHandler>(serviceProvider);
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
    
    [HttpDelete("delete/{memberId}")]
    [Authorize(Roles = "Administrator")]
    public IActionResult DeleteMember(int memberId)
    {
        try 
        {
            DeleteMemberCommandHandler.Handle(new DeleteMemberCommand(memberId,  HttpContext.User.Identity.Name));
            return Ok(new {message = "Deleted member successfully"});
        }
        catch (Exception ex)
        {
            return BadRequest(new {message = "Error deleting member"});
        }
    }

	[HttpPost("uploadImage")]
	[Authorize]
	public IActionResult UploadImage(UploadImageRequest request)
    {
        try 
        {
            var possibleUser = (User)this.Request.HttpContext.Items["User"];
            var command = new UploadImageCommand(possibleUser.Id, request.MemberId, request.FileContentBase64, request.FileName, request.FileType, request.FileSize);
            UploadImageCommandHandler.Handle(command);
            return Ok(new {message = "Image has been accepted"});
        }
        catch (Exception ex)
        {
            return BadRequest(new {message = "Image cannot be accepted"});
        }
	}

    [HttpGet("getImages/{memberId}")]
    [Authorize]
    public MemberImagesResponse GetImages(int memberId)
    {
        try
        {
            var possibleUser = (User)this.Request.HttpContext.Items["User"];
            return GetMemberImagesQueryHandler.Handle(new GetMemberImagesQuery(memberId, possibleUser.Id));
        }
        catch (Exception ex)
        {
            logger.LogDebug($"Error retrieving member image records {ex}");
        }

        return new MemberImagesResponse();
    }
}