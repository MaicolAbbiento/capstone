namespace capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("recensioni")]
    public partial class recensioni
    {
        [Key]
        public int idrecensioni { get; set; }

        public decimal? valutazione { get; set; }

        public string descrizione { get; set; }

        public int? idUtenti { get; set; }

        public int? idprodotti { get; set; }

        public int? utilita { get; set; }

        public virtual prodotti prodotti { get; set; }

        public virtual utenti utenti { get; set; }
    }
}