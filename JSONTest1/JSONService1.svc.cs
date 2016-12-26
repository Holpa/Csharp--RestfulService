using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using Newtonsoft.Json;
//include
using System.Web.Script.Serialization;
using System.Xml;
using System.Collections;

namespace JSONTest1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "JSONService1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select JSONService1.svc or JSONService1.svc.cs at the Solution Explorer and start debugging.
    public class JSONService1 : IJSONService1
    {
        //3 REQUESTS
        // ALL the methods/request will be here 
        // WE HAVE HARDCODED PATH !!!!

        public RFID[] GetRFID()
        {
            return RFIDrepository.RFIDtags.ToArray();
        }
        public string PostRFID(string value)
        {
            string text = value;
            File.AppendAllText("Person.Text", text, Encoding.UTF8);
            string poststring = "This is the POST String of RFID" + value;
            return poststring;
        }
        public void CreateItem(Stream streamOfData)
        {
            StreamReader reader = new StreamReader(streamOfData);
            String res = reader.ReadToEnd();
            NameValueCollection coll = HttpUtility.ParseQueryString(res);
        }
        public string PutRFID()
        {
            string putstring = "This is the PUT String of RFID";
            return putstring;
        }
        //Method to find the Port
        //static int FreeTcpPort()
        //{
        //    TcpListener l = new TcpListener(IPAddress.Loopback, 0);
        //    l.Start();
        //    int port = ((IPEndPoint)l.LocalEndpoint).Port;
        //    l.Stop();
        //    return port;
        //}
        // this method will be invoked by GET and should responds back to the URI, 
        public LoginResponse login(string username, string password)
        {
            //Response, we will ASSUME WE ARE ALWAYS ACCEESSING, wee will need algorithms to decided wether we need to access or not
            //Hard coded
            LoginResponse lResponse = new LoginResponse();
            lResponse.result = new Result();
            // has almost every information of the request
            OperationContext context = OperationContext.Current;
            MessageProperties messageProperties = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpointProperty =
              messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            try
            {


                lResponse.code = "200";
                lResponse.status = "success";
                lResponse.result.address = endpointProperty.Address;
                lResponse.result.permission = "user";
                lResponse.result.port = endpointProperty.Port.ToString();
                lResponse.result.timestamp = GetTimestamp();
                // token here much be retrived from DB
                lResponse.result.token = "F983799C5848C35DB8781C3EFB53EF5B";
                lResponse.result.user = "device";
                makeFile("username:"+username+"/n"+"pasword:"+password, "login.txt");
            }
            catch (Exception e)
            {
                lResponse.code = "999";
                lResponse.status = "failed with exception";
                lResponse.result.address = endpointProperty.Address;
                lResponse.result.permission = "user";
                lResponse.result.port = endpointProperty.Port.ToString();
                lResponse.result.timestamp = GetTimestamp();
                lResponse.result.token = "999";
                lResponse.result.user = "device";
                makeFile(e.ToString() + "\n the string is: ", "ExceptionLogin.txt");
            }


            return lResponse;
        }

        public LogoutResponse logout(string token)
        {
            LogoutResponse oResponse = new LogoutResponse();
            oResponse.result = new Result();
            // has almost every information of the request
            OperationContext context = OperationContext.Current;
            MessageProperties messageProperties = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpointProperty =
              messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            try
            {
                // FULL URI, NOT USED               
                //Response, we will ASSUME WE ARE ALWAYS ACCEESSING, wee will need algorithms to decided wether we need to access or not
                //Hard coded
                oResponse.code = "200";
                oResponse.status = "success";
                oResponse.result.address = "192.168.77.104";
                oResponse.result.permission = "user";
                oResponse.result.port = "49177";
                oResponse.result.timestamp = "12346";
                oResponse.result.token = token;
                oResponse.result.user = "device";
                makeFile(token, "logOut.txt");
            }
            catch (Exception e)
            {
                oResponse.code = "999";
                oResponse.status = "failed with exception";
                oResponse.result.address = "192.168.77.104";
                oResponse.result.permission = "user";
                oResponse.result.port = "49177";
                oResponse.result.timestamp = "12346";
                oResponse.result.token = token;
                oResponse.result.user = "device";
                makeFile(e.ToString() + "\n the string is: " + token, "ExceptionLogout.txt");
                return oResponse;
            }
            return oResponse;
        }


        //This method will be invoked by POST, and will send a RESPONSE
        public VehicleInventoryReportResponse VehicleInventoryReport(string token) 
        {
            OperationContext context = OperationContext.Current;
            MessageProperties messageProperties = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpointProperty =
              messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            // retrieve JSON body
            string success_man = context.RequestContext.RequestMessage.ToString();
            string res = "no res since new parser";

            try
            {

                //serializing proccess
                JavaScriptSerializer ser = new JavaScriptSerializer();

                // FULL URI, NOT USED
                string originalUri = System.ServiceModel.Web.WebOperationContext.Current.IncomingRequest.UriTemplateMatch.RequestUri.OriginalString;
                makeFile(success_man, "RfidPost.txt");

                //connect DB
                string strSQL = null;
                string strSQL1 = null;
                string connString = null;

                DataSet DSData = new DataSet();

                OleDbConnection objConn = default(OleDbConnection);
                OleDbCommand objCmd = default(OleDbCommand);
                OleDbDataReader dtReader = default(OleDbDataReader);

                try
                {
                    Parser pReport = new Parser(success_man);
                    string sVehicle = pReport.getProperty("vehicle");
                    string sReport = pReport.getProperty("report");
                    string sGPS = pReport.getProperty("gps");
                    string sDOP = pReport.getProperty("DOP");

                    connString = "";
                    objConn = new OleDbConnection(connString);
                    objConn.Open();
                    //for the JSON object insert to DB
                    // PROBLEM SQL ONLY ACCEPT TAG as TAGS but TAG inside JSON is ignored !
                    strSQL = string.Format("insert into RFID_WebService(vehicle,report,gps,DOP,token) values('{0}','{1}','{2}','{3}','{4}')"
                        , sVehicle
                        , sReport
                        , sGPS
                        , sDOP
                        , token);
                    string querry = string.Format("select * from RFID_WebService where vehicle ='{0}' and report = '{1}'and gps ='{2}' and DOP = '{3}'"
                        , sVehicle
                        , sReport
                        , sGPS
                        , sDOP); 

                    // Insert tags

                    objCmd = new OleDbCommand(strSQL, objConn);


                    dtReader = objCmd.ExecuteReader();



                    OleDbDataAdapter adapter = new OleDbDataAdapter(querry, objConn);


                    adapter.Fill(DSData);

                    //DSDATA.Tables[0].Rows[0][x]
                    //0 = ref_num
                    //1 = vehicle
                    //2 = report 
                    //3 = gps
                    //4 = DOP
                    //5 = CurrentDate
                    //6 = token
                    foreach(KeyValuePair<string, Tag> pair in pReport.LstTags)
                    {
                        strSQL1 = string.Format("insert into RFID_Inventory(RFID_ID,Tag,GPS,TagStatus,Vehicle,Report) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')"
                            , DSData.Tables[0].Rows[0][0]
                            , pair.Value.ID
                            , pair.Value.Location
                            , pair.Value.Status
                            , sVehicle
                            , sReport);

                        objCmd = new OleDbCommand(strSQL1, objConn);
                        dtReader = objCmd.ExecuteReader();
                    }
                    objConn.Close();

                }
                catch (Exception ex)
                {
                    makeFile(ex.ToString() + "\n the string is: " + res, "ExceptionSQL.txt");
                }
            }
            catch (Exception e)
            {

                makeFile(e.ToString() + "\n the string is: " + res, "ExceptionInventory.txt");

                VehicleInventoryReportResponse exceptionResponse = new VehicleInventoryReportResponse();
                exceptionResponse.code = "999";
                exceptionResponse.status = "exception";
                return exceptionResponse;
            }
            //Response

            //Hard coded
            VehicleInventoryReportResponse sResponse = new VehicleInventoryReportResponse();
            sResponse.code = "200";
            sResponse.status = "success";
            return sResponse;
        }

        private void makeFile(string res, string name)
        {
            //make file
            //larry remote laptop , @"C:\temp\"
            // dev 4, @"C:\Users\sysmgr\Documents\"
            string path = @"C:\temp\" + name;
            try
            {
                // Delete the file if it exists.
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                // Create the file.
                using (FileStream fs = File.Create(path))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(res.ToString());
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        // static output not needed
        public class RFIDrepository
        {
            public static List<RFID> RFIDtags = new List<RFID>()
          {
          new RFID {user="TestUser", address="123",code="111111", permission="Yes", port="8080", status="awake", timestamp="33424523"}
          };
        }
        //time stamp maker
        public static String GetTimestamp()
        {
            DateTime dt = DateTime.Now;
            return dt.ToString("yyyyMMddHHmmssfff");
        }
    }
}
