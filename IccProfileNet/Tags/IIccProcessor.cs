namespace IccProfileNet.Tags
{
    public interface IIccProcessor
    {
        double[] Process(double[] input, IccProfileHeader header);
    }
}
