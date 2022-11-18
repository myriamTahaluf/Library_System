using Enum;

namespace Library_System.ReturnObject
{
    public class Respons_Result
    {
        public bool isSuccess { get; set; }
        public object? responseBody { get; set; }
        public ErrorCodesEnum? errorCode { get; set; }
        public List<KeyValuePair<string, string>>? errorCodes { get; set; }
        public System.Net.HttpStatusCode statusCode { get; set; } = System.Net.HttpStatusCode.OK;
    }
}
