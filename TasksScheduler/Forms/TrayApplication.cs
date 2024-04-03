using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksScheduler.Interfaces;
using TasksScheduler.Properties;
using TasksScheduler.src;
using TasksScheduler.src.Notyfing;

namespace TasksScheduler.forms
{
    public class TrayApplication : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private Main? menuGui = null;

        public Data data = new Data();
        private Main MenuGui
        {
            get
            {
                if (menuGui is null)
                {
                    menuGui = new Main();
                }
                return menuGui;
            }
        }

        public TrayApplication()
        {
            // potem mozna by zrobic wybieranie rodzaju powiadomien, nawet dla zadan
            // a nawet mozna zrobic glosowego lektora, ktory by mowil tytul zadania

            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Menu", null, Menu_Click);
            menu.Items.Add("Lista zadań", null, TaskList_Click);
            menu.Items.Add("Wyjscie", null, Exit);
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.TrayIcon,
                ContextMenuStrip = menu,
                Visible = true
            };
            Application.ApplicationExit += OnApplicationExit;
        }
        private void Menu_Click(object? sender, EventArgs e)
        {
            MenuGui.Show();
        }

        private void TaskList_Click(object? sender, EventArgs e)
        {
            new TaskListForm(this).Show();
        }

        private void OnApplicationExit(object? sender, EventArgs e)
        {
            data.Save();
        }

        void Exit(object? sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

            Application.Exit();
        }

    }
}
