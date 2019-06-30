using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracticaContabilidad.Model
{
    public class LedgerEntry
    {
        public int LedgerEntryId { get; set; }

        public decimal EntryValue { get; set; }

        [Required] public DateTime EntryDate { get; set; }

        [NotMapped] public decimal Credit => EntryValue < 0 ? -EntryValue : 0;
        [NotMapped] public decimal Debit => EntryValue > 0 ? EntryValue : 0;

        [Required] public Account Account { get; set; }

        [MaxLength(1000)] public string Remarks { get; set; }

        [Required] public int AccountId { get; set; }
            }
}