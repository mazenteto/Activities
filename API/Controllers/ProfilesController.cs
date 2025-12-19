using System;
using Application.Profiles.Commands;
using Application.Profiles.DTOs;
using Application.Profiles.Queries;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfilesController:BaseApiController
{
    [HttpPost("add-photo")]
    public async Task<ActionResult<Photo>> AddPhoto(AddPhoto.Command command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpGet("{UserId}/Photos")]
    public async Task<ActionResult<List<Photo>>> GetPhotosForUser(string UserId)
    {
        return HandleResult(await Mediator.Send(new GetProfilePhotos.Query{UserId=UserId}));
    }

    [HttpDelete("{photoId}/photos")]
    public async Task<ActionResult> DeletePhoto(string photoId)
    {
        return HandleResult(await Mediator.Send(new DeletePhoto.Command{PhotoId=photoId}));
    }
    [HttpPut("{photoId}/setMain")]
    public async Task<ActionResult> SetMainPhoto(string photoId)
    {
        return HandleResult(await Mediator.Send(new SetMainPhoto.Command{PhotoId=photoId}));
    }

    [HttpGet("{UserId}")]
    public async Task<ActionResult<UserProfile>> GetProfile(string UserId)
    {
        return HandleResult(await Mediator.Send(new GetProfile.Query{UserId=UserId}));
    }

    [HttpPost("{userId}/follow")]
    public async Task<ActionResult> FollowToggle(string userId)
    {
        return HandleResult(await Mediator
            .Send(new FollowToggle.Command{TargetUserId=userId}));
    }

     [HttpGet("{userId}/follow-list")]
    public async Task<ActionResult> GetFollowings(string userId, string predicate)
    {
        return HandleResult(await Mediator
            .Send(new GetFollowings.Query{UserId=userId,predicate=predicate}));
    }


}
