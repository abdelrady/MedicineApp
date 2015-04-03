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


public class Index_Auto_2fNotes_2fByUserId : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_2fNotes_2fByUserId()
	{
		this.ViewText = @"from doc in docs.Notes
select new { UserId = doc.UserId }";
		this.ForEntityNames.Add("Notes");
		this.AddMapDefinition(docs => 
			from doc in docs
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "Notes", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				UserId = doc.UserId,
				__document_id = doc.__document_id
			});
		this.AddField("UserId");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("UserId");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("UserId");
		this.AddQueryParameterForReduce("__document_id");
	}
}
