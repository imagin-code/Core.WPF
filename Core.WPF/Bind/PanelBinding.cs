﻿using Imagin.Core.Models;
using System;
using System.Linq;
using System.Windows.Data;

namespace Imagin.Core.Data;

public class PanelBinding : Bind
{
    public Type Type { set => SetType(value); }

    public PanelBinding() : base() { }

    public PanelBinding(Type type) : this(".", type) { }

    public PanelBinding(string path, Type type) : base(path)
    {
        SetType(type);
        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
    }

    void SetType(Type input)
    {
        Source = Current.Get<IDockMainViewModel>()?.Panels.FirstOrDefault(i => i.GetType().Equals(input));
    }
}