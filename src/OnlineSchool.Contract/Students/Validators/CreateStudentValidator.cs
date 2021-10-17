using FluentValidation;
using Microsoft.Extensions.Localization;
using OnlineSchool.Contract.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Students.Validators
{
    public class CreateStudentValidator : AbstractValidator<CreateStudentModel>
    {
        public CreateStudentValidator(IStringLocalizer<CreateStudentModel> localizer)
        {
            RuleFor(n => n.Name).NotNull().WithMessage(n => localizer[ValidationSharedResources.NameRequired])
                .Length(2, 100).WithMessage(n => localizer[ValidationSharedResources.NameLength]);

            RuleFor(n => n.Surname).NotNull().WithMessage(n => localizer[ValidationSharedResources.SurnameRequired])
                .Length(2, 100).WithMessage(n => localizer[ValidationSharedResources.SurnameLength]);

            RuleFor(n => n.Gender).NotNull().WithMessage(n => localizer[ValidationSharedResources.GenderRequired]);

            RuleFor(n => n.Email).NotNull().WithMessage(n => localizer[ValidationSharedResources.EmailRequired])
                .EmailAddress().WithMessage(n => localizer[ValidationSharedResources.ValidEmail]);

            RuleFor(n => n.Password).NotNull().WithMessage(n => localizer[ValidationSharedResources.PasswordRequired])
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$").WithMessage(ValidationSharedResources.ValidPassword);

            RuleFor(n => n.Description).MaximumLength(500).WithMessage(n => localizer[ValidationSharedResources.DescriptionLength]);
            RuleFor(n => n.ImagePath).MaximumLength(500).WithMessage(n => localizer[ValidationSharedResources.ImagePathLength]);


        }

    }
}
