using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TasksScheduler.forms
{
    public partial class NotificationForm : Form
    {
        src.Task trigger;
        public NotificationForm(src.Task trigger)
        {
            InitializeComponent();

            /*Text = trigger.Title;
            DescriptionLabel.Text = trigger.Description;
            this.trigger = trigger;*/
            DescriptionLabel.Text = "jkhfkjshdgfjshfdjshgfdjs";

        }

        private void InitializeComponent()
        {
            DescriptionLabel = new Label();
            OKButton = new Button();
            LaterButton = new Button();
            SuspendLayout();
            // 
            // DescriptionLabel
            // 
            DescriptionLabel.AutoSize = true;
            DescriptionLabel.Location = new Point(47, 29);
            DescriptionLabel.Name = "DescriptionLabel";
            DescriptionLabel.Size = new Size(0, 15);
            DescriptionLabel.TabIndex = 0;
            // 
            // OKButton
            // 
            OKButton.Location = new Point(37, 145);
            OKButton.Name = "OKButton";
            OKButton.Size = new Size(128, 47);
            OKButton.TabIndex = 1;
            OKButton.Text = "OK";
            OKButton.UseVisualStyleBackColor = true;
            OKButton.Click += OKButton_Click;
            // 
            // LaterButton
            // 
            LaterButton.Location = new Point(198, 145);
            LaterButton.Name = "LaterButton";
            LaterButton.Size = new Size(126, 47);
            LaterButton.TabIndex = 2;
            LaterButton.Text = "Odłóż za 5 minut";
            LaterButton.UseVisualStyleBackColor = true;
            LaterButton.Click += LaterButton_Click;
            // 
            // NotificationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(358, 207);
            Controls.Add(LaterButton);
            Controls.Add(OKButton);
            Controls.Add(DescriptionLabel);
            Name = "NotificationForm";
            Text = "NotificationForm";
            FormClosing += NotificationForm_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            //this.trigger.Notifycation.StopNotyfing();
            MessageBox.Show("OK");
        }

        private void LaterButton_Click(object sender, EventArgs e)
        {
            //this.trigger.Notifycation.StopNotyfing();
            //this.trigger.postponeLater(5 * 60);
        }

        private void NotificationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.trigger.Notifycation.StopNotyfing();
        }
    }
}
