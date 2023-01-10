using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.User.Notification;
using Jingl.General.Model.User.ViewModel;
using Jingl.General.Model.Agency.Master;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.Service.Interface
{
    public interface IMasterManager
    {
        TalentModel CreateTalent(TalentModel model);
        TalentModel GetTalent(TalentModel model);
        TalentModel UpdateTalent(TalentModel model);
        TalentModel UpdateBilling(TalentModel model);
        IList<TalentModel> GetAllTalent();
        IList<TalentModel> GetAllTalentAgency(TalentParamModel model);
        IList<PayMethodModel> GetAllPayMethod();
        PayMethodModel GetDataPayMethod(PayMethodModel model);
        PayMethodModel CreatePayMethodData(PayMethodModel model);
        PayMethodModel UpdatePayMethodData(PayMethodModel model);

        IList<CategoryModel> GetAllCategory();
        CategoryModel GetDataCategory(CategoryModel model);
        CategoryModel CreateCategoryData(CategoryModel model);
        CategoryModel UpdateCategoryData(CategoryModel model);

        TalentViewModel GetAllTalentCategory();

        IList<TalentCategoryViewModel> GetAllTalentByCategory(int CategoryId);
      
        TalentModel GetTalentProfiles(int UserId);
        IList<CategoryModel> GetCategoryByType(string CategoryType);
        EmailNotificationModel GetEmailNotification(int Id);
        ParameterModel GetParameter(int Id);
        ParameterModel GetParameterByCode(string Code);
        IList<TalentCategoryViewModel> GetTalentCategoryData(int TalentId);
        IList<TalentCategoryViewModel> GetTalentCategoryAllData();
        IList<TalentCategoryViewModel> GetTalentCategoryAllDataNew();
        void DeleteTalentCategoryById(int TalentId);
        TalentCategoryViewModel CreateTalentCategory(TalentCategoryViewModel model);
        DeviceModel CreateDevice(DeviceModel model);
        IList<DeviceModel> GetAllDevice();
        IList<DeviceModel> GetDeviceByUserId(int UserId);
        void DeleteDevice(int Id);
        IList<RegionModel> GetAllRegion();
        IList<ParameterModel> AdmGetAllParameter();
        ParameterModel CreateParam(ParameterModel model);
        ParameterModel UpdateParam(ParameterModel model);
        IList<BankModel> GetAllBank();

        IList<BannerModel> GetAllBanner();
        BannerModel GetBanner(BannerModel model);
        BannerModel CreateBanner(BannerModel model);
        BannerModel UpdateBanner(BannerModel model);
        void DeleteParameter(int id);
        void DeleteBanner(int id);
        void DeleteCategoryData(int id);

        IList<EmailNotificationModel> GetAllEmailNotification();
        IList<TalentCategoryViewModel> GetDistinctAllTalentByCategory();

        #region TalentPerform
        IList<TalentPerformanceModel> GetTalentPerformance();
        IList<TalentPerformanceModel> GetTalentPerformanceAgency(TalentParamModel model);
        IList<TalentPerformanceModel> GetTalentPerformanceByPeriod(string Period);
        #endregion

        #region Voucher
        VoucherModel CheckVoucherCode(int BookId, string VoucherCd, decimal PriceAmount);
        VoucherModel GetVoucherByCode(string VoucherCd);
        VoucherModel CreateVoucher(VoucherModel model);
        VoucherTalentViewModel CreateVoucherTalent(VoucherTalentViewModel model);
        void DeleteVoucher(int id);
        void DeleteVoucherTalentByVoucherId(int VoucherId);
        VoucherModel GetEmptyVoucher();
        VoucherModel GetVoucher(VoucherModel model);
        IList<VoucherTalentViewModel> GetVoucherTalentData(int VoucherId);
        IList<VoucherModel> GetAllVoucher();
        IList<VoucherAllTalentModel> GetAllVoucherByTalent();
        VoucherModel UpdateVoucher(VoucherModel model);
        #endregion


        #region Agency

        IList<AgencyModel> GetAllAgency();
        AgencyModel GetAgency(AgencyModel model);
        AgencyModel CreateAgency(AgencyModel model);
        AgencyModel UpdateAgency(AgencyModel model);
        void DeleteAgency(int id);

        #endregion

        #region Sticker
        IList<StickerModel> GetAllSticker();
        IList<StickerModel> GetAllStickerByCategory(int? CategoryId);
        StickerModel GetSticker(StickerModel model);
        StickerModel CreateSticker(StickerModel model);
        StickerModel UpdateSticker(StickerModel model);
        void DeleteSticker(int id);
        #endregion

        IList<TalentModel> GetAllTalentForDropdown();
        IList<TalentModel> AdmGetAllTalent();
        TalentModel GetTalentByUserId(int UserId);



    }
}
