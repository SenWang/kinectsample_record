using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.IO;
using System.Media;
using NAudio.Wave;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            KinectSensor sensor = KinectSensor.KinectSensors[0];
            sensor.Start();

            AudioSourceSetup(sensor.AudioSource);

            Console.WriteLine("按上開始錄音，按下播放錄音，空白鍵結束程式 ");
            ConsoleKey presskey;
            while ((presskey = Console.ReadKey().Key) != ConsoleKey.Spacebar)
            {
                if(presskey == ConsoleKey.UpArrow)
                    StartRecord(sensor.AudioSource);
                else if (presskey == ConsoleKey.DownArrow)
                    StartPlayback();
            }

            sensor.Stop();
        }

        static void StartRecord(KinectAudioSource audiosource)
        {
            Console.WriteLine("開始錄音");
            int bufferSize = 50000;
            byte[] soundSampleBuffer = new byte[bufferSize];
            Stream kinectAudioStream = audiosource.Start();
            kinectAudioStream.Read(soundSampleBuffer, 0, soundSampleBuffer.Length);
            audiosource.Stop();

            SaveToWaveFile(soundSampleBuffer);
            Console.WriteLine("錄音結束");
        }
        static string filename = "record.wav";
        static void SaveToWaveFile(byte[] sounddata)
        {
                var newFormat = new WaveFormat(16000, 16, 1); //改成2會有特效
                using (WaveFileWriter wfw = new WaveFileWriter(filename, newFormat))
                {
                    wfw.Write(sounddata, 0, sounddata.Length);
                }
        }
        static void StartPlayback()
        {
            Console.WriteLine("開始播放錄音");
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            SoundPlayer sp = new SoundPlayer(fs);
            sp.Play();
            fs.Close();
            Console.WriteLine("播放錄音結束");           
        }

        static void AudioSourceSetup(KinectAudioSource audiosource)
        {
            //audiosource.NoiseSuppression = true;
            //audiosource.AutomaticGainControlEnabled = true;
            //audiosource.EchoCancellationMode = EchoCancellationMode.CancellationAndSuppression;
        }


    }
}
