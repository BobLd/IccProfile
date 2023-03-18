using IccProfileNet.Tags;
using static IccProfileNet.Tags.IccLutABType;

namespace IccProfileNet.Tests
{
    internal static class Helpers
    {
        public static void AssertClutLookup(double[] lookup, params double[] values)
        {
            Assert.Equal(values.Length, lookup.Length);
            for (int i = 0; i < lookup.Length; i++)
            {
                Assert.Equal(values[i], lookup[i], 5);
            }
        }

        public static void AssertTagTableElelement(IccTagTableItem element, string signature, uint offset, uint size)
        {
            Assert.Equal(signature, element.Signature);
            Assert.Equal(offset, element.Offset);
            Assert.Equal(size, element.Size);
        }

        public static IccProfile OpenProfile(string source, string profileName)
        {
            string path = Path.Combine("Profiles", source, profileName);
            var profileBytes = File.ReadAllBytes($"{path}.icc");
            return new IccProfile(profileBytes);
        }

        public static void AssertIccMeasurementType(IccTagTypeBase tag, (double x, double y, double z) backing,
            string standardIlluminant, string standardObserver)
        {
            var meas = tag as IccMeasurementType;
            Assert.NotNull(meas);

            AssertIccXyz(meas.Tristimulus, backing.x, backing.y, backing.z);
            Assert.Equal(standardIlluminant, meas.StandardIlluminant);
            Assert.Equal(standardObserver, meas.StandardObserver);
            // TODO - do the rest
        }

        public static void AssertIccTextType(IccTagTypeBase tag, string text)
        {
            var desc = tag as IccTextType;
            Assert.NotNull(desc);

            Assert.Equal(text, desc.Text);
        }

        public static void AssertIccSignatureType(IccTagTypeBase tag, string signature)
        {
            var desc = tag as IccSignatureType;
            Assert.NotNull(desc);

            Assert.Equal(signature, desc.Signature);
        }

        public static void AssertIccMultiLocalizedUnicodeType(IccTagTypeBase tag, (string language, string country, string text)[] records)
        {
            var desc = tag as IccMultiLocalizedUnicodeType;
            Assert.NotNull(desc);

            Assert.Equal(records.Length, desc.NumberOfRecords);

            for (int r = 0; r < desc.NumberOfRecords; r++)
            {
                var (language, country, text) = records[r];
                var actual = desc.Records[r];
                AssertIccMultiLocalizedUnicodeTypeRecord(actual, language, country, text);
            }
        }

        public static void AssertIccMultiLocalizedUnicodeTypeRecord(IccMultiLocalizedUnicodeType.IccMultiLocalizedUnicodeRecord record,
            string language, string country, string text)
        {
            Assert.Equal(language, record.Language);
            Assert.Equal(country, record.Country);
            Assert.Equal(text, record.Text);
        }

        public static void AssertBaseLutABType(IccTagTypeBase tag, LutABType type,
            int input, int output,
            int offsetBCurve,
            int offsetMatrix, double[] matrix,
            int offsetMCurve,
            int offsetClut,
            int offsetACurve,
            int[] clutInput,
            double[] expectedOutput)
        {
            var lutAB = tag as IccLutABType;
            Assert.NotNull(lutAB);

            Assert.Equal(type, lutAB.Type);
            Assert.Equal(input, lutAB.NumberOfInputChannels);
            Assert.Equal(output, lutAB.NumberOfOutputChannels);

            Assert.Equal(offsetBCurve, lutAB.OffsetFirstBCurve);
            Assert.Equal(type == LutABType.AB ? output : input, lutAB.BCurves.Length);

            Assert.Equal(offsetMatrix, lutAB.OffsetMatrix);
            Assert.NotNull(lutAB.Matrix);
            AssertFloatArray(matrix, lutAB.Matrix, 0.0001f);

            Assert.Equal(offsetMCurve, lutAB.OffsetFirstMCurve);
            Assert.NotNull(lutAB.MCurves);
            Assert.Equal(type == LutABType.AB ? output : input, lutAB.MCurves.Length);

            Assert.Equal(offsetClut, lutAB.OffsetClut);

            Assert.Equal(offsetACurve, lutAB.OffsetFirstACurve);
            Assert.NotNull(lutAB.ACurves);
            Assert.Equal(type == LutABType.AB ? input : output, lutAB.ACurves.Length);

            //lutAB.Clut
        }

        public static void AssertIccXyzType(IccTagTypeBase tag, double x, double y, double z)
        {
            var xyz = tag as IccXyzType;
            Assert.NotNull(xyz);

            AssertIccXyz(xyz.Xyz, x, y, z);
        }

        public static void AssertIccXyz(IccXyz xyz, double x, double y, double z)
        {
            Assert.Equal(x, xyz.X, 0.0001);
            Assert.Equal(y, xyz.Y, 0.0001);
            Assert.Equal(z, xyz.Z, 0.0001);
        }

        public static void AssertIccS15Fixed16ArrayType(IccTagTypeBase tag, double[] values)
        {
            var array = tag as IccS15Fixed16ArrayType;
            Assert.NotNull(array);
            AssertFloatArray(values, array.Values, 0.0001f);
        }

        public static void AssertFloatArray(double[] expected, double[] actual, double precision)
        {
            Assert.Equal(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actual[i], precision);
            }
        }
    }
}
