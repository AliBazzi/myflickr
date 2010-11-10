using System;
namespace MyFlickr.Rest
{
    public enum AccessPermission
    {
        None=-1,Read=0,Write=1,Delete=2
    }

    internal static class AccessPermissionExtensions
    {
        public static AccessPermission GetValue(this string value)
        {
            switch (value)
            {
                case "none":
                    return AccessPermission.None;
                case "read":
                    return AccessPermission.Read;
                case "write":
                    return AccessPermission.Write;
                case "delete":
                    return AccessPermission.Delete;
                default:
                    throw new ArgumentException("value");
            }
        }

        public static void ValidateRange(this AccessPermission accessPermission)
        {
            if ((int)accessPermission >2 || (int)accessPermission <0)
            {
                throw new ArgumentException("accessPermission");
            }
        }
    }
}
