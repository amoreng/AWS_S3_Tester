using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;
using Amazon.S3.Transfer;
using System.Collections.Generic;
using System.Text;

namespace AWS_S3_Tester
{
    public class AmazonS3
    {
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;
        private static readonly string accessKey = AWS_S3_TesterSettings.GetSetting("AWSAccessKey");
        private static readonly string secretKey = AWS_S3_TesterSettings.GetSetting("AWSSecretKey");
        private static readonly string bucketName = AWS_S3_TesterSettings.GetSetting("BucketName");
        private static readonly string filePath = @AWS_S3_TesterSettings.GetSetting("FileOutputPath");
        private static IAmazonS3 client;

       
        public void DownloadAllFiles()
        {
            //firsts calls a list async, then downloads files by key returned in the list
            client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
            DownloadAllFilesAsync().Wait();
            client.Dispose();
        }
        /// <summary>
        ///   <para>Downloads all files in specific S3 Bucket</para>
        ///   <para>It first calls for a list of objects, then downloads by object</para>
        ///   <para>Bucket and FileOutputPath are defined in the App.Config</para>
        /// </summary>
        public static async Task DownloadAllFilesAsync()
        {
            try
            {
                ListObjectsRequest request = new ListObjectsRequest
                {
                    BucketName = bucketName,
                    MaxKeys = 10
                };
                ListObjectsResponse response;
                do
                {
                    response = await client.ListObjectsAsync(request);                    

                    //Download Objects by Key
                    foreach (S3Object entry in response.S3Objects)
                    {
                        //Key is the file + extention, Size is file size in bytes
                        Console.WriteLine("Downloading key = {0} size = {1}...", entry.Key, entry.Size);
                        //TransferUtilityDownloadRequest works in conjunction with TransferUtility     
                        TransferUtilityDownloadRequest transferUtilityDownloadRequest = new TransferUtilityDownloadRequest
                        {
                            BucketName = bucketName,
                            FilePath = String.Format("{0}{1}", filePath, entry.Key),//builds out the file path by file name
                            Key = entry.Key 
                        };
                        TransferUtility tu = new TransferUtility(client);
                        //process the download 
                        tu.Download(transferUtilityDownloadRequest);

                        Console.WriteLine("File {0} downloaded!", entry.Key);
                    }


                } while (response.IsTruncated);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                Console.WriteLine("S3 error occurred. Exception: " + amazonS3Exception.ToString());
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                Console.ReadKey();
            }            
        }

        public void ListAllObjects()
        {
            //lists all objects in bucket
            client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
            ListAllObjectsAsync().Wait();
            client.Dispose();
        }
        /// <summary>
        ///   <para>Lists all files in specific S3 Bucket</para>
        ///   <para>Bucket and FileOutputPath are defined in the App.Config</para>
        /// </summary>
        public static async Task ListAllObjectsAsync()
        {
            try
            {
                ListObjectsRequest request = new ListObjectsRequest
                {
                    BucketName = bucketName,
                    MaxKeys = 10
                };
                ListObjectsResponse response;
                do
                {
                    response = await client.ListObjectsAsync(request);

                    // Process the response.
                    foreach (S3Object entry in response.S3Objects)
                    {
                        Console.WriteLine("key = {0} size = {1}",
                            entry.Key, entry.Size);                        
                    }                   
                   
                } while (response.IsTruncated);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                Console.WriteLine("S3 error occurred. Exception: " + amazonS3Exception.ToString());
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                Console.ReadKey();
            }
        }          
    }    
    
}
