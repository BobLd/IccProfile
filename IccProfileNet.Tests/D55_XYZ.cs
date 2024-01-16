namespace IccProfileNet.Tests
{
    public class D55_XYZ
    {
        private readonly IccProfile _profile;

        public D55_XYZ()
        {
            _profile = Helpers.OpenProfile("color.org", "D55_XYZ");
        }

        [Fact]
        public void Process()
        {
            _profile.TryProcessToPcs(new double[] { 0.5, 0.5, 0.5 }, null, out var test);
        }

        [Fact]
        public void Header()
        {
            Assert.Equal(560, _profile.Data.Length);

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
            Assert.Equal(11, tagTable.Length);

            Helpers.AssertTagTableElelement(tagTable[0], IccTags.ProfileDescriptionTag, 264, 106);
            Helpers.AssertTagTableElelement(tagTable[1], IccTags.BlueColorantTag, 372, 20);
            Helpers.AssertTagTableElelement(tagTable[2], IccTags.BlueTRCTag, 392, 12);
            Helpers.AssertTagTableElelement(tagTable[3], IccTags.GreenColorantTag, 404, 20);
            Helpers.AssertTagTableElelement(tagTable[4], IccTags.GreenTRCTag, 392, 12);
            Helpers.AssertTagTableElelement(tagTable[5], IccTags.LuminanceTag, 424, 20);
            Helpers.AssertTagTableElelement(tagTable[6], IccTags.MeasurementTag, 444, 36);
            Helpers.AssertTagTableElelement(tagTable[7], IccTags.RedColorantTag, 480, 20);
            Helpers.AssertTagTableElelement(tagTable[8], IccTags.RedTRCTag, 392, 12);
            Helpers.AssertTagTableElelement(tagTable[9], IccTags.MediaWhitePointTag, 500, 20);
            Helpers.AssertTagTableElelement(tagTable[10], IccTags.CopyrightTag, 520, 40);
        }

        [Fact]
        public void Tags()
        {
            var tags = _profile.Tags;

            Assert.Equal(11, tags.Count);
        }
    }
}
