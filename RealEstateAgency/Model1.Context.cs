﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealEstateAgency
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RealEstateAgencyEntities : DbContext
    {
        public RealEstateAgencyEntities()
            : base("name=RealEstateAgencyEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Apartments> Apartments { get; set; }
        public virtual DbSet<Archive> Archive { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Metro> Metro { get; set; }
        public virtual DbSet<Owners> Owners { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Sales> Sales { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}