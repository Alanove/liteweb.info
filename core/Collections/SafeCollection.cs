using System;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;

namespace lw.Collections
{
	/// <summary>
	/// Creates thread safe collection.
	/// Adding removing items from the SafeCollection is locked
	/// </summary>
	public class SafeCollection:IDictionary
	{
		private IDictionary _myColection;
		private ReaderWriterLock _locker;
		private const Int32 Timeout = 1000;
		private LockCookie _lc;
		private bool _getMutex=false;
        
		/// <summary>
		/// Create a SafeCollection
		/// </summary>
		public SafeCollection()
		{
			_locker=new ReaderWriterLock();
			_myColection=new HybridDictionary();
		}
		/// <summary>
		/// Created a SafeCollection from an existing <see cref="HybridDictionary"/>
		/// </summary>
		/// <param name="sHybridDictionary"><see cref="HybridDictionary"/></param>
		public SafeCollection(HybridDictionary  sHybridDictionary)
		{
			_locker=new ReaderWriterLock();
			this._myColection=sHybridDictionary;
		}
		public SafeCollection(System.Collections.IDictionary SItemCollection)
		{
			this._locker=new ReaderWriterLock();
			this._myColection=SItemCollection;
		}
		
		/// <summary>
		/// Specifies if the collection is read only
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				try
				{
					if(this._locker.IsWriterLockHeld)
					{
						throw new Exception("The Current Thread Holds a Writer Lock");  
					}
					this._locker.AcquireReaderLock(-1);
					try
					{
						lock(this._myColection.SyncRoot)
							return (this._myColection.IsReadOnly);
					}        
					finally
					{
						this._locker.ReleaseReaderLock();
					}
				}		
				catch(ApplicationException)
				{
					throw new Exception("A non-fatal application error occurs Multithrading");
				}
			}
		}

		/// <summary>
		/// Returns an emurator used over the collection
		/// </summary>
		/// <returns></returns>
		public IDictionaryEnumerator GetEnumerator()
		{
			return(this._myColection.GetEnumerator());
		}

		/// <summary>
		/// Returns an object from the collection
		/// </summary>
		/// <param name="key">Object Key</param>
		/// <returns>Object Result</returns>
		public object this[object key]
		{
			get
			{
				
				try
				{
					if(this._locker.IsWriterLockHeld)
					{ 
						
						throw new Exception("The Current Thread Holds a Writer Lock");  
						
					}
					this._locker.AcquireReaderLock(-1);
					try
					{
						lock(this._myColection.SyncRoot)
							return (this._myColection[key]);
					}        
					finally
					{
						this._locker.ReleaseReaderLock();
					}
				}		
				catch(ApplicationException)
				{
					throw new Exception("A non-fatal application error occurs Multithrading");
				}
			}
			set
			{
				try
				{
					if(this._locker.IsReaderLockHeld)
					{
						try 
						{
							LockCookie LC=this._locker.UpgradeToWriterLock(Timeout);
							_getMutex=true;
						}
						catch(ApplicationException)
						{
							throw new Exception("The Current Thread Holds a Writer Lock");  
						}
						 
					}
					this._locker.AcquireWriterLock(-1);
					try
					{
						lock(this._myColection.SyncRoot)
							this._myColection[key]=value;
						

					}        
					finally
					{
						this._locker.ReleaseWriterLock();
						if(_getMutex==true)
						{
							this._locker.DowngradeFromWriterLock(ref _lc);
						}
					}
				}		
				catch(ApplicationException)
				{
					throw new Exception("A non-fatal application error occurs Multithrading");
				}
			}
		}

		/// <summary>
		/// Removes an object from the collection
		/// </summary>
		/// <param name="key">Object Key</param>
		public void Remove(object key)
		{
			try
			{
				if(this._locker.IsWriterLockHeld)
				{
					throw new Exception("The Current Thread Holds a Writer Lock");  
				}
				this._locker.AcquireReaderLock(-1);
				try
				{
					lock(this._myColection.SyncRoot)
						this._myColection.Remove(key);
				}        
				finally
				{
					this._locker.ReleaseReaderLock();
				}
			}		
			catch(ApplicationException)
			{
				throw new Exception("A non-fatal application error occurs Multithrading");
			}
		}

		/// <summary>
		/// Checks if the collection contains the specified key
		/// </summary>
		/// <param name="key">Object Key</param>
		/// <returns>True if the key is included in the collection</returns>
		public bool Contains(object key)
		{
			try
			{
				if(this._locker.IsWriterLockHeld)
				{
					throw new Exception("The Current Thread Holds a Writer Lock");  
				}
				this._locker.AcquireReaderLock(-1);
				try
				{
					lock(this._myColection.SyncRoot)
						return (this._myColection.Contains(key));
				}        
				finally
				{
					this._locker.ReleaseReaderLock();
				}
			}		
			catch(ApplicationException)
			{
				throw new Exception("A non-fatal application error occurs Multithrading");
			}
		}

		/// <summary>
		/// Clears the collection
		/// </summary>
		public void Clear()
		{
			try
			{
				if(this._locker.IsWriterLockHeld)
				{
					throw new Exception("The Current Thread Holds a Writer Lock");  
				}
				this._locker.AcquireReaderLock(-1);
				try
				{
					lock(this._myColection.SyncRoot)
						this._myColection.Clear();
				}        
				finally
				{
					this._locker.ReleaseReaderLock();
				}
			}		
			catch(ApplicationException)
			{
				throw new Exception("A non-fatal application error occurs Multithrading");
			}
		}

		/// <summary>
		/// Returns the values of the collection
		/// </summary>
		public ICollection Values
		{
			get
			{
				try
				{
					if(this._locker.IsWriterLockHeld)
					{
						throw new Exception("The Current Thread Holds a Writer Lock");  
					}
					this._locker.AcquireReaderLock(-1);
					try
					{
						lock(this._myColection.SyncRoot)
							return (_myColection.Values);
					}        
					finally
					{
						this._locker.ReleaseReaderLock();
					}
				}		
				catch(ApplicationException)
				{
					throw new Exception("A non-fatal application error occurs Multithrading");
				}
			}
		}

		/// <summary>
		/// Safe adds an object to the collection
		/// </summary>
		/// <param name="key">Object Key</param>
		/// <param name="value">Object Value</param>
		public void Add(object key, object value)
		{
			try
			{
				if(this._locker.IsReaderLockHeld)
				{
					try 
					{
						LockCookie LC=this._locker.UpgradeToWriterLock(Timeout);
						_getMutex=true;
					}
					catch(ApplicationException)
					{
						throw new Exception("The Current Thread Holds a Writer Lock");  
					}
				}
				this._locker.AcquireWriterLock(-1);
				try
				{
					lock(this._myColection.SyncRoot)
						this._myColection.Add(key,value);
				}        
				finally
				{
					this._locker.ReleaseWriterLock();
					if(_getMutex==true)
					{
						this._locker.DowngradeFromWriterLock(ref _lc);
					}
				}
			}
			catch(ApplicationException)
			{
				throw new Exception("A non-fatal application error occurs Multithrading");
			}
		}


		/// <summary>
		/// Returns the collection of keys within this collection
		/// </summary>
		public ICollection Keys
		{
			get
			{
				try
				{
					if(this._locker.IsWriterLockHeld)
					{
						throw new Exception("The Current Thread Holds a Writer Lock");  
					}
					this._locker.AcquireReaderLock(-1);
					try
					{
						lock(this._myColection.SyncRoot)
							return (this._myColection.Keys);
					}        
					finally
					{
						this._locker.ReleaseReaderLock();
					}
				}		
				catch(ApplicationException)
				{
					throw new Exception("A non-fatal application error occurs Multithrading");
				}
			}
		}

		/// <summary>
		/// Checks if the collection is fixed in size
		/// </summary>
		public bool IsFixedSize
		{
			get
			{
				try
				{
					if(this._locker.IsWriterLockHeld)
					{
						throw new Exception("The Current Thread Holds a Writer Lock");  
					}
					this._locker.AcquireReaderLock(-1);
					try
					{
						lock(this._myColection.SyncRoot)
							return (this._myColection.IsFixedSize);
					}        
					finally
					{
						this._locker.ReleaseReaderLock();
					}
				}		
				catch(ApplicationException)
				{
					throw new Exception("A non-fatal application error occurs Multithrading");
				}
			}
		}

		
		/// <summary>
		/// Checks if the collection is synchronized
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				try
				{
					if(this._locker.IsWriterLockHeld)
					{
						throw new Exception("The Current Thread Holds a Writer Lock");  
					}
					this._locker.AcquireReaderLock(-1);
					try
					{
						lock(this._myColection.SyncRoot)
							return (this._myColection.IsSynchronized);
					}        
					finally
					{
						this._locker.ReleaseReaderLock();
					}
				}		
				catch(ApplicationException)
				{
					throw new Exception("A non-fatal application error occurs Multithrading");
				}
			}
		}

		/// <summary>
		/// Returns the count of keys in the collection
		/// </summary>
		public int Count
		{
			get
			{
				try
				{
					if(this._locker.IsWriterLockHeld)
					{
						throw new Exception("The Current Thread Holds a Writer Lock");  
					}
					this._locker.AcquireReaderLock(-1);
					try
					{
						lock(this._myColection.SyncRoot)
							return (this._myColection.Count);
					}        
					finally
					{
						this._locker.ReleaseReaderLock();
					}
				}		
				catch(ApplicationException)
				{
					throw new Exception("A non-fatal application error occurs Multithrading");
				}
			}
		}


		public void CopyTo(Array array, int index)
		{
			// TODO:  Add SafeBaseCollection.CopyTo implementation
		}

		public object SyncRoot
		{
			get
			{
				try
				{
					if(this._locker.IsWriterLockHeld)
					{
						throw new Exception("The Current Thread Holds a Writer Lock");  
					}
					this._locker.AcquireReaderLock(-1);
					try
					{
						lock(this._myColection.SyncRoot)
							return (this._myColection.SyncRoot);
					} 
					finally
					{
						this._locker.ReleaseReaderLock();
					}
				}		
				catch(ApplicationException)
				{
					throw new Exception("A non-fatal application error occurs Multithrading");
				}
			}
		}

	
		

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			// TODO:  Add SafeBaseCollection.System.Collections.IEnumerable.GetEnumerator implementation
			return null;
		}

	
	}
	
}
