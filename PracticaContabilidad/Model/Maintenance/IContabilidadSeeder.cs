namespace PracticaContabilidad.Model.Maintenance
{
    /// <summary>
    ///     Defines an interface for services that will seed the database context with
    ///     initial data.
    ///     Thus, we can have one seeder for development, another seeder for the whole Spanish
    ///     PGC or a seeder that reads an xml file, giving us flexibility to initialize
    /// </summary>
    public interface IContabilidadSeeder
    {
        void SeedContext(ContabilidadDbContext context);
    }
}