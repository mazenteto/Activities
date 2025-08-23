using System;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class UpdateActivity
{
    public class Command : IRequest<Activity>
    {
        public required Activity Activity { get; set; }
    }
    public class Handler(AppDBContext context,IMapper mapper) : IRequestHandler<Command, Activity>
    {
        public async Task<Activity> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities
                .FindAsync([request.Activity.Id],cancellationToken)?? throw new Exception("Activity not found");
            mapper.Map(request.Activity, activity);
            await context.SaveChangesAsync(cancellationToken);
            return activity;
        }
    }

}
