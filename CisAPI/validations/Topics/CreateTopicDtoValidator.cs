using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CisAPI.Dtos.Topics;
using FluentValidation;

namespace CisAPI.validations.Topics;

public class CreateTopicDtoValidator : AbstractValidator<CreateTopicDto>
{
    public CreateTopicDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255).WithMessage("Title must be 255 characters or fewer.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");
    }  
}
