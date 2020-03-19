namespace Paises.Models
{
    public class Response
    {
        public bool IsSuccess { get; set; } // tenho net ou nao? API correu bem ou nao? etc

        public string Message { get; set; }

        public object Result { get; set; } // corre tudo bem,
        //guardo um objeto , pode ser uma ligaçao bem feita, pode ser uma rate, qlqlr coisa!!!
    }
}
