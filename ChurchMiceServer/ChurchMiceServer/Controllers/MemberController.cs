﻿using ChurchMiceServer.Controllers.Models;
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

    public MemberController(IServiceProvider serviceProvider, ILogger<MemberController> logger)
    {
        this.logger = logger;

        if (serviceProvider != null)
        {
            this.MemberQueryHandler = ActivatorUtilities.CreateInstance<MemberQueryHandler>(serviceProvider);
            this.CreateMemberCommandHandler = ActivatorUtilities.CreateInstance<CreateMemberCommandHandler>(serviceProvider);
            this.UpdateMemberCommandHandler = ActivatorUtilities.CreateInstance<UpdateMemberCommandHandler>(serviceProvider);
        }
    }

    [HttpGet("getMemberById")]
    [Authorize]
    public MemberResponse GetMemberById(string id)
    {
        try
        {
            return MemberQueryHandler.Handle(new MemberQuery(id));
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
        return CreateMemberCommandHandler.Handle(new CreateMemberCommand(request,  HttpContext.User.Identity.Name));
    }
    
    [HttpPut("update")]
    [Authorize]
    public MemberResponse UpdateMember(MemberRequest request)
    {
        return UpdateMemberCommandHandler.Handle(new UpdateMemberCommand(request,  HttpContext.User.Identity.Name));
    }
}