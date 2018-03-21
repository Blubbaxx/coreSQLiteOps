using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;



//###############################################################################################################################################
//#                                                                                                                                             #
//#                                     coreSQLiteOps - coreSQLiteDelete                                                                        #
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
    public class coreSQLiteDelete
    {
        /// <summary>
        /// This operation runs a DELETE command for a row of a table in any SQLite database. This returns a boolean to check the success of the operation.
        /// </summary>
        /// <param name="database">String with a path to a SQLite Database.</param>
        /// <param name="table">String with the name of the table where you want to delete something.</param>
        /// <param name="pkName">String with the name of the PrimaryKey field to find your requested row.</param>
        /// <param name="id">String with the content of the value of the PK to select the right row.</param>
        /// <param name="debug">If set to true, you will see some more information in the console. Handle with care!</param>
        /// <returns></returns>
        public static bool run(string database, string table, string pkName, string id, bool debug)
        {
            bool result = false;

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
                    var deleteCommand = connection.CreateCommand();
                    deleteCommand.Transaction = transaction;
                    deleteCommand.CommandText = "DELETE FROM " + table + " WHERE " + pkName + " = '" + id + "'";

                    //Debug -> output SQL command inf debug = true
                    if (debug == true)
                    {
                        Console.WriteLine(deleteCommand.CommandText);
                    }

                    deleteCommand.ExecuteNonQuery();
                    transaction.Commit();

                }

                connection.Close();
                connection.Dispose();
            }

            catch(Exception ex)
            {
                result = false;

                //Print Exception if debug = true
                if (debug == true)
                {
                    Console.Write(ex);
                }
            }

            return result;
        }

    }
}
