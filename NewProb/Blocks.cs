using System;
using System.Collections.Generic;
using System.Linq;
using SimMetricsMetricUtilities;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace NewProb
{
	public class Blocks
	{
		// Objects

		public class blocking_scheme
		{
			public int blocking_scheme_id { get; set; }
			public string blocking_scheme_type { get; set; }
			public int blocking_scheme_order { get; set; }
			public string identifier_1 { get; set; }
			public string identifier_2 { get; set; }
			public DateTime created_date { get; set; }
			public DateTime modified_data { get; set; }
		}

		public class blocking_ids
		{
			public string unique_entity_id_1 { get; set; }
			public string unique_entity_id_2 { get; set; }
			public DateTime created_date { get; set; }
		}

		public class blocking_match
		{
			public string person_unique_entity_id_1 { get; set; }
			public string person_unique_entity_id_2 { get; set; }
			public int person_first_name { get; set; }
			public int person_middle_names { get; set; }
			public int person_last_name { get; set; }
			public int person_gender { get; set; }
			public int person_dob_day { get; set; }
			public int person_dob_month { get; set; }
			public int person_dob_year { get; set; }
			public int person_dob_month_year { get; set; }
			public int person_fips_5 { get; set; }
			public int person_phone { get; set; }
			public int person_email { get; set; }
			public int person_match_id_1 { get; set; }
			public int person_match_id_2 { get; set; }
			public int person_match_id_3 { get; set; }
			public DateTime created_date { get; set; }
			public DataObjects.person_identifiers_pair person_identifiers_pair { get; set; }

			public blocking_match (){
				person_identifiers_pair = new DataObjects.person_identifiers_pair ();
			}
		}

		// Methods

		public static IEnumerable<blocking_ids> MakeBlocks(List<DataObjects.person_identifiers> ids_1, List<DataObjects.person_identifiers> ids_2)
		{
			Console.WriteLine ("Starting block 1");
			var blocking_ids_1 = (
                from x in ids_1
                from y in ids_2
                where x.person_first_name == y.person_first_name
                where x.person_last_name == y.person_last_name
                select new blocking_ids () {
				unique_entity_id_1 = x.person_unique_entity_id,
				unique_entity_id_2 = y.person_unique_entity_id
				});
			var blocking_ids = blocking_ids_1;

			Console.WriteLine ("Starting block 2");
			var blocking_ids_2 = (
                from x in ids_1
                from y in ids_2
                where x.person_first_name == y.person_first_name
                where x.person_dob_month == y.person_dob_month
                where x.person_dob_year == y.person_dob_year
                select new blocking_ids () {
				unique_entity_id_1 = x.person_unique_entity_id,
				unique_entity_id_2 = y.person_unique_entity_id
			});
			blocking_ids = blocking_ids.Concat (blocking_ids_2);

			Console.WriteLine ("Starting block 3");
			var blocking_ids_3 = (
			 	from x in ids_1
			 	from y in ids_2
			 	where x.person_last_name == y.person_last_name
			 	where x.person_dob_month == y.person_dob_month
			 	where x.person_dob_year == y.person_dob_year
			 	select new blocking_ids () {
				unique_entity_id_1 = x.person_unique_entity_id,
				unique_entity_id_2 = y.person_unique_entity_id
			});
			blocking_ids = blocking_ids.Concat (blocking_ids_3);

//			Console.WriteLine ("Starting block 4");
//			var blocking_ids_4 = (
//				from x in ids_1
//				from y in ids_2
//				where x.person_dob_day == y.person_dob_day
//				where x.person_dob_month == y.person_dob_month
//				where x.person_dob_year == y.person_dob_year
//				select new blocking_ids () {
//					unique_entity_id_1 = x.person_unique_entity_id,
//					unique_entity_id_2 = y.person_unique_entity_id
//				});
//			blocking_ids = blocking_ids.Concat (blocking_ids_4);

			Console.WriteLine ("Eliminating Block Duplicates");
			var distinct_blocking_ids = blocking_ids.Distinct (new Blocking_Ids_Comparer());

			return distinct_blocking_ids;


		}

		public class Blocking_Ids_Comparer : IEqualityComparer<blocking_ids>
		{
			public bool Equals(blocking_ids x, blocking_ids y)
			{
				return x.unique_entity_id_1.Equals(y.unique_entity_id_1) & x.unique_entity_id_2.Equals(y.unique_entity_id_2);
			}

			public int GetHashCode(blocking_ids obj)
			{
				int hash = 17;
				hash = hash * 23 + (obj.unique_entity_id_1 ?? "").GetHashCode();
				hash = hash * 23 + (obj.unique_entity_id_2 ?? "").GetHashCode();
				return hash;
			}
		}

		public static IEnumerable<DataObjects.person_identifiers_pair> MakeIdentifiersPairList(IEnumerable<blocking_ids> bids, List<DataObjects.person_identifiers> ids_1, List<DataObjects.person_identifiers> ids_2)
		{
			foreach (var b in bids) {
				var first =
					from i_1 in ids_1
					where i_1.person_unique_entity_id == b.unique_entity_id_1
					select i_1;

				var second = 
					from i_2 in ids_2
					where i_2.person_unique_entity_id == b.unique_entity_id_2
					select i_2;

				var pair = new DataObjects.person_identifiers_pair ();
				pair.person_identifiers_1 = first.FirstOrDefault ();
				pair.person_identifiers_2 = second.FirstOrDefault ();

				yield return pair;
			}
		}

		public static List<Blocks.blocking_match> MakeBlockingMatches(IEnumerable<DataObjects.person_identifiers_pair> pairs, List<string> matchFields)
		{
			var jw = new JaroWinkler ();
			var matchVectors = new ConcurrentBag<Blocks.blocking_match>();

			Parallel.ForEach (pairs, pair => {
				var matchVector = new Blocks.blocking_match ();
				matchVector.person_unique_entity_id_1 = pair.person_identifiers_1.person_unique_entity_id;
				matchVector.person_unique_entity_id_2 = pair.person_identifiers_2.person_unique_entity_id;
				matchVector.person_first_name = matchFields.Contains ("person_first_name") 
					&& jw.GetSimilarity (pair.person_identifiers_1.person_first_name, pair.person_identifiers_2.person_first_name) > .90 ? 1 : 0;
				matchVector.person_middle_names = matchFields.Contains ("person_middle_names") 
					&& jw.GetSimilarity (pair.person_identifiers_1.person_middle_names, pair.person_identifiers_2.person_middle_names) > .9 ? 1 : 0;
				matchVector.person_last_name = matchFields.Contains ("person_last_name") 
					&& jw.GetSimilarity (pair.person_identifiers_1.person_last_name, pair.person_identifiers_2.person_last_name) > .90 ? 1 : 0;
				matchVector.person_dob_day = matchFields.Contains ("person_dob_day") 
					&& Int16.Parse(pair.person_identifiers_1.person_dob_day) == Int16.Parse(pair.person_identifiers_2.person_dob_day) ? 1 : 0;
				matchVector.person_dob_month = matchFields.Contains ("person_dob_month") 
					&& Int16.Parse(pair.person_identifiers_1.person_dob_month) == Int16.Parse(pair.person_identifiers_2.person_dob_month) ? 1 : 0;
				matchVector.person_dob_year = matchFields.Contains ("person_dob_year") 
					&& Int16.Parse(pair.person_identifiers_1.person_dob_year) == Int16.Parse(pair.person_identifiers_2.person_dob_year) ? 1 : 0;
				matchVector.person_gender = matchFields.Contains ("person_gender") 
					&& pair.person_identifiers_1.person_gender == pair.person_identifiers_2.person_gender ? 1 : 0;
				matchVector.person_fips_5 = matchFields.Contains ("person_fips_5") 
					&& pair.person_identifiers_1.person_fips_5 == pair.person_identifiers_2.person_fips_5 ? 1 : 0;
				matchVector.person_email = matchFields.Contains ("person_email") 
					&& pair.person_identifiers_1.person_email == pair.person_identifiers_2.person_email ? 1 : 0;
				matchVector.person_phone = matchFields.Contains ("person_phone") 
					&& pair.person_identifiers_1.person_phone == pair.person_identifiers_2.person_phone ? 1 : 0;
				matchVector.person_match_id_1 = matchFields.Contains ("person_match_id_1") 
					&& pair.person_identifiers_1.person_match_id_1 == pair.person_identifiers_2.person_match_id_1 ? 1 : 0;
				matchVector.person_match_id_2 = matchFields.Contains ("person_match_id_2") 
					&& pair.person_identifiers_1.person_match_id_2 == pair.person_identifiers_2.person_match_id_2 ? 1 : 0;
				matchVector.person_match_id_3 = matchFields.Contains ("person_match_id_3") 
					&& pair.person_identifiers_1.person_match_id_3 == pair.person_identifiers_2.person_match_id_3 ? 1 : 0;
				matchVector.person_identifiers_pair.person_identifiers_1 = pair.person_identifiers_1;
				matchVector.person_identifiers_pair.person_identifiers_2 = pair.person_identifiers_2;
				matchVectors.Add(matchVector);
			});

			return matchVectors.ToList<Blocks.blocking_match>();
		}

	}
}

