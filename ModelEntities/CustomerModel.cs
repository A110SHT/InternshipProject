using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Xunit;

namespace ModelEntities
{
    public class CustomerModel
    {
        public Guid CustomerId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required(ErrorMessage ="Password Required!!")]
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public string Role { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public Int64 Contact { get; set; }
        [Required]
        [DisplayName("Purchase Date")]
        public DateTime PurchaseDate { get; set; }
        public string PictureName { get; set; }
        public string[] Pictures { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
