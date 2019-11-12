namespace dotnet_tf_bert
{
    using System;
    using Newtonsoft.Json.Linq;
    using System.IO;
    using Amazon.S3;
    using Amazon.S3.Model;
    using Amazon.S3.Transfer;
    using System.Collections.Generic;

    public static class DownloadModel
    {
        public static void DownloadModelFiles()
        {
            var jsonFile = Path.Combine("..", "credential", "safemail-credhub-values.json");
            var myJsonString = File.ReadAllText(jsonFile);
            var myS3Config = new AwsConfig(myJsonString);
            var basicCredentials = new Amazon.Runtime.BasicAWSCredentials(myS3Config.AccessKey, myS3Config.SecretKey);
            AmazonS3Client s3Client = new AmazonS3Client(basicCredentials, Amazon.RegionEndpoint.GetBySystemName(myS3Config.Region));
            var fileTransferUtility = new TransferUtility(s3Client);

            var request = new ListObjectsRequest();

            var response = s3Client.ListObjectsAsync(myS3Config.Bucket, myS3Config.Folder).Result;

            foreach (S3Object obj in response.S3Objects)
            {
                try
                {
                    var dirList = new List<string> { "tmp" };
                    var fnames = obj.Key.Replace(myS3Config.Folder, "").Split("/", StringSplitOptions.RemoveEmptyEntries);
                    dirList.AddRange(fnames);
                    var filePath = Path.Combine(dirList.ToArray());
                    // if file not changed ,skip download
                    if (File.Exists(filePath))
                    {
                        FileInfo fi = new FileInfo(filePath);
                        if (fi.Length == obj.Size && fi.CreationTime > obj.LastModified)
                        {
                            continue;
                        }
                    }
                    Console.WriteLine("download {0} to {1}", obj.Key, filePath);
                    fileTransferUtility.Download(filePath, myS3Config.Bucket, obj.Key);

                }
                catch (Exception Excep)
                {
                    Console.WriteLine(Excep.Message, Excep.InnerException);
                }
            }
        }
    }

}
public class AwsConfig
{
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string Region { get; set; }
    public string Bucket { get; set; }
    public string Folder { get; set; }
    public AwsConfig(string jsonString)
    {
        var myJObject = Newtonsoft.Json.Linq.JObject.Parse(jsonString).SelectToken("AWS_S3");
        if (myJObject != null)
        {
            AccessKey = myJObject.Value<string>("AWS_ACCESS_KEY_ID");
            SecretKey = myJObject.Value<string>("AWS_SECRET_ACCESS_KEY");
            Region = myJObject.Value<string>("AWS_DEFAULT_REGION");
            Bucket = myJObject.Value<string>("BUCKET_NAME");
            Folder = myJObject.Value<string>("BUCKET_FOLDER");
        }
    }
}