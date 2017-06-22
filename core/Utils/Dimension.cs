
namespace lw.Utils
{
	/// <summary>
	/// A dimension class helper that have width and height
	/// </summary>
	public class Dimension
	{
		#region Private
		double _width = 0, _height = 0;
		bool _valid = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Created a dimension class
		/// </summary>
		public Dimension()
		{
		}

		/// <summary>
		/// Creates a dimension class using double entries
		/// </summary>
		/// <param name="width">The width of class</param>
		/// <param name="height">The height of class</param>
		public Dimension(double width, double height)
		{
			this._width = width;
			this._height = height;
		}

		/// <summary>
		/// Creates a dimension class using int entries
		/// </summary>
		/// <param name="width">The width of class</param>
		/// <param name="height">The height of class</param>
		public Dimension(int width, int height)
		{
			this._width = (double)width;
			this._height = (double)height;
		}

		/// <summary>
		/// Creates a dimension class using a string entry that is 
		/// </summary>
		/// <param name="size">The entry string parameter to be parsed separated by x: WidthxHeight, WXH, ex: 100X200</param>
		public Dimension(string size)
		{
			Parse(size);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Parse the string entry.
		/// The entry string parameter to be parsed separated by x: WidthxHeight, WXH, ex: 100X200
		/// </summary>
		/// <param name="size"></param>
		public void Parse(string size)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(size))
				{
					if (size.ToLower().IndexOf("x") >= 0)
					{
						string[] sizes = size.Split('x');

						_width = double.Parse(sizes[0]);
						_height = double.Parse(sizes[1]);
					}
					else
					{
						_width = _height = double.Parse(size);
					}
					_valid = true;
				}
			}
			catch
			{
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// The (double) width of the dimension
		/// </summary>
		public double Width
		{
			get { return _width; }
			set { _width = value; }
		}
		/// <summary>
		/// The (double) height of the dimention
		/// </summary>
		public double Height
		{
			get { return _height; }
			set { _height = value; }
		}

		/// <summary>
		/// The (int) Width
		/// </summary>
		public int IntWidth
		{
			get { return (int)_width; }
			set { this._width = (double)value; }
		}

		/// <summary>
		/// The (int) Height
		/// </summary>
		public int IntHeight
		{
			get { return (int)_height; }
			set { _height = (double)value; }
		}

		/// <summary>
		/// Returns true if parsing the dimension string was ok.
		/// </summary>
		public bool Valid
		{
			get
			{
				return _valid;
			}
		}
		#endregion
	}
}
