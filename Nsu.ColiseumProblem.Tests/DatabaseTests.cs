using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Nsu.ColiseumProblem.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Nsu.ColiseumProblem.Tests
{
    internal class DatabaseTests
    {

        [Test]
        public void Test_Get_By_Id()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            
            var options = new DbContextOptionsBuilder<ColiseumContext>().UseSqlite(connection).Options;

            using (var context = new ColiseumContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new ColiseumContext(options))
            {
                Condition condition = new();
                for (int i = 0; i < 18; i++)
                {
                    Database.Card redCard = new() { Color = "Red", DeckPosition = i * 2 };
                    Database.Card blackCard = new() { Color = "Black", DeckPosition = i * 2 + 1 };

                    context.Cards.Add(redCard);
                    context.Cards.Add(blackCard);

                    condition.Cards.Add(redCard);
                    condition.Cards.Add(blackCard);
                }
                context.Conditions.Add(condition);
                context.SaveChanges();
            }

            using (var context = new ColiseumContext(options))
            {
                Condition condition = context.Conditions.First();
                string[] colors = context.Cards.Where(x => x.Condition.Id == condition.Id).Select(x => x.Color).ToArray();
                for (int i = 0; i < 18; i++)
                {
                    Assert.That(colors[2 * i], Is.EqualTo("Red"));
                    Assert.That(colors[2 * i + 1], Is.EqualTo("Black"));
                }
            }

            connection.Close();
        }
    }
}
