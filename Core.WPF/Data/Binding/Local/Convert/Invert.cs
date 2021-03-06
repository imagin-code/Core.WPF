using Imagin.Core.Conversion;

namespace Imagin.Core.Data
{
    public class InvertBinding : LocalBinding
    {
        public InvertBinding() : this(".") { }

        public InvertBinding(string path) : base(path) => Converter = InverseBooleanConverter.Default;
    }
}