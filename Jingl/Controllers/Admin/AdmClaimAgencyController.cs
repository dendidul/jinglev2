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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers.Admin
{
    public class AdmClaimAgencyController : AdmMenuController
    {

        private readonly ITransactionManager ITransactionManager;
        private readonly HelperController HelperController;
        private readonly IMasterManager IMasterManager;

        public AdmClaimAgencyController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.ITransactionManager = new TransactionManager(config);
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
            // this.IUserManagementManager = new UserManagementManager(config);
        }

        public IActionResult Index()
        {
            IList<ClaimAgencyModel> model = new List<ClaimAgencyModel>();
            model = ITransactionManager.GetAllClaimAgency();
            return View(model);
        }

        public IActionResult Create()
        {
            //ViewBag.ListBank = new SelectList(HelperController.MainBankAccount, "value", "text");
            ViewBag.ListAgency = new SelectList(HelperController.AgencyList, "value", "text");
            ViewBag.Period = new SelectList(HelperController.PeriodData, "value", "text");
            ClaimAgencyModel model = new ClaimAgencyModel();
            var getPeriod = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "Period").FirstOrDefault();
            model.Period = getPeriod != null ? getPeriod.ParamValue : "";
            return View(model);
        }

        [HttpPost]
        public IActionResult GetBookByAgencyId(string Period, string AgencyId)
        {
            BookModel bm = new BookModel();
            bm.Period = Period;
            bm.AgencyId = Convert.ToInt32(AgencyId);
            var getdata = ITransactionManager.GetBookByAgencyId(bm);
            ClaimAgencyModel model = new ClaimAgencyModel();
            model.BookingList = getdata.ToList();

            return PartialView("~/Views/AdmClaimAgency/ClaimDetails.cshtml", model);
        }

        [HttpPost]
        public IActionResult Create(ClaimAgencyModel model)
        {
            model.CreatedBy = Convert.ToInt32(HelperController.GetCookie("UserId"));
            model.ClaimDate = DateTime.Now;
            var checkdata = ITransactionManager.CreateClaimAgency(model);

            if (checkdata.Status == (int)Registration.Submit)
            {
                var getagencydata = IMasterManager.GetAllAgency().Where(x => x.Id == checkdata.AgencyId).FirstOrDefault();
                ClaimModel CM = new ClaimModel();
                CM.UserId = getagencydata.UserId;
                CM.AccountNumber = getagencydata.AccountNo;
                CM.Amount = model.Amount;
                CM.BankName = getagencydata.BankNm;
                CM.Period = model.Period;
                CM.Message = "Penarikan dari Agency " + getagencydata.AgencyNm;
                CM.Status = (int)Registration.Submit;
                CM.CreatedBy = HelperController.GetCookie("UserId");

                ITransactionManager.CreateClaim(CM);



                var getClaim = ITransactionManager.GetAllClaim().Where(x => x.UserId == getagencydata.UserId).OrderByDescending(x => x.Id).FirstOrDefault();
                var getClaimAgency = ITransactionManager.GetClaimAgency(checkdata.Id);
                getClaimAgency.ClaimId = getClaim.Id;
                ITransactionManager.UpdateClaimAgency(getClaimAgency);

            }





            return Json("OK");
        }


        public IActionResult Edit(int id)
        {
            var getClaimAgency = ITransactionManager.GetClaimAgency(id);
            ViewBag.ListAgency = new SelectList(HelperController.AgencyList, "value", "text");
            ViewBag.Period = new SelectList(HelperController.PeriodData, "value", "text");
            return View(getClaimAgency);

        }

        [HttpPost]
        public IActionResult Edit(ClaimAgencyModel model)
        {
            var getClaimAgencyData = ITransactionManager.GetAllClaimAgency().Where(x => x.Id == model.Id).FirstOrDefault();

            if (getClaimAgencyData != null)
            {
                model.UpdatedBy = Convert.ToInt32(HelperController.GetCookie("UserId"));
                model.ClaimDate = DateTime.Now;
                var checkdata = ITransactionManager.UpdateClaimAgency(model);

                if (checkdata.Status == (int)Registration.Submit)
                {
                    var getagencydata = IMasterManager.GetAllAgency().Where(x => x.Id == checkdata.AgencyId).FirstOrDefault();

                    if (getClaimAgencyData.ClaimId == null)
                    {
                        if(getagencydata != null)
                        {
                            ClaimModel CM = new ClaimModel();
                            CM.UserId = getagencydata.UserId;
                            CM.AccountNumber = getagencydata.AccountNo;
                            CM.Amount = model.Amount;
                            CM.BankName = getagencydata.BankNm;
                            CM.Period = model.Period;
                            CM.Message = "Penarikan dari Agency " + getagencydata.AgencyNm;
                            CM.Status = (int)Registration.Submit;
                            CM.CreatedBy = HelperController.GetCookie("UserId");
                            ITransactionManager.CreateClaim(CM);



                            var getClaim = ITransactionManager.GetAllClaim().Where(x => x.UserId == getagencydata.UserId).OrderByDescending(x => x.Id).FirstOrDefault();
                            var getClaimAgency = ITransactionManager.GetClaimAgency(checkdata.Id);
                            getClaimAgency.ClaimId = getClaim.Id;
                            ITransactionManager.UpdateClaimAgency(getClaimAgency);
                        }

                        
                    }

                    //if (getClaimAgencyData.ClaimId != null)
                    //{

                       
                    //}

                    //else if ()
                    //{
                    //    var getClaim = ITransactionManager.GetAllClaim().Where(x => x.UserId == getagencydata.UserId).OrderByDescending(x => x.Id).FirstOrDefault();
                    //    var getClaimAgency = ITransactionManager.GetClaimAgency(checkdata.Id);
                    //    getClaimAgency.ClaimId = getClaim.Id;
                    //    ITransactionManager.UpdateClaimAgency(getClaimAgency);
                    //}

                    else
                    {


                        var getClaimData = ITransactionManager.GetClaim(model.ClaimId.Value);
                        getClaimData.Status = model.Status;
                        ITransactionManager.UpdateClaim(getClaimData);

                    }


                }
            }






            return Json("OK");
        }

        public IActionResult Details(int id)
        {
            var getClaimAgency = ITransactionManager.GetClaimAgency(id);
            ViewBag.ListAgency = new SelectList(HelperController.AgencyList, "value", "text");
            ViewBag.Period = new SelectList(HelperController.PeriodData, "value", "text");
            return View(getClaimAgency);

        }

        public IActionResult Delete(int id)
        {
            var getClaimAgency = ITransactionManager.GetClaimAgency(id);
            ViewBag.ListAgency = new SelectList(HelperController.AgencyList, "value", "text");
            ViewBag.Period = new SelectList(HelperController.PeriodData, "value", "text");
            return View(getClaimAgency);

        }

        [HttpPost]
        public IActionResult Delete(ClaimAgencyModel model)
        {

            model.UpdatedBy = Convert.ToInt32(HelperController.GetCookie("UserId"));
            model.ClaimDate = DateTime.Now;
            var checkdata = ITransactionManager.UpdateClaimAgency(model);
            return Json("OK");
        }
    }
}