﻿namespace Imagin.Core.Storage
{
    public interface IValidate
    {
        bool Validate(ItemType target, string path);
    }
}