using System;
using System.IO;
using System.Net;
using System.Text;

namespace BasitPortal
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public class DataAdapterRest
    {
        public string endPoint { get; set; }
        public string url { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string wh_id { get; set; }
        public httpVerb httpMethod { get; set; }
        public string cookie { get; set; }
        public string body { get; set; }
        public string contentType { get; set; }
        public string JsonCommand { get; set; }

        public DataAdapterRest()
        {
            endPoint = string.Empty;
            url = string.Empty;
            httpMethod = httpVerb.POST;
            cookie = string.Empty;
            body = string.Empty;
            contentType = "application/json";
            JsonCommand = string.Empty;
            userName = string.Empty;
            password = string.Empty;
        }

        public string makeRequest()
        {
            string strResponseValue = string.Empty;
            string errorData = null;
            endPoint = url + "/ws/cws/execute_onescanner_field_validations";//JDA'in onescanner web servisini kullanabileceğimiz sabit adrestir.

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);

                request.Method = httpMethod.ToString();
                request.Headers.Add("Cookie", makeAuthRequest());
                request.ContentType = contentType;
                request.ServicePoint.Expect100Continue = false;


                using (var streamWriter = new StreamWriter(request.GetRequestStream()))//Body Soap UI'da media type'ın altındaki boşluğa gelecek olan Json komut. 
                {
                    streamWriter.Write("{\"validation_parameter\":\"" + JsonCommand + "\",\"wh_id\":\"" + wh_id + "\", \"devcod\":\"RDT005\", \"usr_id\":\"" + userName + "\",\"password\":\"" + password + "\"}");
                }
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.Created)
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
            string newCookie = string.Empty;

            endPoint = url + "/ws/auth/login";
            cookie = "MOCA-WS-SESSIONKEY=%3Buid%3DMERT%7Csid%3D547fde6a-abe5-4593-b7c5-0e32351230c5%7Cdt%3Dk3qx9d1y%7Csec%3DALL%3B2kS9ElbQpx9ofxuBGsQ5KNEW93;Path=/;HttpOnly";
            string credentials = "{\"usr_id\":\"" + userName + "\", \"password\":\"" + password + "\"}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);

            request.Method = httpMethod.ToString();
            request.Headers.Add("Cookie", cookie);
            request.ContentType = contentType;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))//SoapUI da media type'ın altındaki boşluğa denk gelen kısmı atıyorum. 
            {
                streamWriter.Write(credentials);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("error code : " + response.StatusCode.ToString());
                }
                newCookie = response.GetResponseHeader("Set-Cookie");//Authentacation(giriş) yapıldıktan sonra response(cevap)'dan gelen cookie'yi alıyorum.

            }//End of Using Response

            return newCookie;
        }
        public string getData(string url, string userName, string password, string command, string wh_id)
        {//Genel sorgu gönderip JDA'den JSON veri çektiğimiz

            this.url = url;
            this.userName = userName;
            this.password = password;
            this.JsonCommand = command;
            this.wh_id = wh_id;

            return makeRequest();
        }

    }

}