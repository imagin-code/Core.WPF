﻿using Imagin.Core.Converters;
using Imagin.Core.Linq;
using System.Windows.Data;

namespace Imagin.Core.Data
{
    public class RemoteBinding : Binding, IRemoteBinding
    {
        public virtual RemoteBindingSource RemoteSource { set => Source = this.GetSource(value); }

        public RemoteBinding() : this(".") { }

        public RemoteBinding(string path) : this(path, RemoteBindingSource.Application)
        {
        }

        public RemoteBinding(string path, RemoteBindingSource remoteSource) : base(path)
        {
            Converter 
                = new SimpleConverter<object, object>(i => i ?? Nothing.Do, i => i);
            RemoteSource 
                = remoteSource;
            UpdateSourceTrigger 
                = UpdateSourceTrigger.PropertyChanged;
        }
    }
}