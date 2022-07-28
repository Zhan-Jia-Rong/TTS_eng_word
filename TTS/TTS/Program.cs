using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;


namespace TTS
{
    class Program
    {
        static SpeechSynthesizer TTS = new SpeechSynthesizer();  
        private static void check_installed_sound() //check installed sound package
        {
            foreach (InstalledVoice voice in TTS.GetInstalledVoices())
            {
                VoiceInfo info = voice.VoiceInfo;
                string AudioFormats = "";
                foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                {
                    AudioFormats += String.Format("{0}\n",
                    fmt.EncodingFormat.ToString());
                }

                Console.WriteLine(" Name:          " + info.Name);
                Console.WriteLine(" Culture:       " + info.Culture);
                Console.WriteLine(" Age:           " + info.Age);
                Console.WriteLine(" Gender:        " + info.Gender);
                Console.WriteLine(" Description:   " + info.Description);
                Console.WriteLine(" ID:            " + info.Id);
                Console.WriteLine(" Enabled:       " + voice.Enabled);
                if (info.SupportedAudioFormats.Count != 0)
                {
                    Console.WriteLine(" Audio formats: " + AudioFormats);
                }
                else
                {
                    Console.WriteLine(" No supported audio formats found");
                }

                string AdditionalInfo = "";
                foreach (string key in info.AdditionalInfo.Keys)
                {
                    AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                }

                Console.WriteLine(" Additional Info - " + AdditionalInfo);
                Console.WriteLine();
            }
        }
        private static void tts_exaample() //speak string directly
        {
            //check_installed_sound();
            //TTS.SelectVoice("Microsoft Server Speech Text to Speech Voice (zh-TW,TELE)");
            //TTS.SelectVoiceByHints((VoiceGender)1, (VoiceAge)10);
            
            TTS.Volume = 100;
            TTS.Rate = 1;
            TTS.Speak("I forgot when i was younger , it's was easy");
            TTS.Speak("天后小助理");
            
        }
        private static void sound_output_example() //output sound file(.wav) example
        {
            TTS.SetOutputToWaveFile(@"..\..\..\TTS.wav", new SpeechAudioFormatInfo(64000, AudioBitsPerSample.Sixteen, AudioChannel.Stereo));
            System.Media.SoundPlayer m_SoundPlayer = new System.Media.SoundPlayer(@"..\..\..\TTS.wav"); // Create a SoundPlayer instance to play output audio file.  
            PromptBuilder builder = new PromptBuilder(); // Build a prompt.
            string eng = "I forgot when i was younger, it's was easy.";
            string zh = "(西嘉嘉)";
            builder.AppendText(eng + " " + zh, PromptEmphasis.Moderate);
            TTS.Speak(builder); // Speak the prompt.  
            m_SoundPlayer.Play();
        }
        private static void sound_output() //output sound file(.wav)
        {
            TTS.SetOutputToWaveFile(@"..\..\..\TTS.wav", new SpeechAudioFormatInfo(64000, AudioBitsPerSample.Sixteen, AudioChannel.Stereo));
            PromptBuilder builder = new PromptBuilder(); // Build a prompt.
            TTS.Rate = -2;
            TTS.Volume = 100;
            String line;
            try
            {
                StreamReader sr = new StreamReader(@"..\..\..\Eng_word.txt");
                line = sr.ReadLine(); //Read the first line of text
                string eng = "";
                string zh = "";
                while (line != null) //Continue to read until you reach end of file
                {
                    string eng_word = "";
                    Console.WriteLine(line);
                    for (int i = 0; i < line.Length; i++)
                    {
                        if ((int)line[i] > 127)
                        {
                            eng = line.Substring(0, i);
                            zh = line.Substring(i, (line.Length) - i);
                            break;
                        }
                        else
                        {
                            eng_word = eng_word + line[i] + " ";
                        }
                    }
                    /*  //one by one read eng word.
                    builder.AppendText(zh, PromptEmphasis.Reduced);
                    builder.AppendBreak(PromptBreak.Medium);

                    builder.AppendText(eng, PromptEmphasis.Reduced);
                    builder.AppendBreak(PromptBreak.Medium);

                    builder.AppendText(eng_word, PromptEmphasis.NotSet);
                    builder.AppendBreak(PromptBreak.Large);
                    */

                    // normal read eng word.
                    builder.AppendText(zh, PromptEmphasis.Reduced);
                    builder.AppendBreak(PromptBreak.Medium);

                    builder.AppendText(eng, PromptEmphasis.Reduced);
                    builder.AppendBreak(PromptBreak.Large);

                    line = sr.ReadLine(); //Read the next line
                }
                TTS.Speak(builder); // Speak the prompt.
                sr.Close();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }
        private static void str_zh_eng_spilt() //spilt string test
        {
            string a = "abc  我的超人";
            string eng = "";
            string zh = "";
            for (int i = 0; i < a.Length; i++)
            {
                if ((int)a[i] > 127)
                {
                    eng = a.Substring(0, i);
                    zh = a.Substring(i, (a.Length) - i);
                    break;
                }
            }
            Console.WriteLine(eng);
            Console.WriteLine(zh);
        }
        static void Main(string[] args)
        {
            sound_output();


            Console.ReadKey();
        }
    }
}
