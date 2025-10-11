using Application.Activities.Commands;
using Application.Activities.DTO;
using Application.Activities.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<Activity>>> GetActivities()
    {
        return await Mediator.Send(new GetActivityList.Query());
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> GetActivityDetails(string id)
    {
        return HandleResult(await Mediator.Send(new GetActivityDetails.Query { Id = id }));
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateActivity(CreateActivityDto activityDto)
    {
        return HandleResult(await Mediator.Send(new CreateActivity.Command { ActivityDto = activityDto }));
    }
    [HttpPut]
    public async Task<ActionResult<Activity>> UpdateActivity(EditActivityDto activity)
    {
        return HandleResult(await Mediator.Send(new UpdateActivity.Command { ActivityDto = activity }));
    }
    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteActivity(string Id)
    {
      return  HandleResult(await Mediator.Send(new DeleteActivity.Command { Id = Id }));
    }

}
