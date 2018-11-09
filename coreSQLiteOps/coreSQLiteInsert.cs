using System;
using Microsoft.Data.Sqlite;


//###############################################################################################################################################
//#                                                                                                                                             #
//#                                     coreSQLiteOps - coreSQLiteInsert                                                                        #
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
    public class coreSQLiteInsert
    {

        /// <summary>
        /// This operation runs a INSERT command to any SQLite Database. This returns a boolean to check the success of the operation.
        /// </summary>
        /// <param name="database">String with a path to a SQLite Database.</param>
        /// <param name="table">String with the name of the table where you want to insert something.</param>
        /// <param name="fields">String Array with the names of all table fields where you want to write</param>
        /// <param name="values">String Array with all values you want to insert. This array must have the same length as the fields array</param>
        /// <param name="debug">If set to true, you will see some more information in the console. Handle with care!</param>
        /// <returns>bool</returns>
        public static bool run(string database, string table, string[] fields, string[] values, bool debug)
        {

            // Create check variable to verify the action with debug = false
            bool result = false;


            try
            {
                // Create basic variables

                string fieldsString = "";
                string valuesString = "";

                int fieldsCount = fields.Length;
                int valuesCount = values.Length;

                // Check if every field has a value

                if (fieldsCount != valuesCount)
                {
                    result = false;
                    return result;
                }

                // Collect all fields in one string to use it in SQL command

                foreach (var item in fields)
                {
                    fieldsString = fieldsString + ", " + item;
                }

                foreach (var item in fields)
                {
                    valuesString = valuesString + ", $" + item;
                }

                fieldsString = fieldsString.Substring(2);
                valuesString = valuesString.Substring(2);


                // Start the SQLite Connection

                SqliteConnection connection;

                connection = new SqliteConnection();
                connection.ConnectionString = "Data Source=" + database;
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    //Create SQL command

                    var insertCommand = connection.CreateCommand();
                    insertCommand.Transaction = transaction;
                    insertCommand.CommandText = "INSERT INTO " + table + " ( " + fieldsString + " ) VALUES ( " + valuesString + " )";

                    // if debug = true, then print the SQL command string to the console

                    if (debug == true)
                    {
                        Console.WriteLine(insertCommand.CommandText);
                    }
                    
                    // Add values for the command string

                    for (int i = 0; i < fieldsCount; i++)
                    {
                        insertCommand.Parameters.AddWithValue("$" + fields[i], values[i]);
                    }

                    insertCommand.ExecuteNonQuery();
                    transaction.Commit();

                }

                connection.Close();
                connection.Dispose();

                result = true;
            }

            // Catch every exception during the process and if debug = true, print error to the console.

            catch (Exception ex)
            {
                if (debug == true)
                {
                    Console.Write(ex);
                }

                result = false;
            }

            return result;
        }






    }
}
