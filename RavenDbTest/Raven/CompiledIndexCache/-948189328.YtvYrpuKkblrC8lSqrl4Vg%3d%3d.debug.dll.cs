using Raven.Abstractions;
using Raven.Database.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Raven.Database.Linq.PrivateExtensions;
using Lucene.Net.Documents;
using System.Globalization;
using System.Text.RegularExpressions;
using Raven.Database.Indexing;


public class Index_ProblemByDesc : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_ProblemByDesc()
	{
		this.ViewText = @"docs.Problems.Select(problem => new {
    Description = problem.Description
})";
		this.ForEntityNames.Add("Problems");
		this.AddMapDefinition(docs => docs.Where(__document => string.Equals(__document["@metadata"]["Raven-Entity-Name"], "Problems", System.StringComparison.InvariantCultureIgnoreCase)).Select((Func<dynamic, dynamic>)(problem => new {
			Description = problem.Description,
			__document_id = problem.__document_id
		})));
		this.AddField("Description");
		this.AddField("__document_id");
	}
}
