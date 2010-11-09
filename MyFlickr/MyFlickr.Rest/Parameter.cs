using System;

namespace MyFlickr.Rest
{
    public class Parameter
    {
        public string Name { get; private set; }

        public string Value { get; private set; }

        public Parameter(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name");
            }
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("value");
            }
            this.Name = name;
            this.Value = value;
        }

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
