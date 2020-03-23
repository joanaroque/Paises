namespace Countries.Models
{
    public class Response
    {
        public bool IsSuccess { get; set; } // tenho net ou nao? API correu bem ou nao? etc

        public string Message { get; set; }

        public object Result { get; set; } 
    }
}
