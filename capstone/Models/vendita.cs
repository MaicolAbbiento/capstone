namespace capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vendita")]
    public partial class vendita
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public vendita()
        {
            carello = new HashSet<carello>();
        }

        [Key]
        public int idvendita { get; set; }

        public decimal? prezzotot { get; set; }

        public int? idUtenti { get; set; }

        [StringLength(50)]
        public string stato { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<carello> carello { get; set; }

        public virtual utenti utenti { get; set; }
    }
}