namespace capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("carello")]
    public partial class carello
    {
        [Key]
        public int idcarello { get; set; }

        public int? quantita { get; set; }

        public int? idvendita { get; set; }

        public int? idprodotti { get; set; }

        public virtual vendita vendita { get; set; }

        public virtual prodotti prodotti { get; set; }
    }
}
