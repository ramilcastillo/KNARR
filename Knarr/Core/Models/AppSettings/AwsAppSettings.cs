namespace Knarr.Core.Models.AppSettings
{
    public class AwsAppSettings
    {
        public string BucketName { get; set; }
        public string SubFolderProfile { get; set; }
        public string SubFolderService { get; set; }
        public string BucketLocation { get; set; }
        public string PublicDomain { get; set; }
    }
}
