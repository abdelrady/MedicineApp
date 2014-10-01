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


public class Index_ItemByDesc : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_ItemByDesc()
	{
		this.ViewText = @"docs.Items.Select(movie => new {
    Desc = movie.Desc
})";
		this.ForEntityNames.Add("Items");
		this.AddMapDefinition(docs => docs.Where(__document => string.Equals(__document["@metadata"]["Raven-Entity-Name"], "Items", System.StringComparison.InvariantCultureIgnoreCase)).Select((Func<dynamic, dynamic>)(movie => new {
			Desc = movie.Desc,
			__document_id = movie.__document_id
		})));
		this.AddField("Desc");
		this.AddField("__document_id");
	}
}
