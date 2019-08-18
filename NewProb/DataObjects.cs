using System;
using System.Collections.Generic;

namespace NewProb
{
	public class DataObjects
	{
		public class column_match_m_u
		{
			public string columnName { get; set; }
			public double match { get; set; }
			public double m_prob { get; set; }
			public double u_prob { get; set; }
		}

		public class probability
		{
			public string unique_entity_id_1 { get; set; }
			public string unique_entity_id_2 { get; set; }
			public string match_type_code { get; set; }
			public int blocking_scheme_id { get; set; }
			public float prob { get; set; }
			public DateTime created_date { get; set; }
			public DataObjects.person_identifiers_pair person_identifiers_pair { get; set; }
			public Blocks.blocking_match matchVector { get; set; }

			public probability (){
				person_identifiers_pair = new person_identifiers_pair ();
			}
		}

		public class parameter
		{
			public int match_demographic_log_config_id { get; set; }
			public double column_m { get; set; }
			public double column_u { get; set; }
			public string columnname { get; set; }
			public DateTime created_date { get; set; }
			public DateTime modified_data { get; set; }
		}

		public class person_identifiers
		{
			public string person_unique_entity_id { get; set; }
			public string person_first_name { get; set; }
			public string person_middle_names { get; set; }
			public string person_last_name { get; set; }
			public string person_gender { get; set; }
			public string person_dob_day { get; set; }
			public string person_dob_month { get; set; }
			public string person_dob_year { get; set; }
			public string person_dob_month_year { get; set; }
			public string person_fips_5 { get; set; }
			public string person_phone { get; set; }
			public string person_email { get; set; }
			public string person_match_id_1 { get; set; }
			public string person_match_id_2 { get; set; }
			public string person_match_id_3 { get; set; }
			public DateTime created_date { get; set; }
		}

		public class person_identifiers_pair
		{
			public person_identifiers person_identifiers_1 { get; set; }
			public person_identifiers person_identifiers_2 { get; set; }
		}
	}
}

