using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;
using SimMetricsUtilities;

namespace NewProb
{
	public class MatchProb
	{
		//Probability of a match between two records
		public static double p_match(List<DataObjects.column_match_m_u> matchVector, int E, int N1, int N2)
		{
			double p = 0.00;
			double x_rand = x_matchRandomly(E, N1, N2);
			double product_x_sub_i = x_rand;

			foreach (DataObjects.column_match_m_u matchColumn in matchVector)
			{  
				if (matchColumn.match > 1)
					product_x_sub_i = product_x_sub_i * 1;
				else if (matchColumn.match > 0.00)
					product_x_sub_i = product_x_sub_i * x_agree(matchColumn.m_prob, matchColumn.u_prob);
				else if (matchColumn.match <= 0.00)
					product_x_sub_i = product_x_sub_i * x_disagree(matchColumn.m_prob, matchColumn.u_prob);
			}

			p = product_x_sub_i / (product_x_sub_i + 1);
			return p;
		}

		// The odds that two records match randomly if one knew nothing
		// about agreement/disagreement in any field
		private static double x_matchRandomly(int E, int N1, int N2)
		{
			double x_return = E / (float)((N1 * N2) - E);
			return x_return;
		}

		// x sub i for when two fields match across sources
		private static double x_agree(double m, double u)
		{
			double x_return = m / u;
			return x_return;
		}

		//x sub i for when two fields do not match across sources
		private static double x_disagree(double m, double u)
		{
			double x_return = (1 - m) / (1 - u);
			return x_return;
		}

		public static List<DataObjects.column_match_m_u> transposeBlockMatch(Blocks.blocking_match blockMatch, List<string> matchColumns, IDictionary<String,DataObjects.parameter> parameters)
		{

			var colMatchMU = new List<DataObjects.column_match_m_u>();

			foreach (string colName in matchColumns)
			{
				var newMMU = new DataObjects.column_match_m_u();
				newMMU.columnName = colName;
				PropertyInfo pI = blockMatch.GetType().GetProperty(colName);
				int val = (int)pI.GetValue(blockMatch, null);
				newMMU.match = (double)val;
				newMMU.m_prob = parameters[colName].column_m;
				newMMU.u_prob = parameters[colName].column_u;
				colMatchMU.Add(newMMU);

			}

			return colMatchMU;
		}

		public static List<DataObjects.probability> GetProbabilities(List<Blocks.blocking_match> blockMatches, List<string> matchColumns, IDictionary<string,DataObjects.parameter> parameters,
			int identifiers_1_count, int identifiers_2_count){
			var probs = new ConcurrentBag<DataObjects.probability>();

			var now = DateTime.Now;

			int E = identifiers_1_count > identifiers_2_count ? identifiers_2_count : identifiers_1_count;

			Parallel.ForEach(blockMatches, blockMatch =>
				{
					List<DataObjects.column_match_m_u> matchVector = transposeBlockMatch(blockMatch, matchColumns, parameters);
					double probability = p_match(matchVector, E, identifiers_1_count, identifiers_2_count);

					var matchProb = new DataObjects.probability();
						matchProb.prob = (float)probability;
						matchProb.match_type_code = "P";
						matchProb.created_date = now;
						matchProb.unique_entity_id_1 = blockMatch.person_unique_entity_id_1;
						matchProb.unique_entity_id_2 = blockMatch.person_unique_entity_id_2;
						matchProb.person_identifiers_pair.person_identifiers_1 = blockMatch.person_identifiers_pair.person_identifiers_1;
						matchProb.person_identifiers_pair.person_identifiers_2 = blockMatch.person_identifiers_pair.person_identifiers_2;
						matchProb.matchVector = blockMatch;
					probs.Add(matchProb);

				});

			return probs.ToList<DataObjects.probability>();
		}
	}
}

