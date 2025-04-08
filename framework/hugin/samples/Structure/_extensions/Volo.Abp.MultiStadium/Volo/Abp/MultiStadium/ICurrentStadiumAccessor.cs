namespace Volo.Abp.MultiStadium
{

    public interface ICurrentStadiumAccessor
    {
        BasicStadiumInfo Current { get; set; }
    }
}