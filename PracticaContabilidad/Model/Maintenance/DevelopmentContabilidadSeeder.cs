using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace PracticaContabilidad.Model.Maintenance
{
    /// <summary>
    ///     Seeds the database with some data, but not comprehensively. Used for development testing. Follows Spanish PGC.
    /// </summary>
    public class DevelopmentContabilidadSeeder : IContabilidadSeeder
    {
        public void SeedContext(ContabilidadDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Accounts.Any()) return;

            // Credit: 20.801, Debit: 20.801
            var accounts = new List<Account>
            {
                new Account
                {
                    Code = "400.1", Name = "PEPITO PEREZ S.L.", AccountType = AccountType.Credit, Credit = 1000.50m,
                    Debit = 300.25m
                },
                new Account
                {
                    Code = "572.101", Name = "BANKIA A/C", AccountType = AccountType.Debit, Credit = 1800.5m,
                    Debit = 10200m
                },
                new Account
                {
                    Code = "100", Name = "CAPITAL SOCIAL", AccountType = AccountType.Credit, Credit = 18000, Debit = 0
                },
                new Account
                {
                    Code = "300", Name = "EXISTENCIAS", AccountType = AccountType.Debit, Credit = 0, Debit = 4000.8m
                },
                new Account
                {
                    Code = "640.20", Name = "SUELDO PEPE", AccountType = AccountType.Debit, Credit = 0, Debit = 6299.95m
                }
            };

            context.Accounts.AddRange(accounts);
            context.SaveChanges();
        }
    }
}