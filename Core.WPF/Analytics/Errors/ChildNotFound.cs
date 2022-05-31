﻿using System;

namespace Imagin.Core
{
    public class ChildNotFoundException<Child, Parent> : Exception
    {
        public ChildNotFoundException() : base($"'{typeof(Parent).FullName}' must have logical or visual child of type '{typeof(Child).FullName}'.") { }
    }
}