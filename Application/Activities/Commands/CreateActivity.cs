using System;
using Application.Activities.DTO;
using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class CreateActivity
{
    public class Command : IRequest<Result<string>>
    {
        public required CreateActivityDto ActivityDto { get; set; }
    }

    public class Handler(AppDBContext context,IMapper mapper) : IRequestHandler<Command, Result<string>>
    {
        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            
            var Activity = mapper.Map<Activity>(request.ActivityDto);
            context.Activities.Add(Activity);
            var result=await context.SaveChangesAsync(cancellationToken)>0;
            if(!result) return Result<string>.Failure("Faild to create the activity",400);
            return Result<string>.Success(Activity.Id);
        }
    }

}
