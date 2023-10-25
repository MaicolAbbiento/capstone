namespace capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("prodotti")]
    public partial class prodotti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public prodotti()
        {
            recensioni = new HashSet<recensioni>();
            vendita = new HashSet<vendita>();
        }

        [Key]
        public int idprodotti { get; set; }

        public string fotoprodotto { get; set; }

        [StringLength(20)]
        public string nomeprodotto { get; set; }

        public string contenutodelprodotto { get; set; }

        public decimal? prezzo { get; set; }

        public int? prodottiinmagazzino { get; set; }

        public DateTime? datascadenza { get; set; }

        public string descrizioneshot { get; set; }

        public string descrizione { get; set; }

        public int? valutazione { get; set; }

        public bool? invendita { get; set; }

        public int? idaziende { get; set; }

        public int idcategoria { get; set; }
        public virtual Categoria categoria { get; set; }
        public virtual aziende aziende { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<recensioni> recensioni { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<vendita> vendita { get; set; }
    }
}