using FluentValidation;
using Microsoft.Extensions.Localization;
using OnlineSchool.Contract.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Identity.Validators
{
    public class ResetUserPasswordValidator : AbstractValidator<ResetUserPassword>
    {
        public ResetUserPasswordValidator(IStringLocalizer<ResetUserPassword> localizer)
        {
            RuleFor(n => n.Email).NotNull().WithMessage(n => localizer[ValidationSharedResources.EmailRequired])
              .EmailAddress().WithMessage(n => localizer[ValidationSharedResources.ValidEmail]);
            RuleFor(n => n.Password).NotNull().WithMessage(n => localizer[ValidationSharedResources.PasswordRequired])
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$").WithMessage(ValidationSharedResources.ValidPassword);
        }
    }
}
