using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Drawing;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Drawing.Imaging;

namespace Tagger.Database
{
    public class File
    {
        public int Id;
        public string Name;
        public string Hash;
        public Bitmap Thumbnail;
        public string modtime;

        public static int CountAll()
        {
            Manager.Command.CommandText = "SELECT COUNT(*) FROM files";
            return Convert.ToInt32(Manager.Command.ExecuteScalar());
        }

        public static LinkedList<File> GetAll()
        {
            return GetAll(0, -1);
        }
        public static LinkedList<File> GetAll(int offset, int limit)
        {
            LinkedList<File> files = new LinkedList<File>();

            Manager.Command.CommandText = "SELECT * FROM files ORDER BY modtime LIMIT " + offset + ", " + limit + ";";
            using (DbDataReader reader = Manager.Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    File f = new File();
                    f.Id = reader.GetInt32(0);
                    f.Name = reader.GetString(1);
                    f.Hash = reader.GetString(2);
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream((byte[])reader.GetValue(3)))
                        f.Thumbnail = new Bitmap(ms);
                    f.modtime = reader.GetDateTime(4).ToString();
                    files.AddLast(f);
                }
            }
            return files;
        }

        public static File GetByName(string name)
        {
            Manager.Command.CommandText = "select * from files where name = \"" + name + "\";";
            File f = null;
            using (DbDataReader reader = Manager.Command.ExecuteReader())
            {
                if (reader.Read())
                {
                    f = new File();
                    f.Id = reader.GetInt32(0);
                    f.Name = reader.GetString(1);
                    f.Hash = reader.GetString(2);
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream((byte[])reader.GetValue(3)))
                        f.Thumbnail = new Bitmap(ms);
                }
            }
            return f;
        }

        public static bool ExistsByHash(string hash)
        {
            Manager.Command.CommandText = "select COUNT(*) from files where hash = \"" + hash + "\";";
            return (System.Convert.ToInt32(Manager.Command.ExecuteScalar()) != 0);
        }

        public static File GetByHash(string hash)
        {
            Manager.Command.CommandText = "select * from files where hash = \"" + hash + "\";";
            File f = null;
            using (DbDataReader reader = Manager.Command.ExecuteReader())
            {
                if (reader.Read())
                {
                    f = new File();
                    f.Id = reader.GetInt32(0);
                    f.Name = reader.GetString(1);
                    f.Hash = reader.GetString(2);
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream((byte[])reader.GetValue(3)))
                        f.Thumbnail = new Bitmap(ms);
                }
            }
            return f;
        }

        public static LinkedList<File> GetByTags(LinkedList<String> tags)
        {
            return GetByTags(tags, 0, -1);
        }

        public static LinkedList<File> GetByTags(LinkedList<String> tags, int offset, int limit)
        {
            LinkedList<File> files = new LinkedList<File>();

            String tag_list = "";
            String excluded_tag_list = "";
            int excluded_count = 0;

            foreach(String tag in tags)
            {
                string tag_firstchar = tag[0].ToString();
                if (tag_firstchar.ToString().Equals("-"))
                {
                    //Exclude Tags
                    excluded_tag_list += "'" + tag.Remove(0, 1) + "', ";
                    excluded_count++;
                }
                else tag_list += "'" + tag + "', ";
            }

            if (tag_list.Length > 0)
                tag_list = tag_list.Remove(tag_list.Length - 2);

            if (excluded_tag_list.Length > 0)
            {
                excluded_tag_list = excluded_tag_list.Remove(excluded_tag_list.Length - 2);

                Manager.Command.CommandText = "SELECT files.* FROM filetags, files, tags " +
                                              "WHERE filetags.tag_id = tags.id " +
                                              "AND (tags.name IN (" + tag_list + ")) " +
                                              "AND files.id = filetags.file_id " +
                                              "AND files.id NOT IN (" +
                                              "SELECT files.id FROM filetags,files,tags " +
                                              "WHERE filetags.tag_id = tags.id " +
                                              "AND files.id = filetags.file_id " +
                                              "AND tags.name IN (" + excluded_tag_list + ") " +
                                              "GROUP BY files.id)" +
                                              "GROUP BY files.id " +
                                              "HAVING COUNT( files.id ) = " + (tags.Count - excluded_count) + " " +
                                              "ORDER BY modtime " +
                                              "LIMIT " + offset + ", " + limit + ";";
            }
            else
            {
                Manager.Command.CommandText = "SELECT files.* FROM filetags, files, tags " +
                                              "WHERE filetags.tag_id = tags.id " +
                                              "AND (tags.name IN (" + tag_list + ")) " +
                                              "AND files.id = filetags.file_id " +
                                              "GROUP BY files.id " +
                                              "HAVING COUNT( files.id ) = " + tags.Count + " " +
                                              "ORDER BY modtime " +
                                              "LIMIT " + offset + ", " + limit + ";";
            }

            using (DbDataReader reader = Manager.Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    File f = new File();
                    f.Id = reader.GetInt32(0);
                    f.Name = reader.GetString(1);
                    f.Hash = reader.GetString(2);
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream((byte[])reader.GetValue(3)))
                        f.Thumbnail = new Bitmap(ms);
                    f.modtime = reader.GetDateTime(4).ToString();
                    files.AddLast(f);
                }
            }

            return files;
        }

        public static int CountByTags(LinkedList<String> tags)
        {
            LinkedList<File> files = new LinkedList<File>();

            String tag_list = "";
            String excluded_tag_list = "";
            int excluded_count = 0;

            foreach (String tag in tags)
            {
                string tag_firstchar = tag[0].ToString();
                if (tag_firstchar.ToString().Equals("-"))
                {
                    //Exclude Tags
                    excluded_tag_list += "'" + tag.Remove(0, 1) + "', ";
                    excluded_count++;
                }
                else tag_list += "'" + tag + "', ";
            }

            if(tag_list.Length > 0)
                tag_list = tag_list.Remove(tag_list.Length - 2);

            if (excluded_tag_list.Length > 0)
            {
                excluded_tag_list = excluded_tag_list.Remove(excluded_tag_list.Length - 2);

                Manager.Command.CommandText = "Select count(*) from (" +
                                              "SELECT COUNT(*) FROM filetags, files, tags " +
                                              "WHERE filetags.tag_id = tags.id " +
                                              "AND (tags.name IN (" + tag_list + ")) " +
                                              "AND files.id = filetags.file_id " +
                                              "AND files.id NOT IN (" +
                                              "SELECT files.id FROM filetags,files,tags " +
                                              "WHERE filetags.tag_id = tags.id " +
                                              "AND files.id = filetags.file_id " +
                                              "AND tags.name IN (" + excluded_tag_list + ") " +
                                              "GROUP BY files.id)" +
                                              "GROUP BY files.id " +
                                              "HAVING COUNT( files.id ) = " + (tags.Count - excluded_count) + ");";
            }
            else
            {
                Manager.Command.CommandText = "Select count(*) from (" +
                                              "SELECT COUNT(*) FROM filetags, files, tags " +
                                              "WHERE filetags.tag_id = tags.id " +
                                              "AND (tags.name IN (" + tag_list + ")) " +
                                              "AND files.id = filetags.file_id " +
                                              "GROUP BY files.id " +
                                              "HAVING COUNT( files.id ) = " + tags.Count + ");";
            }

            return Convert.ToInt32(Manager.Command.ExecuteScalar());
        }

        public void Insert()
        {
            DbCommand cmd = Manager.Command;

            cmd.CommandText = "insert into files (name, hash, thumbnail, modtime) values (\"" + this.Name + "\", \"" + this.Hash + "\", @thumb, \"" + this.modtime + "\"); SELECT last_insert_rowid();";

            SQLiteParameter thumb = new SQLiteParameter("@thumb", DbType.Binary, 0xFFFF);
            using (MemoryStream ms = new MemoryStream())
            {
                Thumbnail.Save(ms, ImageFormat.Jpeg);
                thumb.Value = ms.ToArray();
                cmd.Parameters.Add(thumb);
            }

            this.Id = System.Convert.ToInt32(Manager.Command.ExecuteScalar());

            Tag tagme = Tag.GetByName("tagme");

            if (tagme == null)
            {
                tagme = new Tag();
                tagme.Name = "tagme";
                tagme.Insert();
            }

            this.AddTag(tagme);
        }

        public void AddTag(Tag tag)
        {
            Manager.Command.CommandText = "select count(*) from filetags where file_id = " + this.Id + " and tag_id = " + tag.Id + ";";
            if (Convert.ToInt32(Manager.Command.ExecuteScalar()) == 0)
            {
                Manager.Command.CommandText = "insert into filetags (file_id, tag_id) values (" + this.Id + ", " + tag.Id + ");";
                Manager.Command.ExecuteNonQuery();
            }
        }

        public LinkedList<Tag> GetTags()
        {
            LinkedList<Tag> tags = new LinkedList<Tag>();
            
            Manager.Command.CommandText = "SELECT tags.* FROM filetags, files, tags WHERE files.id = filetags.file_id " +
                                          "AND tags.id = filetags.tag_id AND files.id = " + this.Id + ";";
            using (DbDataReader reader = Manager.Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tag t = new Tag();
                    t.Id = reader.GetInt32(0);
                    t.Name = reader.GetString(1);
                    t.Color = reader.GetInt32(2);
                    tags.AddLast(t);
                }
            }
            return tags;
        }

        public void RemoveTags()
        {
            Manager.Command.CommandText = "delete from filetags where file_id = " + this.Id + ";";
            Manager.Command.ExecuteNonQuery();
        }

        public bool RemoveTag(Tag tag)
        {
            Manager.Command.CommandText = "delete from filetags where file_id = " + this.Id + " and tag_id = " + tag.Id + " ;";
            return Manager.Command.ExecuteNonQuery() != 0;
        }

        public void Delete()
        {
            RemoveTags();
            Manager.Command.CommandText = "delete from files where id = " + this.Id + ";";
            Manager.Command.ExecuteNonQuery();
        }

        public static void Tool_TagTable_DateUpdate(int offset, int limit)
        {
            Manager.Command.CommandText = "select id,name from files limit " + offset + ", " + limit + ";";

            Dictionary<int, string> files = new Dictionary<int, string>();

            using (DbDataReader reader = Manager.Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int fileID = reader.GetInt32(0);
                    string filepath = reader.GetString(1);
                    DateTime modifyTime = System.IO.File.GetLastWriteTime(filepath);

                    string modifyMonth = string.Empty;
                    string modifyDay = string.Empty;
                    string modifyHour = string.Empty;
                    string modifyMinute = string.Empty;
                    string modifySecond = string.Empty;

                    if (modifyTime.Month.ToString().Length < 2)
                        modifyMonth = "0" + modifyTime.Month.ToString();
                    else modifyMonth = modifyTime.Month.ToString();

                    if (modifyTime.Day.ToString().Length < 2)
                        modifyDay = "0" + modifyTime.Day.ToString();
                    else modifyDay = modifyTime.Day.ToString();

                    if (modifyTime.Hour.ToString().Length < 2)
                        modifyHour = "0" + modifyTime.Hour.ToString();
                    else modifyHour = modifyTime.Hour.ToString();

                    if (modifyTime.Minute.ToString().Length < 2)
                        modifyMinute = "0" + modifyTime.Minute.ToString();
                    else modifyMinute = modifyTime.Minute.ToString();

                    if (modifyTime.Second.ToString().Length < 2)
                        modifySecond = "0" + modifyTime.Second.ToString();
                    else modifySecond = modifyTime.Second.ToString();

                    string modifyTimeStr = modifyTime.Year.ToString() + "-" + modifyMonth + "-" + modifyDay + " "
                        + modifyHour + ":" + modifyMinute + ":" + modifySecond;
                    files.Add(fileID, modifyTimeStr);
                }
            }

            foreach (KeyValuePair<int, string> kvp in files)
            {
                Manager.Command.CommandText = "update files set modtime = '" + kvp.Value + "' WHERE id = " + kvp.Key + ";";
                Manager.Command.ExecuteNonQuery();
            }
        }
    }
}
