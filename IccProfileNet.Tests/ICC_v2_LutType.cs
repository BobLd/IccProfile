using IccProfileNet.Tags;

namespace IccProfileNet.Tests
{
    public class ICC_v2_LutType
    {
        private readonly IccProfile _profile;

        public ICC_v2_LutType()
        {
            _profile = Helpers.OpenProfile("pdfpig", "ICC_v2_LutType");
        }

        [Fact]
        public void Process()
        {
            _profile.TryProcess(new double[] { 0.5, 0.5, 0.5 }, out var test);
        }

        [Fact]
        public void Header()
        {
            Assert.Equal(557188, _profile.Data.Length);

            var header = _profile.Header;
            Assert.Equal((uint)_profile.Data.Length, header.ProfileSize);
            Assert.Equal("ADBE", header.Cmm);
            Assert.Equal(2, header.VersionMajor);
            Assert.Equal(1, header.VersionMinor);
            Assert.Equal(0, header.VersionBugFix);

            Assert.Equal(IccProfileClass.Output, header.ProfileClass);

            Assert.Equal(IccColourSpaceType.CMYK, header.ColourSpace);
            Assert.Equal(IccProfileConnectionSpace.PCSLAB, header.Pcs);

            Assert.Equal(new DateTime(2007, 08, 20), header.Created);
            Assert.Equal(IccPrimaryPlatforms.AppleComputer, header.PrimaryPlatformSignature);

            Assert.NotNull(header.ProfileFlags); // TODO

            Assert.Equal("ADBE", header.DeviceManufacturer);

            Assert.NotNull(header.DeviceModel); // TODO

            Assert.NotNull(header.DeviceAttributes); // TODO

            Assert.Equal(IccRenderingIntent.Perceptual, header.RenderingIntent);

            Helpers.AssertIccXyz(header.nCIEXYZ, 0.96420, 1.0, 0.82491);

            Assert.Equal("ADBE", header.ProfileCreatorSignature);

            Assert.NotNull(header.ProfileId);
            Assert.False(header.IsProfileIdComputed());
        }

        [Fact]
        public void TagTable()
        {
            var tagTable = _profile.TagTable;
            Assert.NotNull(tagTable);
            Assert.Equal(10, tagTable.Length);

            Helpers.AssertTagTableElelement(tagTable[0], IccTags.ProfileDescriptionTag, 252, 124);
            Helpers.AssertTagTableElelement(tagTable[1], IccTags.MediaWhitePointTag, 376, 20);
            Helpers.AssertTagTableElelement(tagTable[2], IccTags.AToB0Tag, 396, 41478);
            Helpers.AssertTagTableElelement(tagTable[3], IccTags.AToB2Tag, 396, 41478);
            Helpers.AssertTagTableElelement(tagTable[4], IccTags.AToB1Tag, 41876, 41478);
            Helpers.AssertTagTableElelement(tagTable[5], IccTags.BToA0Tag, 83356, 145588);
            Helpers.AssertTagTableElelement(tagTable[6], IccTags.BToA1Tag, 228944, 145588);
            Helpers.AssertTagTableElelement(tagTable[7], IccTags.BToA2Tag, 374532, 145588);
            Helpers.AssertTagTableElelement(tagTable[8], IccTags.GamutTag, 520120, 37009);
            Helpers.AssertTagTableElelement(tagTable[9], IccTags.CopyrightTag, 557132, 55);
        }

        [Fact]
        public void Tags()
        {
            var tags = _profile.Tags;
            Assert.Equal(10, tags.Count);

            var a2b0 = tags["A2B0"] as IccLut16Type;
            Assert.NotNull(a2b0);

            var lookuped = a2b0.LookupClut(new double[] { 0, 0, 0, 0 });
            Helpers.AssertClutLookup(lookuped, 0.9961089, 0.5000076, 0.50000763);

            lookuped = a2b0.LookupClut(new double[] { 6.0 / 9.0, 4.0 / 9.0, 5.0 / 9.0, 8.0 / 9.0 });
            Helpers.AssertClutLookup(lookuped, 0.0124971, 0.4895247, 0.4993973);

            lookuped = a2b0.LookupClut(new double[] { 8.0 / 9.0, 8.0 / 9.0, 8.0 / 9.0, 8.0 / 9.0 });
            Helpers.AssertClutLookup(lookuped, 0.0000000, 0.5000076, 0.5000076);

            double[] resultA2b0 = a2b0.Process(new double[] { 0.5, 0.5, 0.5, 0.5 }, _profile.Header);
            Assert.Equal(3, resultA2b0.Length);

            var b2a0 = tags["B2A0"] as IccLut8Type;
            Assert.NotNull(b2a0);

            var lookuped2 = b2a0.LookupClut(new double[] { 0, 0, 0 });
            Helpers.AssertClutLookup(lookuped2, 0.9686275, 0.7294118, 0.1921569, 0.1058824);

            lookuped2 = b2a0.LookupClut(new double[] { 32.0 / 33.0, 32.0 / 33.0, 32.0 / 33.0, });
            Helpers.AssertClutLookup(lookuped2, 0.0000000, 0.8078431, 0.5450980, 0.0000000);

            lookuped2 = b2a0.LookupClut(new double[] { 22.0 / 33.0, 32.0 / 33.0, 13.0 / 33.0, });
            Helpers.AssertClutLookup(lookuped2, 0.0000000, 0.7843137, 0.0000000, 0.0000000);

            double[] resultB2a0 = b2a0.Process(new double[] { 0.5, 0.5, 0.5 }, _profile.Header);
            Assert.Equal(4, resultB2a0.Length);
        }
    }
}
