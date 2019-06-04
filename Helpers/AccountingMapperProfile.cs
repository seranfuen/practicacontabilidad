using AutoMapper;
using PracticaContabilidad.Model;

namespace PracticaContabilidad.Helpers
{
    public class AccountingMapperProfile : Profile
    {
        public AccountingMapperProfile()
        {
            CreateMap<Account, Account>();
        }
    }
}