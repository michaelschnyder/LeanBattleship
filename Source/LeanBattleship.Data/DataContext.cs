using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Linq;
using LeanBattleship.Model;

namespace LeanBattleship.Data
{
    public class DataContext : DbContext
    {
        public EntitySet<Tournament> Tournaments { get; set; }


        public DataContext(string connectionString) : base(connectionString)
        {
        }
    }
}
