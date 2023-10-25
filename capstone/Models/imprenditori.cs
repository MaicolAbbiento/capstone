namespace capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("imprenditori")]
    public partial class imprenditori
    {
        [Key]
        public int idimpreditori { get; set; }

        public int? idaziende { get; set; }

        public int? idUtenti { get; set; }

        public virtual aziende aziende { get; set; }

        public virtual utenti utenti { get; set; }
    }
}