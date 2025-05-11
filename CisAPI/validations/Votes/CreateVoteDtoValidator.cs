using System;
using FluentValidation;
using CisAPI.Dtos.Votes;

namespace CisAPI.Validations.Votes
{
    public class CreateVoteDtoValidator : AbstractValidator<CreateVoteDto>
    {
        public CreateVoteDtoValidator()
        {

            RuleFor(x => x.IdeaId)
                .NotEmpty().WithMessage("Idea ID is required.")
                .WithMessage("Idea ID must be a valid GUID.");

            RuleFor(x => x.Value)
                .InclusiveBetween(-1, 1).WithMessage("Vote value must be either -1 (dislike) or 1 (like).");
        }

    }
}
