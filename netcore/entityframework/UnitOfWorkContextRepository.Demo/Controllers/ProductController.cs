using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Demo.Models;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using UnitOfWorkContextRepository.Repository;

namespace Demo.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController: ControllerBase
    {
        private readonly IEfCoreRepository _repository;

        public ProductController(IEfCoreRepository repository)
        {
            _repository = repository;
        }

        [Route("insert")]
        [HttpPost]
        public async Task InsertUpdateAsync(ProductDto dto)
        {
            var product = new Product
            {
                ProductCode = dto.ProductCode,
                ProductName = dto.ProductName,
                Vmax = dto.Vmax,
                Vmin = dto.Vmin
            };

            // var results = new List<ValidationResult>();
            // bool isValid = Validator.TryValidateObject(
            //     product,
            //     new ValidationContext(product, null, null),
            //     results,
            //     true);
            
            await _repository.InsertAsync(product,true);
            await Task.CompletedTask;
        }
    }
}
