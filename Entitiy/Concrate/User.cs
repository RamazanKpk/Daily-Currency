using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitiy.Concrate
{
    public class User
    {
        [Key]
        [Display(Name = "User Id")]
        public int UserID { get; set; }
        [Required(ErrorMessage = "{0} alanı gereklidir.")]
        [StringLength(40, ErrorMessage = "En fazla {1} karakter uzunluğundaolmalıdır.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} alanı gereklidir.")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Şifre en az {2}, en fazla {1} karakter uzunluğunda olmalıdır.")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Normal Exchange Rates Authorization")]
        public bool NormalExchangeRatesAuthorization { get; set; }
        [Display(Name = "Cross Exchange Rates Authorization")]
        public bool CrossExchangeRatesAuthorization { get; set; }

        public User()
        {

        }

    }
}
