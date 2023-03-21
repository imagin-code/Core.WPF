﻿using System.Windows;
using System.Windows.Media;

namespace Imagin.Core.Effects;

/// <summary>An effect that intensifies dark regions.</summary>
[Category(ImageEffectCategory.Color), Name("Gloom"), Explicit]
public class GloomEffect : BaseBlendEffect
{
    public static readonly DependencyProperty GloomIntensityProperty = DependencyProperty.Register("GloomIntensity", typeof(double), typeof(GloomEffect), new FrameworkPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(0)));
    /// <summary>Intensity of the gloom image.</summary>
    [Show]
    [Range(0.0, 1.0, 0.01, Style = RangeStyle.Both)]
    public double GloomIntensity
    {
        get => (double)GetValue(GloomIntensityProperty);
        set => SetValue(GloomIntensityProperty, value);
    }

    public static readonly DependencyProperty BaseIntensityProperty = DependencyProperty.Register("BaseIntensity", typeof(double), typeof(GloomEffect), new FrameworkPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(1)));
    /// <summary>Intensity of the base image.</summary>
    [Show]
    [Range(0.0, 1.0, 0.01, Style = RangeStyle.Both)]
    public double BaseIntensity
    {
        get => (double)GetValue(BaseIntensityProperty);
        set => SetValue(BaseIntensityProperty, value);
    }

    public static readonly DependencyProperty GloomSaturationProperty = DependencyProperty.Register("GloomSaturation", typeof(double), typeof(GloomEffect), new FrameworkPropertyMetadata(((double)(0.2D)), PixelShaderConstantCallback(2)));
    /// <summary>Saturation of the gloom image.</summary>
    [Show]
    [Range(0.0, 1.0, 0.01, Style = RangeStyle.Both)]
    public double GloomSaturation
    {
        get => (double)GetValue(GloomSaturationProperty);
        set => SetValue(GloomSaturationProperty, value);
    }

    public static readonly DependencyProperty BaseSaturationProperty = DependencyProperty.Register("BaseSaturation", typeof(double), typeof(GloomEffect), new FrameworkPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(3)));
    /// <summary>Saturation of the base image.</summary>
    [Show]
    [Range(0.0, 1.0, 0.01, Style = RangeStyle.Both)]
    public double BaseSaturation
    {
        get => (double)GetValue(BaseSaturationProperty);
        set => SetValue(BaseSaturationProperty, value);
    }

    public GloomEffect() : base()
    {
        UpdateShaderValue(GloomIntensityProperty);
        UpdateShaderValue(BaseIntensityProperty);
        UpdateShaderValue(GloomSaturationProperty);
        UpdateShaderValue(BaseSaturationProperty);
    }

    public override Color Apply(Color color, double amount = 1) => color;
}