namespace PracticaContabilidad.Model
{
    public class JournalEntryViewModel
    {
        public string Account { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }

        public string Remarks { get; set; }
    }
}