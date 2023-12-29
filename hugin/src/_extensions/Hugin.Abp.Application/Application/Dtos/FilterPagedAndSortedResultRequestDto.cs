using Volo.Abp.Application.Dtos;

namespace Hugin.Application.Dtos
{
    public class FilterPagedAndSortedResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
