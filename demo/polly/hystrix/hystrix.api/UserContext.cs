using Polly;

namespace Demo
{
    public class UserContext
    {
        private static Microsoft.AspNetCore.Http.HttpContext Context
        {
            get
            {
                return ContextAccessor.Current;
            }
        }

        public static UserContext Current
        {
            get
            {
                return Context.RequestServices.GetService(typeof(UserContext)) as UserContext;
            }
        }
    }
}
