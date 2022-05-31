﻿using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Imagin.Core.Behavior
{
    /// <summary>
    /// Auto-positions popup when the parent element's position changes (TO-DO: support scrolling).
    /// </summary>
    public class AutoPositionPopupBehavior : Behavior<Popup>
    {
        const int WM_MOVING = 0x0216;

        DependencyObject GetTopmostParent(DependencyObject Element)
        {
            var Current = Element;
            var Result = Element;

            while (Current != null)
            {
                Result = Current;
                Current = Current is Visual || Current is Visual3D ?
                    VisualTreeHelper.GetParent(Current) :
                    LogicalTreeHelper.GetParent(Current);
            }
            return Result;
        }

        IntPtr HwndMessageHook(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool bHandled)
        {
            if (msg == WM_MOVING)
                Update();

            return IntPtr.Zero;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += (sender, e) =>
            {
                var Root = GetTopmostParent(AssociatedObject.PlacementTarget) as Window;
                if (Root != null)
                {
                    var Helper = new WindowInteropHelper(Root);
                    var Source = HwndSource.FromHwnd(Helper.Handle);

                    if (Source != null)
                        Source.AddHook(HwndMessageHook);
                }
            };
        }

        public void Update()
        {
            var Placement = AssociatedObject.Placement;
            AssociatedObject.Placement = PlacementMode.Relative;
            AssociatedObject.Placement = Placement;
        }
    }
}