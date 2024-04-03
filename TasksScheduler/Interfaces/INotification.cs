using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksScheduler.src.utils;

namespace TasksScheduler.Interfaces
{
    public interface INotification
    {
        public void notify();
        public void StopNotyfing();
        public void StartNotyfing();
        public bool IsNotyfying();
    }
}
