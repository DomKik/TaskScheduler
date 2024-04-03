namespace TasksScheduler
{
    partial class InputControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TitleLabel = new Label();
            InputTextBox = new TextBox();
            SuspendLayout();
            // 
            // TitleLabel
            // 
            TitleLabel.AutoSize = true;
            TitleLabel.Location = new Point(60, 2);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new Size(68, 15);
            TitleLabel.TabIndex = 0;
            TitleLabel.Text = "Nazwa Pola";
            // 
            // InputTextBox
            // 
            InputTextBox.Location = new Point(3, 20);
            InputTextBox.Name = "InputTextBox";
            InputTextBox.Size = new Size(176, 23);
            InputTextBox.TabIndex = 1;
            // 
            // InputControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(InputTextBox);
            Controls.Add(TitleLabel);
            Name = "InputControl";
            Size = new Size(182, 46);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public Label TitleLabel;
        public TextBox InputTextBox;
    }
}
