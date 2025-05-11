using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CisAPI.Dtos.Topics;
using FluentValidation;

namespace CisAPI.validations.Topics;
public class UpdateTopicDtoValidator : AbstractValidator<UpdateTopicDto>
{
      public UpdateTopicDtoValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(255).WithMessage("Title must be 255 characters or fewer.");

        RuleFor(x => x.Description);
    }
}
