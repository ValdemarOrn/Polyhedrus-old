using AudioLib;
using Polyhedrus.Modules;
using Polyhedrus.Parameters;
using Polyhedrus.UI;
using Polyhedrus.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public class SynthController
	{
		public Voice[] Voices;
		public AudioDeviceModule AudioDevice;
		public SynthView View;

		public SynthController()
		{
			Voices = new Voice[1];
			Voices[0] = new Voice();
		}

		public void SetSamplerate(double samplerate)
		{
			foreach (var voice in Voices)
			{
				voice.Osc1.Samplerate = samplerate;
				voice.Osc2.Samplerate = samplerate;
				voice.Filter1.Samplerate = samplerate;
				voice.Filter2.Samplerate = samplerate;
				voice.AmpEnv.Samplerate = samplerate;
			}
		}

		public void NoteOn(int note, double velocity)
		{
			for(int i = 0; i < Voices.Length; i++)
				Voices[i].SetNote(note, velocity);
		}

		public void SetPitchWheel(double value)
		{
			for (int i = 0; i < Voices.Length; i++)
				Voices[i].MidiInput.PitchBend = value;
		}

		internal void CreateView()
		{
			View = new SynthView(this);
		}

		public void Process(double[][] buffer)
		{
			Voices[0].Process(buffer);
		}

		public void SetParameter(ModuleParams module, Enum parameter, object value)
		{
			foreach (var voice in Voices)
				voice.SetParameter(module, parameter, value);
		}

		
	}
}
