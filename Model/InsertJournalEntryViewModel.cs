﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PracticaContabilidad.Model
{
    public class InsertJournalEntryViewModel
    {
        public string Remarks { get; set; }

        [Required] [Display(Name = "Date")] public DateTime EntryDate { get; set; }

        [Display(Name = "Debit Account")]
        [Required]
        public int DebitAccountId { get; set; }

        [Display(Name = "Credit Account")]
        [Required]
        public int CreditAccountId { get; set; }

        [Required]
        [Display(Name = "Amount")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2} €")]
        [Range(0.01, double.MaxValue)]
        public decimal EntryAmount { get; set; }

        public IEnumerable<Account> Accounts { get; set; }
    }
}