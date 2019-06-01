using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracticaContabilidad.Model
{
    /// <summary>
    ///     A ledger account, used to reflect the balance of all transactions
    ///     (entries) entered, debiting or crediting it.
    /// </summary>
    public class Account
    {
        public int AccountId { get; set; }

        /// <summary>
        ///     The code as per the Spanish PGC, it must have certain length to represent main accounts
        ///     and sub-accounts. By way of example:
        ///     400. Suppliers (proveedores)
        ///     400.0. Suppliers in euro (proveedores euros)
        ///     400.0.1 (equals 400.0.0.xxxxxxxx) where xxxx is defined by the user, for accounts payable for one particular
        ///     supplier
        ///     400.0.2 another supplier
        /// </summary>
        [Required]
        [MinLength(1)]
        [MaxLength(15)]
        [RegularExpression("^[0-9]*$")]
        public string Code { get; set; }

        [Required] public string Name { get; set; }

        /// <summary>
        ///     Optionally we say if the account is mainly a debit account (e.g. cash) or
        ///     a credit account (e.g. accounts payable)
        /// </summary>
        public AccountType? AccountType { get; set; }

        public string Description { get; set; }

        [Required] public decimal Debit { get; set; }

        [Required] public decimal Credit { get; set; }

        [NotMapped] public decimal Balance => Debit - Credit;
    }
}