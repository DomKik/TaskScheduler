using System.Windows.Forms;
using TasksScheduler.Properties;

namespace TasksScheduler
{
    public partial class Main : Form
    {
        CreateParams createParams;
        public Main()
        {
            InitializeComponent();
            Icon = Resources.TrayIcon;
        }

        private void InitializeComponent()
        {
            TasksListButton = new Button();
            SuspendLayout();
            // 
            // TasksListButton
            // 
            TasksListButton.Location = new Point(287, 115);
            TasksListButton.Name = "TasksListButton";
            TasksListButton.Size = new Size(171, 37);
            TasksListButton.TabIndex = 0;
            TasksListButton.Text = "Lista zadań";
            TasksListButton.UseVisualStyleBackColor = true;
            TasksListButton.Click += TasksListButton_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(TasksListButton);
            Icon = Resources.TrayIcon;
            Name = "Main";
            Text = "Menu";
            ResumeLayout(false);
        }

        private void TasksListButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO okienko z modyfikacja zadan, dodawaniem zadan i usuwaniem zadan");
        }
    }
}