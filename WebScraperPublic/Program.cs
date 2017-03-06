using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            SQLInsert();
        }
        public static void SQLInsert()
        {
            DataTable MyDataTable = GetData();

            using (SqlConnection sqlconnection = new SqlConnection(@"Data Source=MYPC\SQLExpress; Initial Catalog=MyTable;User ID=Username;Password=Password;"))
            {
                sqlconnection.Open();
                // Copy the DataTable to SQL Server Table using SqlBulkCopy
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlconnection))
                {
                    sqlBulkCopy.DestinationTableName = "RestaurantTable";

                    foreach (var column in MyDataTable.Columns)
                        sqlBulkCopy.ColumnMappings.Add(column.ToString(), column.ToString());

                    sqlBulkCopy.WriteToServer(MyDataTable);
                }
            }
        }


        static DataTable CreateDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Column", typeof(string));
            table.Columns.Add("Column 0", typeof(string));
            table.Columns.Add("Column 1", typeof(string));
            table.Columns.Add("Column 2", typeof(string));
            table.Columns.Add("Column 3", typeof(string));
            table.Columns.Add("Column 4", typeof(string));
            table.Columns.Add("Column 5", typeof(string));
            table.Columns.Add("Column 6", typeof(string));
            table.Columns.Add("Column 7", typeof(string));
            table.Columns.Add("Column 8", typeof(string));
            table.Columns.Add("Column 9", typeof(string));
            table.Columns.Add("Column 10", typeof(string));
            return table;
        }
        public static DataTable GetData()
        {
            //Create new datatable to write data to
            DataTable MyDataTable = CreateDataTable();
            //Website with a space search to return all restaurant names
            string main_url = @"URL with Data";
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument mainpage = new HtmlDocument();
            mainpage = hw.Load(main_url);
            //Iterate through all of the links in the list above.  Manually setting the limit, with i, for now, but need to determine
            // the best way to find the max value
            for (int i = 1; i < 5400; i++)
            {
                //Create an HTMLNodeCollection item based on the first xpath.  May require additional work if the xpath isnt numeric (1-10 etc.)
                HtmlNodeCollection FirstXpath = mainpage.DocumentNode.SelectNodes("[" + i + "]/a");
                //Make sure there is data there
                if (FirstXpath != null)
                {
                    //Pull the "href" attribute (the link) from the first website from the specified xpath 
                    string InspectionURL = "Your Website" + FirstXpath[0].Attributes["href"].Value;
                    //Get the link name.  In this example I am naming all of the first columns after the first link's innertext value
                    string LinkName = FirstXpath[0].InnerText;
                    //In my test case this value contained two values seperated by a comma.  I split them to form the values for column 1 and 2
                    string[] Name = LinkName.Split(',');
                    DataRow row = MyDataTable.NewRow();
                    MyDataTable.Rows.Add(row);
                    MyDataTable.Rows[i - 1]["Table 1"] = Name[0];
                    MyDataTable.Rows[i - 1]["Table 2"] = Name[1].Substring(2);
                    HtmlDocument SecondLink = new HtmlDocument();
                    SecondLink = hw.Load(InspectionURL);
                    //Iterate through the 5 or so inspection pages and pull the results down.
                    Console.WriteLine("Parsing record " + i + " of " + "5400");
                    Console.WriteLine("" + Name[0]);
                    for (int ii = 1; ii < 10;)
                    {
                        //Go to each row to get the links data located on the ii row.  Do this 9 times
                        HtmlNodeCollection SecondLinkData = SecondLink.DocumentNode.SelectNodes("");
                        //Make sure there is data there
                        if (SecondLinkData != null)
                        {
                            //Go to the second page
                            string SecondURL = "URL" + SecondLinkData[0].Attributes["href"].Value;
                            HtmlDocument SecondPage = hw.Load(SecondURL);
                            //Grab the text info from the second URL
                            HtmlNodeCollection SecondXpath = SecondPage.DocumentNode.SelectNodes("");
                            //Make sure there is data there
                            if (SecondXpath != null)
                            //Write the text data of the report to the data table line
                            {
                                MyDataTable.Rows[i - 1][ + ii] = SecondXpath[0].InnerText;
                            }
                            //Continue the loop
                            ii++;
                        }
                        //End the loop
                        else
                        {
                            ii = 10;
                        }

                    }

                }
                //End the loop
                {
                    i = 5402;
                }
            }
            return MyDataTable;
        }
    }
}


