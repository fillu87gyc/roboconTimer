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
			TimerMg = new TimerMg();
			Timer.DataContext = TimerMg;
			dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
			dispatcherTimer.Interval = new TimeSpan(100000);
			dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
			dispatcherTimer.Start();
		}

		void dispatcherTimer_Tick(object sender, EventArgs e)
		{
			TimerMg.Timer = TimerMg.Timer;
		}
		TimerMg TimerMg;
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


		void NoLimit_Click(object sender, RoutedEventArgs e) { }
	}
	public class TimerMg: INotifyPropertyChanged
	{
		private DateTime startDate;
		public TimerMg()
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

				timer = ("0" + min + " : " + sec).ToString();
				return timer;
			}
			set
			{
				if (this.timer == value) { return; }
				this.timer = value;
				var h = this.PropertyChanged;
				if (h != null)
				{
					h(this, new PropertyChangedEventArgs("Timer"));
				}
			}
		}
	}
}

