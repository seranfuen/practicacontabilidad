using System.Collections.Generic;

namespace PracticaContabilidad.Model
{
    public class AccountViewModel
    {
        public int AccountId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Balance { get; set; }

        public IEnumerable<LedgerEntry> Entries { get; set; }
    }
}