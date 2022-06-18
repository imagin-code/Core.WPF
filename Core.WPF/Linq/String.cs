﻿using Imagin.Core.Local.Engine;
using Imagin.Core.Local.Extensions;

namespace Imagin.Core.Linq
{
    public static partial class XString
    {
        public static string Translate(this string input, string prefix = "", string suffix = "")
        {
            var result = (string)LocExtension.GetLocalizedValue(typeof(string), input, LocalizeDictionary.Instance.SpecificCulture, null);
            return result.NullOrEmpty() ? LocalizeDictionary.MissingKeyFormat.F(prefix, input, suffix) : $"{prefix}{result}{suffix}";
        }
    }
}