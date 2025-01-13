using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace Translate.Models
{
    public class TranslateText
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public string ChineseText { get; set; }

        public string? EnglishText { get; set; }

    }
}
