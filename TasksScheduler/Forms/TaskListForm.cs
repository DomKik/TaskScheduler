using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TasksScheduler.Properties;
using TasksScheduler.src.Notyfing;

namespace TasksScheduler.forms
{
    public partial class TaskListForm : Form
    {
        public TrayApplication Creator;
        private src.Task? chosenTask = null;
        private DataGridView TaskViewer;


        public src.Task? ChosenTask
        {
            get { return chosenTask; }
            set
            {
                chosenTask = value;
                DeleteTaskButton.Enabled = (typeof(src.Task).IsInstanceOfType(value));
                EditTaskButton.Enabled = DeleteTaskButton.Enabled;
            }
        }
        public TaskListForm(TrayApplication creator)
        {
            InitializeComponent();
            Creator = creator;
            SetupDataGridView();
            Icon = Resources.TrayIcon;
        }

        private void TaskListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Creator.data.UpdateViewingData();
        }

        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            new TaskAddEditForm(this).ShowDialog();
        }

        private void DeleteTaskButton_Click(object sender, EventArgs e)
        {
            if (chosenTask != null)
            {
                Creator.data.Remove(chosenTask);
                Creator.data.UpdateViewingData();
            }
        }

        private void EditTaskButton_Click(object sender, EventArgs e)
        {
            if (chosenTask != null)
            {
                new TaskAddEditForm(this, chosenTask).ShowDialog();
            }
        }

        private void SetupDataGridView()
        {
            TaskViewer = new DataGridView();

            TaskViewer.ColumnCount = 6;
            TaskViewer.AutoGenerateColumns = false;
            TaskViewer.DataSource = Creator.data.Tasks;
            TaskViewer.AllowUserToAddRows = false;
            TaskViewer.AllowUserToDeleteRows = false;

            TaskViewer.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            TaskViewer.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            TaskViewer.ColumnHeadersDefaultCellStyle.Font =
                new Font(TaskViewer.Font, FontStyle.Bold);

            TaskViewer.Name = "TaskViewer";
            TaskViewer.Location = new Point(8, 8);
            TaskViewer.Size = new Size(630, 250);
            TaskViewer.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            TaskViewer.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            TaskViewer.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            TaskViewer.GridColor = Color.Black;
            TaskViewer.RowHeadersVisible = false;

            TaskViewer.SelectionChanged += TaskViewer_SelectionChanged;

            TaskViewer.Columns[0].Name = "Tytuł";
            TaskViewer.Columns[0].DataPropertyName = "Title";
            TaskViewer.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            TaskViewer.Columns[1].Name = "Opis";
            TaskViewer.Columns[1].DataPropertyName = "Description";
            TaskViewer.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            TaskViewer.Columns[2].Name = "Data i czas";
            TaskViewer.Columns[2].DataPropertyName = "AlarmDateTime";
            TaskViewer.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            TaskViewer.Columns[3].Name = "Interwał";
            TaskViewer.Columns[3].DataPropertyName = "IntervalString";
            TaskViewer.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            TaskViewer.Columns[4].Name = "Aktywne";
            TaskViewer.Columns[4].DataPropertyName = "IsActiveString";
            TaskViewer.Columns[4].DefaultCellStyle = TaskViewer.DefaultCellStyle;
            TaskViewer.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            TaskViewer.Columns[5].Name = "Powiadomienie";
            TaskViewer.Columns[5].DataPropertyName = "NotificationString";
            TaskViewer.Columns[5].DefaultCellStyle = TaskViewer.DefaultCellStyle;
            TaskViewer.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            TaskViewer.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            TaskViewer.MultiSelect = false;

            this.Controls.Add(TaskViewer);
        }

        private void TaskViewer_SelectionChanged(object? sender, EventArgs e)
        {
            if (TaskViewer.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = TaskViewer.SelectedRows[0];
                ChosenTask = (src.Task)selectedRow.DataBoundItem;
            }
            else
            {
                ChosenTask = null;
            }
        }
    }
}
