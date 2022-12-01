namespace CFDIWEB.Interfaces
{
    public interface IPdf
    {
        public Task<List<byte[]>> Pdfversion(List<byte[]> files);
        
    }
}
