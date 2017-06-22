using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using lw.Utils;
using lw.WebTools;

namespace lw.Threading
{
	public class ThreadPool
	{
		public Dictionary<string, ThreadingBase> ThreadsCollection = new Dictionary<string, ThreadingBase>();

		public void _initThreads()
		{
			//ThreadsCollection.Add("Minutly", new Minutly());
			//ThreadsCollection.Add("Hourly", new Hourly());
			//ThreadsCollection.Add("Daily", new Daily());
			//ThreadsCollection.Add("Weekly", new Weekly());
			//ThreadsCollection.Add("Monthly", new Monthly());
			//ThreadsCollection.Add("Yearly", new Yearly());

			DataSet threadingDs = XmlManager.GetDataSet(CTE.Content.Threadings);

			if (threadingDs != null)
			{
				DataTable threadTable = threadingDs.Tables["Threadings"];
				
				if (threadTable != null)
				{
					foreach (DataRow dr in threadTable.Rows)
					{
						string key = dr["Description"].ToString();

						if (ThreadsCollection.Keys.Contains(key))
							continue;

						var thread = (ThreadingBase)ObjectTools.InstanceOfStringClass((string)dr["AssemblyName"], (string)dr["ClassName"]);
						thread.Repeat = (RepeatPattern)Enum.Parse(typeof(RepeatPattern), dr["Pattern"].ToString());
						thread.StartDate = (DateTime)dr["StartDate"];
						thread.Key = key;
						thread.AddToGlobalThreads();

						ThreadsCollection.Add(key, thread);
					}
				}
			}

		}

		public void Init()
		{
			_initThreads();

			foreach (var thread in ThreadsCollection)
			{
				thread.Value.Init();
			}
		}
	}
}
