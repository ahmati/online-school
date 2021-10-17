using FluentValidation;
using Microsoft.Extensions.Localization;
using OnlineSchool.Contract.Resources;
using OnlineSchool.Contract.Students;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Teachers.Validators
{
    public class UpdateTeacherValidator : AbstractValidator<UpdateTeacherModel>
    {
        public UpdateTeacherValidator(IStringLocalizer<UpdateTeacherModel> localizer)
        {
            RuleFor(n => n.Name).NotNull().WithMessage(n => localizer[ValidationSharedResources.NameRequired])
                .Length(2, 100).WithMessage(n => localizer[ValidationSharedResources.NameLength]);

            RuleFor(n => n.Surname).NotNull().WithMessage(n => localizer[ValidationSharedResources.SurnameRequired])
                .Length(2, 100).WithMessage(n => localizer[ValidationSharedResources.SurnameLength]);

            RuleFor(n => n.Gender).NotNull().WithMessage(n => localizer[ValidationSharedResources.GenderRequired]);

            RuleFor(n => n.Description).MaximumLength(500).WithMessage(n => localizer[ValidationSharedResources.DescriptionLength]);
            RuleFor(n => n.ImagePath).NotNull().MaximumLength(500).WithMessage(n => localizer[ValidationSharedResources.ImagePathLength]);


        }

    }
}
