using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GoPOS.Common.Views.Controls
{
    public class CurrentTimeText : TextBlock
    {
        #region static

        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register("Interval", typeof(int), typeof(CurrentTimeText), new PropertyMetadata(1, IntervalChangedCallback));
        public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register("IsRunning", typeof(bool), typeof(CurrentTimeText), new PropertyMetadata(false, IsRunningChangedCallback));
        public static readonly DependencyProperty TimeModeProperty = DependencyProperty.Register("TimeMode", typeof(bool), typeof(CurrentTimeText), new PropertyMetadata(false, TimeModeChangedCallback));

        public string TimeFormat { get; set; }

        private static void IntervalChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CurrentTimeText wpfTimer = (CurrentTimeText)d;
            wpfTimer.timer.Interval = new TimeSpan(0, 0, (int)e.NewValue);
        }

        private static void IsRunningChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CurrentTimeText wpfTimer = (CurrentTimeText)d;
            wpfTimer.timer.IsEnabled = (bool)e.NewValue;
        }

        private static void TimeModeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CurrentTimeText wpfTimer = (CurrentTimeText)d;
            wpfTimer.timeModeElapsed = (bool)e.NewValue;
        }

        #endregion

        private readonly DispatcherTimer timer;
        private bool timeModeElapsed = false;

        /// <summary>
        /// in seconds
        /// </summary>
        [Category("Common")]
        public int Interval
        {
            get
            {
                return (int)this.GetValue(IntervalProperty);
            }
            set
            {
                this.SetValue(IntervalProperty, value);
            }
        }

        [Category("Common")]
        public bool IsRunning
        {
            get
            {
                return (bool)this.GetValue(IsRunningProperty);
            }
            set
            {
                this.SetValue(IsRunningProperty, value);
            }
        }

        [Category("Common")]
        public bool TimeMode
        {
            get
            {
                return (bool)this.GetValue(TimeModeProperty);
            }
            set
            {
                this.SetValue(TimeModeProperty, value);
            }
        }

        public TimeSpan ElapsedTime { get; set; }

        public CurrentTimeText()
        {
            this.timer = new DispatcherTimer(new TimeSpan(0, 0, this.Interval), DispatcherPriority.Normal, this.Timer_Tick, this.Dispatcher);
            this.timer.IsEnabled = false;
            ElapsedTime = new TimeSpan(0, 0, 0);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ElapsedTime = ElapsedTime.Add(TimeSpan.FromSeconds(this.Interval));
            if (timeModeElapsed)
            {
                this.SetValue(TextProperty, ElapsedTime.ToString());
            }
            else
            {
                this.SetValue(TextProperty, DateTime.Now.ToString(string.IsNullOrEmpty(TimeFormat) ? "HH : mm" : TimeFormat));
            }
        }
    }
}
