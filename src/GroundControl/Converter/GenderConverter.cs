using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DataModel;
using System.Collections;

namespace Phrike.GroundControl.Converter
{
    public class GenderConverter : IValueConverter
    {
        private static object StoE(object value)
        {
            switch (((String)value))
            {
                case "Weiblich":
                    return Gender.Female;
                case "Männlich":
                    return Gender.Male;
            }

            return null;
        }
        private static object EtoS(object value)
        {
            switch (((DataModel.Gender)value))
            {
                case Gender.Female:
                    return "Weiblich";
                case Gender.Male:
                    return "Männlich";
            }
            return "Undefiniert";
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Gender[])
            {
                var t = new ArrayList();
                foreach (var v in ((IEnumerable)value))
                {
                    t.Add(EtoS(v));
                }
                return t;
            }
            else
            {
                return EtoS(value);
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is String[])
            {
                var t = new ArrayList();
                foreach (var v in ((IEnumerable)value))
                {
                    t.Add(StoE(v));
                }
                return t;
            }
            else
            {
                return StoE(value);
            }
        }

    }
}
