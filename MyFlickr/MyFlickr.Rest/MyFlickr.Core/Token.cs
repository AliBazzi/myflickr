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
        public static bool operator ==(Token left, Token right)
        {
            if (left is Token)
                return left.Equals(right);
            else if (right is Token)
                return right.Equals(left);
            return true;
        }

        public static bool operator !=(Token left, Token right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return obj is Token && this.Guid == ((Token)obj).Guid;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
