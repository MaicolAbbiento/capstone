namespace capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Drawing;
    using System.Web.Razor.Generator;
    using System.Web.UI.WebControls;

    [Table("aziende")]
    public partial class aziende
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public aziende()
        {
            prodotti = new HashSet<prodotti>();
            imprenditori = new HashSet<imprenditori>();
        }

        [Key]
        public int idaziende { get; set; }

        [Display(Name = "Ragione sociale")]
        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(60)]
        public string nomeazienda { get; set; }

        [Display(Name = "Partita IVA")]
        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(11)]
        public string piva { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(100)]
        public string Paese { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        public string citta { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        public string provinca { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(5, ErrorMessage = "inseire un cap valido")]
        public string CAP { get; set; }

        [Required(ErrorMessage = "Campo obbligatorio")]
        [StringLength(20)]
        [Display(Name = "telfono aziendale")]
        public string telfonoaziendale { get; set; }

        public bool inattesa { get; set; }
        public DateTime? dataAprovazione { get; set; }
        public bool verificata { get; set; }
        public int idcategoria { get; set; }
        public virtual Categoria categoria { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<prodotti> prodotti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<imprenditori> imprenditori { get; set; }
    }
}