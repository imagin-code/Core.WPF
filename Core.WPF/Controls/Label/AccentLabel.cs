﻿using Imagin.Core.Paint;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Core.Controls
{
    public class AccentLabel : Label
    {
        public static readonly DependencyProperty ShadeProperty = DependencyProperty.Register(nameof(Shade), typeof(Shades), typeof(AccentLabel), new FrameworkPropertyMetadata(Shades.Medium));
        public Shades Shade
        {
            get => (Shades)GetValue(ShadeProperty);
            set => SetValue(ShadeProperty, value);
        }
    }
}