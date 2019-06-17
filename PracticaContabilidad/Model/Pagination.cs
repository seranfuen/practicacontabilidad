using System;

namespace PracticaContabilidad.Model
{
    public class Pagination

    {
        public Pagination(int totalItems, int itemsPerPage, int currentPage)
        {
            CurrentPage = currentPage;
            TotalPages = (int) Math.Ceiling((decimal) totalItems / itemsPerPage);
        }

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}