using Autofac.Features.OwnedInstances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using AESC.Starter;
using AESC.Utils.AbpExtensions;
using Lion.AbpPro.DataDictionaryManagement.DataDictionaries;
using Lion.AbpPro.LanguageManagement.LanguageTexts.Aggregates;
using Lion.AbpPro.LanguageManagement.Permissions;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Lion.AbpPro.LanguageManagement.Languages.Aggregates;

namespace AESC.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevController : StarterController
    {
        private readonly IRepository<IdentityUser> _repository;

        protected DbContext Context => _repository.GetDbContext();

        protected IDataDictionaryManager dictionaryManager =>
            LazyServiceProvider.LazyGetRequiredService<IDataDictionaryManager>();

        public DevController(IRepository<IdentityUser> repository)
        {
            _repository = repository;
        }

        [HttpGet("multi-entity-query")]
        public async Task<object> MultiEntityQuery()
        {
            try
            {
                var query = from lang in Context.Set<Language>() select lang;
                var list = await query.PageBy(0, 10).ToListAsync();
                
                return new
                {
                    list
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
