﻿using System.Runtime.InteropServices;

namespace Imagin.Core.Imports
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MONITORINFO
    {
        /// <summary>
        /// </summary>            
        public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

        /// <summary>
        /// </summary>            
        public RECT rcMonitor = new();

        /// <summary>
        /// </summary>            
        public RECT rcWork = new();

        /// <summary>
        /// </summary>            
        public int dwFlags = 0;
    }
}