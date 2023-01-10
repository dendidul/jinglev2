using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Jingl.General.Enum;
using Jingl.General.Model.Admin.Transaction;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.WebApi.Authentication;
using Jingl.WebApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NReco.VideoConverter;
using Xabe.FFmpeg;

namespace Jingl.WebApi.Controllers
{

    [WebAuthetication("apifameo.fameoapp.com")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FilesController : Controller
    {
        private readonly ITransactionManager ITransactionManager;
        private readonly HelperController HelperController;

        public FilesController(IConfiguration config)
        {
            this.ITransactionManager = new TransactionManager(config);           
            this.HelperController = new HelperController(config);
        }

        [HttpPost]
        public async Task<FilesModel> UploadPhotosFile(IFormFile file)
        {
            try
            {

                if (file == null || file.Length == 0)
                    return null;

                var DestFolder = Directory.GetCurrentDirectory() + "/wwwroot/upload/photos/";
                var path = Path.Combine(
                           DestFolder,
                            file.FileName + ".jpg");

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }


                CloudStorageAccount storageAccount = new CloudStorageAccount(
                            new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                                HelperController.AzureAccountName(),
                               HelperController.AzureAccountKey()), true);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference("photos");


                var filename = Guid.NewGuid();

                var newBlob = container.GetBlockBlobReference(filename.ToString() + ".jpg");
                await newBlob.UploadFromFileAsync(Path.Combine(DestFolder, file.FileName.ToString() + ".jpg"));
                var blobUrl = newBlob.Uri.AbsoluteUri;




                var filesModel = new FilesModel();
                filesModel.CreatedBy = "0";
                //filesModel.Link = "/wwwroot/upload/videos/mp4/" + filename.ToString() + ".mp4";
                filesModel.Link = blobUrl;
                filesModel.FileCategory = (int)FileCategory.Photo;
                filesModel.FileName = filename.ToString();
                filesModel.FileType = ".jpg";

                var currentdata = ITransactionManager.CreateFiles(filesModel);


                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                return currentdata;
            }
            catch (Exception ex)
            {

                HelperController.InsertLog(Convert.ToInt32(0), "UploadPhotosFile", ex.Message);
                throw ex;
            }

            //return Json(Request.Form[0]);
        }


        [HttpPost]
        public async Task<FilesModel> UploadVideoFilesData1(IFormFile file, int id)
        {
            try
            {
                var filePath = Path.GetTempFileName();

                if (file.Length > 0)
                {
                    using (var inputStream = new FileStream(filePath, FileMode.Create))
                    {
                        // read file to stream
                        await file.CopyToAsync(inputStream);
                        // stream to byte array
                        byte[] array = new byte[inputStream.Length];
                        inputStream.Seek(0, SeekOrigin.Begin);
                        inputStream.Read(array, 0, array.Length);
                        // get file name
                        //string fName = formFile.FileName;
                        //var file = Request.Form.Files;

                        var filename = Guid.NewGuid();
                        var DestFolder = Directory.GetCurrentDirectory() + "/wwwroot/upload/videos/";

                        var ImgFile = Directory.GetCurrentDirectory() + "/wwwroot/template/img/Jingl_bgr.png";


                        var pathurl = "";
                        if (file.ContentType != "video/mp4")
                        {
                            pathurl = Path.Combine(
                        DestFolder + "/temp/",
                     filename + ".webm");
                        }
                        else
                        {
                            pathurl = Path.Combine(
                        DestFolder + "/temp/",
                     filename + ".mp4");
                        }


                        using (var stream = new FileStream(pathurl, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        var from = Path.Combine(DestFolder, filename.ToString());
                        var to = Path.Combine(DestFolder + "/mp4/", filename.ToString());

                        FFmpeg.ExecutablesPath = Path.Combine(DestFolder + "/mp4/");
                        //await FFmpeg.GetLatestVersion();

                        if (file.ContentType != "video/mp4")
                        {
                            await Conversion.Convert(Path.Combine(DestFolder + "/temp/", filename.ToString() + ".webm"), Path.Combine(DestFolder + "/mp4/", filename.ToString() + ".mp4")).Start();

                        }
                        else
                        {
                            System.IO.File.Copy(Path.Combine(DestFolder + "/temp/", filename.ToString() + ".mp4"), Path.Combine(DestFolder + "/mp4/", filename.ToString() + ".mp4"), true);
                        }



                        var ffMpeg = new NReco.VideoConverter.FFMpegConverter();


                        CloudStorageAccount storageAccount = new CloudStorageAccount(
                         new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                             HelperController.AzureAccountName(),
                            HelperController.AzureAccountKey()), true);

                        // Create the blob client.
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                        // Retrieve reference to a previously created container.
                        CloudBlobContainer container = blobClient.GetContainerReference("videos");

                        var newBlob = container.GetBlockBlobReference(filename.ToString() + ".mp4");
                        await newBlob.UploadFromFileAsync(Path.Combine(DestFolder + "/mp4/", filename.ToString() + ".mp4"));
                        var blobUrl = newBlob.Uri.AbsoluteUri;




                        var filesModel = new FilesModel();
                        filesModel.CreatedBy = id.ToString();
                        //filesModel.Link = "/wwwroot/upload/videos/mp4/" + filename.ToString() + ".mp4";
                        filesModel.Link = blobUrl;
                        filesModel.FileCategory = (int)FileCategory.Video;
                        filesModel.FileName = filename.ToString();
                        filesModel.FileType = ".mp4";

                        var currentdata = ITransactionManager.CreateFiles(filesModel);

                        if (file.ContentType != "video/mp4")
                        {
                            if (System.IO.File.Exists(Path.Combine(DestFolder + "/temp/", filename.ToString() + ".webm")))
                            {
                                System.IO.File.Delete(Path.Combine(DestFolder + "/temp/", filename.ToString() + ".webm"));
                            }
                        }
                        else
                        {
                            if (System.IO.File.Exists(Path.Combine(DestFolder + "/temp/", filename.ToString() + ".mp4")))
                            {
                                System.IO.File.Delete(Path.Combine(DestFolder + "/temp/", filename.ToString() + ".mp4"));
                            }
                        }



                        if (System.IO.File.Exists(Path.Combine(DestFolder + "/mp4/", filename.ToString() + ".mp4")))
                        {
                            System.IO.File.Delete(Path.Combine(DestFolder + "/mp4/", filename.ToString() + ".mp4"));
                        }






                        return currentdata;

                    }
                }

                return null;

            }
            catch (Exception ex)
            {
                HelperController.InsertLog(Convert.ToInt32(id), "UploadVideoFilesData", ex.Message);
                return null;
                throw;

            }

        }

      
    }
}