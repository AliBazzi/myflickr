using System;
namespace MyFlickr.Rest
{
    /// <summary>
    /// represents Access Permission options.
    /// </summary>
    public enum AccessPermission
    {
        /// <summary>
        /// no permission.
        /// </summary>
        None=-1,
        /// <summary>
        /// Read Permission.
        /// </summary>
        Read=0,
        /// <summary>
        /// Write Permission.
        /// </summary>
        Write=1,
        /// <summary>
        /// Delete Permission.
        /// </summary>
        Delete=2
    }

    internal static class AccessPermissionExtensions
    {
        public static AccessPermission GetValue(string value)
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

        public static AccessPermission GetValue(int value)
        {
            switch (value)
            {
                case 0:
                    return AccessPermission.None;
                case 1:
                    return AccessPermission.Read;
                case 2:
                    return AccessPermission.Write;
                case 3:
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
