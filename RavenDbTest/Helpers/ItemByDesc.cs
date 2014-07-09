using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using RavenDbTest.Models;

namespace RavenDbTest.Helpers
{
    public class ItemByDesc : AbstractIndexCreationTask<Item>
    {
        public ItemByDesc()
        {
            Map = movies => from movie in movies
                            select new { movie.Desc };
            Index(x => x.Desc, FieldIndexing.Analyzed);
        }
    }
}