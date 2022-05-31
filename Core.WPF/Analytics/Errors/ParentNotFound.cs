﻿using System;

namespace Imagin.Core
{
    public class ParentNotFoundException<Child, Parent> : Exception
    {
        public ParentNotFoundException() : base($"'{typeof(Child).FullName}' must have logical or visual parent of type '{typeof(Parent).FullName}'.") { }
    }
}