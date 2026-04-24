using AISystemGuardian.Models;
using System.Speech.Synthesis;

namespace AISystemGuardian.Service
{
    public class VoiceService
    {
        private SpeechSynthesizer _synth;

        public VoiceService()
        {
            _synth = new SpeechSynthesizer();
            _synth.SetOutputToDefaultAudioDevice();
            _synth.Rate = 0;
            _synth.Volume = 100;
        }

        public void SpeakAlert(Alert alert) 
        {
            string message = $"{alert.Severity} alert. {alert.Message}";
            _synth.SpeakAsync(message);
        }
    }
}
