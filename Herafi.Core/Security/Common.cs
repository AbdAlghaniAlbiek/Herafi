using System;
using System.Net;
using System.Threading.Tasks;
using Herafi.Core.Helpers;
using Windows.Storage;

namespace Herafi.Core.Security
{
    public class Common
    {
        //We use this statment when we connect to another device 
        //private static readonly string BASE_URL = "http://192.168.43.215";

        //(this device is the server)
        private static readonly string BASE_URL = "http://" + GetIPAddress();

        private static readonly string PORT = "3000";
        public static readonly string URL = $"{BASE_URL}:{PORT}/admin";
        public static readonly string USERS_IMAGES_PATH = $"{BASE_URL}:{PORT}/public/upload/images/users/";
        public static readonly string CRAFTMEN_IMAGES_PATH = $"{BASE_URL}:{PORT}/public/upload/images/craftmen/";
        public static readonly string ADMIN_IMAGES_PATH = $"{BASE_URL}:{PORT}/public/upload/images/admins/";


        public async static Task<string> JWT_PUBLIC_KEY()
        {
            //await StorageFile.GetFileFromPathAsync("./../StaticFiles/Security/PublicKey.xml");
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///StaticFiles/Security/PublicKey.xml"));

            return await FileIO.ReadTextAsync(file);
        }

        public static readonly string AES_KEY = "341%p3s$fc*^g$54";
        public static readonly string AES_IV = "jpbwso01$80*&er+";
        public static readonly string SECRET_KEYWORD = "hOgeNoTjge+/g4Khx1MKlQ==";

        public static readonly string MICROSOFT_APP_ID = "";
        public static readonly string FACEBOOK_APP_ID = "";
        public static readonly string WINDOWS_APP_ID = "";


        public static string App_Environment = Choices.ChooseAppEnvironment(AppEnvironment.Development);

        public static string SUB_TOKEN = "";
        public static string ADMIN_ID = "";

        public static string TOKEN { get => "Bearer "+SUB_TOKEN; }

        public static string GetIPAddress()
        {
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIP)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return address.ToString();
                }
            }
            return string.Empty;
        }
    }
}
