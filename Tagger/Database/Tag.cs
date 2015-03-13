using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Runtime.Serialization;

namespace Tagger.Database
{
    [Serializable()]
    public class Tag : ISerializable
    {
        public int Id;
        public String Name;
        public int Color;
        public String Description;
        public int ResultsCount;

        public Tag() { }

        public static LinkedList<Tag> GetAll()
        {
            LinkedList<Tag> tags = new LinkedList<Tag>();

            Manager.Command.CommandText = "select * from tags";
            using (DbDataReader reader = Manager.Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tag t = new Tag();
                    t.Id = reader.GetInt32(0);
                    t.Name = reader.GetString(1);
                    t.Color = reader.GetInt32(2);
                    t.Description = reader.GetString(3);
                    tags.AddLast(t);
                }
            }
            return tags;
        }

        public static Tag GetByName(String name)
        {
            Manager.Command.CommandText = "select * from tags where name = \"" + name.ToLower() + "\";";
            Tag t = null;
            using (DbDataReader reader = Manager.Command.ExecuteReader())
            {
                if (reader.Read())
                {
                    t = new Tag();
                    t.Id = reader.GetInt32(0);
                    t.Name = reader.GetString(1);
                    t.Color = reader.GetInt32(2);
                    t.Description = reader.GetString(3);
                }
            }
            return t;
        }

        public void Insert()
        {
            Manager.Command.CommandText = "insert into tags (name, color, description) values (\"" + this.Name.ToLower() + "\", " + this.Color + ", \"" + this.Description + "\"); SELECT last_insert_rowid();";
            this.Id = Convert.ToInt32(Manager.Command.ExecuteScalar());
        }

        public void Update()
        {
            Manager.Command.CommandText = "update tags set name = \"" + this.Name.ToLower() + "\", color = " + this.Color + ", description = \"" + this.Description + "\" WHERE id = " + this.Id + ";";
            Manager.Command.ExecuteNonQuery();
        }

        public void RemoveFiles()
        {
            Manager.Command.CommandText = "delete from filetags where tag_id = " + this.Id + ";";
            Manager.Command.ExecuteNonQuery();
        }

        public void Delete()
        {
            RemoveFiles();
            Manager.Command.CommandText = "delete from tags where id = " + this.Id + ";";
            Manager.Command.ExecuteNonQuery();
        }

        override public string ToString()
        {
            return this.Name;
        }

        #region ISerializable Members

        public Tag(SerializationInfo info, StreamingContext ctxt)
        {
            Id = (int)info.GetValue("TagID", typeof(int));
            Name = (String)info.GetValue("TagName", typeof(string));
            Color = (int)info.GetValue("TagColor", typeof(int));
            Description = (String)info.GetValue("TagDescription", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("TagID", Id);
            info.AddValue("TagName", Name);
            info.AddValue("TagColor", Color);
            info.AddValue("TagDescription", Description);
        }

        #endregion
    }
}
