using System;

namespace lw.Utils
{
	/// <summary>
	/// Utils to be used with objects in general.
	/// </summary>
	public class ObjectTools
	{
		/// <summary>
		/// Creates the object ObjectName object from Assembly Name
		/// </summary>
		/// <param name="assemblyName">The assembry from where to read the object</param>
		/// <param name="objectName">The returned object name</param>
		/// <returns>The object that is read from the specified assembly</returns>
		public static object InstanceOfStringClass(string assemblyName, string objectName)
		{
			object objectToCast;
			try
			{
				System.Runtime.Remoting.ObjectHandle objHandleToCast = Activator.CreateInstance(assemblyName, objectName);
				objectToCast = objHandleToCast.Unwrap();
			}
			catch (Exception)
			{
				objectToCast = null;
			}

			return objectToCast;
		}
	}
}
