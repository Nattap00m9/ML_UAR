using System.Globalization;

namespace ML_UAR.Class
{
    public class DateConvertingToDB
    {
        public string DateConvert(string text)
        {
            var txtdate = DateTime.ParseExact(text, "dd/MM/yyyy",CultureInfo.InvariantCulture);

            var year = txtdate.Year <= 1900 ? txtdate.Year + 543 : txtdate.Year;
            var month = txtdate.Month;
            var day = txtdate.Day;

            var date_return = year.ToString() + "-" + month.ToString().PadLeft(2, '0') + "-" + day.ToString().PadLeft(2, '0');

            return date_return;
        }

    }
}
