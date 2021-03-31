using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OrderAppWebApi.Models {

    public class Salesperson {

        public int Id { get; set; }
        [StringLength(30), Required]
        public string Name { get; set; }
        public string StateCode { get; set; }
        [Column(TypeName = "decimal(9,2)")]
        public decimal Sales { get; set; }

    }
}
