namespace TrackerService.Data
{
    public class StorageConnectionInfo
    {
        public StorageConnectionInfo(string connString, string containerName)
        {
            ConnectionString = connString;
            ContainerName = containerName;
        }

        public string ContainerName { get; set; }

        public string ConnectionString { get; set; }
    }
}
