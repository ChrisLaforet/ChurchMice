using ChurchMiceServer.CQS.Commands;
using ChurchMiceServer.CQS.Exceptions;
using ChurchMiceServer.CQS.Responses;
using ChurchMiceServer.Domains.Models;
using ChurchMiceServer.Domains.Proxies;
using ChurchMiceServer.Security.Auth;
using ChurchMiceServer.Utility;

namespace ChurchMiceServer.CQS.CommandHandlers;

public class UploadImageCommandHandler : ICommandHandler<UploadImageCommand, NothingnessResponse>
{
	private readonly IUserProxy userProxy;
	private readonly IMemberProxy memberProxy;
	private readonly ILogger<UploadImageCommandHandler> logger;
    
	public UploadImageCommandHandler(IUserProxy userProxy, IMemberProxy memberProxy, ILogger<UploadImageCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.memberProxy = memberProxy;
		this.logger = logger;
	}

	public NothingnessResponse Handle(UploadImageCommand command)
	{
		var isAdmin = userProxy.GetAssignedRoleLevelCodeFor(command.UploadUserId) == Role.GetAdministrator().Code;
		var member = memberProxy.GetMember(command.MemberId);
		if (!isAdmin)
		{
			CheckIfUserIsPermittedToUploadForMember(command.UploadUserId, member);
		}

		var memberImage = PrepareAndSaveImageFor(member, command);
		if (isAdmin)
		{
			memberProxy.ApproveMemberImage(memberImage.Id);
			logger.LogInformation($"Approved newly uploaded image for {member.Id} by user {command.UploadUserId}");
		}

		return new NothingnessResponse();
	}

	private void CheckIfUserIsPermittedToUploadForMember(string userId, Member member)
	{
		// does the user have permission to upload an image on behalf of the member (either self, editor, or admin)
		if (member.UserId != null && member.UserId == userId)
		{
			return;
		}

		if (memberProxy.GetEditorsForMember(member).SingleOrDefault(editor => editor.EditorId == userId) == null)
		{
			logger.LogWarning($"Attempt to upload image for {member.Id} by user {userId} rejected because they don't have editing rights");
			throw new UserNotPermittedException(userId, $"becaus user does not have editing rights to the member records for {member.Id}");
		}
	}

	private MemberImage PrepareAndSaveImageFor(Member member, UploadImageCommand command)
	{
		var compressedImage = DecodeAndCompressFileContent(command.FileContentBase64);
		var memberImage = memberProxy.AddMemberImageFor(member, command.UploadUserId, compressedImage, command.FileType);

		logger.LogInformation($"Uploaded new image for {member.Id} by user {command.UploadUserId}");

		return memberImage;
	}

	private string DecodeAndCompressFileContent(string fileContentBase64)
	{
		return Compression.CompressToBase64(Convert.FromBase64String(fileContentBase64));
	}
}