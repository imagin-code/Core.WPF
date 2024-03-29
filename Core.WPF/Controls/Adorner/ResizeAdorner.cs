﻿using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Imagin.Core.Controls;

/// <inheritdoc/>
public class ResizeAdorner : TransformAdorner
{
    #region Properties
    
    readonly Thumb top, bottom, left, right, topLeft, topRight, bottomLeft, bottomRight;

    public Axis2D? CoerceAxis 
        => XElement.GetResizeCoerceAxis(AdornedElement as FrameworkElement);

    public CardinalDirection? CoerceDirection 
        => XElement.GetResizeCoerceDirection(AdornedElement as FrameworkElement);

    public double Snap => XElement.GetResizeSnap(AdornedElement as FrameworkElement);

    #endregion

    #region ResizeAdorner

    public ResizeAdorner(FrameworkElement element) : base(element)
    {
        BuildThumb(ref top,
            Cursors.SizeNS);
        BuildThumb(ref left,
            Cursors.SizeWE);
        BuildThumb(ref right,
            Cursors.SizeWE);
        BuildThumb(ref bottom,
            Cursors.SizeNS);
        BuildThumb(ref topLeft,
            Cursors.SizeNWSE);
        BuildThumb(ref topRight,
            Cursors.SizeNESW);
        BuildThumb(ref bottomLeft,
            Cursors.SizeNESW);
        BuildThumb(ref bottomRight,
            Cursors.SizeNWSE);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Handler for resizing from the bottom-right.
    /// </summary>
    void HandleBottomRight(object sender, DragDeltaEventArgs e)
    {
        var thumb = sender as Thumb;
        var target = AdornedElement as FrameworkElement;
        if (CanHandle(thumb))
        {
            if (CoerceAxis != Axis2D.Y)
                target.Width = M.NearestFactor(Math.Max(target.Width + e.HorizontalChange, thumb.DesiredSize.Width), Snap);

            if (CoerceAxis != Axis2D.X)
                target.Height = M.NearestFactor(Math.Max(e.VerticalChange + target.Height, thumb.DesiredSize.Height), Snap);
        }
    }

    /// <summary>
    /// Handler for resizing from the top-right.
    /// </summary>
    void HandleTopRight(object sender, DragDeltaEventArgs e)
    {
        var thumb = sender as Thumb;
        var target = AdornedElement as FrameworkElement;
        if (CanHandle(thumb))
        {
            if (CoerceAxis != Axis2D.Y)
                target.Width = M.NearestFactor(Math.Max(target.Width + e.HorizontalChange, thumb.DesiredSize.Width), Snap);

            if (CoerceAxis != Axis2D.X)
            {
                var height_old = target.Height;
                var height_new = M.NearestFactor(Math.Max(target.Height - e.VerticalChange, thumb.DesiredSize.Height), Snap);
                var top_old = Canvas.GetTop(target);

                target.Height = height_new;
                Canvas.SetTop(target, top_old - (height_new - height_old));
            }
        }
    }

    /// <summary>
    /// Handler for resizing from the top-left.
    /// </summary>
    void HandleTopLeft(object sender, DragDeltaEventArgs e)
    {
        var thumb = sender as Thumb;
        var target = AdornedElement as FrameworkElement;
        if (CanHandle(thumb))
        {
            if (CoerceAxis != Axis2D.Y)
            {
                var width_old = target.Width;
                var width_new = M.NearestFactor(Math.Max(target.Width - e.HorizontalChange, thumb.DesiredSize.Width), Snap);
                var left_old = Canvas.GetLeft(target);

                target.Width = width_new;
                Canvas.SetLeft(target, left_old - (width_new - width_old));
            }
            if (CoerceAxis != Axis2D.X)
            {
                var height_old = target.Height;
                var height_new = M.NearestFactor(Math.Max(target.Height - e.VerticalChange, thumb.DesiredSize.Height), Snap);
                var top_old = Canvas.GetTop(target);

                target.Height = height_new;
                Canvas.SetTop(target, top_old - (height_new - height_old));
            }
        }
    }

    /// <summary>
    /// Handler for resizing from the bottom-left.
    /// </summary>
    void HandleBottomLeft(object sender, DragDeltaEventArgs e)
    {
        var thumb = sender as Thumb;
        var target = AdornedElement as FrameworkElement;
        if (CanHandle(thumb))
        {
            if (CoerceAxis != Axis2D.X)
                target.Height = M.NearestFactor(Math.Max(e.VerticalChange + target.Height, thumb.DesiredSize.Height), Snap);

            if (CoerceAxis != Axis2D.Y)
            {
                var width_old = target.Width;
                var width_new = M.NearestFactor(Math.Max(target.Width - e.HorizontalChange, thumb.DesiredSize.Width), Snap);
                var left_old = Canvas.GetLeft(target);

                target.Width = width_new;
                Canvas.SetLeft(target, left_old - (width_new - width_old));
            }
        }
    }

    /// <summary>
    /// Handler for resizing from the top.
    /// </summary>
    void HandleTop(object sender, DragDeltaEventArgs e)
    {
        var thumb = sender as Thumb;
        var target = AdornedElement as FrameworkElement;
        if (CanHandle(thumb))
        {
            if (CoerceAxis != Axis2D.X)
            {
                var height_old = target.Height;
                var height_new = M.NearestFactor(Math.Max(target.Height - e.VerticalChange, thumb.DesiredSize.Height), Snap);
                var top_old = Canvas.GetTop(target);
                var top_new = top_old - (height_new - height_old);

                target.Height = height_new;
                Canvas.SetTop(target, top_new);
            }
        }
    }

    /// <summary>
    /// Handler for resizing from the left.
    /// </summary>
    void HandleLeft(object sender, DragDeltaEventArgs e)
    {
        var thumb = sender as Thumb;
        var target = AdornedElement as FrameworkElement;
        if (CanHandle(thumb))
        {
            if (CoerceAxis != Axis2D.Y)
            {
                var width_old = target.Width;
                var width_new = M.NearestFactor(Math.Max(target.Width - e.HorizontalChange, thumb.DesiredSize.Width), Snap);
                var left_old = Canvas.GetLeft(target);
                var left_new = left_old - (width_new - width_old);

                target.Width = width_new;
                Canvas.SetLeft(target, left_new);
            }
        }
    }

    /// <summary>
    /// Handler for resizing from the right.
    /// </summary>
    void HandleRight(object sender, DragDeltaEventArgs e)
    {

        var thumb = sender as Thumb;
        var target = AdornedElement as FrameworkElement;
        if (CanHandle(thumb))
        {
            if (CoerceAxis != Axis2D.Y)
            {
                var width = Math.Max(target.Width + e.HorizontalChange, thumb.DesiredSize.Width);
                target.Width = M.NearestFactor(width, Snap);
            }
        }
    }

    /// <summary>
    /// Handler for resizing from the bottom.
    /// </summary>
    void HandleBottom(object sender, DragDeltaEventArgs e)
    {
        var thumb = sender as Thumb;
        var target = AdornedElement as FrameworkElement;
        if (CanHandle(thumb))
        {
            if (CoerceAxis != Axis2D.X)
            {
                var height = Math.Max(e.VerticalChange + target.Height, thumb.DesiredSize.Height);
                target.Height = M.NearestFactor(height, Snap);
            }
        }
    }

    ///

    /// <inheritdoc/>
    protected override Size ArrangeOverride(Size finalSize)
    {
        //Desired width/height
        var dW 
            = AdornedElement.DesiredSize.Width;
        var dH 
            = AdornedElement.DesiredSize.Height;

        //Adorner width/height
        var aW 
            = DesiredSize.Width;
        var aH 
            = DesiredSize.Height;

        top
            .Arrange(new Rect((dW / 2) - (aW / 2), -aH / 2, aW, aH));
        left
            .Arrange(new Rect(-aW / 2, (dH / 2) - (aH / 2), aW, aH));
        right
            .Arrange(new Rect(dW - aW / 2, (dH / 2) - (aH / 2), aW, aH));
        bottom
            .Arrange(new Rect((dW / 2) - (aW / 2), dH - aH / 2, aW, aH));
        topLeft
            .Arrange(new Rect(-aW / 2, -aH / 2, aW, aH));
        topRight
            .Arrange(new Rect(dW - aW / 2, -aH / 2, aW, aH));
        bottomLeft
            .Arrange(new Rect(-aW / 2, dH - aH / 2, aW, aH));
        bottomRight
            .Arrange(new Rect(dW - aW / 2, dH - aH / 2, aW, aH));
            
        return finalSize;
    }

    /// <inheritdoc/>
    protected override void BuildThumb(ref Thumb thumb, Cursor cursor, double height = DefaultThumbHeight, double width = DefaultThumbWidth, DependencyProperty property = null)
        => base.BuildThumb(ref thumb, cursor, height, width, XElement.ResizeThumbStyleProperty);

    ///

    /// <inheritdoc/>
    public override void Subscribe()
    {
        base.Subscribe();
        top.DragDelta
            += HandleTop;
        left.DragDelta
            += HandleLeft;
        right.DragDelta
            += HandleRight;
        bottom.DragDelta
            += HandleBottom;
        topLeft.DragDelta
            += HandleTopLeft;
        topRight.DragDelta
            += HandleTopRight;
        bottomLeft.DragDelta
            += HandleBottomLeft;
        bottomRight.DragDelta
            += HandleBottomRight;
    }

    /// <inheritdoc/>
    public override void Unsubscribe()
    {
        base.Unsubscribe();
        top.DragDelta
            -= HandleTop;
        left.DragDelta
            -= HandleLeft;
        right.DragDelta
            -= HandleRight;
        bottom.DragDelta
            -= HandleBottom;
        topLeft.DragDelta
            -= HandleTopLeft;
        topRight.DragDelta
            -= HandleTopRight;
        bottomLeft.DragDelta
            -= HandleBottomLeft;
        bottomRight.DragDelta
            -= HandleBottomRight;
    }

    #endregion
}