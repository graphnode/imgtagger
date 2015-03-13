using System;
using System.Collections.Generic;
using System.Text;
using Tagger.Database;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace Tagger
{
    //Class for serialization of a Bookmark List
    [Serializable()]
    public class TagBookmark : ISerializable
    {
        private List<TagBookmarkItem> m_tagBookmarkList = new List<TagBookmarkItem>();
        public List<TagBookmarkItem> TagBookmarkList
        {
            get { return m_tagBookmarkList; }
            set { m_tagBookmarkList = value; }
        }

        public TagBookmark(List<TagBookmarkItem> tagBookmarkList)
        {
            this.TagBookmarkList = tagBookmarkList;
        }

        public TagBookmark(SerializationInfo info, StreamingContext ctxt)
        {
            m_tagBookmarkList = (List<TagBookmarkItem>)info.GetValue("TagBookmarkList", typeof(List<TagBookmarkItem>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("TagBookmarkList", m_tagBookmarkList);
        }
    }

    //Class which holds the currently displayed/manipulated Bookmark List
    public static class TagBookmarkTemp
    {
        private static List<TagBookmarkItem> m_tagBookmarkList = new List<TagBookmarkItem>();
        public static List<TagBookmarkItem> TagBookmarkList
        {
            get { return m_tagBookmarkList; }
            set { m_tagBookmarkList = value; }
        }
    }
}
