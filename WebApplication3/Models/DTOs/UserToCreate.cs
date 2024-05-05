using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.DTOs
{
    public class UserToCreate
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserType { get; set; }
        [Required]
        public List<CurrencyAttrDef> Currencies { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }

    //public class CurrencyAttrs
    //{
    //    public List<CurrencyAttrDef> CurrencyAttrDefs { get; set; }
    //}

    public class CurrencyAttrDef
    {
        public bool isMain { get; set; }
        public string Type { get; set; }

    }
}
