using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers.Admin
{
    public class AdmBannerController : AdmMenuController
    {
        private readonly FilesController FilesController;
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly HelperController HelperController;


        public AdmBannerController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.FilesController = new FilesController(config, cookie);
            this.ITransactionManager = new TransactionManager(config);
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.IUserManagementManager = new UserManagementManager(config);
        }

        public IActionResult Index()
        {
            var AdmBanner = IMasterManager.GetAllBanner();
            return View(AdmBanner);
        }

        public IActionResult Create()
        {
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", 1);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerModel model, IFormFile BgrImg)
        {
            try
            {
                int FileId = 0;

                if (BgrImg != null)
                {
                    var BgrImgFileIds = await FilesController.UploadPhotosFile(BgrImg);
                    FileId = BgrImgFileIds.Id;
                }

                BannerModel getbannerdata = new BannerModel();
                
                if (FileId != 0)
                {
                    getbannerdata.FileId = FileId;
                }

                getbannerdata.BannerCategory = model.BannerCategory;
                getbannerdata.BannerNm = model.BannerNm;
                getbannerdata.Link = model.Link;
                getbannerdata.Sequence = model.Sequence;
                getbannerdata.BannerDesc = model.BannerDesc;
                getbannerdata.IsActive = model.IsActive;
                getbannerdata.CreatedBy = Convert.ToInt32(HelperController.GetCookie("UserId"));
                getbannerdata.IsVisible = model.IsVisible;
               // getbannerdata.FileId = FileId;
                //gettalentdata.NPWPFileId = NpwpFileId;
                //gettalentdata.AccountNumberFileId = AcNumFileId;
                //gettalentdata.IdCardFileId = IdCardFileId;
                IMasterManager.CreateBanner(getbannerdata);

          

                // TODO: Add update logic here

                return Json("OK");
            }
            catch (Exception ex)
            {
                return View();

            }
        }

        public IActionResult Edit(int id)
        {
            BannerModel model = new BannerModel();
            model.Id = id;
            var data = IMasterManager.GetBanner(model);
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", 1);
            return View(data);
        }

        [HttpPost]
      
        public async Task<IActionResult> Edit(BannerModel model, IFormFile BgrImg)
        {
            try
            {
                int FileId = 0;

                if (BgrImg != null)
                {
                    var BgrImgFileIds = await FilesController.UploadPhotosFile(BgrImg);
                    FileId = BgrImgFileIds.Id;
                }
                 
                var getbannerdata = IMasterManager.GetBanner(model);
      

                if (FileId != 0)
                {
                    getbannerdata.FileId = FileId;
                }

                getbannerdata.BannerCategory = model.BannerCategory;
                getbannerdata.BannerNm = model.BannerNm;
                getbannerdata.Link = model.Link;
                getbannerdata.Sequence = model.Sequence;
                getbannerdata.BannerDesc = model.BannerDesc;
                getbannerdata.IsActive = model.IsActive;
                getbannerdata.CreatedBy = Convert.ToInt32(HelperController.GetCookie("UserId"));
                getbannerdata.IsVisible = model.IsVisible;
                // getbannerdata.FileId = FileId;
                //gettalentdata.NPWPFileId = NpwpFileId;
                //gettalentdata.AccountNumberFileId = AcNumFileId;
                //gettalentdata.IdCardFileId = IdCardFileId;
                IMasterManager.UpdateBanner(getbannerdata);



                // TODO: Add update logic here

                return Json("OK");
            }
            catch (Exception ex)
            {
                return View();

            }
        }

        public IActionResult Details(int id)
        {
            BannerModel model = new BannerModel();
            model.Id = id;
            var data = IMasterManager.GetBanner(model);
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", 1);
            return View(data);
        }

        public IActionResult Delete(int id)
        {

            BannerModel model = new BannerModel();
            model.Id = id;
            model = IMasterManager.GetBanner(model);
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", 1);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(BannerModel model)
        {


            IMasterManager.DeleteBanner(model.Id);


            return RedirectToAction("Index");
        }
    }
}