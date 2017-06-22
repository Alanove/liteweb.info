using System;
using System.Reflection;

namespace lw.Utils
{
	/// <summary>
	/// Provides a static utility object of methods and properties to interact
	/// with enumerated types.
	/// </summary>
	public static class EnumHelper
	{
		/// <summary>
		/// Gets the <see cref="Description" /> of an <see cref="Enum" />
		/// type value. 
		/// [Description="value"]
		/// </summary>
		/// <param name="en">The <see cref="Enum" /> type value.</param>
		/// <returns>A string containing the text of the
		/// <see cref="Description"/>.</returns> Attribute
		public static string GetDescription(Enum en) 
		{
			Type type = en.GetType();
			MemberInfo[] memInfo = type.GetMember(en.ToString());
			if (memInfo != null && memInfo.Length > 0) 
			{
				object[] attrs = memInfo[0].GetCustomAttributes(typeof(Description), false); 
				if (attrs != null && attrs.Length > 0)
					return ((Description)attrs[0]).Text;
			}
			return en.ToString();
		}

		/// <summary>
		/// Gets the <see cref="Name" /> of an <see cref="Enum" />
		/// type value. 
		/// [Name="value"]
		/// </summary>
		/// <param name="en">The <see cref="Enum" /> type value.</param>
		/// <returns>A string containing the text of the
		/// <see cref="Name"/>.</returns> Attribute
		public static string GetName(Enum en)
		{
			Type type = en.GetType();
			MemberInfo[] memInfo = type.GetMember(en.ToString());
			if (memInfo != null && memInfo.Length > 0)
			{
				object[] attrs = memInfo[0].GetCustomAttributes(typeof(Name), false);
				if (attrs != null && attrs.Length > 0)
					return ((Name)attrs[0]).Text;
			}
			return en.ToString();
		}

        /// <summary>
        /// Gets the <see cref="Name" /> of an <see cref="Enum" />
        /// type value. 
        /// [Name="value"]
        /// </summary>
        /// <param name="en">The <see cref="Enum" /> type value.</param>
        /// <returns>A string containing the text of the
        /// <see cref="Name"/>.</returns> Attribute
        public static int GetPermissionGroup(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(PermissionGroup), false);
                if (attrs != null && attrs.Length > 0)
                    return ((PermissionGroup)attrs[0]).Text;
            }
            return 0;
        }

        public static T EnsureEnum<T>(this object obj) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            string str = obj.EnsureString();
            T retVal = (T)Enum.Parse(typeof(T), str, true);
            return retVal;
        }      

		/// <summary>
		/// Gets the name of the specific enum based on it's value (int)
		/// </summary>
		/// <param name="value">Int number of the required type</param>
		/// <returns>string of the name (type) if type available, null if unavailable</returns>
		public static string GetEnumTypeName<T>(int value)
		{
			var type = typeof(T);

			string name = Enum.GetName(type, value);

			if (name != null)
				return name;
			else
				return null;
		}
	}

	/// <summary>
	/// Returns the Description attribute of the enum
	/// </summary>
	public class Description : Attribute 
	{
		public string Text; 
		public Description(string text) 
		{ 
			Text = text; 
		}
	}

	/// <summary>
	/// Returns the Name attribute of the Enum
	/// </summary>
	public class Name : Attribute
	{
		public string Text;
		public Name(string text)
		{
			Text = text;
		}
	}

    /// <summary>
    /// Returns the Name attribute of the Enum
    /// </summary>
    public class PermissionGroup : Attribute
    {
        public int Text;
        public PermissionGroup(int text)
        {
            Text = text;
        }
    }
}
