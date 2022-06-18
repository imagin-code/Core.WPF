﻿using Imagin.Core.Numerics;

namespace Imagin.Core.Conversion
{
    public class DoubleSizeTypeConverter : StringTypeConverter<double>
    {
        protected override int? Length => 2;

        protected override double Convert(string input) => double.Parse(input);

        protected override object Convert(double[] input) => new DoubleSize(input[0], input[1]);
    }
}