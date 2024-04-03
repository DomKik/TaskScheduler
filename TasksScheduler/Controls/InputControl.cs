using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TasksScheduler
{
    public partial class InputControl : UserControl
    {
        public bool AutoFitTitlePosition { get; set; }
        public string Text 
        { 
            get { return InputTextBox.Text; }
            set { InputTextBox.Text = value; }
        }
        public string Title
        {
            get { return TitleLabel.Text; }
            set
            {
                TitleLabel.Text = value;
                if (AutoFitTitlePosition) { fitTitlePos(); }
            }
        }

        public delegate bool ValidateInput(string input);
        public InputControl()
        {
            AutoFitTitlePosition = true;
            AutoSize = true;
            InitializeComponent();
        }

        private void fitTitlePos()
        {
            int textWidth = TitleLabel.Size.Width;
            TitleLabel.Location = new Point(InputTextBox.Location.X + (InputTextBox.Width - textWidth) / 2, 0);
        }

    }
}
