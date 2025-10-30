using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DT.Source.Managers;

class NetworkManager
{
    public class Response
    {
        public HttpStatusCode Code {  get; set; }
        public string Message {  get; set; }
    }
    public static Response Send(string url, string secret)
    {
        byte[] data = Encoding.ASCII.GetBytes(secret);

        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

        req.Method = "POST";
        req.ContentLength = data.Length;
        req.Timeout = 4000;

        using (var stream = req.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        try
        {
            var res = (HttpWebResponse)req.GetResponse();
            var ress = new StreamReader(res.GetResponseStream()).ReadToEnd();

            return new() { Code = res.StatusCode, Message = ress };
        }
        catch
        {
            return new() { Code = 0, Message = "Cannot reach server" };
        }
    }
}
