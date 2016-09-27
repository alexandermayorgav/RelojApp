using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Logica;
namespace Presentacion
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MDIPrincipal());
           // getVoces();
            //savefile();
            //subeArchivo();
        }

        private static void getVoces()
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            //synth.SelectVoice
            synth.Speak("Acceso correcto ");
            foreach (InstalledVoice voice in synth.GetInstalledVoices())
            {
                VoiceInfo info = voice.VoiceInfo;
                Console.WriteLine(" Voice Name: " + info.Name);
            }
        }

        private static void subeArchivo()
        {
            try
            {
                FTP ftp = new FTP("ftp://Pruebas/", "root", "root");
                ftp.upload("prueba.txt", @"C:\Users\Alexander Mayorga\Documents\Visual Studio 2012\Projects\RelojApp\Presentacion\bin\prueba.txt");
                
               // System.Net.WebClient webClient = new System.Net.WebClient();
               // string sourceFilePath = @"C:\Users\Alexander Mayorga\Documents\Visual Studio 2012\Projects\RelojApp\Presentacion\bin\Debug_4.bmp";
               // string webAddress = "http://pruebas/";
               // string destinationFilePath = webAddress + "Debug_4.bmp";
               //// webClient.Credentials = new System.Net.NetworkCredential("canarymx", "29FM#7@%=wRM");
               // webClient.UploadFile(destinationFilePath, "POST", sourceFilePath);
               // webClient.Dispose();
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        //private static System.IO.Stream Upload(string actionUrl, string paramString, Stream paramFileStream, byte[] paramFileBytes)
        //{
        //    HttpContent stringContent = new StringContent(paramString);
        //    HttpContent fileStreamContent = new StreamContent(paramFileStream);
        //    HttpContent bytesContent = new ByteArrayContent(paramFileBytes);
        //    using (var client = new HttpClient())
        //    using (var formData = new MultipartFormDataContent())
        //    {
        //        //formData.Add(stringContent, "param1", "param1");
        //        formData.Add(fileStreamContent, "file1", "file1");
        //        formData.Add(bytesContent, "file2", "file2");
        //        var response = client.PostAsync(actionUrl, formData).Result;
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return null;
        //        }
        //        return response.Content.ReadAsStreamAsync().Result;
        //    }
        //}
        //private static void savefile()
        //{
        //    //Console.Write("\nPlease enter the URI to post data to : ");
        //    String uriString ="127.0.0.1/Pruebas/";

        //    // Create a new WebClient instance.
        //    WebClient myWebClient = new WebClient();

        //    //Console.WriteLine("\nPlease enter the fully qualified path of the file to be uploaded to the URI");
        //    string fileName = @"C:\Users\Alexander Mayorga\Documents\Visual Studio 2012\Projects\RelojApp\Presentacion\bin\Debug_4.bmp";// = Console.ReadLine();
        //    Console.WriteLine("Uploading {0} to {1} ...", fileName, uriString);
        //    try
        //    {
        //        // Upload the file to the URI.
        //        // The 'UploadFile(uriString,fileName)' method implicitly uses HTTP POST method.
        //        byte[] responseArray = myWebClient.UploadFile(uriString, "Debug_4.bmp");

        //        // Decode and display the response.
        //        Console.WriteLine("\nResponse Received.The contents of the file uploaded are:\n{0}",
        //            System.Text.Encoding.ASCII.GetString(responseArray));
        //    }
        //    catch (Exception e)
        //    { 
        //    MessageBox.Show(e.Message +" \r\n" +e.StackTrace);
        //    }
        //}
    }
}
