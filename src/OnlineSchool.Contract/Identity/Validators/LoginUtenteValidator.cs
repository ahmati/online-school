using Microsoft.Extensions.Localization;
using OnlineSchool.Contract.Identity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using OnlineSchool.Contract.Resources;

namespace OnlineSchool.Contract.Identity.Validators
{
    public class LoginUtenteValidator : AbstractValidator<LoginUtente>
    {
        public LoginUtenteValidator(IStringLocalizer<LoginUtente> localizer)
        {
            RuleFor(n => n.Email).NotNull().WithMessage(n => localizer[ValidationSharedResources.EmailRequired])
               .EmailAddress().WithMessage(n => localizer[ValidationSharedResources.ValidEmail]);
            RuleFor(n => n.Password).NotNull().WithMessage(n => localizer[ValidationSharedResources.PasswordRequired]);
        }
    }
}
