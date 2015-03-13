using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data.SQLite;

namespace Tagger.Database
{
    static public class Manager
    {
        public static DbConnection Connection;
        public static DbCommand Command;

        public static void Initialize(string filename)
        {
            Connection = new SQLiteConnection("Data Source=" + filename + ";Version=3");
            Connection.Open();
            Command = Connection.CreateCommand();

            Command.CommandText = "create table if not exists files (id INTEGER PRIMARY KEY, name TEXT, hash TEXT, thumbnail BLOB, modtime TIMESTAMP);";
            Command.ExecuteNonQuery();

            Command.CommandText = "create table if not exists filetags (file_id INTEGER, tag_id INTEGER);";
            Command.ExecuteNonQuery();

            Command.CommandText = "create table if not exists tags (id INTEGER PRIMARY KEY, name TEXT, color INTEGER, description TEXT);";
            Command.ExecuteNonQuery();
        }

        public static void Terminate()
        {
            Command = null;
            Connection.Close();
            Connection = null;
        }
    }
}
