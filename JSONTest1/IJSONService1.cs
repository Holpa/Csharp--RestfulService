using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.IO;

//include
using System.Web.Script.Serialization;
using System.Net;
//1 WCF
namespace JSONTest1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IJSONService1" in both code and config file together.
    [ServiceContract]
    public interface IJSONService1
    {
        [OperationContract]

        //[Webinvoker]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "login?username={username}&password={password}")]
        LoginResponse login(string username, string password);

        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, UriTemplate = "logout?token={token}")]
        LogoutResponse logout(string token);

        // the RESPONSE WILL BE JSON AUTOMATICALLY !  JSON Request
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped,
           Method = "POST",
           UriTemplate = "VehicleInventoryReport?token={token}")]
        VehicleInventoryReportResponse VehicleInventoryReport(string token);



        // the convention !
   //     [WebInvoke(ResponseFormat = WebMessageFormat.Json,
   //        RequestFormat = WebMessageFormat.Json,
   //        BodyStyle = WebMessageBodyStyle.WrappedRequest,
   //        Method = "POST",
   //        UriTemplate = "api/smtp?token=")]
   //     VehicleInventoryReportResponse smtp(Stream stream);

   //     [WebInvoke(ResponseFormat = WebMessageFormat.Json,
   //RequestFormat = WebMessageFormat.Json,
   //BodyStyle = WebMessageBodyStyle.WrappedRequest,
   //Method = "POST",
   //UriTemplate = "smtp?token=")]
   //     VehicleInventoryReportResponse smtp2(Stream stream);



    }

    //2, JSON classes
    // CLASSES needed to parse JSON
    // Check http://json2csharp.com/ to make Json Classes

    [DataContract(Name = "RFID")]
    public class RFID
    {
        [DataMember(Name = "code")]
        public string code { get; set; }

        [DataMember(Name = "user")]
        public string user { get; set; }

        [DataMember(Name = "status")]
        public string status { get; set; }

        [DataMember(Name = "address")]
        public string address { get; set; }

        [DataMember(Name = "permission")]
        public string permission { get; set; }

        [DataMember(Name = "port")]
        public string port { get; set; }

        [DataMember(Name = "timestamp")]
        public string timestamp { get; set; }

    }




    [DataContract(Name = "Inventory")]
    public class Inventory
    {
        [DataMember(Name = "tags")]
        public string[] tags;
        [DataMember(Name = "tag")]
        public string tag { get; set; }
    }

    [DataContract(Name = "TAG")]
    public class TAG
    {
        [DataMember(Name = "token")]
        public string token { get; set; }
        [DataMember(Name = "vehicle")]
        public string vehicle { get; set; }

        [DataMember(Name = "report")]
        public string report { get; set; }

        [DataMember(Name = "gps")]
        public string gps { get; set; }

        [DataMember(Name = "DOP")]
        public string DOP { get; set; }
        //INVENTORY WILL BE NESTED
        [DataMember(Name = "inventory")]
        public Inventory inventory { get; set; }
    }

    [DataContract(Name = "LoginResponse")]
    public class LoginResponse
    {
        [DataMember(Name = "code")]
        public string code { get; set; }

        [DataMember(Name = "status")]
        public string status { get; set; }

        [DataMember(Name = "result")]
        public Result result { get; set; }
    }

    [DataContract(Name = "VehicleInventoryReportResponse")]
    public class VehicleInventoryReportResponse
    {
        [DataMember(Name = "code")]
        public string code { get; set; }

        [DataMember(Name = "status")]
        public string status { get; set; }
    }
    [DataContract(Name = "LogoutResponse")]
    public class LogoutResponse
    {
        [DataMember(Name = "code")]
        public string code { get; set; }

        [DataMember(Name = "result")]
        public Result result { get; set; }

        [DataMember(Name = "status")]
        public string status { get; set; }
    }

    [DataContract(Name = "result")]
    public class Result
    {
        [DataMember(Name = "address")]
        public string address { get; set; }
        [DataMember(Name = "permission")]
        public string permission { get; set; }
        [DataMember(Name = "port")]
        public string port { get; set; }
        [DataMember(Name = "timestamp")]
        public string timestamp { get; set; }
        [DataMember(Name = "token")]
        public string token { get; set; }
        [DataMember(Name = "user")]
        public string user { get; set; }

    }







}
