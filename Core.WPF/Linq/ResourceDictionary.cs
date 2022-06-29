﻿using Imagin.Core.Analytics;
using Imagin.Core.Collections.Generic;
using Imagin.Core.Media;
using Imagin.Core.Numerics;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Core.Linq
{
    public static class XResourceDictionary
    {
        public static ObjectDictionary Convert(this ResourceDictionary input)
        {
            var result = new ObjectDictionary();
            foreach (DictionaryEntry i in input)
            {
                if (i.Key is string key)
                {
                    if (i.Value is SolidColorBrush solidColorBrush)
                        result.Add(key, new ByteVector4(solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B, solidColorBrush.Color.A));

                    else if (i.Value is LinearGradientBrush linearGradientBrush)
                        result.Add(key, new Gradient(linearGradientBrush));

                    else if (i.Value is RadialGradientBrush radialGradientBrush)
                        result.Add(key, new Gradient(radialGradientBrush));
                }
            }
            return result;
        }

        public static ResourceDictionary Convert(this ObjectDictionary input)
        {
            var result = new ResourceDictionary();
            foreach (var i in input)
            {
                if (i.Value is ByteVector4 color)
                    result.Add(i.Key, new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B)));

                else if (i.Value is Gradient linearGradient)
                    result.Add(i.Key, linearGradient.LinearBrush());

                //We'll have to come back to this in order to support RadialGradientBrush
                else if (i.Value is Gradient radialGradient)
                    result.Add(i.Key, radialGradient.LinearBrush());
            }
            return result;
        }

        //...

        public static Result TryLoad(string filePath, out ResourceDictionary result)
        {
            result = null;
            try
            {
                Serialization.BinarySerializer.Deserialize(filePath, out ObjectDictionary finalResult);
                result = finalResult.Convert();
                return true;
            }
            catch (Exception e)
            {
                Log.Write<ResourceDictionary>(e);
                return e;
            }
        }

        public static Result TrySave(this ResourceDictionary input, string filePath)
        {
            try
            {
                var result = input.Convert();
                Serialization.BinarySerializer.Serialize(filePath, result);
                return true;
            }
            catch(Exception e)
            {
                Log.Write<Resources>(e);
                return e;
            }
        }
    }
}