using Newtonsoft.Json;

namespace yDevs.Shared.Exceptions
{
    public class ErrorDescription
    {
        public int ErrorCode {get;set;}
        public string Message { get; set; }

        // other fields

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}