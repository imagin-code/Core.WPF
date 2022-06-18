﻿using Imagin.Core.Controls;
using Imagin.Core.Models;
using System.Windows.Data;

namespace Imagin.Core.Conversion
{
    [ValueConversion(typeof(ColorDocument), typeof(Document))]
    public class ColorDocumentConverter : Converter<ColorDocument, Document>
    {
        public static ColorDocumentConverter Default { get; private set; } = new ColorDocumentConverter();
        ColorDocumentConverter() { }

        protected override ConverterValue<Document> ConvertTo(ConverterData<ColorDocument> input) => input.Value as Document;

        protected override ConverterValue<ColorDocument> ConvertBack(ConverterData<Document> input) => input.Value as ColorDocument;
    }
}