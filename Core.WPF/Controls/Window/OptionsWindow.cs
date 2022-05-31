﻿using Imagin.Core.Models;
using System;
using System.Windows;

namespace Imagin.Core.Controls
{
    public class OptionsWindow : PropertyWindow
    {
        static OptionsWindow() => SourceProperty.OverrideMetadata(typeof(OptionsWindow), new FrameworkPropertyMetadata(null, null, OnSourceCoerced));

        public OptionsWindow() : base() { }

        static object OnSourceCoerced(DependencyObject sender, object input)
        {
            if (input is not IMainViewOptions)
                throw new NotSupportedException();

            return input;
        }
    }
}