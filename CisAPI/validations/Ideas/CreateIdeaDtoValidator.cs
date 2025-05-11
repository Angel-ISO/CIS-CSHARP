using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CisAPI.Dtos.Ideas;
using FluentValidation;

namespace CisAPI.validations.Ideas
{
    public class CreateIdeaDtoValidator : AbstractValidator<CreateIdeaDto>
    {
        public CreateIdeaDtoValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty().WithMessage("TopicId is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(255).WithMessage("Title must be 255 characters or fewer.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.");
        }
    }

}