﻿using System;
using System.Linq;

namespace IccProfileNet.Tags
{
    public sealed class IccS15Fixed16ArrayType : IccTagTypeBase
    {
        public const int ArrayOffset = 8;

        private readonly Lazy<double[]> _values;
        public double[] Values => _values.Value;

        public IccS15Fixed16ArrayType(byte[] rawData)
        {
            string typeSignature = IccHelper.GetString(rawData, TypeSignatureOffset, TypeSignatureLength);

            if (typeSignature != "sf32")
            {
                throw new ArgumentException(nameof(typeSignature));
            }

            RawData = rawData;

            _values = new Lazy<double[]>(() =>
            {
                // An array of s15Fixed16Number values
                // 8 to end
                return IccHelper.Reads15Fixed16Array(RawData.Skip(ArrayOffset).ToArray());
            });
        }
    }
}
