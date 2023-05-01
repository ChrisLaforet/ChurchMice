﻿using ChurchMiceServer.CQS.Queries;
using ChurchMiceServer.CQS.QueryHandlers;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Proxies;
using ChurchMiceServer.Security;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMiceServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MemberController
{
    private readonly IMemberProxy memberProxy;
    private readonly ILogger<UserController> logger;
    
    private readonly MemberQueryHandler memberQueryHandler;
    
    public MemberController(IServiceProvider serviceProvider, IMemberProxy userProxy, ILogger<UserController> logger)
    {
        this.memberProxy = memberProxy;
        this.logger = logger;
        
        this.memberQueryHandler = ActivatorUtilities.CreateInstance<MemberQueryHandler>(serviceProvider);
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
        
    }
}