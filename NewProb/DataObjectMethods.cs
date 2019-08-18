using System;
using SimMetricsMetricUtilities;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.Data;

namespace NewProb
{
	public class DataObjectMethods
	{
		public static DataTable GetDataTableFromCSVFile(string csv_file_path)
		{
			DataTable csvData = new DataTable();

			try
			{
				using(TextFieldParser csvReader = new TextFieldParser(csv_file_path))
				{
					csvReader.SetDelimiters(new string[] { "," });
					csvReader.HasFieldsEnclosedInQuotes = false;
					string[] colFields = csvReader.ReadFields();

					foreach (string column in colFields)
					{
						DataColumn datecolumn = new DataColumn(column);
						datecolumn.AllowDBNull = true;
						csvData.Columns.Add(datecolumn);
					}

					while (!csvReader.EndOfData)
					{
						string[] fieldData = csvReader.ReadFields();
						//Making empty value as null
						for (int i = 0; i < fieldData.Length; i++)
						{
							if (fieldData[i] == "")
							{
								fieldData[i] = null;
							}
						}

						csvData.Rows.Add(fieldData);
					}
				}
			}
			catch (Exception ex)
			{
			}

			return csvData;
		}

		public static List<DataObjects.person_identifiers> GetListFromTable(DataTable dt) {
			var identifiers = new List<DataObjects.person_identifiers> (dt.Rows.Count);
			foreach (DataRow row in dt.Rows) {
				var pi = new DataObjects.person_identifiers ();
				pi.person_unique_entity_id = row ["internal_id"].ToString ();
				pi.person_first_name = row ["first_name"].ToString ();
				pi.person_last_name = row ["last_name"].ToString ();
				pi.person_gender = row ["gender"].ToString ();
				pi.person_dob_month = row ["dob_month"].ToString ();
				pi.person_dob_day = row ["dob_day"].ToString ();
				pi.person_dob_year = row ["dob_year"].ToString ();
				pi.person_fips_5 = row ["fips"].ToString ();
				identifiers.Add (pi);
			}
			return identifiers;
		}
	}
}

