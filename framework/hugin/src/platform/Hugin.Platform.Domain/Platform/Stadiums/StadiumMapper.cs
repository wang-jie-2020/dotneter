using AutoMapper;
using Volo.Abp.MultiStadium;

namespace Hugin.Platform.Stadiums
{
    public class StadiumMapper : Profile
    {
        public StadiumMapper()
        {
            CreateMap<Stadium, StadiumConfiguration>();
        }
    }
}
