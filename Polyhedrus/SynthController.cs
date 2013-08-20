using AudioLib;
using Polyhedrus.Modules;
using Polyhedrus.Parameters;
using Polyhedrus.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public class SynthController
	{
		Voice[] Voices;

//		public SynthView View;
//		public ViewModel VM;

		public SynthController()
		{
			Voices = new Voice[1];
			Voices[0] = new Voice();

//			VM = new ViewModel(this);
//			View = new SynthView(VM);
		}

		public void SetSamplerate(double samplerate)
		{
			foreach (var voice in Voices)
			{
				// Todo: Check that all modules are set
				voice.Osc1.Samplerate = samplerate;
				voice.Osc2.Samplerate = samplerate;
				voice.Osc3.Samplerate = samplerate;
				voice.Osc4.Samplerate = samplerate;
				voice.Filter1.Samplerate = samplerate;
				voice.Filter2.Samplerate = samplerate;
				voice.AmpEnv.Samplerate = samplerate;
				voice.Filter1Env.Samplerate = samplerate;
				voice.Filter2Env.Samplerate = samplerate;
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
