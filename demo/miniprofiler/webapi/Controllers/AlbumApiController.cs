using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Models;
using Demo.Services;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlbumApiController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumApiController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IEnumerable<Album>> Get()
        {
            var albums = await _albumService.GetAllAsync();
            return albums;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AlbumCreateViewModel albumCreateViewModel)
        {
            var newModel = await _albumService.AddAsync(new Album
            {
                Artist = albumCreateViewModel.Artist,
                Title = albumCreateViewModel.Title,
                CoverUrl = albumCreateViewModel.CoverUrl,
                Price = albumCreateViewModel.Price,
                ReleaseDate = albumCreateViewModel.ReleaseDate
            });
            return Ok();
        }
    }
}
