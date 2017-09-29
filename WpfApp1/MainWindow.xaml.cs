using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		DispatcherTimer dispatcherTimer;
		public MainWindow()
		{
			InitializeComponent();
			NoLimitedTimerMg = new NoLimitedTimerMg();
			LimitedTimerMg = new LimitedTimerMg();

			dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
			dispatcherTimer.Interval = new TimeSpan(100000);
			dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
			dispatcherTimer.Start();
			this.DataContext = new { Timer = "Ready?" };
		}

		void dispatcherTimer_Tick(object sender, EventArgs e)
		{
			LimitedTimerMg.Timer = LimitedTimerMg.Timer;
			NoLimitedTimerMg.Timer = NoLimitedTimerMg.Timer;
		}
		NoLimitedTimerMg NoLimitedTimerMg;
		LimitedTimerMg LimitedTimerMg;
		public int Red { get; set; }
		public int Blue { get; set; }

		private void RedUp_Click(object sender, RoutedEventArgs e)
		{
			Red++;
			RedPanel.Text = "            " + Red.ToString();
		}

		private void RedDown_Click(object sender, RoutedEventArgs e)
		{
			Red--;
			RedPanel.Text = "            " + Red.ToString();
		}

		private void BlueUp_Click(object sender, RoutedEventArgs e)
		{
			Blue++;
			BluePanel.Text = "            " + Blue.ToString();
		}

		private void BlueDown_Click(object sender, RoutedEventArgs e)
		{
			Blue--;
			BluePanel.Text = "            " + Blue.ToString();
		}

		void NoLimit_Click(object sender, RoutedEventArgs e)
		{
			NoLimitedTimerMg.Init();
			Timer.DataContext = NoLimitedTimerMg;
		}
		private void Limited_Click(object sender, RoutedEventArgs e)
		{
			LimitedTimerMg.Init(new TimeSpan(0, 3, 0));
			Timer.DataContext = LimitedTimerMg;
		}

		private void OneLimited_Click(object sender, RoutedEventArgs e)
		{
			LimitedTimerMg.Init(new TimeSpan(0, 1, 0));
			Timer.DataContext = LimitedTimerMg;
		}
		DateTime stoptime;
		private void StopButton_Click(object sender, RoutedEventArgs e)
		{
			//タイマー停止
			Timer.DataContext = new { Timer = Timer.Text };
			stoptime = DateTime.Now;
		}

		private void ReopenButton_Click(object sender, RoutedEventArgs e)
		{
			Timer.DataContext = LimitedTimerMg;
			LimitedTimerMg.SetAddTime(DateTime.Now - stoptime);
			stoptime = DateTime.Now;
		}
	}
	public class NoLimitedTimerMg : INotifyPropertyChanged
	{
		private DateTime startDate;
		public NoLimitedTimerMg()
		{
			startDate = DateTime.Now;
		}

		public int ElapsedMilliSec()
		{
			DateTime endDate = DateTime.Now;
			TimeSpan diff = endDate - startDate;
			return diff.Duration().Seconds;
		}
		public event PropertyChangedEventHandler PropertyChanged;
		string timer;
		public string Timer
		{
			get
			{
				int time = ElapsedMilliSec();
				int min = time / 60;
				int sec = time % 60;
				if (sec >= 10)
					timer = ("0" + min + " : " + sec).ToString();
				else
					timer = ("0" + min + " : 0" + sec).ToString();
				return timer;
			}
			set
			{
				//if (this.timer == value) { return; }
				//this.timer = value;
				var h = this.PropertyChanged;
				if (h != null)
				{
					h(this, new PropertyChangedEventArgs("Timer"));
				}
			}
		}
		public void Init()
		{
			startDate = DateTime.Now;
		}
	}
	public class LimitedTimerMg : INotifyPropertyChanged
	{
		TimeSpan basetime;
		TimeSpan addTime;
		private DateTime startDate;
		public LimitedTimerMg()
		{
			startDate = DateTime.Now;
			addTime = new TimeSpan(0, 0, 0);
		}

		public int ElapsedMilliSec()
		{
			DateTime endDate = DateTime.Now;
			TimeSpan diff = basetime - (endDate - startDate)+addTime;

			int min = diff.Minutes;
			int sec = diff.Seconds;
			int res = min * 60 + sec;
			return res;
		}
		public event PropertyChangedEventHandler PropertyChanged;
		string timer;
		public string Timer
		{
			get
			{
				if (ElapsedMilliSec() > 0)
				{
					int time = ElapsedMilliSec();
					int min = time / 60;
					int sec = time % 60;
					if (sec >= 10)
						timer = ("0" + min + " : " + sec).ToString();
					else
						timer = ("0" + min + " : 0" + sec).ToString();
					return timer;
				}
				else
				{
					timer = "Time Up!!";
					return timer;
				}
			}
			set
			{
				//if (this.timer == value) { return; }
				//this.timer = value;
				var h = this.PropertyChanged;
				if (h != null)
				{
					h(this, new PropertyChangedEventArgs("Timer"));
				}
			}
		}
		public void Init(TimeSpan basetimer)
		{
			startDate = DateTime.Now;
			basetime = basetimer;
			addTime = new TimeSpan(0, 0, 0);
		}
		public void SetAddTime(TimeSpan val)
		{
			addTime = val;
		}
	}
}

