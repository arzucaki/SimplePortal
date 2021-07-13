using System;
using System.IO;
using System.Net;
using System.Text;

namespace BasitPortal
{
    public enum httpVerb2
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public class DataAdapterRP
    {
        public string endPoint { get; set; }
        public string url { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string wh_id { get; set; }
        public httpVerb2 httpMethod { get; set; }
        public string cookie { get; set; }
        public string body { get; set; }
        public string contentType { get; set; }
        public string commandFileName { get; set; }
        public string parameters { get; set; }

        public DataAdapterRP()
        {
            endPoint = string.Empty;
            url = string.Empty;
            httpMethod = httpVerb2.POST;
            cookie = string.Empty;
            body = string.Empty;
            contentType = "application/json";
            commandFileName = string.Empty;
            userName = string.Empty;
            password = string.Empty;
            parameters = string.Empty;
        }

        public string makeRequest()
        {
            string strResponseValue = string.Empty;
            string errorData = null;
            endPoint = url + "/ssmobile/API/QUALITY/"+ commandFileName + "/oncreate/";//JDA'in onescanner web servisini kullanabileceğimiz sabit adrestir.

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);

                request.Method = httpMethod.ToString();
                //request.Headers.Add("Cookie", makeAuthRequest());
                request.ContentType = contentType;
                request.ServicePoint.Expect100Continue = false;

            string bodyy = "{     \"meta\": {         \"id\": \"{{$guid}}\",         \"hw\": \"MSSQLSERVER\",         \"osver\": \"MSSQLSERVER\",         \"appver\": \"MSSQLSERVER\"     },     " + makeAuthRequest() + ",     \"framework\": {         \"current_area\": \"API\",         \"current_process\": \"QUALITY\",         \"current_action\": \"" + commandFileName + "\"     },     \"context\": {" + parameters + "     } }";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))//Body Json. 
                {
                    streamWriter.Write(bodyy);
                }
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new ApplicationException("error code : " + response.StatusCode.ToString());
                    }
                    //Process the response stream... (could be JSON, XML or HTML etc...)
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                strResponseValue = reader.ReadToEnd();
                            }//end of StreamReader
                        }
                    }//End of using ResponseStream
                }//End of Using Response
            }
            catch (WebException exception)
            {
                using (var response = (HttpWebResponse)exception.Response)
                {
                     if (response.StatusCode.ToString() == "NotFound")
                    {
                        return "No Data";
                    }
                    errorData = ReadResponse(response);
                }
            }

            return strResponseValue;
        }

        static string ReadResponse(HttpWebResponse response)
        {
            if (response.CharacterSet == null)
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(response.CharacterSet)))
            {
                return reader.ReadToEnd();
            }
        }

        public string makeAuthRequest()
        /*JDA web servisi önce login olup o loginden gelen cookie bilgisiyle yeniden bağlanılmasını 
        gerektiren bir yapıda olduğu için Authentication'ı önce yapıyoruz ve cookie'yi alıyoruz.*/
        {
            
            string strResponseValue = string.Empty;
            string errorData = null;
            endPoint = url + "/ssmobile/authenticate/";
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);

            request.Method = httpMethod.ToString();
            request.ContentType = contentType;
            request.ServicePoint.Expect100Continue = false;


            using (var streamWriter = new StreamWriter(request.GetRequestStream()))//Body Soap UI'da media type'ın altındaki boşluğa gelecek olan Json komut. 
            {
                streamWriter.Write("{\"meta\": { \"id\": \"{{$guid}}\",\"hw\": \"MSSQLSERVER\", \"osver\": \"MSSQLSERVER\",  \"appver\": \"MSSQLSERVER\" },\"env\":{\"devcod\": \"E-DELIVERYNOTE\",   \"username\": \""+userName+"\", \"password\": \""+password+"\" }  }");
            }
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new ApplicationException("error code : " + response.StatusCode.ToString());
                    }
                    //Process the response stream... (could be JSON, XML or HTML etc...)
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {

                                strResponseValue = reader.ReadToEnd();

                            }//end of StreamReader
                        }
                    }//End of using ResponseStream
                }//End of Using Response
            }
            catch (WebException exception)
            {
                using (var response = (HttpWebResponse)exception.Response)
                {
                    if (response.StatusCode.ToString() == "NotFound")
                    {
                        return "No Data";
                    }
                    errorData = ReadResponse(response);
                }
            }

            return strResponseValue.Substring(1, strResponseValue.Length-2);
        }
        public string getDataList(string url, string userName, string password, string command, string wh_id, string prtnum, string lotnum)
        {//Genel sorgu gönderip JDA'den JSON veri çektiğimiz

            this.url = url;
            this.userName = userName;
            this.password = password;
            commandFileName = command;
            this.wh_id = wh_id;
            parameters = "\"lotnum\": \"" + lotnum + "\",\"prtnum\": \"" + prtnum + "\"";
            return makeRequest();
        }
        public string getConfirmedDataList(string url, string userName, string password, string command, string wh_id, string prtnum, string lotnum, string date, string nextDate)
        {//Genel sorgu gönderip JDA'den JSON veri çektiğimiz

            this.url = url;
            this.userName = userName;
            this.password = password;
            commandFileName = command;
            this.wh_id = wh_id;
            parameters = "\"lotnum\": \"" + lotnum + "\",\"prtnum\": \"" + prtnum + "\",\"date\":\""+date+ "\",\"nextdate\":\"" + nextDate + "\"";
            return makeRequest();
        }
        public string processStatusChange(string url, string userName, string password, string command, string wh_id, string po, string date, string user, string dtlnum, string tostatus)
        {//Genel sorgu gönderip JDA'den JSON veri çektiğimiz

            this.url = url;
            this.userName = userName;
            this.password = password;
            commandFileName = command;
            this.wh_id = wh_id;
            parameters = "\"confirm_num\": \"" + po + "\",\"date\": \"" + date + "\",\"user\":\"" + user + "\",\"dtlnum\": \"" + dtlnum + "\",\"tostatus\": \"" + tostatus + "\"";
            return makeRequest();
        }
        public string processBarcodeStatusChange(string url, string userName, string password, string command, string wh_id, string dtlnum, string tostatus)
        {//Genel sorgu gönderip JDA'den JSON veri çektiğimiz

            this.url = url;
            this.userName = userName;
            this.password = password;
            commandFileName = command;
            this.wh_id = wh_id;
            parameters = "\"dtlnum\": \"" + dtlnum + "\",\"tostatus\": \"" + tostatus + "\"";
            return makeRequest();
        }
    }

}