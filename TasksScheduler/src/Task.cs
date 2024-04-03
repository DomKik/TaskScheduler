using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Serialization;
using TasksScheduler.forms;
using TasksScheduler.Interfaces;
using TasksScheduler.src.Notyfing;
using TasksScheduler.src.utils;

namespace TasksScheduler.src
{
    public class Task
    {
        public int Id { get; set; }
        private DateTime alarmDateTime;
        public DateTime AlarmDateTime 
        { 
            get { return alarmDateTime; } 
            set
            {
                alarmDateTime = value;
                timer.Interval = calcFirstInterval();
            }
        }
        public string Title { get; set; }

        public string? Description { get; set; }
        bool DoShowNotification = true;

        private bool isActive = false;
        public bool IsActive
        {
            get { return isActive; }
            set 
            {
                if (value) { start(); }
                else { pause(); }
            }
        }
        [XmlIgnore, JsonIgnore]
        public string IsActiveString
        {
            get
            { 
                if(isActive) { return "TAK"; }
                else { return "NIE"; }
            }
            set { }
        }

        private long intervalSeconds;
        public long IntervalSeconds 
        { 
            get { return intervalSeconds; } 
            set 
            {
                intervalSeconds = value;
                timer.Interval = calcFirstInterval();
            }
        }
        [XmlIgnore, JsonIgnore]
        public string IntervalString
        {
            get
            {
                if (IsPeriodically == false) { return "-"; }

                int hours = (int)intervalSeconds / 3600;
                int minutes = ((int)intervalSeconds - (hours * 3600)) / 60;
                int seconds = (int)intervalSeconds % 60;
                string value = seconds.ToString() + "s";
                if (minutes > 0 || hours > 0) { value = minutes.ToString() + "m " + value; }
                if (hours > 0) { value = hours.ToString() + "h " + value; }
                return value;
            }
            set { }
        }

        [XmlIgnore]
        public INotification? notifycation;

        [JsonIgnore, XmlIgnore]
        public string NotificationString
        {
            get
            {
                switch(SoundNotificationType)
                {
                    case NotificationOption.Default:
                        return "Domyślne";
                    case NotificationOption.Nokia:
                        return "Nokia";
                    case NotificationOption.VoiceTitleRead:
                        return "Odczyt tytułu";
                    case NotificationOption.VoiceDescriptionRead:
                        return "Odczyt opisu";
                    case NotificationOption.RecordOwn:
                        return "Nagranie własne";
                    case NotificationOption.ChooseOwn:
                        return "Własny dźwięk";
                    default:
                        return "Bez dźwięku";
                }
            }
        }

        public NotificationOption SoundNotificationType { get; set; }

        public double TimeToAbortNotification
        {
            get
            {
                return notificationTimeControl.Interval;
            }
            set
            {
                notificationTimeControl.Interval = value;
            }
        }


        public bool IsSoundNotification;
        public bool IsOnceNotification;
        public bool IsPeriodically;
        public bool NotifyIfMissed;

        public int notifyCount = 0;
        public int MaxNotificationCount;

        private bool isExceptionalInterval;
        private System.Timers.Timer notificationTimeControl = new System.Timers.Timer();
        private System.Timers.Timer postponeTimer = new System.Timers.Timer();
        private System.Timers.Timer timer = new System.Timers.Timer();
        [XmlIgnore, JsonIgnore]
        public System.Timers.Timer Timer { get { return timer; } }

        public Task()
        {
            this.Title = "";
            this.Description = "";
            this.AlarmDateTime = DateTime.Now;
            this.notifycation = null;
            this.IsPeriodically = false;
            this.IntervalSeconds = 0;
            this.IsSoundNotification = true;
            this.isExceptionalInterval = false;
            this.IsOnceNotification = false;
            this.MaxNotificationCount = 1;
            this.NotifyIfMissed = false;

            notificationTimeControl.Elapsed += NotificationTimeControl_Elapsed;
            notificationTimeControl.AutoReset = false;
            notificationTimeControl.Interval = 5 * 60 * 1000;
            postponeTimer.Elapsed += postPoneLaterNotify;
            timer.Elapsed += notify;

            this.SoundNotificationType = NotificationOption.Default;

            this.IsActive = false;
        }

        public Task(DateTime dateTime, string Title, string desc, long IntervalSeconds, bool isPeriodically, bool isActive = true, INotification? notifycation = null)
        {
            this.AlarmDateTime = dateTime;
            this.Title = Title;
            this.Description = desc;
            this.IntervalSeconds = IntervalSeconds;
            this.IsPeriodically = isPeriodically;
            this.NotifyIfMissed = false;

            this.notifycation = notifycation;

            notificationTimeControl.Elapsed += NotificationTimeControl_Elapsed;
            notificationTimeControl.AutoReset = false;
            notificationTimeControl.Interval = 5 * 60 * 1000;
            postponeTimer.Elapsed += postPoneLaterNotify;
            timer.Elapsed += notify;

            this.SoundNotificationType = NotificationOption.Default;

            this.IsActive = isActive;
            this.MaxNotificationCount = 1;
        }

        public void notify(object? sender, ElapsedEventArgs e)
        {
            if(!IsActive) { return; }

            chooseNotification();

            this.notifyCount++;
            if(!IsPeriodically && notifyCount >= this.MaxNotificationCount)
            {
                IsActive = false;
            }
            else if (isExceptionalInterval)
            {
                timer.Interval = IntervalSeconds * 1000;
                isExceptionalInterval = false;
            }
        }

        public void postPoneLaterNotify(object? sender, ElapsedEventArgs e)
        {
            chooseNotification();

            ((System.Timers.Timer)sender!).Stop();
        }

        public void showNotification()
        {
            DialogResult result = MessageBox.Show(Description, Title, MessageBoxButtons.OKCancel);

            if(result == DialogResult.OK)
            {
                this.notifycation!.StopNotyfing();
            }
            else if(result == DialogResult.Cancel)
            {
                this.postponeLater(5 * 60);
            }
            else
            {
                throw new Exception("Nie zdefiniowana akcja MessageBox");
            }
            
        }

        public void postponeLater(int seconds)
        {
            this.notifycation!.StopNotyfing();
            postponeTimer.AutoReset = false;
            postponeTimer.Interval = seconds * 1000;
            postponeTimer.Start();
        }

        public void start()
        {
            timer.Interval = calcFirstInterval();
            timer.Start();
            isActive = true;
        }

        public void pause() 
        { 
            timer.Stop(); 
            isActive = false;
        }

        public bool Validate()
        {
            if(intervalSeconds < 0) { return false; }
            return true;
        }

        public Task getCopy()
        {
            Task copy = new Task();
            copy.Id = Id;
            copy.Title = Title;
            copy.Description = Description;
            copy.AlarmDateTime = alarmDateTime;
            copy.IntervalSeconds = intervalSeconds;
            copy.IsSoundNotification = IsSoundNotification;
            copy.IsPeriodically = IsPeriodically;
            copy.isExceptionalInterval = isExceptionalInterval;
            copy.notifycation = notifycation;
            copy.notifyCount = notifyCount;
            // NIEAKTUALNE TODO ZAKTUALIZOWAC O NOWE ATRYBUTY
            return copy;
        }

        private long calcFirstInterval()
        {
            isExceptionalInterval = true;
            if (AlarmDateTime > DateTime.Now)
            {
                return (long)(AlarmDateTime - DateTime.Now).TotalMilliseconds;
            }
            else
            {
                if(IntervalSeconds > 0)
                {
                    long milisecs = IntervalSeconds * 1000;
                    return milisecs - ((long)(DateTime.Now - AlarmDateTime).TotalMilliseconds % milisecs);
                }
                else
                {
                    this.IsActive = false;
                    return 3000;
                }
            }
        }

        private void chooseNotification()
        {
            if (notifycation!.IsNotyfying()) { return; }

            if (IsSoundNotification)
            {
                if (IsOnceNotification) { this.notifycation.notify(); }
                else 
                { 
                    this.notifycation.StartNotyfing();
                    this.notificationTimeControl.Start();
                }
            }

            if (DoShowNotification) { showNotification(); }
        }

        private void NotificationTimeControl_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (!notifycation!.IsNotyfying()) { return; }
            notifycation!.StopNotyfing();
            NotificationIgnored();
        }

        private void NotificationIgnored()
        {

        }

    }
}
