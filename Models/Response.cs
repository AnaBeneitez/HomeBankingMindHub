namespace HomeBankingMindHub.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public Response()
        {
            
        }
        public Response(int code, string msg)
        {
            StatusCode = code;
            Message = msg;
        }

    }
}
