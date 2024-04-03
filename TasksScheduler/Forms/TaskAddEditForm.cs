using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TasksScheduler.Interfaces;
using TasksScheduler.Properties;
using TasksScheduler.src;
using TasksScheduler.src.Notyfing;
using TasksScheduler.src.utils;

namespace TasksScheduler.forms
{
    public partial class TaskAddEditForm : Form
    {
        private string soundFilename = "";
        private string selectedFilePath;
        private src.Task? editingTask;
        private VoiceRecorder voiceRecorder;

        public TableLayoutPanel AlarmStartDatePanel;
        public Label AlarmDateLabel;
        public Label AlarmTimeLabel;
        public InputControl TaskNameInput;
        public InputControl DescInput;
        public CheckBox IsPeriodicallyCheckBox;
        public DateTimePicker AlarmStartDatePicker;
        public DateTimePicker AlarmStartTimePicker;

        public TableLayoutPanel TextInputPanel;
        public CheckBox IsActiveCheckBox;

        public TableLayoutPanel IntervalPanel;
        public TableLayoutPanel NotificationCountPanel;
        public TableLayoutPanel NotificationTimesPanel;
        public Label NotificationCountLabel;
        public Label IntervalLabel;
        public Label IntervalHoursLabel;
        public Label IntervalMinutesLabel;
        public Label IntervalSecondsLabel;
        public NumericUpDown IntervalHoursNumeric;
        public NumericUpDown IntervalMinutesNumeric;
        public NumericUpDown IntervalSecondsNumeric;
        public NumericUpDown NotificationCountNumeric;

        public Button AddButton;
        public Button SaveButton;
        public Button SubmitButton
        {
            get
            {
                if(editingTask is null) { return AddButton; }
                return SaveButton;
            }
        }

        public TableLayoutPanel SoundPanel;
        public Button SoundChooseButton;
        public CheckBox IsOnceNotificationCheckBox;
        public CheckBox IsSoundCheckbox;
        public ComboBox SoundTypeComboBox;
        public Label ChosenSoundFileLabel;
        public Button RecordVoiceButton;

        public TableLayoutPanel DetailsPanel;
        public CheckBox ReactIsRequiredCheckBox;

        public TaskListForm Creator;
        public TrayApplication Application
        {
            get { return Creator.Creator; }
        }

        public static string[] NotificationOptions = new string[]
        {
            "domyślny",
            "nokia",
            "Głosowy odczyt tytułu",
            "Głosowy odczyt opisu",
            "nagraj własne...",
            "wybierz własny..."
        };

        public TaskAddEditForm(TaskListForm creator, src.Task? editingTask = null)
        {
            this.Creator = creator;
            this.editingTask = editingTask;
            InitializeComponent();
            Icon = Resources.TrayIcon;

            if (editingTask is null) { Text = "Dodawanie zadania"; }
            else { Text = "Edycja zadania"; }

            InitializeDateTimeInput();
            InitializeTextInputs();
            InitializeTimeInput();
            if (editingTask is null) { InitializeAddButton(); }
            else { InitializeSaveButton(); }
            InitializeSoundPanel();
            InitializeDetailsPanel();
            fillInputs();
        }

        private void InitializeDateTimeInput()
        {
            AlarmDateLabel = new Label();
            AlarmDateLabel.Anchor = AnchorStyles.None;
            AlarmDateLabel.AutoSize = true;
            AlarmDateLabel.Text = "Data alarmu";
            AlarmDateLabel.TextAlign = ContentAlignment.MiddleCenter;

            AlarmStartDatePicker = new DateTimePicker();
            AlarmStartDatePicker.Anchor = AnchorStyles.None;

            AlarmTimeLabel = new Label();
            AlarmTimeLabel.Anchor = AnchorStyles.None;
            AlarmTimeLabel.AutoSize = true;
            AlarmTimeLabel.Text = "Godzina alarmu";
            AlarmTimeLabel.TextAlign = ContentAlignment.MiddleCenter;

            AlarmStartTimePicker = new DateTimePicker();
            AlarmStartTimePicker.Format = DateTimePickerFormat.Time;
            AlarmStartTimePicker.ShowUpDown = true;

            AlarmStartDatePanel = new TableLayoutPanel();
            AlarmStartDatePanel.AutoSize = true;
            AlarmStartDatePanel.Anchor = AnchorStyles.None;
            AlarmStartDatePanel.RowCount = 4;
            AlarmStartDatePanel.ColumnCount = 1;

            AlarmStartDatePanel.Controls.Add(AlarmDateLabel, 0, 0);
            AlarmStartDatePanel.Controls.Add(AlarmStartDatePicker, 0, 1);
            AlarmStartDatePanel.Controls.Add(AlarmTimeLabel, 0, 2);
            AlarmStartDatePanel.Controls.Add(AlarmStartTimePicker, 0, 3);

            AlarmStartDatePanel.Location = new Point(10, 10);

            this.Controls.Add(AlarmStartDatePanel);
        }

        private void InitializeTextInputs()
        {
            TaskNameInput = new InputControl();
            TaskNameInput.Title = "Nazwa zadania";
            TaskNameInput.Anchor = AnchorStyles.None;

            DescInput = new InputControl();
            DescInput.Title = "Opis zadania";
            DescInput.Anchor = AnchorStyles.None;
            DescInput.InputTextBox.Multiline = true;
            DescInput.InputTextBox.Size = new Size(DescInput.InputTextBox.Width, DescInput.InputTextBox.Height * 2);
            DescInput.Size = new Size((int)(DescInput.InputTextBox.Width * 2.3), DescInput.Height);

            IsActiveCheckBox = new CheckBox();
            IsActiveCheckBox.AutoSize = true;
            IsActiveCheckBox.Anchor = AnchorStyles.None;
            IsActiveCheckBox.Checked = true;
            IsActiveCheckBox.Text = "Zadanie aktywne";

            TextInputPanel = new TableLayoutPanel();
            TextInputPanel.AutoSize = true;
            TextInputPanel.Anchor = AnchorStyles.None;
            TextInputPanel.RowCount = 3;
            TextInputPanel.ColumnCount = 1;

            TextInputPanel.Controls.Add(IsActiveCheckBox, 0, 0);
            TextInputPanel.Controls.Add(TaskNameInput, 0, 1);
            TextInputPanel.Controls.Add(DescInput, 0, 2);

            TextInputPanel.Location = new Point(AlarmStartDatePanel.Location.X + AlarmStartDatePanel.Width, AlarmStartDatePanel.Location.Y);

            this.Controls.Add(TextInputPanel);
        }

        private void InitializeTimeInput()
        {
            IsPeriodicallyCheckBox = new CheckBox();
            IsPeriodicallyCheckBox.AutoSize = true;
            IsPeriodicallyCheckBox.Anchor = AnchorStyles.None;
            IsPeriodicallyCheckBox.Text = "Zadanie cykliczne";
            IsPeriodicallyCheckBox.CheckedChanged += IsPeriodicallyCheckBox_CheckedChanged;

            this.Controls.Add(IsPeriodicallyCheckBox);

            IntervalLabel = new Label();
            IntervalLabel.Anchor = AnchorStyles.None;
            IntervalLabel.AutoSize = true;
            IntervalLabel.Text = "Odtwarzaj alarm co:";
            IntervalLabel.TextAlign = ContentAlignment.MiddleCenter;

            NotificationCountLabel = new Label();
            NotificationCountLabel.Anchor = AnchorStyles.None;
            NotificationCountLabel.AutoSize = true;
            NotificationCountLabel.Text = "Liczba powiadomień:";
            NotificationCountLabel.TextAlign = ContentAlignment.MiddleCenter;

            NotificationCountNumeric = new NumericUpDown();
            NotificationCountNumeric.Anchor = AnchorStyles.None;
            NotificationCountNumeric.AutoSize = true;
            NotificationCountNumeric.TextAlign = HorizontalAlignment.Center;
            NotificationCountNumeric.Maximum = 100;
            NotificationCountNumeric.Minimum = 1;
            NotificationCountNumeric.ValueChanged += NotificationCountNumeric_ValueChanged;
            NotificationCountNumeric.Value = 1;

            this.Controls.Add(NotificationCountNumeric);

            NotificationCountPanel = new TableLayoutPanel();
            NotificationCountPanel.AutoSize = true;
            NotificationCountPanel.Anchor = AnchorStyles.None;
            NotificationCountPanel.RowCount = 2;
            NotificationCountPanel.ColumnCount = 1;

            NotificationCountPanel.Controls.Add(NotificationCountLabel, 0, 0);
            NotificationCountPanel.Controls.Add(NotificationCountNumeric, 0, 1);

            this.Controls.Add(NotificationCountPanel);

            IntervalHoursLabel = new Label();
            IntervalHoursLabel.Anchor = AnchorStyles.None;
            IntervalHoursLabel.AutoSize = true;
            IntervalHoursLabel.Text = "godzin:";
            IntervalHoursLabel.TextAlign = ContentAlignment.MiddleCenter;

            IntervalHoursNumeric = new NumericUpDown();
            IntervalHoursNumeric.Anchor = AnchorStyles.None;
            IntervalHoursNumeric.AutoSize = true;
            IntervalHoursNumeric.TextAlign = HorizontalAlignment.Center;
            IntervalHoursNumeric.Maximum = Int32.MaxValue;
            IntervalHoursNumeric.Minimum = 0;

            IntervalMinutesLabel = new Label();
            IntervalMinutesLabel.Anchor = AnchorStyles.None;
            IntervalMinutesLabel.AutoSize = true;
            IntervalMinutesLabel.Text = "minut:";
            IntervalMinutesLabel.TextAlign = ContentAlignment.MiddleCenter;

            IntervalMinutesNumeric = new NumericUpDown();
            IntervalMinutesNumeric.Anchor = AnchorStyles.None;
            IntervalMinutesNumeric.AutoSize = true;
            IntervalMinutesNumeric.TextAlign = HorizontalAlignment.Center;
            IntervalMinutesNumeric.Maximum = Int32.MaxValue;
            IntervalMinutesNumeric.Minimum = 0;

            IntervalSecondsLabel = new Label();
            IntervalSecondsLabel.Anchor = AnchorStyles.None;
            IntervalSecondsLabel.AutoSize = true;
            IntervalSecondsLabel.Text = "sekund:";
            IntervalSecondsLabel.TextAlign = ContentAlignment.MiddleCenter;

            IntervalSecondsNumeric = new NumericUpDown();
            IntervalSecondsNumeric.Anchor = AnchorStyles.None;
            IntervalSecondsNumeric.AutoSize = true;
            IntervalSecondsNumeric.TextAlign = HorizontalAlignment.Center;
            IntervalSecondsNumeric.Maximum = Int32.MaxValue;
            IntervalSecondsNumeric.Minimum = 0;

            IntervalPanel = new TableLayoutPanel();
            IntervalPanel.AutoSize = true;
            IntervalPanel.Anchor = AnchorStyles.None;
            IntervalPanel.RowCount = 7;
            IntervalPanel.ColumnCount = 1;

            IntervalPanel.Controls.Add(IntervalLabel, 0, 0);
            IntervalPanel.Controls.Add(IntervalHoursLabel, 0, 1);
            IntervalPanel.Controls.Add(IntervalHoursNumeric, 0, 2);
            IntervalPanel.Controls.Add(IntervalMinutesLabel, 0, 3);
            IntervalPanel.Controls.Add(IntervalMinutesNumeric, 0, 4);
            IntervalPanel.Controls.Add(IntervalSecondsLabel, 0, 5);
            IntervalPanel.Controls.Add(IntervalSecondsNumeric, 0, 6);

            NotificationTimesPanel = new TableLayoutPanel();
            NotificationTimesPanel.AutoSize = true;
            NotificationTimesPanel.Anchor = AnchorStyles.None;
            NotificationTimesPanel.RowCount = 2;
            NotificationTimesPanel.ColumnCount = 1;
            NotificationTimesPanel.Location = new Point(TextInputPanel.Location.X + TextInputPanel.Width,
                                               TextInputPanel.Location.Y + IsPeriodicallyCheckBox.Height);

            NotificationTimesPanel.Controls.Add(NotificationCountPanel, 0, 0);
            NotificationTimesPanel.Controls.Add(IntervalPanel, 0, 1);


            this.Controls.Add(NotificationTimesPanel);

            IsPeriodicallyCheckBox.Location = new Point(TextInputPanel.Location.X + TextInputPanel.Width + (IntervalPanel.Width ) / 2,
                                                        TextInputPanel.Location.Y);

            IsPeriodicallyCheckBox.Checked = true;

        }

        private void InitializeDetailsPanel()
        {
            DetailsPanel = new TableLayoutPanel();
            DetailsPanel.AutoSize = true;
            DetailsPanel.Anchor = AnchorStyles.None;
            DetailsPanel.RowCount = 1;
            DetailsPanel.ColumnCount = 1;

            ReactIsRequiredCheckBox = new CheckBox();
            ReactIsRequiredCheckBox.AutoSize = true;
            ReactIsRequiredCheckBox.Anchor = AnchorStyles.None;
            ReactIsRequiredCheckBox.Checked = false;
            ReactIsRequiredCheckBox.Text = "Powiadamiaj aż do zareagowania";

            DetailsPanel.Controls.Add(ReactIsRequiredCheckBox, 0, 0);

            DetailsPanel.Location = new Point(this.Width - DetailsPanel.Width - 30, IntervalPanel.Location.Y + IntervalPanel.Height);

            Controls.Add(DetailsPanel);
        }

        private void NotificationCountNumeric_ValueChanged(object? sender, EventArgs e)
        {
            IntervalPanel.Visible = (int)NotificationCountNumeric.Value > 1;
        }

        private void InitializeAddButton()
        {
            AddButton = new Button();
            AddButton.Text = "Dodaj zadanie";
            AddButton.Dock = DockStyle.Bottom;
            AddButton.Size = new Size(AddButton.Width, AddButton.Height * 2);

            AddButton.Click += AddButton_Click;

            this.Controls.Add(AddButton);
        }

        private void InitializeSaveButton()
        {
            SaveButton = new Button();
            SaveButton.Text = "Zapisz zadanie";
            SaveButton.Dock = DockStyle.Bottom;
            SaveButton.Size = new Size(SaveButton.Width, SaveButton.Height * 2);

            SaveButton.Click += SaveButton_Click;

            this.Controls.Add(SaveButton);
        }

        private void InitializeSoundPanel()
        {
            int secondRowY = Math.Max(AlarmStartDatePanel.Height, TextInputPanel.Height);
            secondRowY = Math.Max(secondRowY, IntervalPanel.Height);

            IsSoundCheckbox = new CheckBox();
            IsSoundCheckbox.AutoSize = true;
            IsSoundCheckbox.Anchor = AnchorStyles.None;
            IsSoundCheckbox.Checked = true;
            IsSoundCheckbox.Text = "Czy powiadomienie dźwiękowe";
            IsSoundCheckbox.CheckedChanged += IsSoundCheckBox_CheckedChanged;


            IsOnceNotificationCheckBox = new CheckBox();
            IsOnceNotificationCheckBox.AutoSize = true;
            IsOnceNotificationCheckBox.Anchor = AnchorStyles.None;
            IsOnceNotificationCheckBox.Checked = false;
            IsOnceNotificationCheckBox.Text = "Czy powiadomienie dźwiękowe jednorazowe";


            SoundChooseButton = new Button();
            SoundChooseButton.AutoSize = true;
            SoundChooseButton.Anchor = AnchorStyles.None;
            SoundChooseButton.Text = "Wybierz dźwięk powiadomienia";
            SoundChooseButton.Click += SoundChooseButton_Click;

            ChosenSoundFileLabel = new Label();
            ChosenSoundFileLabel.Anchor = AnchorStyles.None;
            ChosenSoundFileLabel.AutoSize = true;
            ChosenSoundFileLabel.Text = "";
            ChosenSoundFileLabel.TextAlign = ContentAlignment.MiddleCenter;

            RecordVoiceButton = new Button();
            RecordVoiceButton.AutoSize = true;
            RecordVoiceButton.Anchor = AnchorStyles.None;
            RecordVoiceButton.Text = "Kliknij aby rozpocząć nagrywanie";
            RecordVoiceButton.Click += RecordVoiceButton_Click; ;


            SoundTypeComboBox = new ComboBox();
            SoundTypeComboBox.AutoSize = true;
            SoundTypeComboBox.Anchor = AnchorStyles.None;
            SoundTypeComboBox.Items.AddRange(NotificationOptions);
            SoundTypeComboBox.SelectedValueChanged += SoundTypeComboBox_SelectedValueChanged;
            SoundTypeComboBox.SelectedIndex = 0;
            SoundTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            SoundPanel = new TableLayoutPanel();
            SoundPanel.AutoSize = true;
            SoundPanel.Anchor = AnchorStyles.None;
            SoundPanel.RowCount = 6;
            SoundPanel.ColumnCount = 1;
            SoundPanel.Location = new Point(20, secondRowY + 20);

            SoundPanel.Controls.Add(IsSoundCheckbox, 0, 0);
            SoundPanel.Controls.Add(IsOnceNotificationCheckBox, 0, 1);
            SoundPanel.Controls.Add(SoundTypeComboBox, 0, 2);
            SoundPanel.Controls.Add(SoundChooseButton, 0, 3);
            SoundPanel.Controls.Add(ChosenSoundFileLabel, 0, 4);
            SoundPanel.Controls.Add(RecordVoiceButton, 0, 5);

            this.Controls.Add(SoundPanel);

        }

        private void RecordVoiceButton_Click(object? sender, EventArgs e)
        {
            Button button = sender as Button;
            
            if (button.Text[0] == 'K')
            {
                voiceRecorder = new VoiceRecorder();
                button.Text = "Zakończ nagrywanie";
                SubmitButton.Enabled = false;
                voiceRecorder.StartRecording();
            }
            else
            {
                button.Text = "Kliknij, aby rozpocząć nagrywanie";
                voiceRecorder.StopRecording();
                SubmitButton.Enabled = true;
                soundFilename = voiceRecorder.Filename;

            }
            
        }

        private void fillInputs()
        {
            if (editingTask is null)
            {
                return;
            }
            IsPeriodicallyCheckBox.Checked = editingTask.IsPeriodically;
            TaskNameInput.Text = editingTask.Title;
            DescInput.Text = editingTask.Description!;
            DateTime date = editingTask.AlarmDateTime.AddHours(-editingTask.AlarmDateTime.Hour).
                                                      AddMinutes(-editingTask.AlarmDateTime.Minute).
                                                      AddSeconds(-editingTask.AlarmDateTime.Second);
            AlarmStartDatePicker.Value = date;
            AlarmStartTimePicker.Value = editingTask.AlarmDateTime;
            IntervalHoursNumeric.Value = editingTask.IntervalSeconds / 3600;
            IntervalMinutesNumeric.Value = (editingTask.IntervalSeconds / 60) % 60;
            IntervalSecondsNumeric.Value = editingTask.IntervalSeconds % 60;

            IsSoundCheckbox.Checked = editingTask.IsSoundNotification;
            IsActiveCheckBox.Checked = editingTask.IsActive;
            IsOnceNotificationCheckBox.Checked = editingTask.IsOnceNotification;
            NotificationCountNumeric.Value = editingTask.MaxNotificationCount;

            /*if(typeof(SoundNotify).IsInstanceOfType(editingTask.notifycation))
            {
                SoundTypeComboBox.SelectedIndex = (int)NotificationOption.Default;
            }
            else if(typeof(MP3SoundNotify).IsInstanceOfType(editingTask.notifycation))
            {
                string chosenSongFilename = ((MP3SoundNotify)editingTask.notifycation).SoundFilename;
                if (chosenSongFilename == "")
                {
                    SoundTypeComboBox.SelectedIndex = (int)NotificationOption.Nokia;
                }
                else
                {
                    SoundTypeComboBox.SelectedIndex = (int)NotificationOption.ChooseOwn;
                    this.soundFilename = chosenSongFilename;
                }
            }
            else if (typeof(WavSoundNotify).IsInstanceOfType(editingTask.notifycation))
            {
                SoundTypeComboBox.SelectedIndex = (int)NotificationOption.RecordOwn;
            }
            else if(typeof(VoiceReader).IsInstanceOfType(editingTask.notifycation))
            {
                if (((VoiceReader)(editingTask.notifycation)).TextToRead.Equals(editingTask.Title))
                {
                    SoundTypeComboBox.SelectedIndex = (int)NotificationOption.VoiceTitleRead;
                }
                SoundTypeComboBox.SelectedIndex = (int)NotificationOption.VoiceDescriptionRead;
            }*/
            SoundTypeComboBox.SelectedIndex = (int)editingTask.SoundNotificationType;
            ChosenSoundFileLabel.Text = soundFilename;


        }

        private void SoundTypeComboBox_SelectedValueChanged(object? sender, EventArgs e)
        {
            if (sender == null) { return; }

            ComboBox comboBox = sender as ComboBox;
            if (comboBox!.SelectedIndex == (int)NotificationOption.ChooseOwn)
            {
                SoundChooseButton.Visible = true;
                ChosenSoundFileLabel.Visible = true;
            }
            else
            {
                SoundChooseButton.Visible = false;
                ChosenSoundFileLabel.Visible = false;
            }

            if(comboBox!.SelectedIndex == (int)NotificationOption.RecordOwn)
            {
                RecordVoiceButton.Visible = true;
            }
            else
            {
                RecordVoiceButton.Visible = false;
            }
        }

        private void AddButton_Click(object? sender, EventArgs e)
        {
            src.Task newTask = getTaskFromInputs();

            if (newTask.Validate())
            {
                Application.data.Add(newTask);
                MessageBox.Show("Zadanie dodano pomyślnie");
            }
            else
            {
                MessageBox.Show("Zadanie nie jest poprawne");
            }
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            src.Task editedTaskToValidate = getTaskFromInputs();
            editedTaskToValidate.IsActive = false;

            if (editedTaskToValidate.Validate())
            {
                editingTask.AlarmDateTime = editedTaskToValidate.AlarmDateTime;
                editingTask.Title = TaskNameInput.Text;
                editingTask.Description = DescInput.Text;
                editingTask.IntervalSeconds = editedTaskToValidate.IntervalSeconds;
                editingTask.IsPeriodically = IsPeriodicallyCheckBox.Checked;
                editingTask.IsActive = IsActiveCheckBox.Checked;
                editingTask.IsSoundNotification = IsSoundCheckbox.Checked;
                editingTask.IsOnceNotification = IsOnceNotificationCheckBox.Checked;
                editingTask.MaxNotificationCount = (int)NotificationCountNumeric.Value;
                editingTask.notifycation = editedTaskToValidate.notifycation;
                editingTask.SoundNotificationType = (NotificationOption)SoundTypeComboBox.SelectedIndex;
                editingTask.notifyCount = 0;

                Application.data.UpdateViewingData();

                MessageBox.Show("Zadanie zapisano pomyślnie");
            }
            else
            {
                MessageBox.Show("Zadanie nie jest poprawne");
            }
        }

        private void SoundChooseButton_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Pliki MP3 (*.mp3)|*.mp3";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Pobierz ścieżkę wybranego pliku
                    selectedFilePath = openFileDialog.FileName;

                    try
                    {
                        string path = DataDisk.SoundFilenamesPath;

                        if(!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        this.soundFilename = openFileDialog.SafeFileName;
                        this.ChosenSoundFileLabel.Text = soundFilename;

                        MessageBox.Show("Plik został pomyślnie wybrany.", "Sukces");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Wystąpił błąd podczas wybierania pliku: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void IsPeriodicallyCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            bool value = ((CheckBox)sender).Checked;
            if (value)
            {
                AlarmDateLabel.Text = "Data startu odliczania alarmu";
                IntervalPanel.Visible = true;
                NotificationCountLabel.Visible = false;
                NotificationCountNumeric.Visible = false;
            }
            else
            {
                AlarmDateLabel.Text = "Data alarmu";
                IntervalPanel.Visible = (int)NotificationCountNumeric.Value > 1;
                NotificationCountLabel.Visible = true;
                NotificationCountNumeric.Visible = true;
            }
        }

        private void IsSoundCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            bool value = ((CheckBox)sender).Checked;
            if (value)
            {
                SoundChooseButton.Visible = true;
                IsOnceNotificationCheckBox.Visible = true;
                SoundTypeComboBox.Visible = true;
            }
            else
            {
                SoundChooseButton.Visible = false;
                IsOnceNotificationCheckBox.Visible = false;
                SoundTypeComboBox.Visible = false;
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // TaskAddEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(683, 436);
            Name = "TaskAddEditForm";
            Text = "Dodawanie zadania";
            ResumeLayout(false);
        }

        private INotification? getSoundFromInputs()
        {
            if (!IsSoundCheckbox.Checked) { return null; }

            int option = (int)SoundTypeComboBox.SelectedIndex;
            int options_count = SoundTypeComboBox.Items.Count;
            INotification? sound = null;

            switch (option)
            {
                case (int)NotificationOption.Default:
                    sound = new SoundNotify();
                    break;
                case (int)NotificationOption.Nokia:
                    sound = new MP3SoundNotify();
                    break;
                case (int)NotificationOption.VoiceTitleRead:
                    sound = new VoiceReader(TaskNameInput.Text);
                    break;
                case (int)NotificationOption.VoiceDescriptionRead:
                    sound = new VoiceReader(DescInput.Text);
                    break;
                case (int)NotificationOption.RecordOwn:
                    sound = new WavSoundNotify(this.soundFilename);
                    break;
                case (int)NotificationOption.ChooseOwn:
                    string destinationFilePath = Path.Combine(DataDisk.SoundFilenamesPath, this.soundFilename);
                    if (!File.Exists(destinationFilePath))
                    {
                        File.Copy(selectedFilePath, destinationFilePath, true);
                    }
                    sound = new MP3SoundNotify(this.soundFilename);
                    break;
                default:
                    sound = null;
                    break;
            }
            return sound;
        }

        public src.Task getTaskFromInputs()
        {
            DateTime datetime = new DateTime(AlarmStartDatePicker.Value.Year, AlarmStartDatePicker.Value.Month, AlarmStartDatePicker.Value.Day);
            datetime = datetime.AddHours(AlarmStartTimePicker.Value.Hour).AddMinutes(AlarmStartTimePicker.Value.Minute).
                                AddSeconds(AlarmStartTimePicker.Value.Second);
            long interval = (int)IntervalHoursNumeric.Value * 3600 + (int)IntervalMinutesNumeric.Value * 60 + (int)IntervalSecondsNumeric.Value;
            INotification? sound = getSoundFromInputs();

            src.Task newTask = new src.Task(datetime,
                                            TaskNameInput.Text,
                                            DescInput.Text,
                                            interval,
                                            IsPeriodicallyCheckBox.Checked,
                                            IsActiveCheckBox.Checked,
                                            sound);
            newTask.IsSoundNotification = IsSoundCheckbox.Checked;
            newTask.IsOnceNotification = IsOnceNotificationCheckBox.Checked;
            newTask.MaxNotificationCount = (int)NotificationCountNumeric.Value;
            newTask.SoundNotificationType = (NotificationOption)SoundTypeComboBox.SelectedIndex;
            return newTask;
        }

    }
}
