﻿using Imagin.Core.Colors;
using Imagin.Core.Linq;
using Imagin.Core.Numerics;
using Imagin.Core.Reflection;
using System;
using System.Windows.Data;

namespace Imagin.Core.Media;

#region NormalColorModel

[Categorize(false), Editable, HideName, Horizontal, Name("Color"), Options, Serializable]
public abstract class NormalColorModel : Namable<Type>, ICloneable
{
    [field: NonSerialized]
    public event EventHandler<EventArgs> ValueChanged;

    [Hide]
    public string Category => Name.Substring(0, 1).ToUpper();

    [Hide]
    public override string Name
    {
        get => base.Name;
        set => base.Name = value;
    }

    [Hide]
    public bool Normalize { get => Get(false, false); set => Set(value, false); }

    [Hide]
    public int Precision { get => Get(2, false); set => Set(value, false); }

    [Hide]
    public override Type Value
    {
        get => base.Value;
        set
        {
            base.Value = value;
            Name = value.Name;
            Update(() => Category);
        }
    }

    NormalColorModel() : this(null) { }

    public NormalColorModel(Type model) : base(model.Name, model) { }

    protected virtual void OnValueChanged() => ValueChanged?.Invoke(this, EventArgs.Empty);

    public abstract object Clone();

    public override string ToString() => Name;

    public abstract ColorModel GetColor();

    public static NormalColorModel Get(Type type)
    {
        foreach (var i in Colour.Types)
        {
            if (i == type)
            {
                if (i.Inherits<ColorModel4>())
                    return new NormalColorModel4(i);

                else if (i.Inherits<ColorModel3>())
                    return new NormalColorModel3(i);
            }
        }
        throw new ArgumentOutOfRangeException(nameof(type));
    }
}

#endregion

#region NormalColorModel<T>

[Serializable]
public abstract class NormalColorModel<T> : NormalColorModel where T : IVector
{
    [Horizontal, Index(0), StringStyle(CanClear = false), UpdateSourceTrigger(UpdateSourceTrigger.LostFocus)]
    [Trigger(nameof(MemberModel.DisplayName), nameof(NameX))]
    [Trigger(nameof(MemberModel.RightText), nameof(UnitX))]
    public string DisplayX
    {
        get => GetDisplayValue(0);
        set => SetDisplayValue(value, 0);
    }

    [Horizontal, Index(1), StringStyle(CanClear = false), UpdateSourceTrigger(UpdateSourceTrigger.LostFocus)]
    [Trigger(nameof(MemberModel.DisplayName), nameof(NameY))]
    [Trigger(nameof(MemberModel.RightText), nameof(UnitY))]
    public string DisplayY
    {
        get => GetDisplayValue(1);
        set => SetDisplayValue(value, 1);
    }

    [Horizontal, Index(2), StringStyle(CanClear = false), UpdateSourceTrigger(UpdateSourceTrigger.LostFocus)]
    [Trigger(nameof(MemberModel.DisplayName), nameof(NameZ))]
    [Trigger(nameof(MemberModel.RightText), nameof(UnitZ))]
    public string DisplayZ
    {
        get => GetDisplayValue(2);
        set => SetDisplayValue(value, 2);
    }

    [Hide]
    public string NameX => $"({Colour.Components[Value][0].Symbol}) {Colour.Components[Value][0].Name}";

    [Hide]
    public string NameY => $"({Colour.Components[Value][1].Symbol}) {Colour.Components[Value][1].Name}";

    [Hide]
    public string NameZ => $"({Colour.Components[Value][2].Symbol}) {Colour.Components[Value][2].Name}";

    [Hide]
    public string UnitX => $"{Colour.Components[Value][0].Unit}";

    [Hide]
    public string UnitY => $"{Colour.Components[Value][1].Unit}";

    [Hide]
    public string UnitZ => $"{Colour.Components[Value][2].Unit}";

    [Hide]
    public One X { get => Get<One>(); set => Set(value); }

    [Hide]
    public One Y { get => Get<One>(); set => Set(value); }

    [Hide]
    public One Z { get => Get<One>(); set => Set(value); }

    NormalColorModel() : this(null) { }

    public NormalColorModel(Type model) : base(model) { }

    protected abstract string GetDisplayValue(int index);

    protected abstract void SetDisplayValue(string input, int index);

    public override void OnPropertyChanged(PropertyEventArgs e)
    {
        base.OnPropertyChanged(e);
        switch (e.PropertyName)
        {
            case nameof(Normalize):
            case nameof(Precision):
                Update(() => DisplayX);
                Update(() => DisplayY);
                Update(() => DisplayZ);
                break;

            case nameof(X):
                Update(() => DisplayX);
                OnValueChanged();
                break;

            case nameof(Y):
                Update(() => DisplayY);
                OnValueChanged();
                break;

            case nameof(Z):
                Update(() => DisplayZ);
                OnValueChanged();
                break;
        }
    }
}

#endregion

#region NormalColorModel3

[Serializable]
public class NormalColorModel3 : NormalColorModel<Vector3>
{
    [Hide]
    public Vector Maximum => new(Colour.Components[Value][0].Maximum, Colour.Components[Value][1].Maximum, Colour.Components[Value][2].Maximum);

    [Hide]
    public Vector Minimum => new(Colour.Components[Value][0].Minimum, Colour.Components[Value][1].Minimum, Colour.Components[Value][2].Minimum);

    NormalColorModel3() : this(null) { }

    public NormalColorModel3(Type model) : base(model) { }

    public override object Clone() => new NormalColorModel3(Value);

    protected override string GetDisplayValue(int index)
    {
        One result = default;
        switch (index)
        {
            case 0:
                result = X;
                break;
            case 1:
                result = Y;
                break;
            case 2:
                result = Z;
                break;
        }

        var aRange = new DoubleRange(0, 1);
        var bRange = new DoubleRange(Minimum[index], Maximum[index]);
        return (Normalize == true ? (double)result : aRange.Convert(bRange.Minimum, bRange.Maximum, result)).Round(Precision).ToString();
    }

    protected override void SetDisplayValue(string input, int index)
    {
        var aRange = new DoubleRange(0, 1);
        var bRange = new DoubleRange(Minimum[index], Maximum[index]);

        var result = Normalize == true ? (One)(input?.Double() ?? 0) : (One)bRange.Convert(aRange.Minimum, aRange.Maximum, input?.Double() ?? 0);
        switch (index)
        {
            case 0: X = result; break;
            case 1: Y = result; break;
            case 2: Z = result; break;
        }
    }

    public override ColorModel GetColor()
    {
        var result3 = M.Denormalize(new Vector3(X, Y, Z), Minimum, Maximum);
        return Colour.New(Value, result3[0], result3[1], result3[2]);
    }
}

#endregion

#region NormalColorModel4

[Serializable]
public class NormalColorModel4 : NormalColorModel<Vector4>
{
    [Hide]
    public Vector Maximum => new(Colour.Components[Value][0].Maximum, Colour.Components[Value][1].Maximum, Colour.Components[Value][2].Maximum, Colour.Components[Value][3].Maximum);

    [Hide]
    public Vector Minimum => new(Colour.Components[Value][0].Minimum, Colour.Components[Value][1].Minimum, Colour.Components[Value][2].Minimum, Colour.Components[Value][3].Minimum);

    [Horizontal, Index(3), StringStyle(CanClear = false), UpdateSourceTrigger(UpdateSourceTrigger.LostFocus), Show]
    [Trigger(nameof(MemberModel.DisplayName), nameof(NameW))]
    [Trigger(nameof(MemberModel.RightText), nameof(UnitW))]
    public string DisplayW
    {
        get => GetDisplayValue(3);
        set => SetDisplayValue(value, 3);
    }

    [Hide]
    public string NameW => $"({Colour.Components[Value][3].Symbol}) {Colour.Components[Value][3].Name}";

    [Hide]
    public string UnitW => $"{Colour.Components[Value][3].Unit}";

    [Hide]
    public One W { get => Get<One>(); set => Set(value); }

    NormalColorModel4() : this(null) { }

    public NormalColorModel4(Type model) : base(model) { }

    public override object Clone() => new NormalColorModel4(Value);

    protected override string GetDisplayValue(int index)
    {
        One result = default;
        switch (index)
        {
            case 0:
                result = X;
                break;
            case 1:
                result = Y;
                break;
            case 2:
                result = Z;
                break;
            case 3:
                result = W;
                break;
        }

        var aRange = new DoubleRange(0, 1);
        var bRange = new DoubleRange(Minimum[index], Maximum[index]);
        return (Normalize == true ? (double)result : aRange.Convert(bRange.Minimum, bRange.Maximum, result)).Round(Precision).ToString();
    }

    protected override void SetDisplayValue(string input, int index)
    {
        var aRange = new DoubleRange(0, 1);
        var bRange = new DoubleRange(Minimum[index], Maximum[index]);

        var result = Normalize == true ? (One)(input?.Double() ?? 0) : (One)bRange.Convert(aRange.Minimum, aRange.Maximum, input?.Double() ?? 0);
        switch (index)
        {
            case 0: X = result; break;
            case 1: Y = result; break;
            case 2: Z = result; break;
            case 3: W = result; break;
        }
    }

    public override void OnPropertyChanged(PropertyEventArgs e)
    {
        base.OnPropertyChanged(e);
        switch (e.PropertyName)
        {
            case nameof(Normalize):
            case nameof(Precision):
                Update(() => DisplayW);
                break;

            case nameof(W):
                Update(() => DisplayW);
                OnValueChanged();
                break;
        }
    }

    public override ColorModel GetColor()
    {
        var result4 = M.Denormalize(new Vector4(X, Y, Z, W), Minimum, Maximum);
        return Colour.New(Value, result4[0], result4[1], result4[2], result4[3]);
    }
}

#endregion

#region NormalColorViewModel

[Serializable]
public class NormalColorViewModel : NamableCategory<Type>
{
    public string FirstLetter => Name.Substring(0, 1).ToUpper();

    public NormalColorViewModel(Type input) : base(input.Name, input.GetCategory() ?? "General", input) { }
}

#endregion