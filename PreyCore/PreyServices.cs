using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.Threading;

namespace PreyCore
{
    public class PreyServices
    {
        /// <summary>
        /// Make a request to prey panel to check if the device was declared as missing
        /// </summary>
        /// <returns>Bool</returns>
        public static async Task<bool> checkIfMissing()
        {

            HttpWebRequest request = WebRequest.Create(Constants.PanelUrl) as HttpWebRequest;
            HttpWebResponse response = null;

            try
            {
                response = await request.GetResponseAsync() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;

            }

            // Todo: if is not FOUND we should try again if we have time in the task runtime
            return response.StatusCode != HttpStatusCode.NotFound;
        }

    }
}
