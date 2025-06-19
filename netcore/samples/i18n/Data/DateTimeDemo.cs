using System.ComponentModel.DataAnnotations.Schema;

namespace i18n.Data;

public class DateTimeDemo
{
    public int Id { get; set; }

    public DateTime? Time1 { get; set; }
    
    public DateTime? Time2 { get; set; }
    
    public DateTime? Time3 { get; set; }
    
    public DateTime? Time4 { get; set; }
}