using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace NewProb
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine(DateTime.Now.ToShortTimeString ());

			var stopwatch = new Stopwatch ();
			stopwatch.Start ();

			var match_fields = TestData.match_fields ();
			var parameters = TestData.match_field_parameters ();

			var dt1 = DataObjectMethods.GetDataTableFromCSVFile ("vdss_dmg_log_reduced_alphanum.csv");
			Console.WriteLine ("Data table 1 loaded");
			Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

			var dt2 = DataObjectMethods.GetDataTableFromCSVFile ("vdoe_dmg_log_reduced_5000.csv");
			Console.WriteLine ("Data table 2 loaded");
			Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

			var n_1 = dt1.Rows.Count;
			var n_2 = dt2.Rows.Count;

			var identifiers_1 = DataObjectMethods.GetListFromTable (dt1);
			Console.WriteLine ("identifiers_1 loaded");
			Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

			var identifiers_2 = DataObjectMethods.GetListFromTable (dt2);
			Console.WriteLine ("identifiers_2 loaded");
			Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

			var block_ids = Blocks.MakeBlocks (identifiers_1, identifiers_2);
			Console.WriteLine ("Blocks made");

			var pairs = Blocks.MakeIdentifiersPairList (block_ids, identifiers_1, identifiers_2);

			Console.WriteLine ("Starting block match");
			var block_matches = Blocks.MakeBlockingMatches (pairs, match_fields);
  			Console.WriteLine ("Block match complete");


			Console.WriteLine ("Generating probabilities");
			var probabilities = MatchProb.GetProbabilities (block_matches, match_fields, parameters, n_1, n_2);
			Console.WriteLine("Probabilities generated");
			Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

			Console.WriteLine ("Creating file");
//			var rows = new List<string> ();
			TextWriter tw = new StreamWriter ("match_probabilities.csv");

			var head = new List<string> ();
			head.Add ("probability");
			head.Add ("unique_entity_id_1");
			head.Add ("unique_entity_id_2");
			head.Add ("person_first_name");
			head.Add ("person_last_name");
			head.Add ("person_dob_month");
			head.Add ("person_dob_day");
			head.Add ("person_dob_year");
			head.Add ("person_fips_5");
			head.Add ("person_first_name_2");
			head.Add ("person_last_name_2");
			head.Add ("person_dob_month_2");
			head.Add ("person_dob_day_2");
			head.Add ("person_dob_year_2");
			head.Add ("person_fips_5_2");
			head.Add ("match_first_name");
			head.Add ("match_last_name");
			head.Add ("match_dob_month");
			head.Add ("match_dob_day");
			head.Add ("match_dob_year");
			head.Add ("match_fips_5");
			tw.WriteLine(string.Join(",", head));

			foreach (DataObjects.probability p in probabilities) {
				var row = new List<string> ();
				row.Add (p.prob.ToString());
				row.Add (p.unique_entity_id_1.ToString());
				row.Add (p.unique_entity_id_2.ToString());
				row.Add (p.person_identifiers_pair.person_identifiers_1.person_first_name);
				row.Add (p.person_identifiers_pair.person_identifiers_1.person_last_name);
				row.Add (p.person_identifiers_pair.person_identifiers_1.person_dob_month);
				row.Add (p.person_identifiers_pair.person_identifiers_1.person_dob_day);
				row.Add (p.person_identifiers_pair.person_identifiers_1.person_dob_year);
				row.Add (p.person_identifiers_pair.person_identifiers_1.person_fips_5);
				row.Add (p.person_identifiers_pair.person_identifiers_2.person_first_name);
				row.Add (p.person_identifiers_pair.person_identifiers_2.person_last_name);
				row.Add (p.person_identifiers_pair.person_identifiers_2.person_dob_month);
				row.Add (p.person_identifiers_pair.person_identifiers_2.person_dob_day);
				row.Add (p.person_identifiers_pair.person_identifiers_2.person_dob_year);
				row.Add (p.person_identifiers_pair.person_identifiers_2.person_fips_5);
				row.Add (p.matchVector.person_first_name.ToString());
				row.Add (p.matchVector.person_last_name.ToString());
				row.Add (p.matchVector.person_dob_month.ToString());
				row.Add (p.matchVector.person_dob_day.ToString());
				row.Add (p.matchVector.person_dob_year.ToString());
				row.Add (p.matchVector.person_fips_5.ToString());
				tw.WriteLine (string.Join (",", row));
//				rows.Add(string.Join(",", row));
			}
			tw.Close ();
			stopwatch.Stop ();
			Console.WriteLine("Total time elapsed: {0}", stopwatch.Elapsed);
		}
	}
}