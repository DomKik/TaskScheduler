using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksScheduler.Interfaces;
using TasksScheduler.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace TasksScheduler.src
{
    public class Data
    {
        public BindingList<Task> Tasks;

        private int maxId = 1;
        private DataDisk dataDisk;

        public Data() 
        {
            dataDisk = new DataDisk();
            Load();
            foreach(Task t in Tasks)
            {
                if(t.Id >= maxId) { maxId = t.Id + 1; }
            }
        }

        public void Add(Task newTask)
        {
            if (newTask == null)
            {
                MessageBox.Show("Nowe zadanie nie może mieć wartość null");
                return;
            }

            newTask.Id = maxId++;
            Tasks.Add(newTask);
        }

        public void Remove(src.Task task) 
        {
            task.IsActive = false;
            Tasks.Remove(task);
        }

        public void Edit(Task editedTask)
        {
            // do zmiany bo indeks != task.id
            Tasks[editedTask.Id] = editedTask;
        }

        public void Save()
        {
            //dataDisk.SerializeObject(Tasks, dataDisk.dataPath);
            dataDisk.SerializeObjectJson(Tasks, dataDisk.dataPathJson);
        }

        public void Load()
        {
            Tasks = dataDisk.DeSerializeObjectJson<BindingList<Task>>(dataDisk.dataPathJson);
            if (Tasks is null)
            {
                Tasks = new BindingList<Task>();
            }
        }

        public void UpdateViewingData()
        {
            Tasks.ResetBindings();
        }
    }
}
