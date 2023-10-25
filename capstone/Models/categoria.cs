using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace capstone.Models
{
    public class Categoria
    {
        [Key]
        public int idcategoria { get; set; }

        [StringLength(50)]
        public string categoria { get; set; }
    }
}