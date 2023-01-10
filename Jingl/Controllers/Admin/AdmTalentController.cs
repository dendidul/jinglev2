using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.User.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers.Admin
{
    public class AdmTalentController : AdmMenuController
    {
        private readonly FilesController FilesController;
        private readonly ITransactionManager ITransactionManager;
        private readonly IMasterManager IMasterManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly HelperController HelperController;

        public AdmTalentController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.FilesController = new FilesController(config, cookie);
            this.ITransactionManager = new TransactionManager(config);
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.IUserManagementManager = new UserManagementManager(config);
        }

        public IActionResult ListApprovalTalent()
        {
            var data = ITransactionManager.GetAllTalentRegistration().Where(x => x.Status == 1).ToList();
            return View(data);
        }

        public IActionResult GetListTalent()
        {
            var data = IMasterManager.AdmGetAllTalent();
            return Json(data.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.ListGender = new SelectList(HelperController.GenderList, "value", "text", 0);
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", 1);
            ViewBag.ListBank = new SelectList(HelperController.MainBankAccount, "value", "text");
            ViewBag.OtherBank = new SelectList(HelperController.OthersBankAccount, "value", "text");
            return View();
        }

        public IActionResult Edit(int id)
        {
            TalentModel model = new TalentModel();
            model.Id = id;
            var data = IMasterManager.GetTalent(model);
            ViewBag.ListGender = new SelectList(HelperController.GenderList, "value", "text", 0);
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", 1);
            ViewBag.ListAgency = new SelectList(HelperController.AgencyList, "value", "text", model.AgencyId);
            data.ListTalentVideo = ITransactionManager.GetTalentVideos(id);
            ViewBag.ListBank = new SelectList(HelperController.MainBankAccount, "value", "text");
            ViewBag.OtherBank = new SelectList(HelperController.OthersBankAccount, "value", "text");

            var categorymodel = IMasterManager.GetCategoryByType("Talent");


            IList<TalentCategoryViewModel> ListCategory = new List<TalentCategoryViewModel>();
            foreach (var i in categorymodel)
            {
                TalentCategoryViewModel tcv = new TalentCategoryViewModel();
                tcv.CategoryId = i.Id;
                tcv.CategoryNm = i.CategoryNm;
                ListCategory.Add(tcv);
            }

            data.TalentCategory = ListCategory;

            if (data != null)
            {
                data.TalentSelectedCategory = IMasterManager.GetTalentCategoryData(data.Id);

            }

            return View(data);
        }

        public IActionResult Details(int id)
        {
            TalentModel model = new TalentModel();
            model.Id = id;
            var data = IMasterManager.GetTalent(model);
            ViewBag.ListGender = new SelectList(HelperController.GenderList, "value", "text", 0);
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", 1);
            ViewBag.ListAgency = new SelectList(HelperController.AgencyList, "value", "text", model.AgencyId);
            data.ListTalentVideo = ITransactionManager.GetTalentVideos(id);
            ViewBag.ListBank = new SelectList(HelperController.MainBankAccount, "value", "text");
            ViewBag.OtherBank = new SelectList(HelperController.OthersBankAccount, "value", "text");

            IList<TalentCategoryViewModel> ListCategory = new List<TalentCategoryViewModel>();
            var categorymodel = IMasterManager.GetCategoryByType("Talent");


            foreach (var i in categorymodel)
            {
                TalentCategoryViewModel tcv = new TalentCategoryViewModel();
                tcv.CategoryId = i.Id;
                tcv.CategoryNm = i.CategoryNm;
                ListCategory.Add(tcv);
            }

            data.TalentCategory = ListCategory;

            if (data != null)
            {
                data.TalentSelectedCategory = IMasterManager.GetTalentCategoryData(data.Id);

            }
            return View(data);
        }

        public IActionResult ListTalent()
        {
            return View();
            //var data = IMasterManager.GetAllTalent().Where(x => x.Status == Convert.ToInt32(Registration.Completed) && x.RoleId == 3);
            //return View(data);
        }

        public IActionResult Approval(int id, string ActionApproval, string Note)
        {
            try
            {
                UserModel User = new UserModel();
                TalentModel Talent = new TalentModel();
                TalentRegModel TalentRegModel = new TalentRegModel();

                var categoryModel = new TalentCategoryViewModel();
                var temp = new TalentCategoryViewModel();

                TalentRegModel = ITransactionManager.GetTalentRegistration(id);
                TalentRegModel.UpdatedBy = HelperController.GetCookie("UserId");

                User.Id = TalentRegModel.UserId.Value;
                User = IUserManagementManager.GetUser(User);

                if (ActionApproval == "-1")
                {
                    User.RoleId = (int)Role.User;
                    TalentRegModel.Status = (int)Registration.Rejected;
                    TalentRegModel.Note = Note;
                }
                else if (ActionApproval == "3")
                {
                    User.RoleId = (int)Role.Talent;
                    TalentRegModel.Status = (int)Registration.Completed;
                    Talent.Status = (int)Registration.Completed;
                    Talent.Level = 1;
                    Talent.Note = Note;
                    Talent.UserId = TalentRegModel.UserId;
                    Talent.Facebook = TalentRegModel.Facebook;
                    Talent.Instagram = TalentRegModel.Instagram;
                    Talent.Profesion = TalentRegModel.Profesion;
                    Talent = IMasterManager.CreateTalent(Talent);

                    categoryModel = new TalentCategoryViewModel();
                    categoryModel.TalentId = Talent.Id;
                    categoryModel.CategoryId = (int)TalentCat.New;
                    temp = IMasterManager.CreateTalentCategory(categoryModel);

                    if (TalentRegModel.CategoryId != null)
                    {

                        if (TalentRegModel.CategoryId != (int)TalentCat.New)
                        {
                            categoryModel = new TalentCategoryViewModel();
                            categoryModel.TalentId = Talent.Id;
                            categoryModel.CategoryId = Convert.ToInt32(TalentRegModel.CategoryId);
                            temp = IMasterManager.CreateTalentCategory(categoryModel);
                        }
                    }

                }


                User = IUserManagementManager.UpdateUser(User);
                ITransactionManager.UpdateTalentRegistration(TalentRegModel);
                HelperController.EmailApprovalTalent(User.Email, ActionApproval, TalentRegModel.TalentNm, TalentRegModel.RegNum, Note).Wait();

                //return RedirectToAction("ListApprovalTalent");
                return Json("OK");
            }
            catch (Exception ex)
            {

                return Json("False");
                throw ex;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TalentModel model, IFormFile ProfImg, IFormFile BgrImg, IFormFile IdCardImg, IFormFile NpwpImg, IFormFile AcNumImg, List<IFormFile> ListVideo, List<int> ListCurrentVideo, string[] SelectedCategory)
        {
            try
            {
                int ProfFileId = 0;
                int BgrImgFileId = 0;
                int IdCardFileId = 0;
                int NpwpFileId = 0;
                int AcNumFileId = 0;

                List<int> VideoListId = new List<int>();

                if (ProfImg != null)
                {
                    var ProfFileIds = await FilesController.UploadPhotosFile(ProfImg);
                    ProfFileId = ProfFileIds.Id;
                    // ProfFileId = ProfFileIds

                    //   ProfFileId = Convert.ToInt32(ProfFileIds.ExecuteResultAsync);
                    //var llala = JsonConvert.DeserializeObject<FilesModel>(ProfFileIds);

                }
                if (BgrImg != null)
                {
                    var BgrImgFileIds = await FilesController.UploadPhotosFile(BgrImg);
                    BgrImgFileId = BgrImgFileIds.Id;
                }
                if (IdCardImg != null)
                {
                    var IdCardFileIds = await FilesController.UploadPhotosFile(IdCardImg);
                    IdCardFileId = IdCardFileIds.Id;
                }
                if (NpwpImg != null)
                {
                    var NpwpImgFileIds = await FilesController.UploadPhotosFile(NpwpImg);
                    NpwpFileId = NpwpImgFileIds.Id;
                }
                if (AcNumImg != null)
                {
                    var AcNumImgFileIds = await FilesController.UploadPhotosFile(AcNumImg);
                    AcNumFileId = AcNumImgFileIds.Id;
                }

                UserModel user = new UserModel();
                user.Id = model.UserId.Value;
                var currentdata = IUserManagementManager.GetUser(user);
                if (ProfFileId != 0)
                {
                    user.ImgProfFileId = ProfFileId;
                }

                if (BgrImgFileId != 0)
                {
                    user.BgrFileId = BgrImgFileId;
                }
                currentdata.FirstName = model.FirstName;
                currentdata.LastName = model.LastName;
                currentdata.PhoneNumber = model.PhoneNumber;
                currentdata.Email = model.Email;
                IUserManagementManager.UpdateUser(user);



                //var gettalentdata = IMasterManager.GetTalent(model);
                TalentModel gettalentdata = new TalentModel();

                if (IdCardFileId != 0)
                {
                    gettalentdata.IdCardFileId = IdCardFileId;
                }
                if (NpwpFileId != 0)
                {
                    gettalentdata.NPWPFileId = NpwpFileId;
                }
                if (AcNumFileId != 0)
                {
                    gettalentdata.AccountNumberFileId = AcNumFileId;
                }


                gettalentdata.TalentNm = model.TalentNm;
                gettalentdata.Instagram = model.Instagram;
                gettalentdata.Facebook = model.Facebook;
                gettalentdata.FollowersCount = model.FollowersCount;
                gettalentdata.Profesion = model.Profesion;
                gettalentdata.IsActive = model.IsActive;

                //gettalentdata.NPWPFileId = NpwpFileId;
                //gettalentdata.AccountNumberFileId = AcNumFileId;
                //gettalentdata.IdCardFileId = IdCardFileId;
                var Talent = IMasterManager.CreateTalent(gettalentdata);

                if (ListVideo != null)
                {
                    ITransactionManager.DeleteTalentVideoByTalentId(Talent.Id);

                    foreach (var i in ListCurrentVideo)
                    {
                        TalentVideoModel videomodel = new TalentVideoModel();
                        videomodel.FileId = i;
                        videomodel.TalentId = model.Id;
                        videomodel.VideoNm = "";
                        videomodel.BookCategory = 0;
                        ITransactionManager.CreateTalentVideo(videomodel);
                    }

                    foreach (var data in ListVideo)
                    {
                        var videoIds = await FilesController.UploadVideoFilesData(data);
                        TalentVideoModel videomodel = new TalentVideoModel();
                        videomodel.FileId = videoIds.Id;
                        videomodel.TalentId = model.Id;
                        videomodel.VideoNm = "";
                        videomodel.BookCategory = 0;
                        ITransactionManager.CreateTalentVideo(videomodel);


                    }
                }


                // TODO: Add update logic here

                return Json("OK");
            }
            catch
            {
                return View();

            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(TalentModel model, IFormFile ProfImg, IFormFile BgrImg, IFormFile IdCardImg, IFormFile NpwpImg, IFormFile AcNumImg, List<IFormFile> ListVideo, List<int> ListCurrentVideo, string[] SelectedCategory, string PriceAmountData, string SalePriceData, string PaymentShareData, List<int> ListCurrentCategory)
        {
            try
            {
                PriceAmountData = PriceAmountData.Replace(",", "").Replace(".", "");
                var PriceAmount = Convert.ToInt32(PriceAmountData);
                SalePriceData = SalePriceData.Replace(",", "").Replace(".", "");
                var SalePrice = Convert.ToInt32(SalePriceData);
                PaymentShareData = PaymentShareData.Replace(",", "").Replace(".", "");
                var PaymentShare = Convert.ToInt32(PaymentShareData);
                int ProfFileId = 0;
                int BgrImgFileId = 0;
                int IdCardFileId = 0;
                int NpwpFileId = 0;
                int AcNumFileId = 0;

                List<int> VideoListId = new List<int>();

                if (ProfImg != null)
                {
                    var ProfFileIds = await FilesController.UploadPhotosFile(ProfImg);
                    ProfFileId = ProfFileIds.Id;
                    // ProfFileId = ProfFileIds

                    //   ProfFileId = Convert.ToInt32(ProfFileIds.ExecuteResultAsync);
                    //var llala = JsonConvert.DeserializeObject<FilesModel>(ProfFileIds);

                }
                if (BgrImg != null)
                {
                    var BgrImgFileIds = await FilesController.UploadPhotosFile(BgrImg);
                    BgrImgFileId = BgrImgFileIds.Id;
                }
                if (IdCardImg != null)
                {
                    var IdCardFileIds = await FilesController.UploadPhotosFile(IdCardImg);
                    IdCardFileId = IdCardFileIds.Id;
                }
                if (NpwpImg != null)
                {
                    var NpwpImgFileIds = await FilesController.UploadPhotosFile(NpwpImg);
                    NpwpFileId = NpwpImgFileIds.Id;
                }
                if (AcNumImg != null)
                {
                    var AcNumImgFileIds = await FilesController.UploadPhotosFile(AcNumImg);
                    AcNumFileId = AcNumImgFileIds.Id;
                }

                UserModel user = new UserModel();
                user.Id = model.UserId.Value;
                var currentdata = IUserManagementManager.GetUser(user);
                if (ProfFileId != 0)
                {
                    currentdata.ImgProfFileId = ProfFileId;
                }

                if (BgrImgFileId != 0)
                {
                    currentdata.BgrFileId = BgrImgFileId;
                }

                currentdata.FirstName = model.FirstName;
                currentdata.LastName = model.LastName;
                currentdata.PhoneNumber = model.PhoneNumber;
                currentdata.Email = model.Email;
                currentdata.Bank = model.Bank;
                currentdata.BeneficiaryName = model.BeneficiaryName;
                currentdata.AccountNumber = model.AccountNumber;
                currentdata.Gender = model.Gender;
                currentdata.Bio = model.Bio;
                IUserManagementManager.UpdateUser(currentdata);



                var gettalentdata = IMasterManager.GetTalent(model);

                if (IdCardFileId != 0)
                {
                    gettalentdata.IdCardFileId = IdCardFileId;
                }
                if (NpwpFileId != 0)
                {
                    gettalentdata.NPWPFileId = NpwpFileId;
                }
                if (AcNumFileId != 0)
                {
                    gettalentdata.AccountNumberFileId = AcNumFileId;
                }

                gettalentdata.TalentNm = model.TalentNm;
                gettalentdata.Instagram = model.Instagram;
                gettalentdata.Facebook = model.Facebook;
                gettalentdata.FollowersCount = model.FollowersCount;
                gettalentdata.Profesion = model.Profesion;
                gettalentdata.IsActive = model.IsActive;
                gettalentdata.AccountNumber = model.AccountNumber;
                gettalentdata.BeneficiaryName = model.BeneficiaryName;
                gettalentdata.Bank = model.Bank;
                gettalentdata.Note = model.Note;
                gettalentdata.IsAvailable = model.IsAvailable;
                gettalentdata.IsUnderAgency = model.IsUnderAgency;
                gettalentdata.AgencyId = model.AgencyId;
                gettalentdata.PriceAmount = PriceAmount;
                gettalentdata.SalePrice = SalePrice;
                gettalentdata.PaymentShare = PaymentShare;
                IMasterManager.UpdateTalent(gettalentdata);

                if (ListVideo != null)
                {
                    ITransactionManager.DeleteTalentVideoByTalentId(model.Id);

                    //foreach (var i in ListCurrentVideo)
                    for (int x = 0; x < ListCurrentVideo.Count; x++)
                    {

                        TalentVideoModel videomodel = new TalentVideoModel();
                        videomodel.FileId = ListCurrentVideo[x];
                        videomodel.TalentId = model.Id;
                        videomodel.VideoNm = "";
                        videomodel.BookCategory = 0;
                        if (ListCurrentCategory.Count > 0)
                        {
                            videomodel.BookCategory = ListCurrentCategory[x];
                        }
                        ITransactionManager.CreateTalentVideo(videomodel);
                    }

                    foreach (var data in ListVideo)
                    {
                        var videoIds = await FilesController.UploadVideoFilesData(data);
                        TalentVideoModel videomodel = new TalentVideoModel();
                        videomodel.FileId = videoIds.Id;
                        videomodel.TalentId = model.Id;
                        videomodel.VideoNm = "";
                        videomodel.BookCategory = 0;
                        ITransactionManager.CreateTalentVideo(videomodel);


                    }
                }

                if (gettalentdata != null)
                {
                    IMasterManager.DeleteTalentCategoryById(gettalentdata.Id);

                    foreach (var i in SelectedCategory)
                    {
                        var categoryModel = new TalentCategoryViewModel();
                        categoryModel.TalentId = gettalentdata.Id;
                        categoryModel.CategoryId = int.Parse(i);
                        var temp = IMasterManager.CreateTalentCategory(categoryModel);
                    }


                }

                #region CREATE/UPDATE TALENT SALDO
                //if (gettalentdata != null)
                //{
                //    //if (gettalentdata.IsPriority)
                //    //{
                //    //    var initSaldo = new SaldoModel();
                //    //    SaldoModel getSaldo = new SaldoModel();

                //    //    initSaldo.TalentId = gettalentdata.Id;
                //    //    getSaldo = ITransactionManager.GetSaldoByTalentId(initSaldo);

                //    //    if (getSaldo != null)
                //    //    {
                //    //        getSaldo.IsActive = "1";
                //    //        ITransactionManager.UpdateSaldo(getSaldo);
                //    //    }
                //    //    else
                //    //    {
                //    //        getSaldo = new SaldoModel();
                //    //        getSaldo.TalentId = gettalentdata.Id;
                //    //        getSaldo.CreatedBy = HelperController.GetCookie("Username");
                //    //        ITransactionManager.CreateSaldo(getSaldo);
                //    //    }
                //    //}
                //    //else if (!gettalentdata.IsPriority)
                //    //{
                //    //    var initSaldo = new SaldoModel();
                //    //    SaldoModel getSaldo = new SaldoModel();

                //    //    initSaldo.TalentId = gettalentdata.Id;
                //    //    getSaldo = ITransactionManager.GetSaldoByTalentId(initSaldo);

                //    //    if (getSaldo != null)
                //    //    {
                //    //        getSaldo.IsActive = "0";
                //    //        ITransactionManager.UpdateSaldo(getSaldo);
                //    //    }
                //    //}

                //}
                #endregion


                // TODO: Add update logic here

                return RedirectToAction("ListTalent");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult EditNew(int id)
        {
            TalentModel model = new TalentModel();
            model.Id = id;
            var data = IMasterManager.GetTalent(model);
            ViewBag.ListGender = new SelectList(HelperController.GenderList, "value", "text", 0);
            ViewBag.ListStatus = new SelectList(HelperController.StatusList, "value", "text", 1);
            data.ListTalentVideo = ITransactionManager.GetTalentVideos(id);
            ViewBag.ListBank = new SelectList(HelperController.MainBankAccount, "value", "text");
            ViewBag.OtherBank = new SelectList(HelperController.OthersBankAccount, "value", "text");

            var categorymodel = IMasterManager.GetCategoryByType("Talent");


            IList<TalentCategoryViewModel> ListCategory = new List<TalentCategoryViewModel>();
            foreach (var i in categorymodel)
            {
                TalentCategoryViewModel tcv = new TalentCategoryViewModel();
                tcv.CategoryId = i.Id;
                tcv.CategoryNm = i.CategoryNm;
                ListCategory.Add(tcv);
            }

            data.TalentCategory = ListCategory;

            if (data != null)
            {
                data.TalentSelectedCategory = IMasterManager.GetTalentCategoryData(data.Id);

            }

            return View(data);
        }

        #region PERFORMANCE
        public IActionResult ListPerform()
        {
            TalentPerformFormModel dataForm = new TalentPerformFormModel();
            List<TalentPerformanceModel> listData = new List<TalentPerformanceModel>();
            var period = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "Period").FirstOrDefault().ParamValue;
            var data = IMasterManager.GetTalentPerformance().Where(x => x.Status == Convert.ToInt32(Registration.Completed) && x.RoleId == (int)Role.Talent);
            listData = data.ToList();
            dataForm.ListData = listData;
            dataForm.Period = "";
            ViewBag.Period = new SelectList(HelperController.PeriodData, "value", "text", period);
            return View(dataForm);
        }
        public IActionResult ListPerformByPeriod(string Period)
        {
            TalentPerformFormModel dataForm = new TalentPerformFormModel();
            List<TalentPerformanceModel> listData = new List<TalentPerformanceModel>();
            if (!string.IsNullOrEmpty(Period))
            {
                listData = IMasterManager.GetTalentPerformanceByPeriod(Period).Where(x => x.Status == Convert.ToInt32(Registration.Completed) && x.RoleId == 3).ToList();
            }
            else
            {
                listData = IMasterManager.GetTalentPerformance().Where(x => x.Status == Convert.ToInt32(Registration.Completed) && x.RoleId == 3).ToList();
            }
            dataForm.ListData = listData;
            dataForm.Period = Period;
            ViewBag.Period = new SelectList(HelperController.PeriodData, "value", "text", Period);
            return View("~/Views/AdmTalent/ListPerform.cshtml", dataForm);
        }
        #endregion

        #region SALDO & TOPUP
        public IActionResult ListSaldo()
        {
            List<SaldoModel> listData = new List<SaldoModel>();
            listData = ITransactionManager.GetAllSaldo().ToList();
            return View(listData);
        }

        public IActionResult Topup(Int64 id)
        {
            #region INIT
            TopupViewModel topupVModel = new TopupViewModel();
            List<TopupModel> listHistoryTopup = new List<TopupModel>();
            TopupModel newTopupModel = new TopupModel();
            SaldoModel saldoModel = new SaldoModel();
            #endregion

            #region FILL SALDO MODEL
            saldoModel.Id = id;
            saldoModel = ITransactionManager.GetSaldoById(saldoModel);
            topupVModel.SaldoModel = saldoModel;
            #endregion

            #region FILL HISTORY TOPUP
            TopupModel topup = new TopupModel();
            topup.SaldoId = id;
            listHistoryTopup = ITransactionManager.GetTopupBySaldoId(topup).ToList();
            topupVModel.ListHistoryTopUp = listHistoryTopup;
            #endregion

            #region FILL TOPUP
            newTopupModel.TopUpAmt = 0;
            topupVModel.TopupModel= newTopupModel;
            #endregion

            return View(topupVModel);

        }

        public IActionResult ListApprovalTopup()
        {
            List<TopupModel> listData = new List<TopupModel>();
            TopupModel topup = new TopupModel();
            topup.TopupStatus = (int)TopupStatus.Request;
            listData = ITransactionManager.GetTopupByStatus(topup).ToList();
            return View(listData);
        }

        [HttpPost]
        public async Task<IActionResult> TopupPost(TopupViewModel model, string TopUpAmtInput)
        {
            try
            {
                #region INIT
                TopUpAmtInput = TopUpAmtInput.Replace(",", "").Replace(".", "");

                var TopUpAmt = Convert.ToInt32(TopUpAmtInput);

                int seqno = 0;

                TopupModel topup = new TopupModel();
                SaldoModel saldo = new SaldoModel();               
                #endregion

                #region CREATE TOPUP
                topup.SaldoId = model.SaldoModel.Id;
                var lastTopUp = ITransactionManager.GetTopupBySaldoId(topup);
                if (lastTopUp != null && lastTopUp.Count > 0)
                {
                    seqno = Convert.ToInt32(lastTopUp.LastOrDefault().SeqNo);
                }
                topup.SeqNo = seqno + 1;
                topup.TopUpAmt = TopUpAmt;
                topup.LastSaldoAmt = model.SaldoModel.SaldoAmt;
                topup.LastUsedSaldoAmt = model.SaldoModel.SaldoUsedAmt;
                topup.CreatedBy = HelperController.GetCookie("Username");
                topup.TopupStatus = (int)TopupStatus.Topup;
                topup.TopupSource = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "TopupSrc" && x.ParamValue == "Finance").FirstOrDefault().ParamCode;
                topup.Notes = "Topup oleh Finance Tanpa Request";
                topup = ITransactionManager.CreateTopup(topup);
                #endregion

                #region UPDATE SALDO   
                saldo.Id = model.SaldoModel.Id;
                saldo = ITransactionManager.GetSaldoById(saldo);
                saldo.SaldoAmt += TopUpAmt;
                ITransactionManager.UpdateSaldo(saldo);
                #endregion


                #region NOTIF
                TalentModel talent = new TalentModel();
                talent.Id = Convert.ToInt32(saldo.TalentId);
                talent = IMasterManager.GetTalent(talent);
                HelperController.EmailApprovalTopup(talent.Email, "2", talent.FirstName, topup.Notes, Convert.ToDecimal(topup.TopUpAmt)).Wait();

                NotificationModel notifmodel = new NotificationModel();
                notifmodel.NotifType = "T";
                notifmodel.To = talent.UserId;
                notifmodel.Message = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "TopupNotif" && x.ParamCode == "2").FirstOrDefault().ParamValue;
                ITransactionManager.InsertNotification(notifmodel);
                #endregion
                // TODO: Add update logic here

                return Json("OK");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> TopupApproval(int id, string ActionApproval, string Note)
        {
            try
            {
                #region INIT
                int seqno = 0;

                TopupModel topup = new TopupModel();
                topup.Id = id;
                topup = ITransactionManager.GetTopupById(topup);

                var lastTopUp = ITransactionManager.GetTopupBySaldoId(topup);
                if (lastTopUp != null && lastTopUp.Count > 0)
                {
                    seqno = Convert.ToInt32(lastTopUp.LastOrDefault().SeqNo);
                }

                SaldoModel saldo = new SaldoModel();
                saldo.Id = Convert.ToInt64(topup.SaldoId);
                saldo = ITransactionManager.GetSaldoById(saldo);
                #endregion

                if (ActionApproval == "-1")
                {
                    #region UPDATE TOPUP
                    topup.SeqNo = seqno + 1;
                    topup.LastSaldoAmt = saldo.SaldoAmt;
                    topup.LastUsedSaldoAmt = saldo.SaldoUsedAmt;
                    topup.UpdatedBy = HelperController.GetCookie("Username");
                    topup.TopupStatus = (int)TopupStatus.Rejected;
                    topup.Notes = Note;
                    topup = ITransactionManager.TopupApproval(topup);
                    #endregion
                }
                else if (ActionApproval == "2")
                {

                    #region UPDATE TOPUP
                    topup.SeqNo = seqno + 1;
                    topup.LastSaldoAmt = saldo.SaldoAmt;
                    topup.LastUsedSaldoAmt = saldo.SaldoUsedAmt;
                    topup.UpdatedBy = HelperController.GetCookie("Username");
                    topup.TopupStatus = (int)TopupStatus.Topup;
                    topup.Notes = Note;
                    topup = ITransactionManager.TopupApproval(topup);
                    #endregion

                    #region UPDATE SALDO
                    saldo.SaldoAmt += topup.TopUpAmt;
                    ITransactionManager.UpdateSaldo(saldo);
                    #endregion

                }

                #region NOTIF
                TalentModel talent = new TalentModel();
                talent.Id = Convert.ToInt32(saldo.TalentId);
                talent = IMasterManager.GetTalent(talent);
                HelperController.EmailApprovalTopup(talent.Email, ActionApproval, talent.FirstName, Note, Convert.ToDecimal(topup.TopUpAmt)).Wait();

                NotificationModel notifmodel = new NotificationModel();
                notifmodel.NotifType = "T";
                notifmodel.To = talent.UserId;
                notifmodel.Message = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "TopupNotif" && x.ParamCode == ActionApproval).FirstOrDefault().ParamValue;
                ITransactionManager.InsertNotification(notifmodel);
                #endregion
                return Json("OK");
            }
            catch (Exception ex)
            {

                return Json("False");
                throw ex;
            }
        }
        #endregion


    }
}