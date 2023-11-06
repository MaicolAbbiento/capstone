using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace capstone.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Db")
        {
        }

        public virtual DbSet<aziende> aziende { get; set; }
        public virtual DbSet<carello> carello { get; set; }
        public virtual DbSet<imprenditori> imprenditori { get; set; }
        public virtual DbSet<prodotti> prodotti { get; set; }
        public virtual DbSet<recensioni> recensioni { get; set; }
        public virtual DbSet<utenti> utenti { get; set; }
        public virtual DbSet<vendita> vendita { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
    }
}