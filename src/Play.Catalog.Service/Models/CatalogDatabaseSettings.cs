namespace Play.Catalog.Service.Models 
{
    public class CatalogDatabaseSettings: ICatalogDatabaseSettings
    {
        public string ConnectionString {get; set;}
        public string DatabaseName { get; set; }
        public string ItemsCollectionName { get; set; }
    }

    public interface ICatalogDatabaseSettings {
        string ConnectionString {get; set;}
        string DatabaseName { get; set; }
        string ItemsCollectionName { get; set; }
    }
}