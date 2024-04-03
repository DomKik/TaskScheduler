using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksScheduler.Interfaces;
using System.Speech.Synthesis;
using TasksScheduler.src.utils;

namespace TasksScheduler.src.Notyfing
{
    public class VoiceReader : INotification
    {
        public string TextToRead;
        private bool isNotyfying;
        SpeechSynthesizer synthesizer = new SpeechSynthesizer();

        public VoiceReader(string textToRead)
        {
            TextToRead = textToRead;
            isNotyfying = false;
        }
        public VoiceReader() 
        {
            TextToRead = "";
            isNotyfying = false;
        }

        public async void notify()
        {
            await System.Threading.Tasks.Task.Run(() => synthesizer.Speak(TextToRead));
        }

        public void StartNotyfing()
        {
            synthesizer.SpeakCompleted += Synthesizer_SpeakCompleted;
            isNotyfying = true;
            synthesizer.SpeakAsync(TextToRead);
        }

        private void Synthesizer_SpeakCompleted(object? sender, SpeakCompletedEventArgs e)
        {
            ((SpeechSynthesizer)sender).SpeakAsync(TextToRead);
        }

        public void StopNotyfing()
        {
            synthesizer.SpeakCompleted -= Synthesizer_SpeakCompleted;
            synthesizer.SpeakAsyncCancelAll();
            isNotyfying = false;
        }

        public bool IsNotyfying()
        {
            return isNotyfying;
        }
    }
}
