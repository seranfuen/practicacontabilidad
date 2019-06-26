using System.Collections.Generic;
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

            var accounts = new List<Account>();
            accounts.AddRange(GetGroup1());

            context.Accounts.AddRange(accounts);
            context.SaveChanges();
        }

        private IEnumerable<Account> GetGroup1()
        {
            var accounts = new List<Account>();
            accounts.AddRange(GetGroup10());
            accounts.AddRange(GetGroup11());
            accounts.AddRange(GetGroup12());
            accounts.AddRange(GetGroup13());
            accounts.AddRange(GetGroup14());

            return accounts;
        }

        private IEnumerable<Account> GetGroup14()
        {
            return new List<Account>
            {
                new Account
                {
                    AccountType = AccountType.Credit, Code = "140000000",
                    Name = "Provisión por retribuciones a largo plazo al personal"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "141000000", Name = "Provisión para impuestos"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "142000000",
                    Name = "Provisión para otras responsabilidades"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "143000000",
                    Name = "Provisión por el desmantelamiento, retiro o rehabilitación del inmovilizado"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "145000000",
                    Name = "Provisión para actuaciones medioambientales"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "146000000", Name = "Provisión para restructuraciones"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "147000000",
                    Name = "Provisión por transacciones con pagos basados en instrumentos de patrimonio"
                }
            };
        }

        private IEnumerable<Account> GetGroup13()
        {
            return new List<Account>
            {
                new Account
                {
                    AccountType = AccountType.Credit, Code = "130000000", Name = "Subvenciones oficiales de capital"
                },
                new Account
                {
                    AccountType = AccountType.Debit, Code = "131000000",
                    Name = "Donaciones y legados de capital"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "132000000",
                    Name = "Otras subvenciones, donaciones y legados"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "133000000",
                    Name = "Ajustes por valoración en activos financieros disponibles para la venta"
                },
                new Account {AccountType = AccountType.Credit, Code = "134000000", Name = "Operaciones de cobertura"},
                new Account {AccountType = AccountType.Credit, Code = "135000000", Name = "Diferencias de conversión"},
                new Account
                {
                    AccountType = AccountType.Credit, Code = "136000000",
                    Name =
                        "Ajustes por valoración en activos no corrientes y grupos enajenables de elementos, mantenidos para la venta"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "137000000",
                    Name = "Ingresos fiscales a distribuir en varios ejercicios"
                }
            };
        }

        private IEnumerable<Account> GetGroup12()
        {
            return new List<Account>
            {
                new Account {AccountType = AccountType.Credit, Code = "120000000", Name = "Remanente"},
                new Account
                {
                    AccountType = AccountType.Debit, Code = "121000000",
                    Name = "Resultados negativos de ejercicios anteriores"
                },
                new Account {AccountType = AccountType.Credit, Code = "129000000", Name = "Resultado del ejercicio"}
            };
        }

        private IEnumerable<Account> GetGroup11()
        {
            return new List<Account>
            {
                new Account
                {
                    AccountType = AccountType.Credit, Code = "110000000", Name = "Prima de emisión o asunción"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "111000000", Name = "Otros instrumentos de patrimonio neto"
                },
                new Account {AccountType = AccountType.Credit, Code = "112000000", Name = "Reserva legal"},
                new Account {AccountType = AccountType.Credit, Code = "113000000", Name = "Reserva voluntaria"},
                new Account {AccountType = AccountType.Credit, Code = "114000000", Name = "Reservas especiales"},
                new Account {AccountType = AccountType.Credit, Code = "114100000", Name = "Reservas estatutarias"},
                new Account
                {
                    AccountType = AccountType.Credit, Code = "114200000", Name = "Reserva por capital amortizado"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "114300000", Name = "Reserva por fondo de comercio"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "114400000",
                    Name = "Reserva por acciones propias aceptadas en garantía"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "115000000",
                    Name = "Reservas por pérdidas y ganancias actuariales y otros ajustes"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "118000000", Name = "Aportaciones de socios o propietarios"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "119000000",
                    Name = "Diferencias por ajuste del capital a euros"
                }
            };
        }

        private static IEnumerable<Account> GetGroup10()
        {
            return new List<Account>
            {
                new Account {AccountType = AccountType.Credit, Code = "100000000", Name = "Capital social"},
                new Account {AccountType = AccountType.Credit, Code = "101000000", Name = "Fondo social"},
                new Account {AccountType = AccountType.Credit, Code = "102000000", Name = "Capital"},
                new Account
                {
                    AccountType = AccountType.Debit, Code = "103000000", Name = "Socios por desembolsos no exigidos"
                },
                new Account
                {
                    AccountType = AccountType.Debit, Code = "104000000",
                    Name = "Socios por aportaciones no dinerarias pendientes"
                },
                new Account
                {
                    AccountType = AccountType.Debit, Code = "108000000",
                    Name = "Acciones o participaciones propias en situaciones especiales"
                },
                new Account
                {
                    AccountType = AccountType.Credit, Code = "109000000",
                    Name = "Acciones o participaciones propias para reducción de capital"
                }
            };
        }
    }
}