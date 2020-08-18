using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Models
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(18)]
        public string IdCardNo { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }
    }
}
