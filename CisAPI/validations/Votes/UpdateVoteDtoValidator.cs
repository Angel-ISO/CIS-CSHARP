using FluentValidation;
using CisAPI.Dtos.Votes;

namespace CisAPI.Validations.Votes
{
    public class UpdateVoteDtoValidator : AbstractValidator<UpdateVoteDto>
    {
        public UpdateVoteDtoValidator()
        {
            RuleFor(x => x.Value)
                .InclusiveBetween(-1, 1).WithMessage("Vote value must be either -1 (dislike) or 1 (like).");
        }

        private bool BeAValidGuid(Guid id)
        {
            return id != Guid.Empty;
        }
    }
}
