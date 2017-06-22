using System;
using System.Collections.Generic;
using System.Timers;

namespace lw.Threading
{
	/// <summary>
	/// Base class for the Threading or pretimed classes
	/// </summary>
	public abstract class ThreadingBase
	{
		/// <summary>
		/// A static field indicating all the available threading timers in the system.
		/// </summary>
		public static Dictionary<string, Timer> ThreadingTimersMap = new Dictionary<string, Timer>();

		/// <summary>
		/// A static field indicating all the currently running timers, these values are being changed depending on the status of the timer
		/// The string key is the same that is used in the <see cref="ThreadingTimersMap"/>
		/// </summary>
		public static Dictionary<string , bool> RunningTimers = new Dictionary<string, bool>();


		#region constructor

		/// <summary>
		/// Creates an instance of ThreadingBase
		/// </summary>
		protected ThreadingBase()
		{
			
		}
		/// <summary>
		/// Creates a TheadingBase class
		/// </summary>
		/// <param name="repeat"><seealso cref="RepeatPattern"/> Defines the repeat patern of the class</param>
		/// <param name="startDate">Defines the start date of the repeat</param>
		protected ThreadingBase(RepeatPattern repeat, DateTime startDate)
		{
			this._repeat = repeat;
			this._startDate = startDate;
			AddToGlobalThreads();
		}


		/// <summary>
		/// Ads the thread to the global timers that are automatically called with timers.
		/// It's essential to call this method when creating this class with an empty constructor.
		/// </summary>
		public void AddToGlobalThreads()
		{
			RunningTimers[_key] = false;
			ThreadingTimersMap[_key] = new Timer();
		}

		#endregion

		/// <summary>
		/// This should be overriden to define the Action of the class
		/// </summary>
		public abstract void Action();

		/// <summary>
		/// Calls Action when the time is met
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void TimerAction(object sender, ElapsedEventArgs e)
		{
			if (RunningTimers[_key])
				return;
			RunningTimers[_key] = true;

			DisposeTimer();
			
			Action();
			RunningTimers[_key] = false;
			Init();
		}

		void DisposeTimer()
		{
			var threadingTimer = ThreadingTimersMap[_key];

			if (threadingTimer != null)
			{
				threadingTimer.Stop();
				threadingTimer.Dispose();
			}
			ThreadingTimersMap[_key] = null;
		}

		/// <summary>
		/// This function re-init the timer when the difference time exceeds the max limit of the Timer interval
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void WaitingTimer(object sender, ElapsedEventArgs e)
		{
			if (RunningTimers[_key])
				return;

			Init();
		}


		/// <summary>
		/// Inits the class and defines when the next Action should be called
		/// </summary>
		public void Init()
		{
			if (RunningTimers[_key])
				return;

			DisposeTimer();

			Timer threadingTimer = null;

			DateTime now = DateTime.Now;
			TimeSpan span = now - _startDate;
			DateTime tempDate;
			double max = 24 * 24 * 60 * 60 * 1000;

			if (span.Milliseconds < 0)
			{
				threadingTimer = new Timer(Math.Abs(span.Milliseconds));
				threadingTimer.Elapsed += new ElapsedEventHandler(TimerAction);
				threadingTimer.Start();
				return;
			}
			switch (_repeat)
			{
				case RepeatPattern.BySecond:

					tempDate = _startDate.AddSeconds(Math.Floor(span.TotalSeconds));
					TimeSpan secondsSpan = tempDate.AddSeconds(1) - DateTime.Now;
					threadingTimer = new Timer(secondsSpan.TotalMilliseconds);
					threadingTimer.Elapsed += new ElapsedEventHandler(TimerAction);
					threadingTimer.Start();
					break;
				case RepeatPattern.Minutly:
					tempDate = _startDate.AddMinutes(Math.Floor(span.TotalMinutes));
					TimeSpan minutesSpan = tempDate.AddMinutes(1) - DateTime.Now;
					threadingTimer = new Timer(minutesSpan.TotalMilliseconds);
					threadingTimer.Elapsed += new ElapsedEventHandler(TimerAction);
					threadingTimer.Start();

					break;
				case RepeatPattern.Hourly:
					tempDate = _startDate.AddHours(Math.Floor(span.TotalHours));
					TimeSpan hoursSpan = tempDate.AddHours(1) - DateTime.Now;
					threadingTimer = new Timer(hoursSpan.TotalMilliseconds);
					threadingTimer.Elapsed += new ElapsedEventHandler(TimerAction);
					threadingTimer.Start();
					break;
				case RepeatPattern.Daily:
					tempDate = _startDate.AddDays(Math.Floor(span.TotalDays));
					TimeSpan dailySpan = tempDate.AddDays(1) - DateTime.Now;
					threadingTimer = new Timer(dailySpan.TotalMilliseconds);
					threadingTimer.Elapsed += new ElapsedEventHandler(TimerAction);
					threadingTimer.Start();
					break;
				case RepeatPattern.Weekly:
					tempDate = new DateTime(now.Year, now.Month, now.Day, _startDate.Hour, _startDate.Minute, _startDate.Second);
					tempDate = tempDate.AddDays(-1 * (int)now.DayOfWeek + 1 * (int)_startDate.DayOfWeek);
					if (tempDate < now)
						tempDate = tempDate.AddDays(7);
					TimeSpan weeklySpan = tempDate - now;
					threadingTimer = new Timer(weeklySpan.TotalMilliseconds);
					threadingTimer.Elapsed += new ElapsedEventHandler(TimerAction);
					threadingTimer.Start();

					break;
				case RepeatPattern.Monthly:
					tempDate = new DateTime(now.Year, now.Month, _startDate.Day, _startDate.Hour, _startDate.Minute, _startDate.Second);
					if (tempDate < now)
						tempDate = tempDate.AddMonths(1);
					while (tempDate.Month - now.Month == 2)
					{
						tempDate.AddDays(-1);
					}
					TimeSpan montlySpan = tempDate - now;

					if (montlySpan.TotalMilliseconds > max)
					{
						threadingTimer = new Timer(max);
						threadingTimer.Elapsed += new ElapsedEventHandler(WaitingTimer);
						threadingTimer.Start();
					}
					else
					{
						threadingTimer = new Timer(montlySpan.TotalMilliseconds);
						threadingTimer.Elapsed += new ElapsedEventHandler(TimerAction);
						threadingTimer.Start();
					}
					break;
				case RepeatPattern.Yearly:
					tempDate = new DateTime(now.Year, _startDate.Month, _startDate.Day, _startDate.Hour, _startDate.Minute, _startDate.Second);
					if (tempDate < now)
						tempDate = tempDate.AddYears(1);
					while (tempDate.Year - now.Year == 2)
					{
						//tempDate.AddDays(-1);
					}
					TimeSpan yearlySpan = tempDate - now;
					if (yearlySpan.TotalMilliseconds > max)
					{
						threadingTimer = new Timer(max);
						threadingTimer.Elapsed += new ElapsedEventHandler(WaitingTimer);
						threadingTimer.Start();
					}
					else
					{
						threadingTimer = new Timer(yearlySpan.TotalMilliseconds);
						threadingTimer.Elapsed += new ElapsedEventHandler(TimerAction);
						threadingTimer.Start();
					}
					break;
			}
			ThreadingTimersMap[_key] = threadingTimer;
		}

		#region Properties
		RepeatPattern _repeat;
		/// <summary>
		/// Indicates the <see cref="RepeatPattern"/> of this thread
		/// </summary>
		public RepeatPattern Repeat 
		{
			get
			{
				return _repeat;
			}
			set
			{
				_repeat = value;
			}
		}


		DateTime _startDate;
		/// <summary>
		/// Idicates the start date of the thread
		/// </summary>
		public DateTime StartDate
		{
			get { return _startDate; }
			set { _startDate = value; }
		}

		private string _key;
		/// <summary>
		/// Indicates the Unique Key of this thread.
		/// </summary>
		public string Key
		{
			get { return _key; }
			set { _key = value; }
		}

		#endregion
	}
}
