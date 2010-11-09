using System;

namespace MyFlickr.Core
{
    public struct Token : IEquatable<Token>
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
            return left.Guid == right.Guid;
        }

        public static bool operator !=(Token left, Token right)
        {
            return !(left == right);
        }

        public bool Equals(Token other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return this == (Token)obj;
        }

        public override int GetHashCode()
        {
            return this.Guid.GetHashCode();
        }
    }
}
