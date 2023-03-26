namespace IccProfileNet.Tests
{
    public class ISO22028_2_ROMM_RGB
    {
        private readonly IccProfile _profile;

        public ISO22028_2_ROMM_RGB()
        {
            _profile = Helpers.OpenProfile("color.org", "ISO22028-2_ROMM-RGB");
        }

        [Fact]
        public void Process()
        {
            _profile.TryProcess(new double[] { 0.5, 0.5, 0.5 }, out var test);
        }

        [Fact]
        public void Header()
        {
            Assert.Equal(864, _profile.Data.Length);

            var header = _profile.Header;
            Assert.Equal((uint)_profile.Data.Length, header.ProfileSize);
            Assert.Equal("none", header.Cmm);
            Assert.Equal(4, header.VersionMajor);
            Assert.Equal(0, header.VersionMinor);
            Assert.Equal(0, header.VersionBugFix);

            Assert.Equal(IccProfileClass.ColorSpace, header.ProfileClass);

            Assert.Equal(IccColourSpaceType.RGB, header.ColourSpace);
            Assert.Equal(IccProfileConnectionSpace.PCSXYZ, header.Pcs);

            Assert.Equal(new DateTime(2006, 11, 19, 15, 19, 53), header.Created);
            Assert.Equal(IccPrimaryPlatforms.Unidentified, header.PrimaryPlatformSignature);

            Assert.NotNull(header.ProfileFlags); // TODO

            Assert.Equal("none", header.DeviceManufacturer);

            Assert.NotNull(header.DeviceModel); // TODO

            Assert.NotNull(header.DeviceAttributes); // TODO

            Assert.Equal(IccRenderingIntent.Perceptual, header.RenderingIntent);

            Helpers.AssertIccXyz(header.nCIEXYZ, 0.96420, 1.0, 0.82491);

            Assert.Equal("none", header.ProfileCreatorSignature);

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
            Assert.Equal(6, tagTable.Length);

            Helpers.AssertTagTableElelement(tagTable[0], IccTags.ProfileDescriptionTag, 204, 84);
            Helpers.AssertTagTableElelement(tagTable[1], IccTags.AToB0Tag, 288, 212);
            Helpers.AssertTagTableElelement(tagTable[2], IccTags.BToA0Tag, 500, 212);
            Helpers.AssertTagTableElelement(tagTable[3], IccTags.MediaWhitePointTag, 712, 20);
            Helpers.AssertTagTableElelement(tagTable[4], IccTags.CopyrightTag, 732, 88);
            Helpers.AssertTagTableElelement(tagTable[5], IccTags.ChromaticAdaptationTag, 820, 44);
        }

        [Fact]
        public void Tags()
        {
            var tags = _profile.Tags;

            Assert.Equal(6, tags.Count);
        }
    }
}
