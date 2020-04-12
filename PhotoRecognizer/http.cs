using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace PhotoRecognizer
{
    class http
    {
        private const string URL = "http://d951204t.beget.tech/upload_script.php";
        public static async Task<System.IO.Stream> Upload( string paramString, Stream paramFileStream)//, byte[] paramFileBytes)
        {
            //HttpContent stringContent = new StringContent(paramString);
            HttpContent fileStreamContent = new StreamContent(paramFileStream);
            //HttpContent bytesContent = new ByteArrayContent(paramFileBytes);
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                //formData.Add(stringContent, "param1", "param1");
                formData.Add(fileStreamContent, "file1", "file1");
                //formData.Add(bytesContent, "file2", "file2");
                var response = await client.PostAsync(URL, formData);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return await response.Content.ReadAsStreamAsync();
            }
        }
        public static void SendFile(string path)
        {            
            using (var client = new System.Net.WebClient())
            {
                client.UploadFile(URL, path);
            }
        }
    }
}