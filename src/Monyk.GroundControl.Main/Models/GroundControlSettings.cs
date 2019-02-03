using Monyk.GroundControl.Db;

namespace Monyk.GroundControl.Main.Models
{
    public class GroundControlSettings
    {
        public class GroundControlDatabaseSettings
        {
            public DatabaseType Type { get; set; }
            public string ConnectionString { get; set; }
        }
        public GroundControlDatabaseSettings Database { get; set; }
    }
}
