using IccProfileNet.Tags;

namespace IccProfileNet.Tests
{
    public class PRMG_v201_MR
    {
        private readonly IccProfile _profile;

        public PRMG_v201_MR()
        {
            _profile = Helpers.OpenProfile("color.org", "PRMG_v2.0.1_MR");
        }

        [Fact]
        public void Process()
        {
            _profile.TryProcess(new double[] { 0.5, 0.5, 0.5 }, out var test);
        }

        [Fact]
        public void Header()
        {
            Assert.Equal(2601512, _profile.Data.Length);

            var header = _profile.Header;
            Assert.Equal((uint)_profile.Data.Length, header.ProfileSize);
            Assert.Equal("appl", header.Cmm);
            Assert.Equal(4, header.VersionMajor);
            Assert.Equal(2, header.VersionMinor);
            Assert.Equal(0, header.VersionBugFix);

            Assert.Equal(IccProfileClass.Output, header.ProfileClass);

            Assert.Equal(IccColourSpaceType.CMYK, header.ColourSpace);
            Assert.Equal(IccProfileConnectionSpace.PCSLAB, header.Pcs);

            Assert.Equal(new DateTime(2013, 02, 25, 15, 18, 08), header.Created);
            Assert.Equal(IccPrimaryPlatforms.MicrosoftCorporation, header.PrimaryPlatformSignature);

            Assert.NotNull(header.ProfileFlags); // TODO

            Assert.Equal("", header.DeviceManufacturer);

            Assert.NotNull(header.DeviceModel); // TODO

            Assert.NotNull(header.DeviceAttributes); // TODO

            Assert.Equal(IccRenderingIntent.Perceptual, header.RenderingIntent);

            Helpers.AssertIccXyz(header.nCIEXYZ, 0.96420, 1.0, 0.82491);

            Assert.Equal("KODA", header.ProfileCreatorSignature);

            Assert.NotNull(header.ProfileId); // TODO
        }

        [Fact]
        public void TagTable()
        {
            var tagTable = _profile.TagTable;
            Assert.NotNull(tagTable);
            Assert.Equal(13, tagTable.Length);

            Helpers.AssertTagTableElelement(tagTable[0], IccTags.CopyrightTag, 288, 166);
            Helpers.AssertTagTableElelement(tagTable[1], IccTags.ProfileDescriptionTag, 2601448, 64);
            Helpers.AssertTagTableElelement(tagTable[2], IccTags.MediaWhitePointTag, 456, 20);
            Helpers.AssertTagTableElelement(tagTable[3], IccTags.AToB1Tag, 476, 503324);
            Helpers.AssertTagTableElelement(tagTable[4], IccTags.BToA1Tag, 539856, 321824);
            Helpers.AssertTagTableElelement(tagTable[5], IccTags.AToB0Tag, 1183504, 503324);
            Helpers.AssertTagTableElelement(tagTable[6], IccTags.BToA0Tag, 861680, 321824);
            Helpers.AssertTagTableElelement(tagTable[7], IccTags.AToB2Tag, 2008652, 503324);
            Helpers.AssertTagTableElelement(tagTable[8], IccTags.BToA2Tag, 1686828, 321824);
            Helpers.AssertTagTableElelement(tagTable[9], IccTags.GamutTag, 503800, 36056);
            Helpers.AssertTagTableElelement(tagTable[10], IccTags.CharTargetTag, 2511976, 88308);
            Helpers.AssertTagTableElelement(tagTable[11], "imnK", 2600284, 762);
            Helpers.AssertTagTableElelement(tagTable[12], "imnM", 2601048, 399);
        }

        [Fact]
        public void Tags()
        {
            var tags = _profile.Tags;

            var a2b1 = tags["A2B1"] as IccLutABType;
            Assert.NotNull(a2b1);

            var lookuped = a2b1.LookupClut(new double[] { 0, 0, 0, 0 });
            Helpers.AssertClutLookup(lookuped, 1.0000000, 0.5019608, 0.501960784);

            lookuped = a2b1.LookupClut(new double[] { 16.0 / 17.0, 16.0 / 17.0, 16.0 / 17.0, 0 });
            Helpers.AssertClutLookup(lookuped, 0.1503624, 0.5011521, 0.502784771);

            lookuped = a2b1.LookupClut(new double[] { 16.0 / 17.0, 5.0 / 17.0, 10.0 / 17.0, 14.0 / 17.0 });
            Helpers.AssertClutLookup(lookuped, 0.1265278, 0.4333562, 0.494773785);

            lookuped = a2b1.LookupClut(new double[] { 16.0 / 17.0, 16.0 / 17.0, 16.0 / 17.0, 16.0 / 17.0, });
            Helpers.AssertClutLookup(lookuped, 0.0250401, 0.4938125, 0.504463264);

            var imnK = tags["imnM"] as IccTextType;
            Assert.NotNull(imnK);

            var imnM = tags["imnM"] as IccTextType;
            Assert.NotNull(imnM);

            Assert.Equal(13, tags.Count);
        }
    }
}
