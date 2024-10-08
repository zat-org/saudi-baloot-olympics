﻿using System.Text.RegularExpressions;
using FluentValidation;

namespace BalootOlympicsTeamsApi.Common;

public static class InputValidators
{
    public static IRuleBuilderOptions<T, string?> Name<T>(this IRuleBuilder<T, string?> ruleBuilder, string propName)
    {
        return ruleBuilder
        .NotEmpty()
        .WithName(propName)
        .MinimumLength(2)
        .Matches(@"^[\p{L}0-9\s]+$")
        .WithMessage("يجب ان يحتوى {PropertyName} علي حروف عربية او انجليزية او ارقام");
    }
    public static IRuleBuilderOptions<T, string?> OTPCode<T>(this IRuleBuilder<T, string?> ruleBuilder, string propName)
    {
        return ruleBuilder
        .NotEmpty()
        .WithName(propName)
        .Length(6, 6)
        .Matches(@"^\d{6}$")
        .WithMessage("{PropertyName} غير صالح");
    }
    public static IRuleBuilderOptions<T, Guid> GuidId<T>(this IRuleBuilder<T, Guid> ruleBuilder, string propName)
    {
        return ruleBuilder
        .NotEmpty()
            .WithName(propName)
            .Configure(c => c.CascadeMode = CascadeMode.Stop)
            .Must(v => v != Guid.Empty)
            .WithMessage("يجب الا يكون {PropertyName} خاليا");
    }
    public static IRuleBuilderOptions<T, string?> Username<T>(this IRuleBuilder<T, string?> ruleBuilder, string propName)
    {
        return ruleBuilder
        .NotEmpty()
        .Configure(config => config.CascadeMode = CascadeMode.Stop)
        .WithName(propName)
        .Length(3, 25)
        .Matches("^[a-zA-Z][a-zA-Z0-9_.-]*$")
        .WithMessage("يجب ان يتكون {PropertyName} من حروف انجليزية او ارقام او _ . -");

    }
    public static IRuleBuilderOptions<T, string?> Password<T>(this IRuleBuilder<T, string?> ruleBuilder, string propName)
    {
        return ruleBuilder
        .NotEmpty()
        .Configure(config => config.CascadeMode = CascadeMode.Stop)
        .WithName(propName)
        .MinimumLength(6)
        .Matches(@"^(?=.*[a-zA-Z])(?=.*\d).{6,}$")
        .WithMessage("يجب ان تحتوي {PropertyName} علي حروف و ارقام");
    }
    public static IRuleBuilderOptions<T, string?> FCM_Token<T>(this IRuleBuilder<T, string?> ruleBuilder, string propName)
    {
        return ruleBuilder
        .NotEmpty()
        .WithName(propName)
        .WithMessage("Invalid FCM Token");
    }
    public static IRuleBuilderOptions<T, DateTime?> BirthDate<T>(this IRuleBuilder<T, DateTime?> ruleBuilder, string propName)
    {
        return ruleBuilder
        .NotEmpty()
        .WithName(propName)
        .LessThanOrEqualTo(DateTime.UtcNow.AddYears(-7))
        .WithMessage("برجاء ادخال {PropertyName} صحيح")
        .GreaterThanOrEqualTo(DateTime.UtcNow.AddYears(-150))
        .WithMessage("برجاء ادخال {PropertyName} صحيح");
    }
    public static IRuleBuilderOptions<T, string?> Phone<T>(this IRuleBuilder<T, string?> ruleBuilder, string propName)
    {
        string[] PhonePatterns =
        [
            @"^(\+201)[0-2,5][0-9]{8}$" , // EgyptPattern
            @"^(\+9665)[0-1,3-9][0-9]{7}$" ,  //SaudiArabiaPattern
            @"^(\+9647)[3-9][0-9]{8}$", //IraqPattern
            @"^(\+9627)[5,7-9][0-9]{7}$", // JordanPattern
            @"^(\+9733)[1-4,6-9][0-9]{6}$",//BahrainPattern 
            @"^(\+971)[2-9]\d{8}$", //UAEPattern
            @"^(\+974)[3567]\d{7}$", //QatarPattern 
            @"^(\+965)[569]\d{7}$", //KuwaitPattern
            @"^(\+968)[0-9]{8}$", //OmanPattern
        ];

        return ruleBuilder
            .NotEmpty()
            .Configure(config => config.CascadeMode = CascadeMode.Stop)
            .WithName(propName)
            .Must(number => number is not null && PhonePatterns.Any(pattern => new Regex(pattern).IsMatch(number)))
            .WithMessage("يجب ادخال {PropertyName} صحيح ويستخدم الواتس اب");
    }
    // public static IRuleBuilderOptions<T, IFormFile> File<T>(this IRuleBuilder<T, IFormFile> ruleBuilder, string propName, FileSettings fileSettings)
    // {
    //     return ruleBuilder
    //         .NotEmpty()
    //         .WithName(propName)
    //         .WithMessage("{PropertyName} حقل مطلوب")
    //         .Must(file => file.Length > 0)
    //         .WithMessage("{PropertyName} لا يمكن ان يكون ملف فارغ")
    //         .Must(file => file.Length <= fileSettings.MaxBytes)
    //         .WithMessage("{PropertyName} لا يمكن ان يتعدى ملف الـ  MB 30 بالحجم")
    //         .Must(file => fileSettings.AcceptedFileTypes.Any(x => $".{x.ToLower()}".Equals(Path.GetExtension(file.FileName).ToLower())))
    //         .WithMessage("يرجي ارفاق {PropertyName} بامتداد مناسب");
    // }

}