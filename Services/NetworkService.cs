namespace Countries.Services
{
    using Models;
    using System.Net;

    public static class NetworkService
    {
        // esta classe vai ver se temos ligaçao á net, 
        //para ir buscar os dados á API, ou se vai buscar á base de dados

        public static Response CheckConnection()//retorna objeto do tipo Response
        {
            var client = new WebClient();

            try
            {
                //fazer um ping ao servidor da google pra ver se ha net, ou nao
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return new Response
                    { //corre tudo bem
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
