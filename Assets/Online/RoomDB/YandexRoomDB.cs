using System.IO;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace Online.RoomDB
{
    public class YandexRoomDB : IRoomDB
    {
        private const string accessKey = "YCAJE9Fm0xgl8rZTypVi3QjZy";
        private const string secretKey = "YCPLj4ZieUhVV4KO-cFjx8JupHfe7ofFHREsbPsY";
        private AmazonS3Client s3client;

        public YandexRoomDB()
        {
            var configsS3 = new AmazonS3Config
            {
                ServiceURL = "https://s3.yandexcloud.net"
            };
            var creds = new BasicAWSCredentials(accessKey, secretKey);
            s3client = new AmazonS3Client(creds, configsS3);
        }

        public void SaveRoom(string text)
        {
            PutObjectRequest r = new PutObjectRequest
                {BucketName = "testkn", Key = "тест.txt", ContentBody = text};
            var t = s3client.PutObjectAsync(r).Result;
        }

        public string LoadRoom()
        {
            GetObjectRequest request = new GetObjectRequest {BucketName = "testkn", Key = "тест.txt"};
            var o = s3client.GetObjectAsync(request).Result;
            var reader = new StreamReader(o.ResponseStream);
            return reader.ReadToEnd();
        }
    }
}