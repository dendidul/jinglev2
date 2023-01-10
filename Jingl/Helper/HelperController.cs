using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CookieManager;
using Jingl.General.Enum;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.Transaction.API;
using Jingl.General.Model.Admin.Transaction.API.FasPay;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.Admin.ViewModel;
using Jingl.General.Utility;
using Jingl.Service.Interface;
using Jingl.Service.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Jingl.Web.Helper
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class HelperController : Controller
    {
        private ICookie _cookie;
        private IConfiguration _config;
        private readonly IUserManagementManager IUserManagementManager;
        private readonly IMasterManager IMasterManager;
        private readonly ITransactionManager ITransactionManager;
        private readonly Logger _logger;
        public HelperController(IConfiguration config, ICookie cookie)
        {
            this._config = config;
            this._cookie = cookie;
            this.IUserManagementManager = new UserManagementManager(config);
            this._logger = new Logger(config);
            this.IMasterManager = new MasterManager(config);
            this.ITransactionManager = new TransactionManager(config);

        }

        public string CookieTimeOut()
        {
            return _config.GetSection("TimeOut:Cookie").Value.ToString();
        }

        public string CategoryTalentData(int talentId)
        {
            string category = "";
            try
            {
                var talentcategory = IMasterManager.GetTalentCategoryData(talentId);
                foreach (var i in talentcategory)
                {
                    CategoryModel categoryModel = new CategoryModel();
                    categoryModel.Id = i.CategoryId;
                    var CategoryData = IMasterManager.GetDataCategory(categoryModel);

                    if (CategoryData != null)
                    {
                        if (i.CategoryId == talentcategory.LastOrDefault().CategoryId)
                        {
                            category += CategoryData.CategoryNm;
                        }
                        else
                        {
                            category += CategoryData.CategoryNm + " - ";

                        }

                    }
                }


            }
            catch (Exception ex)
            {

                throw;
            }

            return category;
        }

        public string JinglUrl()
        {
            return _config.GetSection("UrlSystem:Jingl").Value.ToString();
        }

        public UserModel GetUserData()
        {
            var UserData = new UserModel();
            try
            {
                UserData.Id = Convert.ToInt32(GetCookie("UserId"));
                UserData = IUserManagementManager.GetUser(UserData);

            }
            catch (Exception ex)
            {

                throw;
            }
            return UserData;
        }


        public string JinglEmail()
        {
            return _config.GetSection("JinglEmail:Email").Value.ToString();
        }


        public string GoogleStorageProjectName()
        {
            return _config.GetSection("GoogleStorage:ProjectName").Value.ToString();
        }

        public string AzureAccountName()
        {
            return _config.GetSection("AzureConnection:Account").Value.ToString();
        }

        public string AzureAccountKey()
        {
            return _config.GetSection("AzureConnection:key").Value.ToString();
        }

        public string SendGridApiKey()
        {
            return _config.GetSection("SendGrid:ApiKey").Value.ToString();
        }

        public string JinglPassword()
        {
            return _config.GetSection("JinglEmail:Password").Value.ToString();
        }



        public FasPayConfigurationModel GetFasPayConfig()
        {
            FasPayConfigurationModel model = new FasPayConfigurationModel();
            model.MerchantId = _config.GetSection("FastPay:MerchantId").Value.ToString();
            model.MerchantName = _config.GetSection("FastPay:MerchantName").Value.ToString();
            model.Password = _config.GetSection("FastPay:Password").Value.ToString();
            model.UserId = _config.GetSection("FastPay:UserId").Value.ToString();
            return model;
        }

        public string MidtranServerKey()
        {
            return _config.GetSection("Snap:Serverkey").Value.ToString();
        }

        public string MidtransClientKey()
        {
            return _config.GetSection("Snap:Clientkey").Value.ToString();
        }

        public string ApiUrl()
        {
            return _config.GetSection("Snap:BaseUrl").Value.ToString();
        }

        public string ApiUrlV2()
        {
            return _config.GetSection("Snap:BaseUrlV2").Value.ToString();
        }

        public string GetCookie(string key)
        {
            //return _cookie.Get(key);
            if (key == "UserId")
            {
                var cookie = _cookie.Get(key);
                if (cookie == "")
                {
                    return "0";
                }
                else
                {
                    return _cookie.Get(key);
                }
            }
            else
            {
                return _cookie.Get(key);
            }

        }

        public string DestinationLogFolder()
        {
            return _config.GetSection("Logging:DestinationFolder:Web").Value.ToString();
        }

        public void SetCookie(string key, string value)
        {
            CookieOptions option = new CookieOptions();
            //option.Expires = DateTime.Now.AddDays(Convert.ToInt32(CookieTimeOut()));
            option.Expires = DateTime.Now.AddDays(30);
            _cookie.Set(key, value, option);
            //_cookie.Set()



        }
        /// <summary>  
        /// Delete the key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public void RemoveCookie(string key)
        {
            _cookie.Remove(key);
            //Response.Cookies.Delete(key);
        }

        public void InsertLog(int userId, string function, string message)
        {
            UserModel model = new UserModel();
            model.Id = userId;
            var userdata = IUserManagementManager.GetUser(model);
            var userName = userdata != null ? userdata.UserName : "";

            _logger.WriteFunctionLog(DestinationLogFolder(), userName, function, message, "Web");

            //Response.Cookies.Delete(key);
        }

        public bool ConfigUserDataToCookies(string UserId)
        {
            try
            {
                UserModel model = new UserModel();
                model.Id = Convert.ToInt32(UserId);
                var checkdata = IUserManagementManager.GetUser(model);

                if (checkdata != null)
                {
                    //CookieOptions options = new CookieOptions();
                    //options.Expires = DateTime.Now.AddDays(1);
                    //Response.Cookies.Append("Role_ID", checkdata != null ? checkdata.RoleId.ToString():"", options);
                    //Response.Cookies.Append("UserName", model.UserName, options);

                    string FirstName = checkdata.FirstName != null ? checkdata.FirstName : "";
                    string LastName = checkdata.LastName != null ? checkdata.LastName : "";
                    string UserName = FirstName + " " + LastName;
                    SetCookie("UserName", UserName);
                    SetCookie("Role_ID", checkdata != null ? checkdata.RoleId.ToString() : "");
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        public bool HasLogin()
        {
            try
            {
                var getUserId = GetCookie("UserId");

                if (getUserId != "")
                {

                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        [HttpPut("{recipient}")]
        [Route("~/api/Helper/SendVerificationToken")]
        public async Task<bool> SendVerificationToken([FromRoute] string id , string code, string recipient, string FirstName = null)
        {
            //try
            //{

            //    string Email = JinglEmail();
            //    string Password = JinglPassword();
            //    EmailService service = new EmailService();
            //    service.SendVerificationCode(Email, Password, code, recipient, subject);
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    return false;

            //}

            try
            {
                int UserId = Convert.ToInt32(GetCookie("UserId"));

                UserModel curUser = new UserModel();
                curUser.Id = UserId;
                curUser = IUserManagementManager.GetUser(curUser);
                curUser.Email = recipient;

                var Template = IMasterManager.GetEmailNotification(3);
                var EmailTemplate = Template.TemplateValue;

                if(string.IsNullOrEmpty(curUser.VerificationCode) || string.IsNullOrEmpty(code))
                {
                    UserModel UM = new UserModel();
                    UM = curUser;
                    IUserManagementManager.SetVerificationCode(UM);
                    UM = IUserManagementManager.GetUser(UM);
                    code = UM.VerificationCode;
                }

                EmailTemplate = EmailTemplate.Replace("@verificationcode", code);

                if (!string.IsNullOrEmpty(FirstName))
                {
                    EmailTemplate = EmailTemplate.Replace("@FirstName", FirstName);
                }
                else
                {
                    EmailTemplate = EmailTemplate.Replace("@FirstName", recipient);
                }

                var client = new SendGridClient(SendGridApiKey());
                var from = new EmailAddress("noreply@fameoapp.com", "FameoNotification");
                var To = new EmailAddress(recipient, "");
                var msg = MailHelper.CreateSingleEmail(from, To, "Fameo - Verification Code ", EmailTemplate, EmailTemplate);
                var response = await client.SendEmailAsync(msg);
                IUserManagementManager.UpdateUser(curUser);
                return true;
            }
            catch (Exception ex)
            {
                InsertLog(0, "NotificationEmail", ex.Message);
                return false;

            }

        }


        [HttpPut("{recipient}")]
        [Route("~/api/Helper/SendVerificationTokenAfterSignup")]
        public async Task<bool> SendVerificationTokenAfterSignup([FromRoute] int id, string code, string recipient, string FirstName = null)
        {
            
            try
            {
                int UserId = Convert.ToInt32(GetCookie("UserId"));

                UserModel curUser = new UserModel();
                curUser.Id = id;
                curUser = IUserManagementManager.GetUser(curUser);
                curUser.Email = recipient;

                var Template = IMasterManager.GetEmailNotification(3);
                var EmailTemplate = Template.TemplateValue;

                if (string.IsNullOrEmpty(curUser.VerificationCode) || string.IsNullOrEmpty(code))
                {
                    UserModel UM = new UserModel();
                    UM = curUser;
                    IUserManagementManager.SetVerificationCode(UM);
                    UM = IUserManagementManager.GetUser(UM);
                    code = UM.VerificationCode;
                }

                EmailTemplate = EmailTemplate.Replace("@verificationcode", code);

                if (!string.IsNullOrEmpty(FirstName))
                {
                    EmailTemplate = EmailTemplate.Replace("@FirstName", FirstName);
                }
                else
                {
                    EmailTemplate = EmailTemplate.Replace("@FirstName", recipient);
                }

                var client = new SendGridClient(SendGridApiKey());
                var from = new EmailAddress("noreply@fameoapp.com", "FameoNotification");
                var To = new EmailAddress(recipient, "");
                var msg = MailHelper.CreateSingleEmail(from, To, "Fameo - Verification Code ", EmailTemplate, EmailTemplate);
                var response = await client.SendEmailAsync(msg);
                IUserManagementManager.UpdateUser(curUser);
                return true;
            }
            catch (Exception ex)
            {
                InsertLog(0, "NotificationEmail", ex.Message);
                return false;

            }

        }


        public async Task<bool> SendEmailForgotPassword(string UserId, string recipient,string RequestCode)
        {
            //try
            //{

            //    string Email = JinglEmail();
            //    string Password = JinglPassword();
            //    EmailService service = new EmailService();
            //    service.SendVerificationCode(Email, Password, code, recipient, subject);
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    return false;

            //}

            try
            {
                var Template = IMasterManager.GetEmailNotification(4);
                var EmailTemplate = Template.TemplateValue;

                UserModel data = new UserModel();
                data.Id = Convert.ToInt32(UserId);

                var getdata = IUserManagementManager.GetUser(data);

                // EmailTemplate = EmailTemplate.Replace("@link", JinglUrl() + "\\Account\\ForgotPassword?UserId=" + UserId);
                EmailTemplate = EmailTemplate.Replace("@link", JinglUrl() + "ForgotPassword/"+ RequestCode);
                EmailTemplate = EmailTemplate.Replace("@FirstName", getdata.FirstName);


                var client = new SendGridClient(SendGridApiKey());
                var from = new EmailAddress("noreply@fameoapp.com", "FameoNotification");
                var To = new EmailAddress(recipient, "");
                var msg = MailHelper.CreateSingleEmail(from, To, "Fameo - Forgot Password ", EmailTemplate, EmailTemplate);
                var response = await client.SendEmailAsync(msg);
                return true;
            }
            catch (Exception ex)
            {
                InsertLog(0, "NotificationEmail", ex.Message);
                return false;

            }

        }


        public static List<OptionModel> GenderList
        {
            get
            {
                List<OptionModel> listItems = new List<OptionModel>();
                listItems.Add(new OptionModel { value = "M", text = "Laki - Laki" });
                listItems.Add(new OptionModel { value = "F", text = "Perempuan" });
                return listItems;
            }
        }

        public List<OptionModel> MainBankAccount
        {
            get
            {
                List<OptionModel> listItems = new List<OptionModel>();
                var ListBank = IMasterManager.GetAllBank().Where(x => x.BankType == "Main").ToList();

                foreach (var i in ListBank)
                {

                    listItems.Add(new OptionModel { value = i.BankCode, text = i.BankDescription });


                }
                listItems.Add(new OptionModel { value = "Others", text = "Others" });

                return listItems;
            }
        }

        public List<OptionModel> AgencyList
        {
            get
            {
                List<OptionModel> listItems = new List<OptionModel>();
                var AgencyList = IMasterManager.GetAllAgency().ToList();

                foreach (var i in AgencyList)
                {

                    listItems.Add(new OptionModel { value = i.Id.ToString(), text = i.AgencyNm });


                }
                //listItems.Add(new OptionModel { value = "Others", text = "Others" });

                return listItems;
            }
        }


        public List<OptionModel> UserAgentList
        {
            get
            {
                List<OptionModel> listItems = new List<OptionModel>();
                var UserList = IUserManagementManager.AdmGetAllUser().Where(x => x.RoleId == (int)Role.Agency).ToList();

                foreach (var i in UserList)
                {

                    listItems.Add(new OptionModel { value = i.Id.ToString(), text = i.UserName });


                }
                //listItems.Add(new OptionModel { value = "Others", text = "Others" });

                return listItems;
            }
        }

        public List<OptionModel> OthersBankAccount
        {
            get
            {
                List<OptionModel> listItems = new List<OptionModel>();
                var ListBank = IMasterManager.GetAllBank().Where(x => x.BankType == "Others").ToList();

                foreach (var i in ListBank)
                {

                    listItems.Add(new OptionModel { value = i.BankCode, text = i.BankDescription });


                }

                return listItems;
            }
        }

        public static List<OptionModel> SupportStatusList
        {
            get
            {
                List<OptionModel> listItems = new List<OptionModel>();
                listItems.Add(new OptionModel { value = "1", text = "Submit" });
                listItems.Add(new OptionModel { value = "2", text = "Replied" });
                return listItems;
            }
        }

        public static List<OptionModel> StatusList
        {
            get
            {
                List<OptionModel> listItems = new List<OptionModel>();
                listItems.Add(new OptionModel { value = "1", text = "Aktif" });
                listItems.Add(new OptionModel { value = "0", text = "Tidak Aktif" });
                return listItems;
            }
        }

        public List<OptionModel> TransactionStatusList
        {
            get
            {
                var getParameter = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "TransStat").ToList();

                List<OptionModel> listItems = new List<OptionModel>();
                foreach (var i in getParameter)
                {
                    listItems.Add(new OptionModel { value = i.ParamCode, text = i.ParamValue });
                }

                return listItems;
            }
        }

        public List<OptionModel> PeriodData
        {
            get
            {
                var getParameter = ITransactionManager.GetAllBook().Select(x => x.Period).ToList().Distinct();

                List<OptionModel> listItems = new List<OptionModel>();
                foreach (var i in getParameter)
                {
                    if (i != null)
                    {
                        listItems.Add(new OptionModel { value = i, text = i });
                    }

                }

                return listItems;
            }
        }


        public List<OptionModel> BookCategoryList
        {
            get
            {
                var getParameter = IMasterManager.GetCategoryByType("Book");

                List<OptionModel> listItems = new List<OptionModel>();
                foreach (var i in getParameter)
                {
                    listItems.Add(new OptionModel { value = i.Id.ToString(), text = i.CategoryNm });
                }

                return listItems;
            }
        }

        public List<OptionModel> RegistrationStatusList
        {
            get
            {
                var getParameter = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "RegStat").ToList();

                List<OptionModel> listItems = new List<OptionModel>();
                foreach (var i in getParameter)
                {
                    listItems.Add(new OptionModel { value = i.ParamCode, text = i.ParamValue });
                }

                return listItems;
            }
        }


        public static List<OptionModel> PaymentMethodList
        {
            get
            {
                List<OptionModel> listItems = new List<OptionModel>();
                listItems.Add(new OptionModel { value = "bank_transfer", text = "Bank Transfer" });
                listItems.Add(new OptionModel { value = "gopay", text = "GOPAY" });
                listItems.Add(new OptionModel { value = "ovo", text = "OVO" });
                return listItems;
            }
        }

        public static List<OptionModel> CountryList
        {
            get
            {
                List<OptionModel> listItems = new List<OptionModel>();
                listItems.Add(new OptionModel { value = "Indonesia", text = "Indonesia" });
                return listItems;
            }
        }

        public static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }


        public string GenerateSignatureKey(string param)
        {

            var MD5hashed = EncryptProvider.Md5(param);
            var SHA1hashed = EncryptProvider.Sha1(MD5hashed.ToLower()).ToLower();
            return SHA1hashed;

        }




        public async Task<T> SnapApiPost<T, U>(string actionUrl, U requestBody) where T : new()
        {
            string contentResult = "";
            var returnValue = new T();

            try
            {
                using (var client = new HttpClient())
                {
                    //Base UR: of web api
                    client.BaseAddress = new Uri(ApiUrl());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                                                                "Basic",
                                                                Convert.ToBase64String(
                                                                    System.Text.ASCIIEncoding.ASCII.GetBytes(
                                                                        string.Format("{0}:{1}", MidtranServerKey(), ""))));

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    //action to web api
                    string json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var Url = client.BaseAddress + actionUrl;
                    //var Url = "https://app.midtrans.com/snap/v1/transactions";
                    var response = await client.PostAsync(Url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        contentResult = await response.Content.ReadAsStringAsync();
                        returnValue = JsonConvert.DeserializeObject<T>(contentResult);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertLog(0, "SnapApiPost", ex.Message);
            }

            return returnValue;
        }


        public async Task<T> FasPayApiPost<T, U>(U requestBody, string UrlPost) where T : new()
        {
            string contentResult = "";
            var returnValue = new T();

            try
            {
                using (var client = new HttpClient())
                {
                    //Base UR: of web api
                    client.BaseAddress = new Uri(UrlPost);
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    //                                            "Basic",
                    //                                            Convert.ToBase64String(
                    //                                                System.Text.ASCIIEncoding.ASCII.GetBytes(
                    //                                                    string.Format("{0}:{1}", MidtranServerKey(), "")))); ;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    //action to web api
                    string json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    //var Url = client.BaseAddress + actionUrl;
                    var response = await client.PostAsync(UrlPost, content);

                    if (response.IsSuccessStatusCode)
                    {
                        contentResult = await response.Content.ReadAsStringAsync();
                        returnValue = JsonConvert.DeserializeObject<T>(contentResult);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertLog(0, "FasPayApiPost", ex.Message);
            }

            return returnValue;
        }



        public async Task<TransactionResultModel> GetTransactionStatus(string TransactionId)

        {
            string contentResult = "";
            var returnValue = new TransactionResultModel();

            try
            {
                using (var client = new HttpClient())
                {
                    //Base UR: of web api
                    client.BaseAddress = new Uri(ApiUrlV2());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                                                                "Basic",
                                                                Convert.ToBase64String(
                                                                    System.Text.ASCIIEncoding.ASCII.GetBytes(
                                                                        string.Format("{0}:{1}", MidtranServerKey(), "")))); ;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    //action to web api
                    //string json = JsonConvert.SerializeObject(model);
                    //var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var Url = client.BaseAddress + TransactionId + "/status";
                    //var response = await client.PostAsync(Url, content);
                    var response = await client.GetAsync(Url);

                    if (response.IsSuccessStatusCode)
                    {
                        contentResult = await response.Content.ReadAsStringAsync();
                        returnValue = JsonConvert.DeserializeObject<TransactionResultModel>(contentResult);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertLog(0, "GetTransactionStatus", ex.Message);

            }
            return returnValue;

        }





        public async Task<bool> VerifyPaymentNotificationOrder(TransactionResultModel model)

        {
            string contentResult = "";
            var returnValue = new TransactionResultModel();

            try
            {
                using (var client = new HttpClient())
                {
                    //Base UR: of web api
                    client.BaseAddress = new Uri(ApiUrlV2());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                                                                "Basic",
                                                                Convert.ToBase64String(
                                                                    System.Text.ASCIIEncoding.ASCII.GetBytes(
                                                                        string.Format("{0}:{1}", MidtranServerKey(), "")))); ;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    //action to web api
                    string json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var Url = client.BaseAddress + model.transaction_id + "/status";
                    //var response = await client.PostAsync(Url, content);
                    var response = await client.GetAsync(Url);

                    if (response.IsSuccessStatusCode)
                    {
                        contentResult = await response.Content.ReadAsStringAsync();
                        returnValue = JsonConvert.DeserializeObject<TransactionResultModel>(contentResult);

                        if (returnValue != null)
                        {
                            if (returnValue.status_code == "404")
                            {
                                return false;
                            }
                            else
                            {
                                if (returnValue.signature_key == model.signature_key)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }



                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {
                        return false;
                    }
                }
            }

            catch (Exception ex)
            {
                InsertLog(0, "VerifyPaymentNotificationOrder", ex.Message);
                return false;
            }

        }

        public async Task<bool> EmailNewUserSignUp(string sendto)
        {
            try
            {
                var Template = IMasterManager.GetAllEmailNotification().Where(x => x.TemplateNm == "U-UserSignup").FirstOrDefault();
                var EmailTemplate = Template.TemplateValue;
                var Company = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "Company" && x.ParamCode == "Company").FirstOrDefault();
                var EmailAddress = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "EmailAddress" && x.ParamCode == "EmailAddress").FirstOrDefault();
                var AppName = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "AppName" && x.ParamCode == "AppName").FirstOrDefault();

                EmailTemplate = EmailTemplate.Replace("(CURRENTYEAR)", DateTime.Now.Year.ToString());
                EmailTemplate = EmailTemplate.Replace("(EMAILADDRESS)", EmailAddress.ParamValue);
                EmailTemplate = EmailTemplate.Replace("(COMPANY)", Company.ParamValue);               
                EmailTemplate = EmailTemplate.Replace("|MC:SUBJECT|", AppName.ParamValue);

                var client = new SendGridClient(SendGridApiKey());
                var from = new EmailAddress("noreply@fameoapp.com", "FameoNotification");
                var To = new EmailAddress(sendto, "");
                var msg = MailHelper.CreateSingleEmail(from, To, "Fameo - You're All Signed Up.", EmailTemplate, EmailTemplate);
                var response = await client.SendEmailAsync(msg);
                return true;
            }
            catch (Exception ex)
            {
                InsertLog(0, "EmailNewUserSignUp", ex.Message);
                return false;

            }

        }


        public async Task<bool> EmailSupport(string Subject, string Message,string UserId, string EmailTo)
        {
            try
            {
                var Template = IMasterManager.GetAllEmailNotification().Where(x => x.TemplateNm == "EmailSupport").FirstOrDefault();
                var EmailTemplate = Template.TemplateValue;
                var Company = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "Company" && x.ParamCode == "Company").FirstOrDefault();
                var EmailAddress = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "EmailAddress" && x.ParamCode == "EmailAddress").FirstOrDefault();
                var AppName = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "AppName" && x.ParamCode == "AppName").FirstOrDefault();

                UserModel usm = new UserModel();
                usm.Id = Convert.ToInt32(UserId);
                usm = IUserManagementManager.GetUser(usm);               
                var UserName = usm != null ? usm.UserName:"";              
                EmailTemplate = EmailTemplate.Replace("(CURRENTYEAR)", DateTime.Now.Year.ToString());
                EmailTemplate = EmailTemplate.Replace("(EMAILADDRESS)", EmailAddress.ParamValue);
                EmailTemplate = EmailTemplate.Replace("(COMPANY)", Company.ParamValue);
                EmailTemplate = EmailTemplate.Replace("[user]", UserName);
                EmailTemplate = EmailTemplate.Replace("[body]", Message);
                EmailTemplate = EmailTemplate.Replace("[EmailTo]", EmailTo);

                var client = new SendGridClient(SendGridApiKey());
                var from = new EmailAddress("noreply@fameoapp.com", "FameoNotification");
                 var To = new EmailAddress("hello@fameoapp.com", "");
               // var To = new EmailAddress("dendidul@gmail.com", "");
                var msg = MailHelper.CreateSingleEmail(from, To, "FameoSupport - "+ Subject, EmailTemplate, EmailTemplate);
                var response = await client.SendEmailAsync(msg);
                return true;
            }
            catch (Exception ex)
            {
                InsertLog(0, "EmailSupport", ex.Message);
                return false;

            }

        }


        public async Task<bool> EmailClaimRequest(string UserId,int Status,string ClaimNo, decimal Amount)
        {
            try
            {
                var Template = IMasterManager.GetAllEmailNotification().Where(x => x.TemplateNm == "T-WithDrawProgress").FirstOrDefault();
                if (Status == (int)Registration.Submit)
                {
                    Template = IMasterManager.GetAllEmailNotification().Where(x => x.TemplateNm == "T-WithDrawRequest").FirstOrDefault();
                }
                else if(Status == (int)Registration.Completed)
                {
                    Template = IMasterManager.GetAllEmailNotification().Where(x => x.TemplateNm == "T-WithDrawBankDone").FirstOrDefault();
                }
                   

              
                var EmailTemplate = Template.TemplateValue;
                var Company = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "Company" && x.ParamCode == "Company").FirstOrDefault();
                var EmailAddress = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "EmailAddress" && x.ParamCode == "EmailAddress").FirstOrDefault();
                var AppName = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "AppName" && x.ParamCode == "AppName").FirstOrDefault();
                var ClaimStatus = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "EmClaimStat" && x.ParamCode == Status.ToString()).FirstOrDefault(); ;
                
                UserModel usm = new UserModel();
                usm.Id = Convert.ToInt32(UserId);
                usm = IUserManagementManager.GetUser(usm);
                var TalentName = usm != null ? usm.Name : "";
                var UserEmail =  usm != null ? usm.Email : "";
                var StatusName = ClaimStatus != null ? ClaimStatus.ParamValue : "";


                EmailTemplate = EmailTemplate.Replace("(CURRENTYEAR)", DateTime.Now.Year.ToString());
                EmailTemplate = EmailTemplate.Replace("(EMAILADDRESS)", EmailAddress.ParamValue);
                EmailTemplate = EmailTemplate.Replace("(COMPANY)", Company.ParamValue);
                EmailTemplate = EmailTemplate.Replace("[TalentName]", TalentName);
                EmailTemplate = EmailTemplate.Replace("[ClaimNo]", ClaimNo);
                EmailTemplate = EmailTemplate.Replace("[Amount]", "Rp." +  Amount.ToString("#,##0"));
                EmailTemplate = EmailTemplate.Replace("(VALUEAMOUNT)", "Rp." + Amount.ToString("#,##0"));
                EmailTemplate = EmailTemplate.Replace("[ClaimStatus]", StatusName);
                EmailTemplate = EmailTemplate.Replace("(DATETIME)", DateTime.Now.ToString("dddd, dd MMMM yyyy"));
               

                var client = new SendGridClient(SendGridApiKey());
                var from = new EmailAddress("noreply@fameoapp.com", "FameoNotification");
                 var To = new EmailAddress(UserEmail, "");
               //  var To = new EmailAddress("dendidul@gmail.com", "");
                var msg = MailHelper.CreateSingleEmail(from, To, "Fameo - " + ClaimNo, EmailTemplate, EmailTemplate);
                var response = await client.SendEmailAsync(msg);
                return true;
            }
            catch (Exception ex)
            {
                InsertLog(0, "EmailSupport", ex.Message);
                return false;

            }

        }



        public async Task<bool> EmailCompletedTransfer(string sendto, string Type, string Name, string Number)
        {
            try
            {
                var Template = IMasterManager.GetEmailNotification(2);
                var EmailTemplate = Template.TemplateValue;
                var Checkdata = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == Type && x.ParamName == "NotificationComplete").FirstOrDefault();
                var Statusmessage = Checkdata != null ? Checkdata.ParamValue : "";
                EmailTemplate = EmailTemplate.Replace("@status", Statusmessage);

                EmailTemplate = EmailTemplate.Replace("@Message", "Hei " + Name + " " + Statusmessage);

                var client = new SendGridClient(SendGridApiKey());
                var from = new EmailAddress("noreply@fameoapp.com", "FameoNotification");
                var To = new EmailAddress(sendto, "");
                var msg = MailHelper.CreateSingleEmail(from, To, Number + " - " + Statusmessage, EmailTemplate, EmailTemplate);
                var response = await client.SendEmailAsync(msg);
                return true;
            }
            catch (Exception ex)
            {
                InsertLog(0, "EmailApprovalTalent", ex.Message);
                return false;

            }

        }


        public async Task<bool> EmailApprovalTalent(string sendto, string StatusCode, string Name, string Number, string Note)
        {
            try
            {
                var Template = IMasterManager.GetEmailNotification(2);
                var EmailTemplate = Template.TemplateValue;
                var Checkdata = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "MsgApprovalTalent").FirstOrDefault();
                var CheckdataBody = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "MsgApprovalTalentBody").FirstOrDefault();
                var CheckdataNotes = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "MsgApprovalTalentNotes").FirstOrDefault();
                var Statusmessage = Checkdata != null ? Checkdata.ParamValue : "";
                var StatusmessageBody = CheckdataBody != null ? CheckdataBody.ParamValue : "";
                var StatusmessageNotes = CheckdataNotes != null ? CheckdataNotes.ParamValue : "";
                EmailTemplate = EmailTemplate.Replace("@status", Statusmessage);
                if (string.IsNullOrEmpty(Note))
                {
                    Note = StatusmessageNotes;
                }
                if (StatusCode == "-1")
                {
                    EmailTemplate = EmailTemplate.Replace("@Message", "Hi " + Name + ", " + StatusmessageBody + " <br/> <p> Note : </p> <p>" + Note + "</p>");
                }
                else if (StatusCode == "1")
                {
                    EmailTemplate = EmailTemplate.Replace("@Message", "Hi " + Name + ", " + StatusmessageBody);
                }
                else
                {
                    EmailTemplate = EmailTemplate.Replace("@Message", "Hi " + Name + " " + StatusmessageBody);

                }

                //if (EmailType == EmailTargetType.Talent)
                //{
                //    EmailTemplate = EmailTemplate.Replace("@linkHalaman", JinglUrl() + "\\Account\\BookingDetail?bookId=" + BookId.ToString());

                //}
                //else
                //{
                //    EmailTemplate = EmailTemplate.Replace("@linkHalaman", JinglUrl() + "\\Booking\\BookingHistory?bookId=" + BookId.ToString());
                //}


                var client = new SendGridClient(SendGridApiKey());
                var from = new EmailAddress("noreply@fameoapp.com", "FameoNotification");
                var To = new EmailAddress(sendto, "");
                var msg = MailHelper.CreateSingleEmail(from, To, Number + " - " + Statusmessage, EmailTemplate, EmailTemplate);
                var response = await client.SendEmailAsync(msg);
                return true;
            }
            catch (Exception ex)
            {
                InsertLog(0, "EmailApprovalTalent", ex.Message);
                return false;

            }

        }

        public async Task<bool> NotificationEmail(string sendto, string StatusCode, EmailTargetType EmailType, int BookId, string OrderId, string FirstName)
        {
            try
            {
                BookModel Bmodel = new BookModel();
                Bmodel.Id = BookId;
                var getBookData = ITransactionManager.GetDataBook(Bmodel);
                Bmodel = getBookData;

                var Template = IMasterManager.GetEmailNotification(1);
                var EmailTemplate = Template.TemplateValue;

                var Company = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "Company" && x.ParamCode == "Company").FirstOrDefault();
                var EmailAddress = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "EmailAddress" && x.ParamCode == "EmailAddress").FirstOrDefault();
                var AppName = IMasterManager.AdmGetAllParameter().Where(x => x.ParamName == "AppName" && x.ParamCode == "AppName").FirstOrDefault();


                var CheckSubjdata = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "UOrdSubjEmail").FirstOrDefault();
                var Subj = CheckSubjdata != null ? CheckSubjdata.ParamValue : "";

                var CheckMsgdata = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "UOrdMsgEmail").FirstOrDefault();
                var Msg = CheckMsgdata != null ? CheckMsgdata.ParamValue : "";



                if (EmailType == EmailTargetType.Talent)
                {
                    CheckSubjdata = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "TOrdSubjEmail").FirstOrDefault();
                    Subj = CheckSubjdata != null ? CheckSubjdata.ParamValue : "";

                    CheckMsgdata = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "TOrdMsgEmail").FirstOrDefault();
                    Msg = CheckMsgdata != null ? CheckMsgdata.ParamValue : "";

                    if (Bmodel != null)
                    {
                        if (Bmodel.Status == (int)BookingFlow.ProjectAccepted)
                        {
                            Template = IMasterManager.GetAllEmailNotification().Where(x => x.TemplateNm == "T-OrdNewOrder").FirstOrDefault();
                            EmailTemplate = Template.TemplateValue;

                            EmailTemplate = EmailTemplate.Replace("(CURRENTYEAR)", DateTime.Now.Year.ToString());
                            EmailTemplate = EmailTemplate.Replace("(EMAILADDRESS)", EmailAddress.ParamValue);
                            EmailTemplate = EmailTemplate.Replace("(COMPANY)", Company.ParamValue);
                            EmailTemplate = EmailTemplate.Replace("(talent_name)", Bmodel != null ? Bmodel.TalentNm : "");
                            EmailTemplate = EmailTemplate.Replace("(customer_name)", FirstName);
                            EmailTemplate = EmailTemplate.Replace("|MC:SUBJECT|", AppName.ParamValue);

                        }
                        else if (Bmodel.Status == (int)BookingFlow.ProjectCompleted)
                        {
                            Template = IMasterManager.GetAllEmailNotification().Where(x => x.TemplateNm == "T-OrdVideoSent").FirstOrDefault();
                            EmailTemplate = Template.TemplateValue;

                            EmailTemplate = EmailTemplate.Replace("(CURRENTYEAR)", DateTime.Now.Year.ToString());
                            EmailTemplate = EmailTemplate.Replace("(EMAILADDRESS)", EmailAddress.ParamValue);
                            EmailTemplate = EmailTemplate.Replace("(COMPANY)", Company.ParamValue);
                            EmailTemplate = EmailTemplate.Replace("(talent_name)", Bmodel != null ? Bmodel.TalentNm : "");
                            EmailTemplate = EmailTemplate.Replace("(customer_name)", FirstName);
                            EmailTemplate = EmailTemplate.Replace("|MC:SUBJECT|", AppName.ParamValue);
                        }
                        else if(Bmodel.Status == (int)BookingFlow.ReactionVideo)
                        {
                            Template = IMasterManager.GetAllEmailNotification().Where(x => x.TemplateNm == "U-OrdReactionVideo").FirstOrDefault();
                            EmailTemplate = Template.TemplateValue;

                            EmailTemplate = EmailTemplate.Replace("(CURRENTYEAR)", DateTime.Now.Year.ToString());
                            EmailTemplate = EmailTemplate.Replace("(EMAILADDRESS)", EmailAddress.ParamValue);
                            EmailTemplate = EmailTemplate.Replace("(COMPANY)", Company.ParamValue);
                            EmailTemplate = EmailTemplate.Replace("(talent_name)", Bmodel != null ? Bmodel.TalentNm : "");
                            EmailTemplate = EmailTemplate.Replace("(customer_name)", FirstName);
                            EmailTemplate = EmailTemplate.Replace("|MC:SUBJECT|", AppName.ParamValue);
                        }

                        else
                        {
                            EmailTemplate = EmailTemplate.Replace("@FirstName", FirstName);
                            EmailTemplate = EmailTemplate.Replace("@status", Subj);
                            EmailTemplate = EmailTemplate.Replace("@Message", Msg);
                            EmailTemplate = EmailTemplate.Replace("@linkHalaman", JinglUrl() + "\\Account\\BookingDetail?bookId=" + BookId.ToString() + "&IsEmail=1");
                        }
                    }
                    else
                    {
                        EmailTemplate = EmailTemplate.Replace("@FirstName", FirstName);
                        EmailTemplate = EmailTemplate.Replace("@status", Subj);
                        EmailTemplate = EmailTemplate.Replace("@Message", Msg);
                        EmailTemplate = EmailTemplate.Replace("@linkHalaman", JinglUrl() + "\\Account\\BookingDetail?bookId=" + BookId.ToString() + "&IsEmail=1");
                    }

                }
                else
                {
                    if (Bmodel != null)
                    {
                        if (Bmodel.Status == (int)BookingFlow.ProjectAccepted)
                        {
                            Template = IMasterManager.GetAllEmailNotification().Where(x => x.TemplateNm == "U-OrdConfirm").FirstOrDefault();
                            EmailTemplate = Template.TemplateValue;
                            EmailTemplate = EmailTemplate.Replace("(CURRENTYEAR)", DateTime.Now.Year.ToString());
                            EmailTemplate = EmailTemplate.Replace("(EMAILADDRESS)", EmailAddress.ParamValue);
                            EmailTemplate = EmailTemplate.Replace("(COMPANY)", Company.ParamValue);
                            EmailTemplate = EmailTemplate.Replace("(talent_name)", Bmodel != null ? Bmodel.TalentNm : "");
                            EmailTemplate = EmailTemplate.Replace("(customer_name)", FirstName);
                            EmailTemplate = EmailTemplate.Replace("|MC:SUBJECT|", AppName.ParamValue);
                        }
                        else if (Bmodel.Status == (int)BookingFlow.MaterialAccepted)
                        {
                            Template = IMasterManager.GetAllEmailNotification().Where(x => x.TemplateNm == "U-OrdVideoSent").FirstOrDefault();
                            EmailTemplate = Template.TemplateValue;


                            EmailTemplate = EmailTemplate.Replace("(CURRENTYEAR)", DateTime.Now.Year.ToString());
                            EmailTemplate = EmailTemplate.Replace("(EMAILADDRESS)", EmailAddress.ParamValue);
                            EmailTemplate = EmailTemplate.Replace("(COMPANY)", Company.ParamValue);
                            EmailTemplate = EmailTemplate.Replace("(talent_name)", Bmodel != null ? Bmodel.TalentNm : "");
                            EmailTemplate = EmailTemplate.Replace("(customer_name)", FirstName);
                            EmailTemplate = EmailTemplate.Replace("|MC:SUBJECT|", AppName.ParamValue);
                        }

                        else
                        {
                            EmailTemplate = EmailTemplate.Replace("@FirstName", FirstName);
                            EmailTemplate = EmailTemplate.Replace("@status", Subj);
                            EmailTemplate = EmailTemplate.Replace("@Message", Msg);
                            EmailTemplate = EmailTemplate.Replace("@linkHalaman", JinglUrl() + "\\Account\\BookingDetail?bookId=" + BookId.ToString() + "&IsEmail=1");
                        }
                    }
                    else
                    {
                        EmailTemplate = EmailTemplate.Replace("@FirstName", FirstName);
                        EmailTemplate = EmailTemplate.Replace("@status", Subj);
                        EmailTemplate = EmailTemplate.Replace("@Message", Msg);
                        EmailTemplate = EmailTemplate.Replace("@linkHalaman", JinglUrl() + "\\Booking\\CheckBookingData?bookId=" + BookId.ToString() + "&IsEmail=1");
                    }
                }


                var client = new SendGridClient(SendGridApiKey());
                var from = new EmailAddress("NoReply@Fameo.com", "FameoNotification");
                var To = new EmailAddress(sendto, "");
              //var To = new EmailAddress("dendidul@gmail.com", "");
                var msg = MailHelper.CreateSingleEmail(from, To, OrderId + " - " + Subj, EmailTemplate, EmailTemplate);
                var response = await client.SendEmailAsync(msg);
                return true;
            }
            catch (Exception ex)
            {
                InsertLog(0, "NotificationEmail", ex.Message);
                return false;

            }


        }


        public async Task<bool> EmailApprovalTopup(string sendto, string StatusCode, string Name, string Note, decimal TopupAmt = 0)
        {
            try
            {
                var Template = IMasterManager.GetEmailNotification(2);
                var EmailTemplate = Template.TemplateValue;
                var Checkdata = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "TopupStat").FirstOrDefault();
                var CheckdataBody = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "MsgApvTopupBody").FirstOrDefault();
                var CheckdataNotes = IMasterManager.AdmGetAllParameter().Where(x => x.ParamCode == StatusCode && x.ParamName == "MsgApvTopupNotes").FirstOrDefault();
                var Statusmessage = Checkdata != null ? Checkdata.ParamValue : "";
                var StatusmessageBody = CheckdataBody != null ? CheckdataBody.ParamValue : "";
                var StatusmessageNotes = CheckdataNotes != null ? CheckdataNotes.ParamValue : "";
                EmailTemplate = EmailTemplate.Replace("@status", Statusmessage);
                if (string.IsNullOrEmpty(Note))
                {
                    Note = StatusmessageNotes;
                }

                if (StatusCode == "-1")
                {
                    EmailTemplate = EmailTemplate.Replace("@Message", "Hi " + Name + ", " + StatusmessageBody + " <br/> <p> Note : </p> <p>" + Note + "</p>");
                }
                else if (StatusCode == "1")
                {
                    EmailTemplate = EmailTemplate.Replace("@Message", "Hi " + Name + ", " + StatusmessageBody);
                }
                else
                {
                    if (TopupAmt > 0)
                    {
                        StatusmessageBody = StatusmessageBody.Replace("@SaldoAmt", string.Format(new System.Globalization.CultureInfo("id-ID"), "{0:C}", TopupAmt));
                    }
                    else
                    {
                        StatusmessageBody = StatusmessageBody.Replace("@SaldoAmt", "");

                    }
                    EmailTemplate = EmailTemplate.Replace("@Message", "Hi " + Name + " " + StatusmessageBody);

                }


                var client = new SendGridClient(SendGridApiKey());
                var from = new EmailAddress("noreply@fameoapp.com", "FameoNotification");
                var To = new EmailAddress(sendto, "");
                var msg = MailHelper.CreateSingleEmail(from, To, "Topup - " + Statusmessage, EmailTemplate, EmailTemplate);
                var response = await client.SendEmailAsync(msg);
                return true;
            }
            catch (Exception ex)
            {
                InsertLog(0, "EmailApprovalTopup", ex.Message);
                return false;

            }

        }


    }
}