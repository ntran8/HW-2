// See https://aka.ms/new-console-template for more information


using System;
using System.Data;
using Npgsql;

class Sample
{
    static void Main(string[] args)
    {
        // Connect to a PostgreSQL database
        NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1:5432;User Id=postgres; " +
           "Password=daystodream123;Database=prods;");
        conn.Open();

        // Define a query returning a single row result set
        NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM product", conn);

        // Execute the query and obtain the value of the first column of the first row
        //Int64 count = (Int64)command.ExecuteScalar();
        NpgsqlDataReader reader = command.ExecuteReader();

        DataTable dt = new DataTable();
        dt.Load(reader);

        print_results_ques_7(dt);

        conn.Close();
    }

    static void print_results_ques_7(DataTable data)
    {
        DataTable new_Table = new DataTable();
        foreach (DataColumn col in data.Columns)
        {
            if (col.ColumnName.Contains("prod_id") || col.ColumnName.Contains("prod_desc") || col.ColumnName.Contains("prod_quantity"))
            {
                new_Table.Columns.Add(col.ColumnName, col.DataType);
            }
        }

        foreach (DataRow dataRow in data.Rows)
        {
            int prod_quantity = Convert.ToInt32(dataRow["prod_quantity"]);
            if (prod_quantity >= 12 && prod_quantity <= 30)
            {
                DataRow new_Row = new_Table.NewRow();
                foreach (DataColumn col in new_Table.Columns)
                {
                    new_Row[col.ColumnName] = dataRow[col.ColumnName];
                }
                new_Table.Rows.Add(new_Row);
            }
        }
        data = new_Table;
        Console.WriteLine();
        Dictionary<string, int> colWidths = new Dictionary<string, int>();

        foreach (DataColumn col in data.Columns)
        {
            Console.Write(col.ColumnName);
            var maxLabelSize = data.Rows.OfType<DataRow>()
                    .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                    .OrderByDescending(m => m).FirstOrDefault();

            colWidths.Add(col.ColumnName, maxLabelSize);
            for (int i = 0; i < maxLabelSize - col.ColumnName.Length + 14; i++) Console.Write(" ");
        }

        Console.WriteLine();

        foreach (DataRow dataRow in data.Rows)
        {
            for (int j = 0; j < dataRow.ItemArray.Length; j++)
            {
                Console.Write(dataRow.ItemArray[j]);
                for (int i = 0; i < colWidths[data.Columns[j].ColumnName] - dataRow.ItemArray[j].ToString().Length + 14; i++) Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}


