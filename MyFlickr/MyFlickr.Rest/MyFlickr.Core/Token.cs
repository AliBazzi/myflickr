using System;

namespace MyFlickr.Core
{
    public class Token
    {
        private Guid Guid;

        private Token(Guid guid)
        {
            this.Guid = guid;
        }

        public static Token GenerateToken()
        {
            return new Token(Guid.NewGuid());
        }

        public static bool operator == (Token left, Token right)
        {
            if (left is Token)
            {
                return left.Equals(right);
            }
            if (right is Token)
            {
                return right.Equals(left);
            }
            return false;
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
            return this.Guid.GetHashCode();
        }
    }
}
