﻿using Imagin.Core.Linq;
using static Imagin.Core.Numerics.M;

namespace Imagin.Core.Controls
{
    public class Int32UpDown : NumericUpDown<int>
    {
        public override int AbsoluteMaximum => int.MaxValue;

        public override int AbsoluteMinimum => int.MinValue;

        public override int DefaultIncrement => 1;

        public override int DefaultValue => 0;

        public override bool IsRational => true;

        public override bool IsSigned => true;

        public Int32UpDown() : base() { }

        protected override int GetValue(string input) => input.Int32();

        protected override string ToString(int input) => input.ToString(StringFormat);

        protected override bool CanIncrease() => Value < Maximum;

        protected override bool CanDecrease() => Value > Minimum;

        protected override object OnMaximumCoerced(object input) => Clamp((int)input, AbsoluteMaximum, Value);

        protected override object OnMinimumCoerced(object input) => Clamp((int)input, Value, AbsoluteMinimum);

        protected override object OnValueCoerced(object input) => Clamp((int)input, Maximum, Minimum);

        public override void Increase() => SetCurrentValue(ValueProperty.Property, Value + Increment);

        public override void Decrease() => SetCurrentValue(ValueProperty.Property, Value - Increment);
    }
}