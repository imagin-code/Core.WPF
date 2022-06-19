﻿using System;
using System.Reflection;

namespace Imagin.Core.Reflection
{
    public class ListPropertyModel : ListMemberModel
    {
        public override bool CanWrite => Member.GetSetMethod(true) != null;

        new public PropertyInfo Member => (PropertyInfo)base.Member;

        public override Type Type => Member.PropertyType;

        //...

        public ListPropertyModel(MemberData data, int depthIndex) : base(data, depthIndex) { }

        //...

        protected override void SetValue(object source, object value) => Member.SetValue(source, value, null);

        protected override object GetValue(object input) => Member.GetValue(input);
    }
}