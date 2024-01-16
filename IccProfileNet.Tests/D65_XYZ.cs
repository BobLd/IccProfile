namespace IccProfileNet.Tests
{
    public class D65_XYZ
    {
        private readonly IccProfile _profile;

        public D65_XYZ()
        {
            _profile = Helpers.OpenProfile("color.org", "D65_XYZ");
        }

        [Fact]
        public void Process()
        {
            _profile.TryProcessToPcs(new double[] { 0.5, 0.5, 0.5 }, null, out var test);
        }

        [Fact]
        public void Header()
        {
            Assert.Equal(968, _profile.Data.Length);

            var header = _profile.Header;
            Assert.Equal((uint)_profile.Data.Length, header.ProfileSize);
            Assert.Equal("none", header.Cmm);
            Assert.Equal(2, header.VersionMajor);
            Assert.Equal(4, header.VersionMinor);
            Assert.Equal(0, header.VersionBugFix);

            Assert.Equal(IccProfileClass.Display, header.ProfileClass);

            Assert.Equal(IccColourSpaceType.RGB, header.ColourSpace);
            Assert.Equal(IccProfileConnectionSpace.PCSXYZ, header.Pcs);

            Assert.Equal(new DateTime(2004, 07, 21, 18, 57, 42), header.Created);
            Assert.Equal(IccPrimaryPlatforms.Unidentified, header.PrimaryPlatformSignature);

            Assert.NotNull(header.ProfileFlags); // TODO

            Assert.Equal("none", header.DeviceManufacturer);

            Assert.NotNull(header.DeviceModel); // TODO

            Assert.NotNull(header.DeviceAttributes); // TODO

            Assert.Equal(IccRenderingIntent.MediaRelativeColorimetric, header.RenderingIntent);

            Helpers.AssertIccXyz(header.nCIEXYZ, 0.96420, 1.0, 0.82491);

            Assert.Equal("none", header.ProfileCreatorSignature);

            Assert.NotNull(header.ProfileId);
            Assert.False(header.IsProfileIdComputed());
        }

        [Fact]
        public void TagTable()
        {
            var tagTable = _profile.TagTable;
            Assert.NotNull(tagTable);
            Assert.Equal(16, tagTable.Length);

            Helpers.AssertTagTableElelement(tagTable[0], IccTags.ProfileDescriptionTag, 324, 106);
            Helpers.AssertTagTableElelement(tagTable[1], IccTags.BlueColorantTag, 432, 20);
            Helpers.AssertTagTableElelement(tagTable[2], IccTags.BlueTRCTag, 452, 12);
            Helpers.AssertTagTableElelement(tagTable[3], IccTags.DeviceModelDescTag, 464, 136);
            Helpers.AssertTagTableElelement(tagTable[4], IccTags.GreenColorantTag, 600, 20);
            Helpers.AssertTagTableElelement(tagTable[5], IccTags.GreenTRCTag, 452, 12);
            Helpers.AssertTagTableElelement(tagTable[6], IccTags.LuminanceTag, 620, 20);
            Helpers.AssertTagTableElelement(tagTable[7], IccTags.MeasurementTag, 640, 36);
            Helpers.AssertTagTableElelement(tagTable[8], "bkpt", 676, 20);
            Helpers.AssertTagTableElelement(tagTable[9], IccTags.RedColorantTag, 696, 20);
            Helpers.AssertTagTableElelement(tagTable[10], IccTags.RedTRCTag, 452, 12);
            Helpers.AssertTagTableElelement(tagTable[11], IccTags.TechnologyTag, 716, 12);
            Helpers.AssertTagTableElelement(tagTable[12], IccTags.ViewingCondDescTag, 728, 135);
            Helpers.AssertTagTableElelement(tagTable[13], IccTags.MediaWhitePointTag, 864, 20);
            Helpers.AssertTagTableElelement(tagTable[14], IccTags.CopyrightTag, 884, 40);
            Helpers.AssertTagTableElelement(tagTable[15], IccTags.ChromaticAdaptationTag, 924, 44);
        }

        [Fact]
        public void Tags()
        {
            var tags = _profile.Tags;

            Assert.Equal(16, tags.Count);
        }
    }
}
