using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.BusinessLogic
{
    public static class ValueFormatToString
    {
        public const string DateToDdMMYYY = "dd/MM/yyyy";
        public const string DateToddMMMyyyy = "dd MMM yyyy";
        public const string DateToDdMMYYY_HHmmss = "dd/MM/yyyy HH:mm:ss";
        public const string DecimalItalianCurrencyEuro = "#,##0.00 €";
        public const string DecimalItalianCurrency = "#,##0.00";
        public const string PercentageItalian = "#,##0.00";
        public const string Currency = "N2";
        public const string TimeToHHmm = "HH:mm";
    }
}
