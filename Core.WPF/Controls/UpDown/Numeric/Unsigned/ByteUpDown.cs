﻿using Imagin.Core.Linq;
using System.Windows.Input;
using static Imagin.Core.Numerics.M;

namespace Imagin.Core.Controls
{
    public class ByteUpDown : NumericUpDown<byte>
    {
        public override byte AbsoluteMaximum => byte.MaxValue;

        public override byte AbsoluteMinimum => byte.MinValue;

        public override byte DefaultIncrement => 1;

        public override byte DefaultValue => 0;

        public override bool IsRational => true;

        public override bool IsSigned => false;

        public ByteUpDown() : base() { }

        protected override byte GetValue(string input) => input.Byte();

        protected override string ToString(byte input) => input.ToString(StringFormat);

        protected override bool CanIncrease() => Value < Maximum;

        protected override bool CanDecrease() => Value > Minimum;

        protected override object OnMaximumCoerced(object input) => Clamp((byte)input, AbsoluteMaximum, Value);

        protected override object OnMinimumCoerced(object input) => Clamp((byte)input, Value, AbsoluteMinimum);

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            e.Handled = CaretIndex > 0 && e.Text == "-" ? true : e.Handled;
        }

        protected override object OnValueCoerced(object input) => Clamp((byte)input, Maximum, Minimum);

        public override void Increase() => SetCurrentValue(ValueProperty.Property, (Value + Increment).Byte());

        public override void Decrease() => SetCurrentValue(ValueProperty.Property, (Value - Increment).Byte());
    }
}