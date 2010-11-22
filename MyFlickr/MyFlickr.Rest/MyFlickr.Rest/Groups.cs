using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a Collection of Groups
    /// </summary>
    public class GroupCollection : IEnumerable<Group>
    {
        private XElement data;

        internal GroupCollection(XElement element)
        {
            this.data = element;
            this.GroupsCount = element.Elements("group").Count();
        }

        /// <summary>
        /// the Number of the Groups in the collection
        /// </summary>
        public int GroupsCount { get; private set; }

        /// <summary>
        /// the Groups Objects
        /// </summary>
        public IEnumerable<Group> Groups
        {
            get
            {
                return this.data.Elements("group").Select(elm =>
                    new Group(elm.Attribute("nsid").Value, elm.Attribute("name").Value, elm.Attribute("admin").Value.ToBoolean()
                        , elm.Attribute("eighteenplus").Value.ToBoolean()));
            }
        }

        public IEnumerator<Group> GetEnumerator()
        {
            foreach (var group in this.Groups)
                yield return group;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// represent a Flickr Group
    /// </summary>
    public class Group
    {
        internal Group(string id, string name, bool isAdmin, bool isOver18)
        {
            this.ID = id;
            this.Name = name;
            this.IsAdmin = isAdmin;
            this.IsOver18 = isOver18;
        }
        /// <summary>
        /// the ID that identifies the group
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the name of the Group
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// determine whether the calling user is an administrator of the group.
        /// </summary>
        public bool IsAdmin { get; private set; }

        /// <summary>
        /// determine if the group is visible to members over 18 only.
        /// </summary>
        public bool IsOver18 { get; private set; }
    }
}