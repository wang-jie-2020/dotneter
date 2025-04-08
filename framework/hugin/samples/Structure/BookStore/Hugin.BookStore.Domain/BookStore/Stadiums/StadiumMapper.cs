using AutoMapper;
using Volo.Abp.MultiStadium;

namespace Hugin.BookStore.Stadiums
{
    public class StadiumMapper : Profile
    {
        public StadiumMapper()
        {
            CreateMap<Stadium, StadiumConfiguration>();
        }
    }
}
