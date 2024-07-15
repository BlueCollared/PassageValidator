namespace RecordsRemoteListener
{
    public class SoftwareVersion
    {
        public string fileName;
        public string fileRelativePath;
        public DateTimeOffset effectiveFrom;
        public string SHA1;
        public string version;
    }

    public class SoftwareVersionsAtServer
    {
        public List<SoftwareVersion> versions = new();
    }
}