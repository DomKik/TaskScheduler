namespace TasksScheduler.forms
{
    partial class TaskListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            AddTaskButton = new Button();
            DeleteTaskButton = new Button();
            EditTaskButton = new Button();
            SuspendLayout();
            // 
            // AddTaskButton
            // 
            AddTaskButton.Location = new Point(652, 35);
            AddTaskButton.Name = "AddTaskButton";
            AddTaskButton.Size = new Size(167, 42);
            AddTaskButton.TabIndex = 0;
            AddTaskButton.Text = "Dodaj zadanie";
            AddTaskButton.UseVisualStyleBackColor = true;
            AddTaskButton.Click += AddTaskButton_Click;
            // 
            // DeleteTaskButton
            // 
            DeleteTaskButton.Enabled = false;
            DeleteTaskButton.Location = new Point(652, 149);
            DeleteTaskButton.Name = "DeleteTaskButton";
            DeleteTaskButton.Size = new Size(167, 41);
            DeleteTaskButton.TabIndex = 1;
            DeleteTaskButton.Text = "Usuń Zadanie";
            DeleteTaskButton.UseVisualStyleBackColor = true;
            DeleteTaskButton.Click += DeleteTaskButton_Click;
            // 
            // EditTaskButton
            // 
            EditTaskButton.Enabled = false;
            EditTaskButton.Location = new Point(652, 92);
            EditTaskButton.Name = "EditTaskButton";
            EditTaskButton.Size = new Size(167, 41);
            EditTaskButton.TabIndex = 2;
            EditTaskButton.Text = "Edytuj Zadanie";
            EditTaskButton.UseVisualStyleBackColor = true;
            EditTaskButton.Click += EditTaskButton_Click;
            // 
            // TaskListForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(831, 450);
            Controls.Add(EditTaskButton);
            Controls.Add(DeleteTaskButton);
            Controls.Add(AddTaskButton);
            Name = "TaskListForm";
            Text = "Przegląd zdefiniowanych zadań";
            FormClosing += TaskListForm_FormClosing;
            ResumeLayout(false);
        }

        #endregion

        private Button AddTaskButton;
        private Button DeleteTaskButton;
        private Button EditTaskButton;
    }
}