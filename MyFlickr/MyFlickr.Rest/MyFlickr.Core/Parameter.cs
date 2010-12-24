using System;

namespace MyFlickr.Core
{
    /// <summary>
    /// represents a parameter that is passed during the call of Flickr Method
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// the name of the Parameter
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// the value of the parameter
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Create a parameter object
        /// </summary>
        /// <param name="name">the name of the parameter</param>
        /// <param name="value">te value of the parameter</param>
        public Parameter(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name");
            }
            if (value == null)
            {
                this.ShouldBeDropped = true;
            }
            else
            {
                this.Value = value.ToString();
            }
            this.Name = name;
        }

        /// <summary>
        /// determine whether the Parameter should be dropped and not sent during the Method call or Not
        /// </summary>
        public bool ShouldBeDropped { get; private set; }

        public override string ToString()
        {
            return this.ToString(false);
        }

        public string ToString(bool removeEquality)
        {
            return string.Format("{0}{1}{2}",this.Name,removeEquality ? string.Empty :"=" ,this.Value);
        }
    }
}
