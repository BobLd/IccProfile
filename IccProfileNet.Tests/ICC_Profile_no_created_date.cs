using IccProfileNet.Tags;

namespace IccProfileNet.Tests
{
    public class ICC_Profile_no_created_date
    {
        private readonly IccProfile _profile;

        public ICC_Profile_no_created_date()
        {
            _profile = Helpers.OpenProfile("pdfpig", "ICC_Profile_no_created_date");
        }

        [Fact]
        public void Process()
        {
            _profile.TryProcessToPcs(new double[] { 0.5, 0.5, 0.5 }, null, out var test);
        }

        [Fact]
        public void Header()
        {
            Assert.Equal(3588, _profile.Data.Length);

            var header = _profile.Header;
            Assert.Equal((uint)_profile.Data.Length, header.ProfileSize);

            Assert.Equal("appl", header.Cmm);
            Assert.Equal(2, header.VersionMajor);
            Assert.Equal(0, header.VersionMinor);
            Assert.Equal(0, header.VersionBugFix);

            Assert.Equal(IccProfileClass.Display, header.ProfileClass);

            Assert.Equal(IccColourSpaceType.Gray, header.ColourSpace);
            Assert.Equal(IccProfileConnectionSpace.PCSXYZ, header.Pcs);

            Assert.Null(header.Created);
            Assert.Equal(IccPrimaryPlatforms.AppleComputer, header.PrimaryPlatformSignature);

            Assert.NotNull(header.ProfileFlags); // TODO

            Assert.Equal("none", header.DeviceManufacturer);

            Assert.NotNull(header.DeviceModel); // TODO

            Assert.NotNull(header.DeviceAttributes); // TODO

            Assert.Equal(IccRenderingIntent.Perceptual, header.RenderingIntent);

            Helpers.AssertIccXyz(header.nCIEXYZ, 0.96420, 1.0, 0.82491);

            Assert.Equal("appl", header.ProfileCreatorSignature);

            Assert.NotNull(header.ProfileId);
            Assert.False(header.IsProfileIdComputed());
        }

        [Fact]
        public void TagTable()
        {
            var tagTable = _profile.TagTable;
            Assert.NotNull(tagTable);
            Assert.Equal(5, tagTable.Length);

            Helpers.AssertTagTableElelement(tagTable[0], IccTags.GrayTRCTag, 192, 2060);
            Helpers.AssertTagTableElelement(tagTable[1], IccTags.MediaWhitePointTag, 2252, 20);
            Helpers.AssertTagTableElelement(tagTable[2], IccTags.CopyrightTag, 2272, 35);
            Helpers.AssertTagTableElelement(tagTable[3], IccTags.ProfileDescriptionTag, 2308, 121);
            Helpers.AssertTagTableElelement(tagTable[4], "dscm", 2432, 1154);
        }

        [Fact]
        public void Tags()
        {
            var tags = _profile.Tags;
            Assert.Equal(5, tags.Count);

            var dscm = tags["dscm"] as IccMultiLocalizedUnicodeType;

            Assert.NotNull(dscm);
            Assert.Equal(dscm.NumberOfRecords, dscm.Records.Length);
        }
    }
}
