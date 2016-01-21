using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using System.Collections;
using System.Windows.Data;
using System.Globalization;

namespace Phrike.GroundControl.Converter
{
    public class RhConverter : IValueConverter
    {
        private static object StoE(object value)
        {
            switch (((String)value))
            {
                case "Positiv":
                    return RhFactor.Positive;
                case "Negativ":
                    return RhFactor.Negative;
            }

            return null;
        }
        private static object EtoS(object value)
        {
            switch (((DataModel.RhFactor)value))
            {
                case RhFactor.Positive:
                    return "Positiv";
                case RhFactor.Negative:
                    return "Negativ";
            }
            return "Undefiniert";
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RhFactor[])
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
