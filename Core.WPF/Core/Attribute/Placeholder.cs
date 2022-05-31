﻿using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PlaceholderAttribute : Attribute
    {
        public readonly string Placeholder;

        public PlaceholderAttribute(string placeholder) => Placeholder = placeholder;
    }
}