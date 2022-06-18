﻿using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Core.Conversion
{
    [ValueConversion(typeof(object[]), typeof(Visibility))]
    public class EnumFlagsToVisibilityMultiConverter : MultiConverter<Visibility>
    {
        public static EnumFlagsToVisibilityMultiConverter Default { get; private set; } = new EnumFlagsToVisibilityMultiConverter();
        EnumFlagsToVisibilityMultiConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            object a = null, b = null;

            var Result = true;

            var i = 0;
            foreach (var j in values)
            {
                if (Numerics.M.Even(i))
                {
                    a = j;
                }
                else
                {
                    b = j;
                    if (a != null && b != null)
                    {
                        Result = Result && a.As<Enum>().HasFlag(b as Enum);
                        a = null;
                        b = null;
                    }
                }
                i++;
            }

            return Result.Visibility();
        }
    }
}