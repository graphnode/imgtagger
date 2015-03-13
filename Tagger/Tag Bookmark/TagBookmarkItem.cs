using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Serialization;
using Tagger.Database;

namespace Tagger
{
    //Class representing a Menu Item containing a Tag
    [Serializable()]
    public class TagBookmarkItem : ToolStripMenuItem, ISerializable, ICloneable
    {
        public Tag Tag;
        public int FileCount;

        public TagBookmarkItem() { }

        public TagBookmarkItem(Tag t)
            : base()
        {
            this.Tag = t;
            this.Text = Tag.Name;
            this.FileCount = 0;
        }

        #region ISerializable Members

        public TagBookmarkItem(SerializationInfo info, StreamingContext ctxt)
        {
            this.Tag = (Tag)info.GetValue("TagBookmarkItemTag", typeof(Tag));
            this.Text = Tag.Name;
            this.ForeColor = Color.FromArgb(Tag.Color);
            this.FileCount = (int)info.GetValue("TagBookmarkItemFileCount", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("TagBookmarkItemTag", this.Tag);
            info.AddValue("TagBookmarkItemFileCount", this.FileCount);
        } 

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            TagBookmarkItem tbi = new TagBookmarkItem();
            tbi.Tag = this.Tag;
            tbi.Text = this.Tag.Name;
            tbi.ForeColor = Color.FromArgb(this.Tag.Color);
            tbi.FileCount = 0;

            return tbi;
        }

        #endregion
    }
}
