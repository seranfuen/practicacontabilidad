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
        [MinLength(1)]
        [MaxLength(11)]
        [RegularExpression("^[0-9]+.[0-9]+$")]
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