using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using RavenDbTest.Models;

namespace RavenDbTest.Helpers
{
    public class ItemByName : AbstractIndexCreationTask<Item>
    {
        public ItemByName()
        {
            Map = movies => from movie in movies
                            select new {movie.Name};
            Index(x => x.Name, FieldIndexing.Analyzed);
        }
    }
}