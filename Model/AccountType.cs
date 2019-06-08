using System.ComponentModel.DataAnnotations;

namespace PracticaContabilidad.Model
{
    public enum AccountType
    {
        [Display(Name = "Acreedora")] Credit = 1,
        [Display(Name = "Deudora")] Debit = 2
    }
}