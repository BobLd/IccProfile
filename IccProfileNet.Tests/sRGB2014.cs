using IccProfileNet.Tags;

namespace IccProfileNet.Tests
{
#pragma warning disable IDE1006 // Naming Styles
    public class sRGB2014
#pragma warning restore IDE1006 // Naming Styles
    {
        private readonly IccProfile _profile;

        public sRGB2014()
        {
            _profile = Helpers.OpenProfile("color.org", "sRGB2014");
        }

        [Fact]
        public void Process()
        {
            _profile.TryProcess(new double[] { 0.5, 0.5, 0.5 }, out var test);
        }

        [Fact]
        public void Header()
        {
            Assert.Equal(3024, _profile.Data.Length);

            var header = _profile.Header;
            Assert.Equal((uint)_profile.Data.Length, header.ProfileSize);
            Assert.Equal("", header.Cmm);
            Assert.Equal(2, header.VersionMajor);
            Assert.Equal(0, header.VersionMinor);
            Assert.Equal(0, header.VersionBugFix);

            Assert.Equal(IccProfileClass.Display, header.ProfileClass);

            Assert.Equal(IccColourSpaceType.RGB, header.ColourSpace);
            Assert.Equal(IccProfileConnectionSpace.PCSXYZ, header.Pcs);

            Assert.Equal(new DateTime(2015, 02, 15), header.Created);
            Assert.Equal(IccPrimaryPlatforms.Unidentified, header.PrimaryPlatformSignature);

            Assert.NotNull(header.ProfileFlags); // TODO

            Assert.Equal("", header.DeviceManufacturer);

            Assert.NotNull(header.DeviceModel); // TODO

            Assert.NotNull(header.DeviceAttributes); // TODO

            Assert.Equal(IccRenderingIntent.Perceptual, header.RenderingIntent);

            Helpers.AssertIccXyz(header.nCIEXYZ, 0.96420, 1.0, 0.82491);

            Assert.Equal("", header.ProfileCreatorSignature);

            Assert.NotNull(header.ProfileId);
            Assert.True(header.IsProfileIdComputed());
            var profileIDCheck = _profile.ComputeProfileId();
            Assert.Equal(profileIDCheck, header.ProfileId);
        }

        [Fact]
        public void TagTable()
        {
            var tagTable = _profile.TagTable;
            Assert.NotNull(tagTable);
            Assert.Equal(16, tagTable.Length);

            Helpers.AssertTagTableElelement(tagTable[0], IccTags.ProfileDescriptionTag, 324, 99);
            Helpers.AssertTagTableElelement(tagTable[1], IccTags.BlueColorantTag, 424, 20);
            Helpers.AssertTagTableElelement(tagTable[2], IccTags.BlueTRCTag, 444, 2060);
            Helpers.AssertTagTableElelement(tagTable[3], IccTags.GreenTRCTag, 444, 2060);
            Helpers.AssertTagTableElelement(tagTable[4], IccTags.RedTRCTag, 444, 2060);
            Helpers.AssertTagTableElelement(tagTable[5], IccTags.DeviceModelDescTag, 2504, 136);
            Helpers.AssertTagTableElelement(tagTable[6], IccTags.GreenColorantTag, 2640, 20);
            Helpers.AssertTagTableElelement(tagTable[7], IccTags.LuminanceTag, 2660, 20);
            Helpers.AssertTagTableElelement(tagTable[8], IccTags.MeasurementTag, 2680, 36);
            Helpers.AssertTagTableElelement(tagTable[9], "bkpt", 2716, 20);
            Helpers.AssertTagTableElelement(tagTable[10], IccTags.RedColorantTag, 2736, 20);
            Helpers.AssertTagTableElelement(tagTable[11], IccTags.TechnologyTag, 2756, 12);
            Helpers.AssertTagTableElelement(tagTable[12], IccTags.ViewingCondDescTag, 2768, 135);
            Helpers.AssertTagTableElelement(tagTable[13], IccTags.MediaWhitePointTag, 2904, 20);
            Helpers.AssertTagTableElelement(tagTable[14], IccTags.CopyrightTag, 2924, 55);
            Helpers.AssertTagTableElelement(tagTable[15], IccTags.ChromaticAdaptationTag, 2980, 44);
        }
        [Fact]
        public void Tags()
        {
            var tags = _profile.Tags.Values.ToArray();

            Assert.Equal(16, tags.Length);

            var desc = tags[0];
            Helpers.AssertIccTextType(desc, "\tsRGB2014"); // Why starts with a tab? Should not

            var bXYZ = tags[1];
            Helpers.AssertIccXyzType(bXYZ, 0.14307, 0.06061, 0.71410);

            var bTRC = tags[2];
            var curveBTRC = bTRC as IccCurveType;
            double t1 = curveBTRC.Process(0.799609);
            Assert.Equal(0.603159, t1, 5);

            t1 = curveBTRC.Process(0.999022);
            Assert.Equal(0.997772, t1, 5);

            t1 = curveBTRC.Process(0.999511);
            Assert.Equal(0.998886, t1, 5);

            t1 = curveBTRC.Process(1.0);
            Assert.Equal(1.0, t1);

            t1 = curveBTRC.Process(0.0);
            Assert.Equal(0.0, t1);

            var gTRC = tags[3];
            // TODO

            var rTRC = tags[4];
            // TODO

            var dmdd = tags[5];
            Helpers.AssertIccTextType(dmdd, ".IEC 61966-2-1 Default RGB Colour Space - sRGB"); // Why starts with a point? Should not

            var gXYZ = tags[6];
            Helpers.AssertIccXyzType(gXYZ, 0.38515, 0.71687, 0.09708);

            var lumi = tags[7];
            Helpers.AssertIccXyzType(lumi, 0, 80, 0); // X and Z are always ignored

            var meas = tags[8];
            Helpers.AssertIccMeasurementType(meas, (0, 0, 0), "D65", "1931");

            var bkpt = tags[9];
            Helpers.AssertIccXyzType(bkpt, 0.00241, 0.00250, 0.00206);

            var rXYZ = tags[10];
            Helpers.AssertIccXyzType(rXYZ, 0.43607, 0.22249, 0.01392);

            var tech = tags[11];
            Helpers.AssertIccSignatureType(tech, "CRT ");

            var vued = tags[12];
            Helpers.AssertIccTextType(vued, "-Reference Viewing Condition in IEC 61966-2-1"); // Why starts with a dash? Should not

            var wtpt = tags[13];
            Helpers.AssertIccXyzType(wtpt, 0.96420, 1, 0.82491);

            var cprt = tags[14];
            Helpers.AssertIccTextType(cprt, "Copyright International Color Consortium, 2015");

            var chad = tags[15];
            Helpers.AssertIccS15Fixed16ArrayType(chad, new double[]
            {
                1.047913, 0.022934, -0.050201,
                0.029602, 0.990463, -0.017075,
                -0.009247, 0.015060, 0.751785
            });
        }
    }
}