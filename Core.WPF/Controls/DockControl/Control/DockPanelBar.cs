﻿using Imagin.Core.Collections;
using Imagin.Core.Linq;
using System.Windows.Controls;

namespace Imagin.Core.Controls
{
    public class DockPanelBar : ToolBar, IDockPanelSource
    {
        public DockRootControl Root { get; private set; }

        public ICollectionChanged Source => ItemsSource as ICollectionChanged;

        public DockPanelBar() : base()
        {
            this.RegisterHandler(i => Root = this.FindParent<DockRootControl>());
        }
    }
}