namespace Upload_File.Models
{
    public class File
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public byte[] Content { get; set; }
    }
}
