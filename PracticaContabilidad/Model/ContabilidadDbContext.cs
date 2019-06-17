﻿using Microsoft.EntityFrameworkCore;

namespace PracticaContabilidad.Model
{
    public class ContabilidadDbContext : DbContext
    {
        public ContabilidadDbContext()
        {

        }
        public ContabilidadDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<LedgerEntry> LedgerEntries { get; set; }
    }
}