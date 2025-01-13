using System.ComponentModel.DataAnnotations.Schema;

namespace i18n.Data;

public class DateTimeOffsetDemo
{
    public int Id { get; set; }

    public DateTimeOffset? Time11 { get; set; }
    
    public DateTimeOffset? Time12 { get; set; }
    
    public DateTimeOffset? Time13 { get; set; }
    
    public DateTimeOffset? Time14 { get; set; }
}