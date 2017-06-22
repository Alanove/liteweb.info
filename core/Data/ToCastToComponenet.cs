
using lw.Collections;

namespace lw.Data
{
	/// <summary>
	/// Summary description for ToCastToComponenet.
	/// </summary>
	////////////////Class to handle Object Component////////////
	public  class ToCastToComponenet : System.ComponentModel.Component
	{

		protected SafeCollection _DataComponents = new SafeCollection();
		

		protected virtual void initData()
		{
		}
		protected void AddDataComponent(string name,object dataComponent)
		{
			lock(this._DataComponents.SyncRoot)
			{
				this._DataComponents.Add(name, dataComponent);
			}
		}

		public object GetDataComponent(string name)
		{
			return(this._DataComponents[name]);
		}

	}
	/////////////////End Class To handle Object/////////

}
