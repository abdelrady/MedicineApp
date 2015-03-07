using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using RavenDbTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RavenDbTest.Helpers
{
    public class ProblemByDesc : AbstractIndexCreationTask<Problem>
    {
        public ProblemByDesc()
        {
            Map = problems => from problem in problems
                              select new { problem.Description };
            Index(x => x.Description, FieldIndexing.Analyzed);
        }
    }
}