using System.Collections.Generic;
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
        ///     and sub-accounts.
        ///     We expect either a string of up to 9 digits or optionally a dot. The dot establishes that whatever comes after
        ///     it will go to the last part of the code, so that for example:
        ///     421 is transformed to  421000000 (9 digits)
        ///     42.1 is transformed to 420000001
        ///     420.100  is transformed to 420000100
        /// </summary>
        [Required]
        [MinLength(9)]
        [MaxLength(9)]
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "Nombre")] [Required] public string Name { get; set; }

        /// <summary>
        ///     Optionally we say if the account is mainly a debit account (e.g. cash) or
        ///     a credit account (e.g. accounts payable)
        /// </summary>
        [Display(Name = "Tipo")]
        public AccountType? AccountType { get; set; }

        [Display(Name = "Descripción")] public string Description { get; set; }

        [Display(Name = "Debe")] [Required] public decimal Debit { get; set; }

        [Display(Name = "Haber")] [Required] public decimal Credit { get; set; }

        [Display(Name = "Saldo")] [NotMapped] public decimal Balance => Debit - Credit;

        private ICollection<LedgerEntry> LedgerEntries { get; set; }
    }
}