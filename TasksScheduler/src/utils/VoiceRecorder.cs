using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksScheduler.src.utils
{
    public class VoiceRecorder
    {
        public WaveIn? waveSource = null;
        public WaveFileWriter? waveFile = null;
        public string Filename;

        public VoiceRecorder() 
        {
            Filename = "record" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss");
        }

        private void waveSource_DataAvailable(object? sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }

        private void waveSource_RecordingStopped(object? sender, StoppedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
        }

        public void StartRecording()
        {
            waveSource = new WaveIn();
            waveSource.WaveFormat = new WaveFormat(44100, 1);

            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

            string path = Path.Combine(DataDisk.SoundFilenamesPath, Filename);
            waveFile = new WaveFileWriter(path, waveSource.WaveFormat);

            waveSource.StartRecording();
        }

        public void StopRecording() 
        {
            waveSource!.StopRecording();
        }
    }
}
