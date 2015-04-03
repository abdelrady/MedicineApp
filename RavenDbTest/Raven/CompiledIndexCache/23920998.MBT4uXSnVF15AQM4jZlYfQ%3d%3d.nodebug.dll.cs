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


public class Index_Auto_2fNotesUsers_2fByEmailAndPassword : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_2fNotesUsers_2fByEmailAndPassword()
	{
		this.ViewText = @"from doc in docs.NotesUsers
select new { Password = doc.Password, Email = doc.Email }";
		this.ForEntityNames.Add("NotesUsers");
		this.AddMapDefinition(docs => 
			from doc in docs
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "NotesUsers", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				Password = doc.Password,
				Email = doc.Email,
				__document_id = doc.__document_id
			});
		this.AddField("Password");
		this.AddField("Email");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("Password");
		this.AddQueryParameterForMap("Email");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("Password");
		this.AddQueryParameterForReduce("Email");
		this.AddQueryParameterForReduce("__document_id");
	}
}
