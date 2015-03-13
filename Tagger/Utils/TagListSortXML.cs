using System;
using System.Collections.Generic;
using System.Text;
using Tagger.Database;

namespace Tagger.Utils
{
    static class TagListSortXML
    {
        /// <summary>
        /// Create XML file for tag List
        /// </summary>
        /// <param name="tags">List of tags</param>
        public static void CreateListXML(LinkedList<Tag> tags)
        {
            string execPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

            if (!System.IO.File.Exists(execPath + "\\taglist " + Manager.Connection.DataSource + ".xml"))
            {
                System.Xml.XmlTextWriter xmlWriter = new System.Xml.XmlTextWriter(execPath + "\\taglist " + Manager.Connection.DataSource + ".xml", null);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("taglist");
                foreach (Tag t in tags)
                {
                    xmlWriter.WriteStartElement("tagid");
                    xmlWriter.WriteValue(t.Id);
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }
        }

        /// <summary>
        /// Sorts a tag list using xml file
        /// </summary>
        /// <param name="tiList">List of TagItem to sort</param>
        /// <returns>Sorted TagItem list</returns>
        public static List<TagItem> ReadListXML(List<TagItem> tiList)
        {
            string execPath = System.IO.Path.GetDirectoryName( System.Windows.Forms.Application.ExecutablePath );


            if (System.IO.File.Exists(execPath + "\\taglist " + Manager.Connection.DataSource + ".xml"))
            {
                System.Xml.XmlTextReader xmlReader = new System.Xml.XmlTextReader(execPath + "\\taglist " + Manager.Connection.DataSource + ".xml");

                List<TagItem> sortedList = new List<TagItem>();
                xmlReader.Read();
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == System.Xml.XmlNodeType.Text)
                    {
                        int tagID = Int32.Parse(xmlReader.Value);
                        TagItem resultTi = tiList.Find(delegate(TagItem ti) { return ti.Tag.Id == tagID; });
                        sortedList.Add(resultTi);
                    }
                }

                return sortedList;
            }
            else return null;
        }

        public static void UpdateListXML(List<TagItem> tiList)
        {
            string execPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

            if (System.IO.File.Exists(execPath + "\\taglist " + Manager.Connection.DataSource + ".xml"))
            {
                System.Xml.XmlTextWriter xmlWriter = new System.Xml.XmlTextWriter(execPath + "\\taglist " + Manager.Connection.DataSource + ".xml", null);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("taglist");

                foreach (TagItem ti in tiList)
                {
                    xmlWriter.WriteStartElement("tagid");
                    xmlWriter.WriteValue(ti.Tag.Id);
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }
        }
    }
}
