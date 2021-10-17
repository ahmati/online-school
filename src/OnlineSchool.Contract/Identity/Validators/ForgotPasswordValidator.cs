using FluentValidation;
using Microsoft.Extensions.Localization;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Identity.Validators
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPassword>
    {
        public ForgotPasswordValidator(IStringLocalizer<ResetPassword> localizer)
        {
            RuleFor(n => n.Email).NotNull().WithMessage(n => localizer[ValidationSharedResources.EmailRequired])
               .EmailAddress().WithMessage(n => localizer[ValidationSharedResources.ValidEmail]);
        }
    }
}
