using System;
using System.Linq;

namespace IccProfileNet.Tags
{
    /// <summary>
    /// XYZ type.
    /// </summary>
    public sealed class IccXyzType : IccTagTypeBase
    {
        public const int XyzOffset = 8;

        //public string Signature { get; internal set; }

        private readonly Lazy<IccXyz> _xyz;
        /// <summary>
        /// XYZ point.
        /// </summary>
        public IccXyz Xyz => _xyz.Value;

        public IccXyzType(byte[] rawData)
        {
            string typeSignature = IccHelper.GetString(rawData, TypeSignatureOffset, TypeSignatureLength);

            if (typeSignature != "XYZ ")
            {
                throw new ArgumentException(nameof(typeSignature));
            }

            RawData = rawData;

            _xyz = new Lazy<IccXyz>(() =>
            {
                return IccHelper.ReadXyz(RawData
                    .Skip(XyzOffset)
                    .ToArray());
            });
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Xyz.ToString();
        }
    }
}
