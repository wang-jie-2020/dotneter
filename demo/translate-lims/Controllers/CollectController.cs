using Microsoft.AspNetCore.Mvc;
using Translate.Data;
using Translate.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Translate.Controllers
{
    [ApiController]
    [Route("collect")]
    public class CollectController
    {
        private readonly ICollectService _collectService;

        public CollectController(ICollectService collectService)
        {
            _collectService = collectService;
        }

        [HttpGet]
        [Route("demo")]
        public async Task Demo()
        {
            //await _collectService.CollectFormFileAsync(@"D:\Y远景\LiMS\TVC.Vue\src\extension\applicationform\applicationform\WipOperation\WipOperationModelBody.vue");
            //await _collectService.CollectFormFileAsync(@"D:\Y远景\LiMS\TVC.Vue\src\extension\applicationform\applicationform\SampleCancelHeader.js");
            //D:\Y远景\LiMS\TVC.Vue\src\extension\applicationform\applicationform\WipOperation\WipOperationModelBody.vue
            //D:\Y远景\LiMS\TVC.Server\TVC.ApplicationForm\Services\ApplicationForm\Partial\OrderHeaderService.cs

            await _collectService.CollectFormFileAsync(@"D:\Y远景\LiMS\TVC.Server\TVC.ApplicationForm\Services\ApplicationForm\Partial\OrderHeaderService.cs");
        }

        [HttpGet]
        [Route("vue")]
        public async Task Vue()
        {
            var folders = new string[]
            {
                @"D:\Y远景\LiMS\TVC.Vue\src\views",
                @"D:\Y远景\LiMS\TVC.Vue\src\extension"
            };

            foreach (var folder in folders)
            {
                await _collectService.CollectFromFolderAsync(folder, true);
            }
        }


        [HttpGet]
        [Route("net")]
        public async Task Net()
        {
            var folders = new string[]
            {
                @"D:\Y远景\LiMS\TVC.Server\TVC.ApplicationForm",
                @"D:\Y远景\LiMS\TVC.Server\TVC.Bop",
                @"D:\Y远景\LiMS\TVC.Server\TVC.Common",
                @"D:\Y远景\LiMS\TVC.Server\TVC.DeviceCommon",
                @"D:\Y远景\LiMS\TVC.Server\TVC.Email",
                @"D:\Y远景\LiMS\TVC.Server\TVC.EnterpriseWechat",
                @"D:\Y远景\LiMS\TVC.Server\TVC.LessonLearn",
                @"D:\Y远景\LiMS\TVC.Server\TVC.Other",
                @"D:\Y远景\LiMS\TVC.Server\TVC.Parameters",
                @"D:\Y远景\LiMS\TVC.Server\TVC.PersonnelHandover",
                @"D:\Y远景\LiMS\TVC.Server\TVC.Physicochemical",
                @"D:\Y远景\LiMS\TVC.Server\TVC.Production",
                @"D:\Y远景\LiMS\TVC.Server\TVC.ThirdParty.Sap",
                @"D:\Y远景\LiMS\TVC.Server\TVC.Wms",
                @"D:\Y远景\LiMS\TVC.Server\TVC.WorkFlow",
                @"D:\Y远景\LiMS\TVC.Server\VOL.WebApi"
            };

            foreach (var folder in folders)
            {
                await _collectService.CollectFromFolderAsync(folder, true);
            }
        }
    }
}



