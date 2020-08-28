using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieLib
{
    class SafeHttpClient : HttpClient
    {
        public new async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            try
            {
                return await base.GetAsync(requestUri);
            }
            catch (Exception e)
            {
                ErrorManager.PrintConnectionError(e);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.BadGateway);
        }
    }
}
