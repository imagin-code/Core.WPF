﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagin.Core.Media;

/// <summary>Read Write Mode for the BitmapContext.</summary>
public enum ReadWriteMode
{
    /// <summary>On Dispose of a BitmapContext, do not Invalidate</summary>
    ReadOnly,
    /// <summary>On Dispose of a BitmapContext, invalidate the bitmap</summary>
    ReadWrite
}