﻿using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CommandAttribute : Attribute
    {
        public readonly string CommandName;

        public CommandAttribute(string commandName) => CommandName = commandName;
    }
}