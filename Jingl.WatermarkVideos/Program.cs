using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Threading;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Web;
using Microsoft.Azure.WebJobs;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
//using NReco.VideoInfo;

namespace Jingl.WatermarkVideos
{
    public class Program
    {

        public IDbConnection Connection
        {
            get
            {
                //return new SqlConnection("Server=tcp:sophielastic.database.windows.net,1433;Initial Catalog=JINGL_STG;Persist Security Info=False;User ID=sophielastic;Password=SophieHappy33;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");

                //return new SqlConnection("Server=tcp:jinglprod.database.windows.net,1433;Initial Catalog=JINGPROD;Persist Security Info=False;User ID=jinglprod;Password=SophieHappy33;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");
                return new SqlConnection("Server=tcp:fameodb.database.windows.net,1433;Initial Catalog=Fameoprd;Persist Security Info=False;User ID=fameodb;Password=azurefameo1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");

                //return new SqlConnection("Server=tcp:fameodb.database.windows.net,1433;Initial Catalog=FameoprdStaging;Persist Security Info=False;User ID=fameodb;Password=azurefameo1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");

                // return new SqlConnection("Server=localhost\\sqlexpress;Initial Catalog=FameoPrd;Persist Security Info=False;User ID=dendi;Password=osama123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=3600;");

            }
        }

        public static string StorageConn()
        {
            //return "DefaultEndpointsProtocol=https;AccountName=storagejingdev;AccountKey=YIFi0Prlo82TsPeY4I9pTpanpRfU596aJvJ/0p42GzonQXpb3aq0u6I2IQXtVtDqQIiHQp2BIhVkFxbmLpnjBA==;EndpointSuffix=core.windows.net";
            //return "DefaultEndpointsProtocol=https;AccountName=storagejingle;AccountKey=wL5D9X0qdWLQu/Ov2V32ff9Q2NhCxAVqiMmEOK+oHVbkRk6+5HdqKkVtd86qq1RnArtX4oUb8urL7txCrxQd0g==;EndpointSuffix=core.windows.net";
            //return "DefaultEndpointsProtocol=https;AccountName=fameostorage;AccountKey=Yr0ICyMCAJv2b6RPSh3YbIST7PYPljTxP0a7jp7Fi2Sc6rv3lJA1LTQMpcESZECis6Bk7hpNDWKR50cAeGsgqA==;EndpointSuffix=core.windows.net";
            return "DefaultEndpointsProtocol=https;AccountName=fameostoragesa;AccountKey=S4QHnLSHfbSZhtPKhU+MH6ZPVCfDmvbVmnZvvdqnRZNV7vm41gvfC99/ZxFrq+idVxZ1dRE1dLk/mbCTnTNPIQ==;EndpointSuffix=core.windows.net";
        }

        public static void Main()
        {
            ProcessMethod().Wait();
        }

        static void WatermarkProcess()
        {
            try
            {
                Program program = new Program();

                var DestFolder = Directory.GetCurrentDirectory() + "/videos/temp/";
                var watermarkFolder = Directory.GetCurrentDirectory() + "/videos/watermark/";
                var compressFolder = Directory.GetCurrentDirectory() + "/videos/compress/";
                var ThumbnailsFolder = Directory.GetCurrentDirectory() + "/videos/thumb/";
                var RelocationMoovFolder = Directory.GetCurrentDirectory() + "/videos/relocationmoov/";



                var ListFiles = program.GetFiles();

                if (ListFiles.Count > 0)
                {


                    foreach (var i in ListFiles)
                    {
                        Console.WriteLine("..........................................................................");
                        Console.WriteLine("Starting Watermark Process for " + i.Id);

                        FilesModel model = new FilesModel();
                        model.Id = i.Id;
                        model.Link = i.Link;
                        model.FileName = i.FileName;
                        model.FileType = i.FileType;

                        var FileData = i.FileName + i.FileType;

                        var filethumb = i.FileName + ".png";

                        // var containerName = "videos";
                        // var containerName = "watermark";
                        var containerName = "fameovideos";
                        //string storageConnection = StorageConn();
                        //CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);
                        //CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
                        //CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(containerName);
                        //CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(FileData);
                       // MemoryStream memStream = new MemoryStream();

                        Console.WriteLine("starting download from " + i.Link + " to Temp Folder");
                        // blockBlob.DownloadToStream(memStream);

                        DownloadFromGoogleCloud(Path.Combine(DestFolder, FileData), containerName);

                        Console.WriteLine("finish download ");

                        Console.WriteLine("..........................................................................");


                        // SaveMemoryStream(memStream, Path.Combine(DestFolder, FileData));

                       

                        Console.WriteLine("..........................................................................");

                        SetWatermark(Path.Combine(DestFolder, FileData), Path.Combine(compressFolder, FileData), Path.Combine(watermarkFolder, FileData), Path.Combine(RelocationMoovFolder, FileData));
                        SetThumbnails(Path.Combine(RelocationMoovFolder, FileData), Path.Combine(ThumbnailsFolder, filethumb));
                        Console.WriteLine("..........................................................................");
                        //string Uri = UploadToAzure(Path.Combine(watermarkFolder, FileData), FileData,"watermark");
                        string Uri = UploadToGoogleCloud(Path.Combine(RelocationMoovFolder, FileData), FileData, "fameowatermarkvideos");
                        model.Link = Uri;
                        program.UpdateWaterMarkLink(model);

                        // string Urithumb = UploadToAzure(Path.Combine(ThumbnailsFolder, filethumb), filethumb, "photos");
                        string Urithumb = UploadToGoogleCloud(Path.Combine(ThumbnailsFolder, filethumb), filethumb, "fameophotos");
                        model.thumbnails = Urithumb;
                       program.UpdateThumbnailsLink(model);

                        Console.WriteLine("..........................................................................");


                        Console.WriteLine("Starting Delete File in Temp Folder");
                        if (File.Exists(Path.Combine(DestFolder, FileData)))
                        {
                            File.Delete(Path.Combine(DestFolder, FileData));
                        }

                        if (File.Exists(Path.Combine(ThumbnailsFolder, filethumb)))
                        {
                            File.Delete(Path.Combine(ThumbnailsFolder, filethumb));
                        }


                        if (File.Exists(Path.Combine(watermarkFolder, FileData)))
                        {
                            File.Delete(Path.Combine(watermarkFolder, FileData));
                        }

                        if (File.Exists(Path.Combine(compressFolder, FileData)))
                        {
                            File.Delete(Path.Combine(compressFolder, FileData));
                        }

                        if (File.Exists(Path.Combine(RelocationMoovFolder, FileData)))
                        {
                            File.Delete(Path.Combine(RelocationMoovFolder, FileData));
                        }

                        Console.WriteLine("Finish Delete File in Temp Folder");
                        Console.WriteLine("..........................................................................");


                        Console.WriteLine("Done Watermark Process for " + i.Id);
                        Console.WriteLine("..........................................................................");

                    }
                }
                else
                {
                    Console.Write(".");
                }

            }
            catch (Exception ex)
            {
                Program program = new Program();
                program.InputErrorLog("WatermarkService", ex.Message, "WatermarkProcess");
                throw ex;
            }



        }

        static void SetThumbnails(string PathSource, string PathDestination)
        {
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

            ffMpeg.GetVideoThumbnail(PathSource, PathDestination, 1.0f);
        }
           

        [NoAutomaticTriggerAttribute]
        public static async Task ProcessMethod()
        {
            while (true)
            {
                try
                {
                    Program Program = new Program();
                   Program.WatermarkProcess();
                   // Program.SetThumbnails();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occurred in processing pending  requests. Error : {0}", ex.Message); 
                   
                }
                await Task.Delay(TimeSpan.FromMinutes(10));
            }
        }


        static int GCD(int a, int b)
        {
            int Remainder;

            while (b != 0)
            {
                Remainder = a % b;
                a = b;
                b = Remainder;
            }

            return a;
        }


        public static void SetWatermark(string PathSource,string PathCompress,string PathWatermark, string PathDestination)
        {
            Console.WriteLine("starting set watermark " + PathSource);
            var ImgFile = Directory.GetCurrentDirectory() + "/videos/img/watermarkphotos.png";
           // var ImgFile = Directory.GetCurrentDirectory() + "/videos/img/NewImage.png";
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            NReco.VideoConverter.FFMpegConverter wrap = new FFMpegConverter();


            //NReco.VideoInfo.FFProbe ffProbe = new NReco.VideoInfo.FFProbe();
            //MediaInfo videoInfo = ffProbe.GetMediaInfo(PathSource);

            //int iWidth = videoInfo.Streams[0].Width;
            //int iHeight = videoInfo.Streams[0].Height;
            // int NewHeight = Convert.ToInt32(iHeight * 480 / iWidth);
           // int NewWidth = 480; //iWidth < 480? iWidth: 480; 
            // int NewHeight = 0;

            //'min(320,iw)'

            //wrap.Invoke("-i " + PathSource + " -vf scale=" + NewWidth + ":-2 " + PathCompress + " ");

          //  string command = "-i " + PathSource + " -vf \"scale='min(480,iw)':-2\" " + PathCompress + " ";
             wrap.Invoke("-i " + PathSource + " -vf \"scale='min(480,iw)':-2\" " + PathCompress + " ");
            wrap.Invoke("-i " + PathCompress + " -i " + ImgFile + " -filter_complex \"overlay=main_w-overlay_w-5:5\" " + PathWatermark + " ");
            wrap.Invoke("-i " + PathWatermark + " -codec copy -map 0 -movflags +faststart " + PathDestination + "");


            // var l =  string.Format("{0}:{1}", iWidth / GCD(iWidth, iHeight), iHeight / GCD(iWidth, iHeight));



            //int ratioWidth = iWidth / GCD(iWidth, iHeight);
            //int ratioHeight = iHeight / GCD(iWidth, iHeight);


            ////landscape
            //if (iWidth > iHeight)
            //{
            //    //NewWidth = iWidth / 2 < 640 ? iWidth : iWidth / 2;
            //    //NewHeight = iHeight / 2 < 480 ? iHeight : iHeight / 2;
            //    //NewWidth = iWidth > 720 ? 720 : iWidth;
            //    //NewHeight = ratioWidth / ratioHeight * 540;
            //    //wrap.Invoke("-i " + PathSource + " -vf scale=" + iWidth + ":" + NewHeight + " PathDestination + ");

            //    //decimal Rate = Convert.ToDecimal(ratioHeight) / Convert.ToDecimal(ratioWidth);
            //    //var Height = Rate * NewWidth;
            //    //NewHeight = Convert.ToInt32(Height);
            //    //wrap.Invoke("-i " + PathSource + " -vf scale=" + NewWidth + ":" + NewHeight + " " + PathDestination + " ");

            //    decimal Rate = Convert.ToDecimal(ratioWidth) / Convert.ToDecimal(ratioHeight);
            //    var Height = Rate * NewWidth;
            //    NewHeight = Convert.ToInt32(Height);
            //    //string command = "-i " + PathSource + " -vf scale=" + NewWidth + ":" + NewHeight + " " + PathDestination + " ";
            //    // wrap.Invoke("-i " + PathSource + " -vf scale=" + NewWidth + ":" + NewHeight + " " + PathCompress + " ");
            //    wrap.Invoke("-i " + PathSource + " -vf scale=" + NewWidth + ":-2 " + PathCompress + " ");
            //    wrap.Invoke("-i " + PathCompress + " -i " + ImgFile + " -filter_complex \"overlay=main_w-overlay_w-5:5\" " + PathWatermark + " ");
            //    wrap.Invoke("-i " + PathWatermark + " -codec copy -map 0 -movflags +faststart " + PathDestination + "");


            //    // wrap.Invoke("-i " + PathSource + "  -vf \"scale="+ NewWidth + ":-1\" " + PathDestination + " ");
            //    // wrap.Invoke("-i " + PathSource + " -vf scale=" + NewWidth + ":-1 " + ImgFile + " -filter_complex \"overlay=main_w-overlay_w-5:5\" " + PathDestination + " ");

            //}
            //else
            //{
            //    decimal Rate = Convert.ToDecimal(ratioHeight) / Convert.ToDecimal(ratioWidth);
            //   // decimal Rate = Convert.ToDecimal(16) / Convert.ToDecimal(9);
            //    var Height = Rate * NewWidth;
            //    NewHeight = Convert.ToInt32(Height);
            //    string command = "-i " + PathSource + " -vf scale=" + NewWidth + ":" + NewHeight + " " + PathDestination + " ";
            //    //wrap.Invoke("-i " + PathSource + " -vf scale="+ NewWidth + ":"+ NewHeight + " "+ PathCompress + " ");
            //    wrap.Invoke("-i " + PathSource + " -vf scale=" + NewWidth + ":-2 "   + PathCompress + " ");
            //    wrap.Invoke("-i " + PathCompress + " -i " + ImgFile + " -filter_complex \"overlay=main_w-overlay_w-5:5\" " + PathWatermark + " ");
            //    wrap.Invoke("-i " + PathWatermark + " -codec copy -map 0 -movflags +faststart " + PathDestination + "");

            //    //  wrap.Invoke("-i " + PathSource +" -filter_complex \"[0:v]scale = " + iWidth + ":-2 [bg];[bg] [1:v] overlay=main_w-overlay_w-5:5\"   " + PathDestination + " ");
            //    // wrap.Invoke("-i " + PathSource + " -i " + ImgFile + " -filter_complex \"[0:v]scale = " + iWidth + ":-2 [bg];[bg] [1:v] overlay=main_w-overlay_w-5:5\"   " + PathDestination + " ");



            //}


           // wrap.Invoke("-i " + PathSource + "-c:v libx264 -b:v 1.5M -c:a aac -b:a 128k " + PathDestination + "");

            //wrap.Invoke("-i " + PathSource + " -i " + ImgFile + "  -filter_complex \"overlay=main_w-overlay_w-5:5\"   -vcodec libx265 -crf 28  " + PathDestination + " ");
          //    wrap.Invoke("-i " + PathSource + " -vf scale=" + iWidth + ":-2 "  + ImgFile + " -filter_complex \"overlay=main_w-overlay_w-5:5\" " + PathDestination + " ");
            //wrap.Invoke("-i " + PathSource + " -vf scale=" + iWidth + ":-2 -i " + ImgFile + " -filter_complex \"overlay=main_w-overlay_w-5:5\" " + PathDestination + " ");
            //wrap.Invoke("-i " + PathSource + " -s 640x480 -i " + ImgFile + " -filter_complex \"overlay=main_w-overlay_w-5:5\" " + PathDestination + " ");
            //wrap.Invoke("-i " + PathSource + " -i " + ImgFile + "  -filter_complex \"overlay=main_w-overlay_w-5:5\"   -vcodec libx265 -crf 28  " + PathDestination + " ");
            //wrap.Invoke("-i " + PathSource + " -codec:v libx264 -preset fast -movflags +faststart " + PathDestination + " ");
          //    wrap.Invoke("-i " + PathSource + " -i " + ImgFile+ " -filter_complex \"[0:v]scale = "+ iWidth + ":-2 [bg];[bg] [1:v] overlay=main_w-overlay_w-5:5\"   " + PathDestination + " ");
           // wrap.Invoke(" -i " + PathSource + " -vcodec libx265 -crf 28 " + PathDestination + " ");

            Console.WriteLine("Finish set watermark " + PathSource);
        }

        public static string UploadToAzure(string FileSource, string filename,string reference)
        {
            Console.WriteLine("starting Upload to Azure  " + FileSource);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConn());

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(reference);

            var newBlob = container.GetBlockBlobReference(filename);
            newBlob.UploadFromFileAsync(FileSource).Wait();
            var blobUrl = newBlob.Uri.AbsoluteUri;

            Console.WriteLine("Finish Upload to Azure " + blobUrl);

            return blobUrl;
        }

        public static string UploadToGoogleCloud(string FileSource, string filename, string reference)
        {
            Console.WriteLine("starting Upload to Google Cloud  " + FileSource);


            string BucketName = reference;
            string ProjectName = "versatile-age-284708";
            string blobUrl = "";

            var pathcredential = Path.Combine(Directory.GetCurrentDirectory(), "clientsecret.json");
            var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromJson(System.IO.File.ReadAllText(pathcredential));


            using (var storageClient = Google.Cloud.Storage.V1.StorageClient.Create(credential))
            {
                using (var fileStream = new System.IO.FileStream(Path.Combine(FileSource), System.IO.FileMode.Open))
                {

                    string objectName = Path.GetFileName(filename.ToString());
                    storageClient.UploadObject(BucketName, objectName, null, fileStream);
                    blobUrl = "https://storage.googleapis.com/" + BucketName + "/" + objectName;

                }
            }

            Console.WriteLine("Finish Upload to Google Cloud " + blobUrl);
            return blobUrl;


        }


        public static void SaveMemoryStream(MemoryStream ms, string FileName)
        {
            FileStream outStream = File.OpenWrite(FileName);
            ms.WriteTo(outStream);
            outStream.Flush();
            outStream.Close();

        }

        public static void DownloadFromGoogleCloud(string FileName,string reference)
        {


            var pathcredential = Path.Combine(Directory.GetCurrentDirectory(), "clientsecret.json");
            var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromJson(System.IO.File.ReadAllText(pathcredential));
           

            using (var storageClient = Google.Cloud.Storage.V1.StorageClient.Create(credential))
            {
                MemoryStream memStream = new MemoryStream();
                string objectName = Path.GetFileName(FileName);
                // storageClient.UploadObject(bucketName, objectName, null, fileStream);
                storageClient.DownloadObjectAsync(reference, objectName, memStream);

                FileStream outStream = File.OpenWrite(FileName);
                memStream.WriteTo(outStream);
                outStream.Flush();
                outStream.Close();

                using (var outputFile = File.OpenWrite(FileName))
                {
                    storageClient.DownloadObject(reference, objectName, outputFile);

                }
            }

        }

        public IList<FilesModel> GetFiles()
        {
            Console.WriteLine("Get files processing ... ");
            var ListFiles = new List<FilesModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                ListFiles = conn.Query<FilesModel>("sp_GetFilesIsNotWatermark", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }
            Console.WriteLine("Get files done ... ");
            return ListFiles;
        }

        public void UpdateThumbnailsLink(FilesModel model)
        {
            Console.WriteLine("Update thumbnails  For " + model.Id);
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                conn.Execute("update Tbl_Trx_Files set Thumbnails = @thumbnails where id = @Id ",
                    new
                    {
                        @thumbnails = model.thumbnails,                        
                        @Id = model.Id
                    }
                    );



            }
            Console.WriteLine("Update thumbnails For " + model.Id + " Done");


        }


        public void UpdateWaterMarkLink(FilesModel model)
        {
            Console.WriteLine("Update Iswatermark  For " + model.Id);
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                conn.Execute("update Tbl_Trx_Files set IsWatermark = @IsWatermark , link = @Link , fileName = @FileName, FileType = @FileType where id = @Id ",
                    new
                    {
                        @IsWatermark = 1,
                        @link = model.Link,
                        @FileName = model.FileName,
                        FileType = model.FileType,
                        @Id = model.Id
                    }
                    );



            }
            Console.WriteLine("Update Watermark For " + model.Id + " Done");


        }


        public void InputErrorLog(string ErrorSource, string message, string functionName)
        {
            var data = new ErrorLogModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@ErrorSource", ErrorSource);
                param.Add("@ErrorMessage", message);
                param.Add("@FunctionName", functionName);


                data = conn.Query<ErrorLogModel>("sp_Tbl_Error_LogInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
        }

    }

    public class ErrorLogModel
    {
        public int Id { get; set; }

        public string ErrorSource { get; set; }

        public string ErrorMessage { get; set; }
        public string FunctionName { get; set; }

        public DateTime? CreatedDate { get; set; }
    }







    public class FilesModel
    {
        public int Id { get; set; }

        public string Link { get; set; }

        public string FileName { get; set; }
        public string thumbnails { get; set; }

        public string FileDesc { get; set; }

        public string FileType { get; set; }

        public int? FileCategory { get; set; }

        public int? ViewCount { get; set; }

        public TimeSpan? FileDuration { get; set; }

        public int? OwnerId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsWatermark { get; set; }

        public bool? IsActive { get; set; }
    }
}
