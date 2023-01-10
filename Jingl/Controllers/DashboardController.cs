using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.Admin.ViewModel;
using Jingl.General.Model.User.ViewModel;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Jingl.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jingl.Web.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IMasterManager IMasterManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly ICookie _cookie;
        private readonly HelperController HelperController;

        public DashboardController(IConfiguration config, ICookie cookie)
        {
            this.IUserManagementManager = new UserManagementManager(config);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);
            this.HelperController = new HelperController(config, cookie);
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult TransactionOrder()
        {
            var model = new BarChartModel();
            var period = IMasterManager.AdmGetAllParameter().Where(z => z.ParamCode == "Period").FirstOrDefault().ParamValue;
            var data = ITransactionManager.GetTransactionOrders(period);

            var x = new Xaxis();
            var y = new Yaxis();
            var TransctionData = new List<TransactionData>();

            List<string> LabelList = new List<string>();
            
            foreach (var i in data)
            {
                LabelList.Add(i.DateData.ToString());

            }

            x.labels = LabelList;
            y.format = "currency";
            y.unit = "IDR";


            var valuedataDone = new TransactionData();
            var valuedataInProgress = new TransactionData();
            var valuedataAbandonCart = new TransactionData();
            var valuedataAbandonOrder = new TransactionData();

            var ListAmountDone = new List<int>();
            var ListAmountInProgress = new List<int>();
            var ListAmountAbandonCart = new List<int>();
            var ListAmountAbandonOrder = new List<int>();

            foreach (var date in data)
            {

                valuedataDone.name = "Total Order Done";             

                foreach(var value in data.Where(v=>v.DateData == date.DateData).ToList())
                {

                    ListAmountDone.Add(value.TotalOrderDone);
                }
                valuedataDone.data = ListAmountDone;
           }



            foreach (var date in data.OrderBy(b => b.OrderDate))
            {

                valuedataInProgress.name = "Total InProgress";


                foreach (var value in data.Where(v => v.DateData == date.DateData).ToList())
                {

                    ListAmountInProgress.Add(value.TotalOrderInProgress);
                }
                valuedataInProgress.data = ListAmountInProgress;
            }

            foreach (var date in data.OrderBy(b => b.OrderDate))
            {

                valuedataAbandonCart.name = "Total Abandon Cart / Order";

                foreach (var value in data.Where(v => v.DateData == date.DateData).ToList())
                {

                    ListAmountAbandonCart.Add(value.TotalOrderAbandonCart);
                }
                valuedataAbandonCart.data = ListAmountAbandonCart;
            }

            //foreach (var date in data.OrderBy(b => b.OrderDate))
            //{

            //    valuedataAbandonOrder.name = "Total Abandon Order";

            //    foreach (var value in data.Where(v => v.DateData == date.DateData).ToList())
            //    {

            //        ListAmountAbandonOrder.Add(value.TotalOrderAbandonOrder);
            //    }
            //    valuedataAbandonOrder.data = ListAmountAbandonOrder;
            //}

            TransctionData.Add(valuedataDone);
            TransctionData.Add(valuedataInProgress);
            TransctionData.Add(valuedataAbandonCart);
            //TransctionData.Add(valuedataAbandonOrder);

            model.series = TransctionData;
            model.x_axis = x;
          //  model.y_axis = y;



            return Json(model);
        }


        public IActionResult GetTotalPotentialDataPerPeriod()
        {

            var model = new NumberSecondaryModel();
            try
            {
                var itemdata = new List<valuedata>();


                var period = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "Period").FirstOrDefault().ParamValue;
                var data = ITransactionManager.GetPotensialSalesPerPeriod(period);

                itemdata.Add(new valuedata { text = "Total Potential Order", value = Convert.ToInt32(data.Amount) });
                

                model.item = itemdata.ToList();


            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "GetTotalPotentialDataPerPeriod", ex.Message);
                throw ex;
            }

            return Json(model);
        }

        public IActionResult GetTotalSalesDataPerPeriod()
        {

            var model = new NumberSecondaryModel();
            try
            {
                var itemdata = new List<valuedata>();


                var period = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "Period").FirstOrDefault().ParamValue;
                var data = ITransactionManager.GetTotalSalesPerPeriod(period);

                itemdata.Add(new valuedata { text = "Total Sales Order", value = Convert.ToInt32(data.Amount) });


                model.item = itemdata.ToList();


            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "GetTotalSalesDataPerPeriod", ex.Message);
                throw ex;
            }

            return Json(model);
        }


        public IActionResult GetBestTalentData()
        {
            
            var model = new DashboardViewModel();
            try
            {
                var itemdata = new List<Itemdata>();

               
                TalentPerformFormModel dataForm = new TalentPerformFormModel();
                List<TalentPerformanceModel> listData = new List<TalentPerformanceModel>();

                var period = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "Period").FirstOrDefault().ParamValue;
                var data = IMasterManager.GetTalentPerformanceByPeriod(period).Where(x=>x.CompletedBook > 0).OrderByDescending(x=>x.CompletedBook).Take(3).ToList();
                listData = data.ToList();
               
                foreach (var i in data)
                {
                    itemdata.Add(new Itemdata { label = i.TalentNm, value = i.CompletedBook });
                }

                model.items = itemdata.ToList();


            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "GetBestTalentData", ex.Message);
                throw ex;
            }

            return Json(model);
        }


        public IActionResult GetNewTalentData()
        {
            var model = new DashboardViewModel();
            try
            {
                var itemdata = new List<Itemdata>();

                //data = IMasterManager.GetAllTalent().Where(x => x.Status == 3).OrderByDescending(x => x.CompletedBook).Take(5).ToList();
                //foreach (var i in data)
                //{
                //    itemdata.Add(new Itemdata { label = i.TalentNm, value = i.CompletedBook });
                //}

                //model.items = itemdata.ToList();

                TalentPerformFormModel dataForm = new TalentPerformFormModel();
                List<TalentPerformanceModel> listData = new List<TalentPerformanceModel>();

                var period = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "Period").FirstOrDefault().ParamValue;
                var data = IMasterManager.GetTalentPerformanceByPeriod(period).OrderByDescending(x => x.CreatedDate).Take(3).ToList();
                listData = data.ToList();
                //dataForm.ListData = listData;

                foreach (var i in data)
                {
                    itemdata.Add(new Itemdata { label = i.TalentNm, value = i.CompletedBook });
                }

                model.items = itemdata.ToList();


            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "GetNewTalentData", ex.Message);
                throw ex;
            }

            return Json(model);
        }

        public IActionResult GetWorstTalentData()
        {
            var model = new DashboardViewModel();
            try
            {
                var itemdata = new List<Itemdata>();

                //data = IMasterManager.GetAllTalent().Where(x => x.Status == 3).OrderByDescending(x => x.CompletedBook).Take(5).ToList();
                //foreach (var i in data)
                //{
                //    itemdata.Add(new Itemdata { label = i.TalentNm, value = i.CompletedBook });
                //}

                //model.items = itemdata.ToList();

                TalentPerformFormModel dataForm = new TalentPerformFormModel();
                List<TalentPerformanceModel> listData = new List<TalentPerformanceModel>();

                var period = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == "Period").FirstOrDefault().ParamValue;
                var data = IMasterManager.GetTalentPerformanceByPeriod(period).Where(x=>x.TotalBook > 0).OrderBy(x => x.OrderPercentage).Take(3).ToList();
                listData = data.ToList();
                //dataForm.ListData = listData;

                foreach (var i in data)
                {
                    itemdata.Add(new Itemdata { label = i.TalentNm, value = i.TotalBook - i.CompletedBook });
                }

                model.items = itemdata.ToList();


            }
            catch (Exception ex)
            {

                HelperController.InsertLog(0, "GetNewTalentData", ex.Message);
                throw ex;
            }

            return Json(model);
        }


    }
}