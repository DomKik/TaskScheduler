using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksScheduler.Properties;

namespace TasksScheduler.src.Notyfing
{
    public class SoundNotify : TasksScheduler.Interfaces.INotification
    {
        private bool isNotyfying = false;
        System.Media.SoundPlayer notifySound = new System.Media.SoundPlayer(Resources.NotificationSound);

        public async void notify()
        {
            await System.Threading.Tasks.Task.Run(() => notifySound.PlaySync());
        }

        public void StartNotyfing()
        {
            notifySound.PlayLooping();
            isNotyfying = true;
        }

        public void StopNotyfing()
        {
            notifySound.Stop();
            isNotyfying = false;
        }

        public bool IsNotyfying()
        {
            return isNotyfying;
        }
    }
}
