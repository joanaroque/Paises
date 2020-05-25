namespace Countries.Services
{
    using Models;
    using System.Net;

    public static class NetworkService
    {
        /// <summary>
        /// this class will see if we have a connection to the net,
        ///  to fetch the data from the API, or to fetch the database
        /// </summary>
        /// <returns></returns>
        public static Response CheckConnection()//retorna objeto do tipo Response
        {
            var client = new WebClient();

            try
            {
                //ping the google server to see if there is a net, or not
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return new Response
                    {
                        IsSuccess = true
                    };
                }
            }
            catch (System.Exception)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = "Configure a sua ligação á internet."
                };
            }
        }
    }
}
