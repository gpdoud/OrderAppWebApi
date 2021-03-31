using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OrderAppWebApi.Models {
    public class Customer {

        public int Id { get; set; } // Primary Key
        [StringLength(10), Required]
        public string Code { get; set; } // Must be unique value
        [StringLength(50), Required]
        public string Name { get; set; }
        public bool IsNational { get; set; }
        [Column(TypeName = "decimal(9,2)")]
        public decimal Sales { get; set; }
        public DateTime? Created { get; set; }

    }
}
