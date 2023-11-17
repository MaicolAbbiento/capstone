using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace capstone.Models
{
    public class prodottiqury
    {
        public int idprodotti { get; set; }
        public string fotoprodotto { get; set; }

        [Required(ErrorMessage = "campo obligatorio")]
        [StringLength(500)]
        [Display(Name = "inserire il nome del prodotto")]
        public string nomeprodotto { get; set; }

        [Display(Name = "inserire il prezzo")]
        [Required(ErrorMessage = "campo obligatorio")]
        public decimal? prezzo { get; set; }

        public int valutazione { get; set; }
        public string categoria { get; set; }
    }
}