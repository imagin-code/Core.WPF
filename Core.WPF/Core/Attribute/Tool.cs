﻿using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ToolAttribute : Attribute
    {
        public ToolAttribute() { }
    }
}