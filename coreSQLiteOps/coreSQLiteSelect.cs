using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;


//###############################################################################################################################################
//#                                                                                                                                             #
//#                                     coreSQLiteOps - coreSQLiteSelect                                                                        #
//#                                                                                                                                             #
//#     Copyright (c) <2018> <Florian Lenz-Teufel>                                                                                              #            
//#     Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated                            #
//#     documentation files (the "Software"), to deal in the Software without restriction, including without limitation                         #
//#     the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,                            #
//#     and to permit persons to whom the Software is furnished to do so, subject to the following conditions:                                  #
//#                                                                                                                                             #
//#     The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.          #
//#                                                                                                                                             #
//#     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED                           #
//#     TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL                           #
//#     THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,                 #
//#     TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                #
//#                                                                                                                                             #
//#                                                                                                                                             #
//###############################################################################################################################################


namespace coreSQLiteOps
{
    public class coreSQLiteSelect
    {
        /// <summary>
        /// This operation runs a SELECT command for a single value to any SQLite Database. It returns a string with the requested value.
        /// </summary>
        /// <param name="database">String with a path to a SQLite Database.</param>
        /// <param name="field">String with a field name where you want to select something.</param>
        /// <param name="table">String with the name of the table where you want to select something.</param>
        /// <param name="pkName">String with the name of the PrimaryKey field to find your requested value</param>
        /// <param name="id">String with the content of the value of the PK to select the right row.</param>
        /// <param name="debug">If set to true, you will see some more information in the console. Handle with care!</param>
        /// <returns>string</returns>
        public static string run(string database, string field, string table, string pkName, string id, bool debug)
        {

            // Create the basic "result"
            string result = "false";

            try
            {
                // Create a SQLite connection
                SqliteConnection connection;

                connection = new SqliteConnection();
                connection.ConnectionString = "Data Source=" + database;
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {

                    //Create SQL command
                    var selectCommand = connection.CreateCommand();
                    selectCommand.Transaction = transaction;
                    selectCommand.CommandText = "SELECT " + field + " FROM " + table + " WHERE " + pkName + " = " + id;

                    //Debug -> output SQL command inf debug = true
                    if (debug == true)
                    {
                        Console.WriteLine(selectCommand.CommandText);
                    }

                    //Read the data content
                    using (var reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var read = reader.GetString(0);
                            result = Convert.ToString(read);
                        }
                    }

                    transaction.Commit();
                }


                connection.Close();
                connection.Dispose();

            }

            catch(Exception ex)
            {
                result = "false";

                //Print Exception if debug = true
                if (debug == true)
                {
                    Console.Write(ex);
                }
            }


            return result;

        }

        /// <summary>
        /// This operation runs a SELECT command for a field in a table of any database. This operation returns a string array with all values of a field in the selected table.
        /// </summary>
        /// <param name="database">String with a path to a SQLite Database.</param>
        /// <param name="field">String with a field name where you want to select something.</param>
        /// <param name="table">String with the name of the table where you want to select something.</param>
        /// <param name="debug">If set to true, you will see some more information in the console. Handle with care!</param>
        /// <returns></returns>
        public static string[] runStack(string database, string field, string table, bool debug)
        {

            string[] read;

            try
            {
                //Open new SQLite connection
                SqliteConnection connection;

                connection = new SqliteConnection();
                connection.ConnectionString = "Data Source=" + database;
                connection.Open();

                //Create a List with strings to store the database content while reading
                List<string> readCollection = new List<string>();
                

                using (var transaction = connection.BeginTransaction())
                {

                    //Create SQL command
                    var selectCommand = connection.CreateCommand();
                    selectCommand.Transaction = transaction;
                    selectCommand.CommandText = "SELECT " + field + " FROM "+ table;

                    //if debug = true, print the SQL command to the console
                    if (debug == true)
                    {
                        Console.WriteLine(selectCommand.CommandText);
                    }

                    using (var reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            readCollection.Add(Convert.ToString(reader[0]));
                        }
                    }

                    transaction.Commit();
                }

                connection.Close();
                connection.Dispose();

                //create a array with the readed list
                read = readCollection.ToArray();
  
            }

            catch(Exception ex)
            {
                read = new string[1];
                read[0] = "false";

                //if debug = true, print the Exception to the console.
                if (debug == true)
                {
                    Console.Write(ex);
                }
            }

            return read;

        }



        














    }
}
