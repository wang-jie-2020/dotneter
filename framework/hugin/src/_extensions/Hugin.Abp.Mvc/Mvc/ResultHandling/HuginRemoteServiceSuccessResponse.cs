namespace Hugin.Mvc.ResultHandling
{
    public class HuginRemoteServiceSuccessResponse
    {
        public string Success => "true";

        public string Code => "0";

        public object Result { get; }

        public HuginRemoteServiceSuccessResponse(object result)
        {
            Result = result;
        }
    }
}









