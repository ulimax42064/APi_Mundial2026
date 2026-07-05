namespace TupApi.Config
{
    /// <summary>
    /// Mapea de forma fuertemente tipada la sección "MongoDbSettings" definida en el appsettings.json.
    /// Permite centralizar las credenciales y configuraciones de infraestructura de MongoDB.
    /// </summary>
    public class MongoDbSettings
    {
        /// <summary>
        /// La cadena de conexión a tu servidor MongoDB (ej: mongodb://localhost:27017).
        /// </summary>
        public string ConnectionString { get; set; } = null!;

        /// <summary>
        /// El nombre de la base de datos específica que vamos a usar en este TP.
        /// </summary>
        public string DatabaseName { get; set; } = null!;
    }
}