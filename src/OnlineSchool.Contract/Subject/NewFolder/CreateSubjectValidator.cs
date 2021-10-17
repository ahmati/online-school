using FluentValidation;
using Microsoft.Extensions.Localization;
using OnlineSchool.Contract.Resources;
using OnlineSchool.Contract.Students;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Subject.NewFolder
{
    public class CreateSubjectValidator : AbstractValidator<CreateSubjectModel>
    {
        public CreateSubjectValidator(IStringLocalizer<CreateSubjectModel> localizer)
        {
            RuleFor(n => n.Name).NotNull().WithMessage(n => localizer[ValidationSharedResources.NameRequired])
                .Length(2, 50).WithMessage(n => localizer[ValidationSharedResources.SubjectNameLength]);

            RuleFor(n => n.Description).NotNull().WithMessage(n => localizer[ValidationSharedResources.Description])
                .MaximumLength(500).WithMessage(n => localizer[ValidationSharedResources.DescriptionLength]);

            RuleFor(n => n.Price.ToString()).NotNull().WithMessage(n => localizer[ValidationSharedResources.Price])
               .Matches(@"^\d+.?\d{0,2}$").WithMessage(n => localizer[ValidationSharedResources.ValidPassword])
               .MaximumLength(10).WithMessage(n => localizer[ValidationSharedResources.PriceLength]);

            RuleFor(n => n.Color).MaximumLength(50).WithMessage(n => localizer[ValidationSharedResources.ColorLength]);

        }

    }
}
