using Application.Activities.Commands;
using Application.Activities.DTO;
using Application.Activities.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<ActivityDTO>>> GetActivities()
    {
        return await Mediator.Send(new GetActivityList.Query());
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityDTO>> GetActivityDetails(string id)
    {
        return HandleResult(await Mediator.Send(new GetActivityDetails.Query { Id = id }));
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateActivity(CreateActivityDto activityDto)
    {
        return HandleResult(await Mediator.Send(new CreateActivity.Command { ActivityDto = activityDto }));
    }
    [HttpPut("{Id}")]
    [Authorize(Policy ="IsActivityHost")]
    public async Task<ActionResult<ActivityDTO>> UpdateActivity(string Id,EditActivityDto activity)
    {
        activity.Id = Id;
        return HandleResult(await Mediator.Send(new UpdateActivity.Command { ActivityDto = activity }));
    }
    [HttpDelete("{Id}")]
    [Authorize(Policy ="IsActivityHost")]
    public async Task<ActionResult> DeleteActivity(string Id)
    {
        return HandleResult(await Mediator.Send(new DeleteActivity.Command { Id = Id }));
    }
    
    [HttpPost("{Id}/attend")]
    public async Task<ActionResult> Attend(string Id)
    {
        
        return HandleResult(await Mediator.Send(new UpdateAttendance.Command { Id = Id }));
    }


}
