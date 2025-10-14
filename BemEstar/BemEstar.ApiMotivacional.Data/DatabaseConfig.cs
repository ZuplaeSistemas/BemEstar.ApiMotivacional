namespace BemEstar.ApiMotivacional.Data
{
    public class DatabaseConfig
    {
        public string ConnectionString { get; }

        public DatabaseConfig() 
        {
            ConnectionString = "Host=localhost;Port=5432;Database=motivacional;Username=postgres;Password=123456";
        }
    }
}
