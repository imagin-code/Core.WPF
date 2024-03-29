﻿using Imagin.Core.Colors;
using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Core.Controls;

[TemplatePart(Name = nameof(PART_Canvas), Type = typeof(Canvas))]
public class ColorSelector : Control
{
    Canvas PART_Canvas;

    ///

    public static readonly DependencyProperty DepthProperty = DependencyProperty.Register(nameof(Depth), typeof(double), typeof(ColorSelector), new FrameworkPropertyMetadata(0.0));
    public double Depth
    {
        get => (double)GetValue(DepthProperty);
        set => SetValue(DepthProperty, value);
    }

    public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof(Model), typeof(Type), typeof(ColorSelector), new FrameworkPropertyMetadata(typeof(HSB)));
    public Type Model
    {
        get => (Type)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    public static readonly DependencyProperty ProfileProperty = DependencyProperty.Register(nameof(Profile), typeof(WorkingProfile), typeof(ColorSelector), new FrameworkPropertyMetadata(WorkingProfile.Default));
    public WorkingProfile Profile
    {
        get => (WorkingProfile)GetValue(ProfileProperty);
        set => SetValue(ProfileProperty, value);
    }

    ///

    public ColorSelector() : base() { }

    ///

    Vector2<One> CoerceDepth(Vector2<One> a)
    {
        if (Depth > 0)
        {
            var b = (new Vector2(a.X, a.Y) * Depth).Round() / Depth;
            a = new(b[0], b[1]);
        }
        return a;
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);
        if (Mouse.LeftButton == MouseButtonState.Pressed)
        {
            var point = Normalize(e.GetPosition(this));
            point = CoerceDepth(point);

            OnMouseChanged(point);
            CaptureMouse();
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (Mouse.LeftButton == MouseButtonState.Pressed)
        {
            var point = Normalize(e.GetPosition(this));
            point = CoerceDepth(point);

            OnMouseChanged(point);
        }
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        base.OnMouseUp(e);
        if (Mouse.LeftButton == MouseButtonState.Released)
            ReleaseMouseCapture();
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        Mark();
    }

    ///

    /// <summary>Gets a <see cref="Point"/> in range [0, 1].</summary>
    protected Vector2<One> Normalize(Point input)
    {
        input = input.Coerce(new Point(ActualWidth, ActualHeight), new Point(0, 0));
        input = new Point(input.X, ActualHeight - input.Y);
        input = new Point(input.X / ActualWidth, input.Y / ActualHeight);
        return new Vector2<One>(input.X, input.Y);
    }

    ///

    protected virtual void Mark() { }

    protected virtual void OnMouseChanged(Vector2<One> input) { }

    ///

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        PART_Canvas = Template.FindName(nameof(PART_Canvas), this) as Canvas;
    }
}