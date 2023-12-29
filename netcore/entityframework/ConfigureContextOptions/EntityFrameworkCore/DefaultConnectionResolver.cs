using Microsoft.Extensions.Configuration;

namespace Demo.EntityFrameworkCore
{
    public class DefaultConnectionResolver : IConnectionStringResolver
    {
        private readonly IConfiguration _configuration;

        public DefaultConnectionResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(string connectionStringName)
        {
            var connectionString = _configuration.GetConnectionString(connectionStringName);
            if (connectionString != null)
            {
                return connectionString;
            }

            return _configuration.GetConnectionString("Default");
        }
    }
}
