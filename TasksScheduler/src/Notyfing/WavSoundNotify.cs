using Microsoft.VisualBasic.Devices;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksScheduler.Interfaces;
using TasksScheduler.src.utils;

namespace TasksScheduler.src.Notyfing
{
    public class WavSoundNotify : INotification
    {
        [JsonIgnore]
        private LoopStream? loop;
        [JsonIgnore]
        private Stream? stream = null;
        [JsonIgnore]
        private bool isNotyfying;
        [JsonIgnore]
        private WaveOut? waveOut;

        private string soundFilename = "";
        public string SoundFilename
        {
            get { return soundFilename; }
            set
            {
                soundFilename = value;
                init();
            }
        }

        public WavSoundNotify(string soundFilename="") 
        {
            isNotyfying = false;
            this.soundFilename = soundFilename;
            init();
        }

        public WavSoundNotify()
        {
            isNotyfying = false;
            init();
        }

        private void init()
        {
            byte[] fileByte = readSongFromDisk();
            stream = new MemoryStream(fileByte);
            WaveFileReader reader = new NAudio.Wave.WaveFileReader(stream);
            loop = new LoopStream(reader);
            loop.Position = 0;
            waveOut = new WaveOut();
            waveOut.Init(loop);
        }

        private byte[] readSongFromDisk()
        {
            if (soundFilename != "")
            {
                return File.ReadAllBytes(Path.Combine(DataDisk.SoundFilenamesPath, this.soundFilename));
            }
            return new byte[0];
        }

        private void playOnce()
        {
            loop!.Position = 0;
            loop.EnableLooping = false;
            waveOut!.Play();
        }

        public async void notify()
        {
            await System.Threading.Tasks.Task.Run(() => playOnce());
        }

        public void StartNotyfing()
        {
            isNotyfying = true;

            loop!.Position = 0;
            loop!.EnableLooping = true;
            waveOut!.Play();
        }
        public void StopNotyfing()
        {
            isNotyfying = false;
            waveOut!.Stop();
        }
        public bool IsNotyfying()
        {
            return isNotyfying;
        }

    }
}
