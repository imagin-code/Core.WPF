using Imagin.Core.Config;
using Imagin.Core.Conversion;
using Imagin.Core.Linq;
using Imagin.Core.Media;
using Imagin.Core.Models;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Core.Controls
{
    public partial class ColorWindow : PickerWindow
    {
        public static readonly DependencyProperty ActiveDocumentProperty = DependencyProperty.Register(nameof(ActiveDocument), typeof(ColorDocument), typeof(ColorWindow), new FrameworkPropertyMetadata(null));
        public ColorDocument ActiveDocument
        {
            get => (ColorDocument)GetValue(ActiveDocumentProperty);
            set => SetValue(ActiveDocumentProperty, value);
        }
        
        public static readonly DependencyProperty DocumentsProperty = DependencyProperty.Register(nameof(Documents), typeof(DocumentCollection), typeof(ColorWindow), new FrameworkPropertyMetadata(null));
        public DocumentCollection Documents
        {
            get => (DocumentCollection)GetValue(DocumentsProperty);
            set => SetValue(DocumentsProperty, value);
        }

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register(nameof(Options), typeof(ColorControlOptions), typeof(ColorWindow), new FrameworkPropertyMetadata(null));
        public ColorControlOptions Options
        {
            get => (ColorControlOptions)GetValue(OptionsProperty);
            set => SetValue(OptionsProperty, value);
        }

        readonly ColorControlOptions options = null;

        public ColorWindow() : base()
        {
            SetCurrentValue(DocumentsProperty, new DocumentCollection());

            options = new ColorControlOptions($@"{ApplicationProperties.GetFolderPath(DataFolders.Documents)}\{nameof(ColorWindow)}\Options.data", Documents);
            options.Load(out BaseSavable newOptions);
            options = newOptions as ColorControlOptions;

            SetCurrentValue(OptionsProperty, options);

            var document = new ColorDocument(options.Profiles) { CanClose = false, CanFloat = false };
            Documents.Add(document);

            SetCurrentValue(ActiveDocumentProperty, document);

            this.Bind(ValueProperty, $"{nameof(ActiveDocument)}.{nameof(ColorDocument.NewColor)}", this, System.Windows.Data.BindingMode.TwoWay, ColorToSolidColorBrushConverter.Default);
            InitializeComponent();
        }

        public ColorWindow(string title, Color color) : this()
        {
            SetCurrentValue(TitleProperty,
                title);
            SetCurrentValue(ValueProperty,
                color);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            options.Save();
        }
    }
}