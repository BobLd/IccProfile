using IccProfileNet.Tags;
using static IccProfileNet.Tags.IccLutABType;

namespace IccProfileNet.Tests
{
#pragma warning disable IDE1006 // Naming Styles
    public class sRGB_v4_ICC_preference_displayclass
#pragma warning restore IDE1006 // Naming Styles
    {
        private readonly IccProfile _profile;

        public sRGB_v4_ICC_preference_displayclass()
        {
            _profile = Helpers.OpenProfile("color.org", "sRGB_v4_ICC_preference_displayclass");
        }

        [Fact]
        public void Process()
        {
            _profile.TryProcess(new double[] { 0.5, 0.5, 0.5 }, out var test);
        }

        [Fact]
        public void Header()
        {
            Assert.Equal(60988, _profile.Data.Length);

            var header = _profile.Header;
            Assert.Equal((uint)_profile.Data.Length, header.ProfileSize);
            Assert.Equal("none", header.Cmm);
            Assert.Equal(4, header.VersionMajor);
            Assert.Equal(2, header.VersionMinor);
            Assert.Equal(0, header.VersionBugFix);

            Assert.Equal(IccProfileClass.Display, header.ProfileClass);

            Assert.Equal(IccColourSpaceType.RGB, header.ColourSpace);
            Assert.Equal(IccProfileConnectionSpace.PCSLAB, header.Pcs);

            Assert.Equal(new DateTime(2009, 08, 10, 17, 28, 01), header.Created);
            Assert.Equal(IccPrimaryPlatforms.Unidentified, header.PrimaryPlatformSignature);

            Assert.NotNull(header.ProfileFlags); // TODO

            Assert.Equal("none", header.DeviceManufacturer);

            Assert.NotNull(header.DeviceModel); // TODO

            Assert.NotNull(header.DeviceAttributes); // TODO

            Assert.Equal(IccRenderingIntent.Perceptual, header.RenderingIntent);

            Assert.NotNull(header.nCIEXYZ);
            Helpers.AssertIccXyz(header.nCIEXYZ, 0.96420f, 1.0f, 0.82491f);

            Assert.Equal("none", header.ProfileCreatorSignature);

            Assert.NotNull(header.ProfileId); // TODO
        }

        [Fact]
        public void TagTable()
        {
            var tagTable = _profile.TagTable;
            Assert.NotNull(tagTable);
            Assert.Equal(9, tagTable.Length);

            Helpers.AssertTagTableElelement(tagTable[0], IccTags.ProfileDescriptionTag, 240, 146);
            Helpers.AssertTagTableElelement(tagTable[1], IccTags.AToB0Tag, 388, 29712);
            Helpers.AssertTagTableElelement(tagTable[2], IccTags.AToB1Tag, 30100, 436);
            Helpers.AssertTagTableElelement(tagTable[3], IccTags.BToA0Tag, 30536, 29748);
            Helpers.AssertTagTableElelement(tagTable[4], IccTags.BToA1Tag, 60284, 508);
            Helpers.AssertTagTableElelement(tagTable[5], IccTags.PerceptualRenderingIntentGamutTag, 60792, 12);
            Helpers.AssertTagTableElelement(tagTable[6], IccTags.MediaWhitePointTag, 60804, 20);
            Helpers.AssertTagTableElelement(tagTable[7], IccTags.CopyrightTag, 60824, 118);
            Helpers.AssertTagTableElelement(tagTable[8], IccTags.ChromaticAdaptationTag, 60944, 44);
        }

        [Fact]
        public void Tags()
        {
            var tags = _profile.Tags.Values.ToArray();

            Assert.Equal(9, tags.Length);

            var desc = tags[0];
            Helpers.AssertIccMultiLocalizedUnicodeType(desc, new[]
            {
                ("en", "US", "sRGB v4 ICC preference perceptual intent beta display class")
            });

            var a2b0 = tags[1] as IccLutABType;
            Assert.NotNull(a2b0);
            Helpers.AssertBaseLutABType(a2b0, LutABType.AB, 3, 3, 32,
                80, new double[]
                {
                    1, 0, 0,
                    0, 1, 0,
                    0, 0, 1,

                    0, 0, 0
                },
                128, 176, 29676,
                Array.Empty<int>(),
                Array.Empty<double>());

            var lookuped = a2b0.LookupClut(new double[] { 0, 0, 0 });
            Helpers.AssertClutLookup(lookuped, 0.0311131, 0.5019608, 0.5019608);

            lookuped = a2b0.LookupClut(new double[] { 1.0 / 17.0, 0, 0 });
            Helpers.AssertClutLookup(lookuped, 0.0328832, 0.5297169, 0.5202869);

            lookuped = a2b0.LookupClut(new double[] { 0, 1.0 / 17.0, 0 });
            Helpers.AssertClutLookup(lookuped, 0.0374914, 0.4688487, 0.5247883);

            lookuped = a2b0.LookupClut(new double[] { 0, 0, 1.0 / 17.0 });
            Helpers.AssertClutLookup(lookuped, 0.0301671, 0.5177539, 0.4666972);

            lookuped = a2b0.LookupClut(new double[] { 0, 1.0 / 17.0, 1.0 / 17.0 });
            Helpers.AssertClutLookup(lookuped, 0.0554208, 0.4788739, 0.4859541);

            lookuped = a2b0.LookupClut(new double[] { 10.0 / 17.0, 7.0 / 17.0, 4.0 / 17.0 });
            Helpers.AssertClutLookup(lookuped, 0.5173571, 0.5677424, 0.636713207);

            lookuped = a2b0.LookupClut(new double[] { 16.0 / 17.0, 16.0 / 17.0, 16.0 / 17.0 });
            Assert.Equal(3, lookuped.Length);

            var a2b1 = tags[2] as IccLutABType;
            Assert.NotNull(a2b1);
            Helpers.AssertBaseLutABType(a2b1, LutABType.AB, 3, 3, 32,
                80, new double[]
                {
                    0, 1, 0,
                    1.69034, -1.69034, 0,
                    0, 0.676132, -0.676132,

                    0, 0.501968, 0.501968
                },
                128, 248, 316,
                Array.Empty<int>(),
                Array.Empty<double>());

            var a2b1CurveM0 = a2b1.MCurves[0] as IccParametricCurveType;
            double t0 = a2b1CurveM0.Process(19660.0 / (double)ushort.MaxValue) * (double)ushort.MaxValue;
            Assert.InRange(t0, 39321, 45874);

            var a2b1CurveM1 = a2b1.MCurves[1] as IccParametricCurveType;
            double t1 = a2b1CurveM1.Process(19660.0 / (double)ushort.MaxValue) * (double)ushort.MaxValue;
            Assert.InRange(t1, 39321, 45874);

            var a2b1CurveM2 = a2b1.MCurves[2] as IccParametricCurveType;
            double t2 = a2b1CurveM2.Process(19660.0 / (double)ushort.MaxValue) * (double)ushort.MaxValue;
            Assert.InRange(t2, 39321, 45874);
            Assert.InRange(t0, t1, t2);

            var a2b1CurveB0 = a2b1.BCurves[0] as IccCurveType;
            Assert.Equal(0.5, a2b1CurveB0.Process(0.5));
            Assert.Equal(0, a2b1CurveB0.Process(0));
            Assert.Equal(1, a2b1CurveB0.Process(1));

            double[] input = new double[] { 0.5, 0.5, 0.5 };
            var output = a2b1.Process(input, _profile.Header);

            Assert.Equal(3, output.Length);

            var b2a0 = tags[3];
            Helpers.AssertBaseLutABType(b2a0, LutABType.BA, 3, 3, 32,
                80, new double[]
                {
                    1, 0, 0,
                    0, 1, 0,
                    0, 0, 1,

                    0, 0, 0
                },
                128, 176, 29676,
                Array.Empty<int>(),
                Array.Empty<double>());

            var b2a1 = tags[4];
            Helpers.AssertBaseLutABType(b2a1, LutABType.BA, 3, 3, 32,
                152, new double[]
                {
                    1, 0.591599, 0,
                    1, 0, 0,
                    1, 0, -1.479,

                    -0.296967, 0, 0.742401
                },
                200, 320, 388,
                Array.Empty<int>(),
                Array.Empty<double>());

            var rig0 = tags[5];
            Helpers.AssertIccSignatureType(rig0, "prmg");

            var wtpt = tags[6];
            Helpers.AssertIccXyzType(wtpt, 0.96420, 1.0, 0.82491);

            var cprt = tags[7];
            Helpers.AssertIccMultiLocalizedUnicodeType(cprt, new[]
            {
                ("en", "US", "Copyright 2007 International Color Consortium")
            });

            var chad = tags[8];
            Helpers.AssertIccS15Fixed16ArrayType(chad, new double[]
            {
                1.048019, 0.023010, -0.050171,
                0.029724, 0.990341, -0.017075,
                -0.009232, 0.010284, 0.752136
            });
        }
    }
}
