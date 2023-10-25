namespace capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("utenti")]
    public partial class utenti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public utenti()
        {
            carello = new HashSet<carello>();
            imprenditori = new HashSet<imprenditori>();
            recensioni = new HashSet<recensioni>();
        }

        [Key]
        public int idUtenti { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Campo obbligatorio")]
        public string nome { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(100)]
        public string cognome { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(40)]
        public string password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(40)]
        public string confermapassword { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(50)]
        public string username { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(50)]
        public string email { get; set; }

        [StringLength(100)]
        public string residenza { get; set; }

        [StringLength(55)]
        public string citta { get; set; }

        public bool? accauntVerificato { get; set; }

        [StringLength(20)]
        public string role { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(20)]
        public string numeroditelefono { get; set; }

        [StringLength(16)]
        public string codicefiscale { get; set; }

        [StringLength(50)]
        public string numeroCartaDiCredito { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<carello> carello { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<imprenditori> imprenditori { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<recensioni> recensioni { get; set; }
    }
}