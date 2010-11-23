using System;

namespace MyFlickr.Core
{
    public class Parameter
    {
        public string Name { get; private set; }

        public string Value { get; private set; }

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
