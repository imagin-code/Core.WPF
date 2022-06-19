﻿using Imagin.Core.Reflection;
using System.IO;

namespace Imagin.Core.Storage
{
    /// <summary>
    /// Represents a <see cref="Folder"/> or <see cref="Drive"/>.
    /// </summary>
    public abstract class Container : Item
    {
        ItemCollection items = new();
        [Hidden]
        public ItemCollection Items
        {
            get => items;
            private set => this.Change(ref items, value);
        }

        [DisplayName("Path")]
        [Featured]
        [ReadOnly]
        [MemberStyle(StringStyle.FolderPath)]
        public override string Path
        {
            get => base.Path;
            set
            {
                base.Path = value;
                items.Path = value;
            }
        }

        public override FileSystemInfo Read() => new DirectoryInfo(path);

        protected Container(ItemType type, Origin origin, string path) : base(type, origin, path) { }
    }
}