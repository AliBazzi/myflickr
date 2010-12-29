using System;

namespace MyFlickr.Core
{
    /// <summary>
    /// represents a parameter that is passed during the call of Flickr Method.
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// the name of the Parameter.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// the value of the parameter.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Create a parameter object.
        /// </summary>
        /// <param name="name">the name of the parameter.</param>
        /// <param name="value">the value of the parameter.</param>
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
        /// determine whether the Parameter should be dropped and not sent during the Method call or Not.
        /// </summary>
        public bool ShouldBeDropped { get; private set; }

        /// <summary>
        /// Return string Representation of the Instance.
        /// </summary>
        /// <returns>a String.</returns>
        public override string ToString()
        {
            return this.ToString(false);
        }

        /// <summary>
        /// Return string Representation of the Instance.
        /// </summary>
        /// <param name="removeEquality">whether to Add the Equality or Not.</param>
        /// <returns>a String.</returns>
        public string ToString(bool removeEquality)
        {
            return string.Format("{0}{1}{2}",this.Name,removeEquality ? string.Empty :"=" ,this.Value);
        }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Parameter Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Parameter left, Parameter right)
        {
            if (left is Parameter)
                return left.Equals(right);
            else if (right is Parameter)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Parameter Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Parameter left, Parameter right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determine whether a Given Object Equals this Object.
        /// </summary>
        /// <param name="obj">Instance</param>
        /// <returns>True or False</returns>
        public override bool Equals(object obj)
        {
            return obj is Parameter && this.Name == ((Parameter)obj).Name && this.Value == ((Parameter)obj).Value;
        }

        /// <summary>
        /// Serve as Hash Function for a Particular Type.
        /// </summary>
        /// <returns>Hashed Value</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
