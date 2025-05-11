using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CisAPI.Dtos.Ideas;
using FluentValidation;

namespace CisAPI.validations.Ideas
{
    public class UpdateIdeaDtoValidator : AbstractValidator<UpdateIdeaDto>
    {
        public UpdateIdeaDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(255).WithMessage("Title must be 255 characters or fewer.");

            RuleFor(x => x.Content);
        }
    }

}