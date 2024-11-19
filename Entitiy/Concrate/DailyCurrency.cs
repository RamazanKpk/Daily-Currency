using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitiy.Concrate
{
    public class DailyCurrency
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "Unit")]
        public int Unit { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "{0} alanı gereklidir.")]
        [StringLength(10, ErrorMessage = "En fazla {1} karakter uzunluğundaolmalıdır.")]
        [Display(Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Required(ErrorMessage = "{0} alanı gereklidir.")]
        [StringLength(40, ErrorMessage = "En fazla {1} karakter uzunluğundaolmalıdır.")]
        [Display(Name = "Currency")]
        public string CurrencyName { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [Display(Name = "Forex Buying")]
        public decimal? ForexBuying { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [Display(Name = "Forex Selling")]
        public decimal? ForexSelling { get; set; } = 0;

        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [Display(Name = "Banknote Buying")]
        public decimal? BanknoteBuying { get; set; } = 0;

        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [Display(Name = "Banknote Selling")]
        public decimal? BanknoteSelling { get; set; } = 0;

        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [Display(Name = "Cross Rate USD")]
        public decimal? CrossRateUSD { get; set; } = 0;

        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        [Display(Name = "Cross Rate Other")]
        public decimal? CrossRateOther { get; set; } = 0;
        public decimal FBChangeRate { get; set; } = 0;
        public decimal FSChangeRate { get; set; } = 0;
        public decimal BBChangeRate { get; set; } = 0;
        public decimal BSChangeRate { get; set; } = 0;
        public decimal CRUsdChangeRate { get; set; } = 0;
        public decimal CROChangeRate { get; set; } = 0;
    }
}
