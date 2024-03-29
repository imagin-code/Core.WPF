﻿using System;

namespace Imagin.Core.Storage;

[Flags]
[Serializable]
public enum ItemType
{
    [Hide]
    Nothing = 0,
    File = 1,
    Folder = 2,
    Shortcut = 4,
    Drive = 8,
    Root = 16,
    [Hide]
    All = File | Folder | Shortcut | Drive | Root
}
