using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Nsu.ColiseumProblem.Database
{
    public class Condition
    {
        public int Id { get; set; }
        virtual public ICollection<Card> Cards { get; } = new List<Card>();
    }

    public class Card
    {
        public int Id { get; set; }
        public string Color { get; set; } = null!;
        public int DeckPosition { get; set; }

        virtual public Condition Condition { get; set; } = null!;
    }

    public class ColiseumContext : DbContext
    {
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Card> Cards { get; set; }

        public ColiseumContext(DbContextOptions<ColiseumContext> options) 
            : base(options)
        {
        }
    }
}
