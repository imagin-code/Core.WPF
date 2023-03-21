﻿using Imagin.Core.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Imagin.Core.Controls
{
    [ContentProperty(nameof(Templates))]
    public class KeyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Default { get; set; } = new();

        public List<KeyTemplate> Templates { get; set; } = new();

        public override DataTemplate SelectTemplate(object item, DependencyObject container) => Templates.FirstOrDefault(i => item?.Equals(i.DataKey) == true) ?? Default;
    }
}