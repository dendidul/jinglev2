using System;
using WebPush;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
//using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Azure.WebJobs;

namespace Jingl.PushNotification
{
    class Program
    {
        public IDbConnection Connection
        {
            get
            {
                //return new SqlConnection("Server=tcp:sophielastic.database.windows.net,1433;Initial Catalog=JINGL_STG;Persist Security Info=False;User ID=sophielastic;Password=SophieHappy33;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");
                //return new SqlConnection("Server=tcp:jinglprod.database.windows.net,1433;Initial Catalog=JINGPROD;Persist Security Info=False;User ID=jinglprod;Password=SophieHappy33;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");
                //return new SqlConnection("Server=tcp:jingldbserver.database.windows.net,1433;Initial Catalog=Fameoprod;Persist Security Info=False;User ID=jingldbserversa;Password=jingldbserver1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");
                return new SqlConnection("Server=tcp:fameodb.database.windows.net,1433;Initial Catalog=Fameoprd;Persist Security Info=False;User ID=fameodb;Password=azurefameo1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=3600;");


            }
        }

        public static void Main()
        {
            //Program Program = new Program();
            //TimerCallback callback = new TimerCallback(PushNotification);

            //Timer stateTimer = new Timer(callback, null, 0, 6000);

            //// loop here forever
            //for (; ; )
            //{
            //    // add a sleep for 100 mSec to reduce CPU usage
            //    Thread.Sleep(100);
            //}

            // var host = new JobHost();
            //// host.CallAsync(typeof(Program).GetMethod("ProcessMethod"));
            // // The following code ensures that the WebJob will be running continuously
            // host.StartAsync();
            ProcessMethod().Wait();
           // ProcessCancelBooking().Wait();


        }

        [NoAutomaticTriggerAttribute]
        public static async Task ProcessMethod()
        {
            while (true)
            {
                try
                {
                  
                    Program Program = new Program();
                    Program.PushNotification();
                    
                  //  Program.CancelledBooking();
                    // log.WriteLine("..");
                    //Console.Write("..");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occurred in processing pending requests. Error : {0}", ex.Message); ;
                    //log.WriteLine("Error occurred in processing pending altapay requests. Error : {0}", ex.Message);
                }
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }

       


        public void PushNotification()

        {
            Program Program = new Program();
          

            var ListData = new List<NotificationMessageModel>();
            var TalentNotification = Program.GetNotificationTalent();
            var UserNotification = Program.GetNotificationUser();
            if (TalentNotification.Count > 0 || UserNotification.Count > 0)
            {
                foreach (var data in TalentNotification)
                {
                    NotificationMessageModel model = new NotificationMessageModel();
                    model.To = data.To;
                    model.Id = data.Id;
                    model.bookId = data.bookId;
                    model.Message = data.Message;
                    model.PushAuth = data.PushAuth;
                    model.PushP256DH = data.PushP256DH;
                    model.PushEndpoint = data.PushEndpoint;
                    model.url = "https://www.fameoapp.com/Booking/BookingDetail?bookId=" + model.bookId + "&notificationId=" + model.Id;
                   // model.url = "https://localhost:44374/Booking/BookingDetail?bookId=" + model.bookId + "&notificationId=" + model.Id;
                    ListData.Add(model);
                }

                foreach (var data in UserNotification)
                {
                    NotificationMessageModel model = new NotificationMessageModel();
                    model.To = data.To;
                    model.bookId = data.bookId;
                    model.Id = data.Id;
                    model.Message = data.Message;
                    model.PushAuth = data.PushAuth;
                    model.PushP256DH = data.PushP256DH;
                    model.PushEndpoint = data.PushEndpoint;
                    model.url = "https://www.fameoapp.com/Booking/CheckBookingData?bookId=" + model.bookId+"&notificationId="+model.Id;
                   // model.url = "https://localhost:44374/Booking/CheckBookingData?bookId=" + model.bookId + "&notificationId=" + model.Id;
                    ListData.Add(model);
                }

                foreach (var data in ListData)
                {
                    //var endpoint = data.PushEndpoint;
                    //var pre = data.PushP256DH;
                    //var auth = data.PushAuth;

                    //var pushSubscription = new PushSubscription(endpoint, pre, auth);
                    //var vapidDetails = new VapidDetails("mailto:example@example.com", vapidPublicKey, vapidPrivateKey);

                    //var webPushClient = new WebPushClient();
                    //PayLoad load = new PayLoad();
                    //load.title = "Fameo Notification";
                    //load.message = data.Message;
                    //load.url = data.url;
                    //var lala = JsonConvert.SerializeObject(load);
                    //webPushClient.SendNotification(pushSubscription, lala, vapidDetails);

                    var message = SendNotification(data);

                    if(message == "Success")
                    {
                        Program.UpdateIsSent(data.Id);
                    }
                    else
                    {
                        Program.UpdateIsSent(data.Id);
                    }

                }
              //  Program.UpdateIsSent();
                Console.WriteLine("... Sending Notification..");
            }
            else
            {
                Console.Write(".");
            }




        }


        public string SendNotification(NotificationMessageModel data)
        {
            string message = "";
            try
            {
                string vapidPublicKey = "BAVga6_CDUnXjegxtgYKT-kgviHOUGQc877x-8c1usw_QoFt2jk_zyI68elT6Yn1kTM9kFe7FiMWbyyIwUiCP84";
                string vapidPrivateKey = "8j786rNbCZuNXxsi7f_s9Mm1kjxEKQ3uXv0Qn_GP5vQ";

                var endpoint = data.PushEndpoint;
                var pre = data.PushP256DH;
                var auth = data.PushAuth;

                var pushSubscription = new PushSubscription(endpoint, pre, auth);
                var vapidDetails = new VapidDetails("mailto:example@example.com", vapidPublicKey, vapidPrivateKey);

                var webPushClient = new WebPushClient();
                PayLoad load = new PayLoad();
                load.title = "Fameo Notification";
                load.message = data.Message;
                load.url = data.url;
                var jsonobject = JsonConvert.SerializeObject(load);
                webPushClient.SendNotification(pushSubscription, jsonobject, vapidDetails);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Error";
               // 
                return message;
              //  throw ex;
            }

            return message;
        }



        //static public void PushNotification(Object stateInfo)        

        //{
        //    Program Program = new Program();
        //    string vapidPublicKey = "BAVga6_CDUnXjegxtgYKT-kgviHOUGQc877x-8c1usw_QoFt2jk_zyI68elT6Yn1kTM9kFe7FiMWbyyIwUiCP84";
        //    string vapidPrivateKey = "8j786rNbCZuNXxsi7f_s9Mm1kjxEKQ3uXv0Qn_GP5vQ";

        //    var ListData = new List<NotificationMessageModel>();
        //    var TalentNotification = Program.GetNotificationTalent();
        //    var UserNotification = Program.GetNotificationUser();
        //    if(TalentNotification.Count > 0 || UserNotification.Count > 0)
        //    {
        //        foreach (var data in TalentNotification)
        //        {
        //            NotificationMessageModel model = new NotificationMessageModel();
        //            model.To = data.To;
        //            model.Message = data.Message;
        //            model.PushAuth = data.PushAuth;
        //            model.PushP256DH = data.PushP256DH;
        //            model.PushEndpoint = data.PushEndpoint;
        //            ListData.Add(model);
        //        }

        //        foreach (var data in UserNotification)
        //        {
        //            NotificationMessageModel model = new NotificationMessageModel();
        //            model.To = data.To;
        //            model.Message = data.Message;
        //            model.PushAuth = data.PushAuth;
        //            model.PushP256DH = data.PushP256DH;
        //            model.PushEndpoint = data.PushEndpoint;
        //            ListData.Add(model);
        //        }

        //        foreach (var data in ListData)
        //        {
        //            var endpoint = data.PushEndpoint;
        //            var pre = data.PushP256DH;
        //            var auth = data.PushAuth;

        //            var pushSubscription = new PushSubscription(endpoint, pre, auth);
        //            var vapidDetails = new VapidDetails("mailto:example@example.com", vapidPublicKey, vapidPrivateKey);

        //            var webPushClient = new WebPushClient();
        //            PayLoad load = new PayLoad();
        //            load.title = "Jingl Notification";
        //            load.message = data.Message;
        //            var lala = JsonConvert.SerializeObject(load);
        //            webPushClient.SendNotification(pushSubscription, lala, vapidDetails);
        //        }
        //        Program.UpdateIsSent();
        //        Console.WriteLine("... Sending Notification..");
        //    }
        //    else
        //    {
        //        Console.Write(".");
        //    }




        //}


        public IList<NotificationMessageModel> GetNotificationUser()
        {
            var ListNotification = new List<NotificationMessageModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                ListNotification = conn.Query<NotificationMessageModel>("SP_GetNotificationUserNotSend", param,
                           commandType: CommandType.StoredProcedure).ToList();



            }

            return ListNotification;
        }

        public void UpdateIsSent(int id)
        {
           
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();

                conn.Execute("update Tbl_Trx_Notification set IsSent = 1 where id = " + id);



            }

         
        }


       
        public IList<NotificationMessageModel> GetNotificationTalent()
        {
            var ListNotification = new List<NotificationMessageModel>();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();



                ListNotification = conn.Query<NotificationMessageModel>("SP_GetNotificationTalentNotSend", param,
                commandType: CommandType.StoredProcedure).ToList();



            }

            return ListNotification;
        }

       

      

        public class NotificationMessageModel
        {
            public int Id { get; set; }
            public int To { get; set; }
            public string Message { get; set; }
            public string PushEndpoint { get; set; }
            public string PushAuth { get; set; }
            public string PushP256DH { get; set; }
            public string url { get; set; }
            public string bookId { get; set; }
        }

       

        public class PayLoad
        {
            public string title { get; set; }
            public string message { get; set; }
            public string url { get; set; }
        }
    }
}
