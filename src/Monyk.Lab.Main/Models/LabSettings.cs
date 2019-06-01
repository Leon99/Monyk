using Monyk.Common.Db.Models;

namespace Monyk.Lab.Main.Models
{
    public class LabSettings
    {
        public DatabaseSettings Database { get; set; }

        public string GroundControlBaseUrl { get; set; }
    }
}
