using System;

namespace MyFlickr.Core
{
    /// <summary>
    /// Token that represents a Ticket for the Caller , to Monitor his Request when it's done
    /// </summary>
    public class Token
    {
        private Guid Guid;

        private Token(Guid guid)
        {
            this.Guid = guid;
        }

        internal static Token GenerateToken()
        {
            return new Token(Guid.NewGuid());
        }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Token Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Token left, Token right)
        {
            if (left is Token)
                return left.Equals(right);
            else if (right is Token)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Token Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Token left, Token right)
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
            return obj is Token && this.Guid == ((Token)obj).Guid;
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
