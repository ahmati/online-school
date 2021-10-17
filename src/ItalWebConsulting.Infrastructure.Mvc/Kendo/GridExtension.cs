using ItalWebConsulting.Infrastructure.BusinessLogic;
using Kendo.Mvc.UI.Fluent;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using System;
using System.Collections.Generic;
using System.Text;

namespace InfoWeb.Infrastructure.Mvc.Kendo
{
    public static class GridExtension
    {
        public static GridBoundColumnBuilder<T> ItalianDateFormat<T>(this GridBoundColumnBuilder<T> column)
            where T : class
        {
            return column.Format("{0:" + ValueFormatToString.DateToDdMMYYY + "}");
        }

        public static GridBoundColumnBuilder<T> ItalianDateTimeFormat<T>(this GridBoundColumnBuilder<T> column)
         where T : class
        {
            return column.Format("{0:" + ValueFormatToString.DateToDdMMYYY + "}");
        }

        public static GridBoundColumnBuilder<T> DecimalFormat<T>(this GridBoundColumnBuilder<T> column, bool viewCurrency = false)
            where T : class
        {
            return viewCurrency ? column.Format("{0:" + ValueFormatToString.DecimalItalianCurrencyEuro + "}")
                : column.Format("{0:" + ValueFormatToString.DecimalItalianCurrencyEuro + "}");
        }

        public static GridBoundColumnBuilder<T> PercentageFormat<T>(this GridBoundColumnBuilder<T> column)
        where T : class
        {
            return column.Format(@"{0:" + ValueFormatToString.PercentageItalian + "\\%}");
        }

        public static GridBoundColumnBuilder<T> CurrencyFormat<T>(this GridBoundColumnBuilder<T> columnBuilder)
            where T : class
        {
            return columnBuilder.Format("{0:" + ValueFormatToString.Currency + "}");
        }

        public static NumericTextBoxBuilder<T> CurrencyFormat<T>(this NumericTextBoxBuilder<T> nTxt, int decimals = 0) where T : struct
        {
            return decimals == 0 ? nTxt.Format("#,00").Spinners(false).Culture("it-IT") : nTxt.Format("#,00.00").Decimals(decimals).Spinners(false).Culture("it-IT");
        }

        

        //public static NumericTextBoxBuilder<T> HourMinuteFormat<T>(this NumericTextBoxBuilder<T> nTxt, int decimals = 0) where T : struct
        //{
        //    return decimals == 0 ? nTxt.Min(0).Format("00:00").Spinners(true).Culture("it-IT") : nTxt.Format("00:00").Spinners(true).Culture("it-IT");
        //}

        public static GridBuilder<T> PageableItalianLocalization<T>(this GridBuilder<T> grid, string emptyMsg, string itemManyName, int[] pageSizes, bool isRefreshEnabled) where T : class
        {
            return grid.Pageable(p => p.Enabled(true).Info(true).Refresh(isRefreshEnabled).PageSizes(pageSizes)
                .Messages(m => m.Display("{0} - {1} di {2} " + itemManyName)
                .Empty(emptyMsg)
                .Page("Pagina").Of("di {0}").ItemsPerPage(itemManyName + " per pagina")
                .First("Vai alla prima pagina").Previous("Vai alla pagina precedente").Next("Vai alla pagina successiva").Last("Vai all'ultima pagina")
                .Refresh("Ricarica")));
        }

        public static GridBoundColumnBuilder<T> Checkable<T>(this GridBoundColumnBuilder<T> column, string name = "")
           where T : class
        {
            var mb = (column.Column).Member;
            if (!string.IsNullOrWhiteSpace(name))
                name = "id='" + name + "'";

            return column.Groupable(false).Sortable(false).ClientTemplate(@"<input " + name + " class='checkbox' data-field= '" + mb + "' disabled='disabled'  type='checkbox' #= " + mb + " ? checked='checked' : '' # ></input>");
        }
       

        
    }
}
