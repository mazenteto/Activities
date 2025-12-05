using System;
using Application.Activities.DTO;
using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class UpdateActivity
{
    public class Command : IRequest<Result<ActivityDTO>>
    {
        public required EditActivityDto ActivityDto { get; set; }
    }
    public class Handler(AppDBContext context,IMapper mapper) : IRequestHandler<Command, Result<ActivityDTO>>
    {
        public async Task<Result<ActivityDTO>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities
                .FindAsync([request.ActivityDto.Id], cancellationToken);
                if(activity==null) return Result<ActivityDTO>.Failure("Activity not found",404);
            mapper.Map(request.ActivityDto, activity);
            var result=await context.SaveChangesAsync(cancellationToken)>0;
            if(!result) return Result<ActivityDTO>.Failure("Faild to Update the activity",400);
            return Result<ActivityDTO>.Success(mapper.Map<ActivityDTO>(activity));
        }
    }

}
