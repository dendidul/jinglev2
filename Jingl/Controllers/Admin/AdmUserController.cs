using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Utility;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers.Admin
{
    public class AdmUserController : AdmMenuController
    {

        private readonly IMasterManager IMasterManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly HelperController HelperController;


        public AdmUserController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.IUserManagementManager = new UserManagementManager(config);
        }

        public IActionResult Index()
        {
            //var AdmUser = IUserManagementManager.AdmGetAllUser();
            //return View(AdmUser);
            return View();
        }

        public IActionResult GetListUser()
        {
            var AdmUser = IUserManagementManager.AdmGetAllUser();
            return Json(AdmUser.ToList());
        }


        public IActionResult Create()
        {
            ViewBag.ListRegion = new SelectList(IMasterManager.GetAllRegion(), "Region", "Region", 0);
            ViewBag.ListGender = new SelectList(HelperController.GenderList, "value", "text", 0);
            ViewBag.ListCountry = new SelectList(HelperController.CountryList, "value", "text", 0);
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", 1);
            ViewBag.ListRole = new SelectList(IUserManagementManager.GetAllRole(), "Id", "RoleNm", 0);
            ViewBag.ListBank = new SelectList(HelperController.MainBankAccount, "value", "text");
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserModel model)
        {
            model.Password = Encryptor.Encrypt(model.Password);
            model.CreatedBy = HelperController.GetCookie("UserId");
            var data = IUserManagementManager.CreateUser(model);

            if(model.RoleId == 3)
            {
                TalentModel tModel = new TalentModel();
                tModel.Status = (int)Registration.Completed;
                tModel.UserId = data.Id;
                tModel.CreatedBy = HelperController.GetCookie("UserId").ToString();
                IMasterManager.CreateTalent(tModel);

                var getData = IUserManagementManager.GetUser(data);
                getData.IsVerified = "1";
                IUserManagementManager.UpdateUser(getData);
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RegiterUserAsTalent(UserModel model)
        {
            TalentModel talentModel = new TalentModel();
            talentModel.TalentNm = model.FirstName + " " + model.LastName;
            talentModel.Gender = model.Gender;
            talentModel.Status = (int)Registration.Submit;
            talentModel.BirthDate = model.BirthDate;
            talentModel.UserId = model.Id;
            talentModel.IsActive = (int)Status.Active;
            talentModel.RdyVideo = 1;
            IMasterManager.CreateTalent(talentModel);

            return Json("OK");
        }

        public IActionResult Edit(int id)
        {
            UserModel model = new UserModel();
            model.Id = id;
            int status = 0;
            model = IUserManagementManager.GetUser(model);

            model.Password = Encryptor.Decrypt(model.Password);


            ViewBag.ListBank = new SelectList(HelperController.MainBankAccount, "value", "text");
            ViewBag.ListRegion = new SelectList(IMasterManager.GetAllRegion(), "Region", "Region", model.Region);
            ViewBag.ListGender = new SelectList(HelperController.GenderList, "value", "text", model.Gender);
            ViewBag.ListCountry = new SelectList(HelperController.CountryList, "value", "text", model.Country);
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", model.IsActive);
            ViewBag.ListRole = new SelectList(IUserManagementManager.GetAllRole(), "Id", "RoleNm", model.RoleId);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(UserModel model)
        {
            
            var getcurrentdata = IUserManagementManager.GetUser(model);

            getcurrentdata.FirstName = model.FirstName;
            getcurrentdata.LastName = model.LastName;
            getcurrentdata.Bio = model.Bio;
            getcurrentdata.BirthDate = model.BirthDate;
            getcurrentdata.Country = model.Country;
            getcurrentdata.Region = model.Region;
            getcurrentdata.UpdatedBy = HelperController.GetCookie("UserId").ToString();
            getcurrentdata.UpdatedDate = DateTime.Now;
            getcurrentdata.UserName = model.UserName;
            getcurrentdata.Password = Encryptor.Encrypt(model.Password);
            getcurrentdata.PhoneNumber = model.PhoneNumber;
            getcurrentdata.Email = model.Email;
            getcurrentdata.Gender = model.Gender;
            getcurrentdata.IsActive = model.IsActive;
            getcurrentdata.RoleId = model.RoleId;
            getcurrentdata.Bank = model.Bank;
            getcurrentdata.BeneficiaryName = model.BeneficiaryName;
            getcurrentdata.AccountNumber = model.AccountNumber;
            IUserManagementManager.UpdateUser(getcurrentdata);

            TalentModel tModel = new TalentModel();
            tModel = IMasterManager.GetAllTalent().Where(x => x.UserId == model.Id).FirstOrDefault();


            if (model.RoleId == 3)
            {
                
                if (tModel == null)
                {
                    TalentModel NewtModel = new TalentModel();
                    NewtModel.Status = (int)Registration.Completed;
                    NewtModel.UserId = model.Id;
                    NewtModel.CreatedBy = HelperController.GetCookie("UserId").ToString();
                    IMasterManager.CreateTalent(NewtModel);
                }
                else
                {
                    tModel.Status = (int)Registration.Completed;
                    tModel.UserId = model.Id;
                    tModel.UpdatedBy = HelperController.GetCookie("UserId").ToString();
                    IMasterManager.UpdateTalent(tModel);
                }
            }
            else
            {
                if(tModel != null)
                {
                    tModel.Status = (int)Registration.Submit;
                    tModel.UserId = model.Id;
                    tModel.UpdatedBy = HelperController.GetCookie("UserId").ToString();
                    IMasterManager.UpdateTalent(tModel);
                }
            }


            return RedirectToAction("Index");
        }


        public IActionResult Details(int id)
        {

            UserModel model = new UserModel();
            model.Id = id;
            model = IUserManagementManager.GetUser(model);
            model.Password = Encryptor.Decrypt(model.Password);


            ViewBag.ListRegion = new SelectList(IMasterManager.GetAllRegion(), "Region", "Region", model.Region);
            ViewBag.ListGender = new SelectList(HelperController.GenderList, "value", "text", model.Gender);
            ViewBag.ListCountry = new SelectList(HelperController.CountryList, "value", "text", model.Country);
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", model.IsActive);
            ViewBag.ListRole = new SelectList(IUserManagementManager.GetAllRole(), "Id", "RoleNm", model.RoleId);
            ViewBag.ListBank = new SelectList(HelperController.MainBankAccount, "value", "text");
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            UserModel model = new UserModel();
            model.Id = id;
            model = IUserManagementManager.GetUser(model);
            ViewBag.ListRegion = new SelectList(IMasterManager.GetAllRegion(), "Region", "Region", model.Region);
            ViewBag.ListGender = new SelectList(HelperController.GenderList, "value", "text", model.Gender);
            ViewBag.ListCountry = new SelectList(HelperController.CountryList, "value", "text", model.Country);
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", model.IsActive);
            ViewBag.ListRole = new SelectList(IUserManagementManager.GetAllRole(), "Id", "RoleNm", model.RoleId);
            ViewBag.ListBank = new SelectList(HelperController.MainBankAccount, "value", "text");

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(UserModel model)
        {


            IUserManagementManager.DeleteUser(model.Id);


            return RedirectToAction("Index");
        }

    }


}