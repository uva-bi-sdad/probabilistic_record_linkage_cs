using System;
using System.Collections.Generic;

namespace NewProb
{
	public class TestData
	{
		public static List<string> match_fields ()
		{
			var match_fields = new List<string> ();
			match_fields.Add ("person_first_name");
			match_fields.Add ("person_last_name");
			match_fields.Add ("person_dob_day");
			match_fields.Add ("person_dob_month");
			match_fields.Add ("person_dob_year");
			match_fields.Add ("person_fips_5");

			return match_fields;
		}

		public static IDictionary<string,DataObjects.parameter> match_field_parameters ()
		{
			var parameters = new Dictionary<string,DataObjects.parameter> ();
			var column = "";

			var parameter_1 = new DataObjects.parameter ();
			column = "person_first_name";
			parameter_1.columnname = column;
			parameter_1.column_m = 0.97;
			parameter_1.column_u = 0.001;
			parameters.Add(column, parameter_1);

			var parameter_2 = new DataObjects.parameter ();
			column= "person_last_name";
			parameter_2.columnname = column;
			parameter_2.column_m = 0.99;
			parameter_2.column_u = 0.001;
			parameters.Add (column, parameter_2);

			var parameter_3 = new DataObjects.parameter ();
			column= "person_dob_day";
			parameter_3.columnname = column;
			parameter_3.column_m = 0.95;
			parameter_3.column_u = 0.0323;
			parameters.Add (column, parameter_3);

			var parameter_4 = new DataObjects.parameter ();
			column= "person_dob_month";
			parameter_4.columnname = column;
			parameter_4.column_m = 0.99;
			parameter_4.column_u = 0.0833;
			parameters.Add (column, parameter_4);

			var parameter_5 = new DataObjects.parameter ();
			column= "person_dob_year";
			parameter_5.columnname = column;
			parameter_5.column_m = 0.99;
			parameter_5.column_u = 0.1;
			parameters.Add (column, parameter_5);

			var parameter_6 = new DataObjects.parameter ();
			column = "person_fips_5";
			parameter_6.columnname = column;
			parameter_6.column_m = 0.8;
			parameter_6.column_u = 0.05;
			parameters.Add (column, parameter_6);

			var parameter_7 = new DataObjects.parameter ();
			column= "gender";
			parameter_7.columnname = column;
			parameter_7.column_m = 0.99;
			parameter_7.column_u = 0.5;
			parameters.Add (column, parameter_7);

			return parameters;
		}

		public static List<DataObjects.person_identifiers> identifiers_1 ()
		{
			var identifiers = new List<DataObjects.person_identifiers> ();

			var pi_1 = new DataObjects.person_identifiers ();
			pi_1.person_unique_entity_id = "218493";
			pi_1.person_first_name = "Cameron";
			pi_1.person_last_name = "Bryzinski";
			pi_1.person_dob_month = "4";
			pi_1.person_dob_day = "16";
			pi_1.person_dob_year = "2002";
			pi_1.person_gender = "M";
			pi_1.person_fips_5 = "57500";
			identifiers.Add (pi_1);

			var pi_2 = new DataObjects.person_identifiers ();
			pi_2.person_unique_entity_id = "218494";
			pi_2.person_first_name = "Carson";
			pi_2.person_last_name = "Alan";
			pi_2.person_dob_month = "5";
			pi_2.person_dob_day = "18";
			pi_2.person_dob_year = "2003";
			pi_2.person_gender = "M'";
			pi_2.person_fips_5 = "57500";
			identifiers.Add (pi_2);

			return identifiers;
		}

		public static List<DataObjects.person_identifiers> identifiers_2 ()
		{
			var identifiers = new List<DataObjects.person_identifiers> ();

			var pi_1 = new DataObjects.person_identifiers ();
			pi_1.person_unique_entity_id = "3832";
			pi_1.person_first_name = "Cameron";
			pi_1.person_last_name = "Bryzinski";
			pi_1.person_dob_month = "4";
			pi_1.person_dob_day = "16";
			pi_1.person_dob_year = "2002";
			pi_1.person_gender = "M";
			pi_1.person_fips_5 = "57400";
			identifiers.Add (pi_1);

			var pi_2 = new DataObjects.person_identifiers ();
			pi_2.person_unique_entity_id = "3833";
			pi_2.person_first_name = "Carsoon";
			pi_2.person_last_name = "Alan";
			pi_2.person_dob_month = "3";
			pi_2.person_dob_day = "18";
			pi_2.person_dob_year = "2003";
			pi_2.person_gender = "M";
			pi_2.person_fips_5 = "57300";
			identifiers.Add (pi_2);

			return identifiers;
		}

		public static List<Blocks.blocking_scheme> blocking_schemes ()
		{
			var blocking_schemes = new List<Blocks.blocking_scheme> ();

			var blocking_scheme_1 = new Blocks.blocking_scheme ();
			blocking_scheme_1.blocking_scheme_id = 1;
			blocking_scheme_1.blocking_scheme_order = 1;
			blocking_scheme_1.blocking_scheme_type = "PERSON";
			blocking_scheme_1.identifier_1 = "person_first_name";
			blocking_scheme_1.identifier_2 = "person_last_name";
			blocking_schemes.Add (blocking_scheme_1);

			var blocking_scheme_2 = new Blocks.blocking_scheme ();
			blocking_scheme_2.blocking_scheme_id = 1;
			blocking_scheme_2.blocking_scheme_order = 1;
			blocking_scheme_2.blocking_scheme_type = "PERSON";
			blocking_scheme_2.identifier_1 = "person_first_name";
			blocking_scheme_2.identifier_2 = "person_dob_month_year";
			blocking_schemes.Add (blocking_scheme_2);

			var blocking_scheme_3 = new Blocks.blocking_scheme ();
			blocking_scheme_3.blocking_scheme_id = 1;
			blocking_scheme_3.blocking_scheme_order = 1;
			blocking_scheme_3.blocking_scheme_type = "PERSON";
			blocking_scheme_3.identifier_1 = "person_last_name";
			blocking_scheme_3.identifier_2 = "person_dob_month_year";
			blocking_schemes.Add (blocking_scheme_3);

			return blocking_schemes;
		}

		public static List<Blocks.blocking_match> blockMatches ()
		{
			var block_matches = new List<Blocks.blocking_match> ();

			var block_match_1 = new Blocks.blocking_match ();
			block_match_1.person_unique_entity_id_1 = "A";
			block_match_1.person_unique_entity_id_2 = "B";
			block_match_1.person_first_name = 1;
			block_match_1.person_last_name = 1;
			block_match_1.person_dob_day = 1;
			block_match_1.person_dob_month = 1;
			block_match_1.person_dob_year = 1;
			block_match_1.person_fips_5 = 1;
			block_matches.Add (block_match_1);

			var block_match_2 = new Blocks.blocking_match ();
			block_match_2.person_unique_entity_id_1 = "C";
			block_match_2.person_unique_entity_id_2 = "D";
			block_match_2.person_first_name = 0;
			block_match_2.person_last_name = 1;
			block_match_2.person_dob_day = 1;
			block_match_2.person_dob_month = 1;
			block_match_2.person_dob_year = 1;
			block_match_2.person_fips_5 = 0;
			block_matches.Add (block_match_2);

			var block_match_3 = new Blocks.blocking_match ();
			block_match_3.person_unique_entity_id_1 = "E";
			block_match_3.person_unique_entity_id_2 = "F";
			block_match_3.person_first_name = 1;
			block_match_3.person_last_name = 0;
			block_match_3.person_dob_day = 1;
			block_match_3.person_dob_month = 1;
			block_match_3.person_dob_year = 1;
			block_match_3.person_fips_5 = 1;
			block_matches.Add (block_match_3);

			return block_matches;
		}
	}
}

