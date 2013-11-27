using AudioLib;
using Polyhedrus.Modules;
using Polyhedrus.Parameters;
using Polyhedrus.Utils;
using Polyhedrus.WT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public sealed partial class SynthController
	{
		Voice[] Voices;
		List<MidiNote> Notes;
		long TriggerIndex;
		int Polyphony { get { return Voices.Length; } }
		Dictionary<ParameterKey, object> Parameters;

		private WavetableData osc1Wavetable;
		private WavetableData osc2Wavetable;
		private WavetableData osc3Wavetable;
		private WavetableData osc4Wavetable;

		public SynthController()
		{
			LowProfile.Fourier.Double.TransformNative.Setup();
			WavetableContext.Setup();
			Parameters = new Dictionary<ParameterKey, object>();
			TriggerIndex = 1;
			Notes = new List<MidiNote>(32);
			Voices = new Voice[6];

			for(int i = 0; i < Voices.Length; i++)
				Voices[i] = new Voice();

			SetDefaults();
		}

		public string[] Wavetables
		{
			get { return WavetableContext.AvailableWavetables.Keys.ToArray(); }
		}

		public Dictionary<AudioLib.Modules.LFO.Wave, string> LFOWaves
		{
			get { return AudioLib.Modules.LFO.WaveNames; }
		}

		public WavetableData GetWavetable(ModuleParams module)
		{
			switch (module)
			{
				case ModuleParams.Osc1:
					return osc1Wavetable;
				case ModuleParams.Osc2:
					return osc2Wavetable;
				case ModuleParams.Osc3:
					return osc3Wavetable;
				case ModuleParams.Osc4:
					return osc4Wavetable;
				default:
					return null;
			}
		}
		
		public void SetWavetable(ModuleParams module, string wavetableName)
		{
			switch(module)
			{
				case ModuleParams.Osc1:
					osc1Wavetable = WavetableData.FromFile(WavetableContext.AvailableWavetables[wavetableName]);
					for (int i = 0; i < Voices.Length; i++)
					{
						if (i == 0)
							Voices[0].Osc1.SetWave(osc1Wavetable.Data);
						else
							Voices[i].Osc1.Wavetable = Voices[0].Osc1.Wavetable;
					}
					return;
				case ModuleParams.Osc2:
					osc2Wavetable = WavetableData.FromFile(WavetableContext.AvailableWavetables[wavetableName]);
					for (int i = 0; i < Voices.Length; i++)
					{
						if (i == 0)
							Voices[0].Osc2.SetWave(osc2Wavetable.Data);
						else
							Voices[i].Osc2.Wavetable = Voices[0].Osc2.Wavetable;
					}
					return;
				case ModuleParams.Osc3:
					osc3Wavetable = WavetableData.FromFile(WavetableContext.AvailableWavetables[wavetableName]);
					for (int i = 0; i < Voices.Length; i++)
					{
						if (i == 0)
							Voices[0].Osc3.SetWave(osc3Wavetable.Data);
						else
							Voices[i].Osc3.Wavetable = Voices[0].Osc3.Wavetable;
					}
					return;
				case ModuleParams.Osc4:
					osc4Wavetable = WavetableData.FromFile(WavetableContext.AvailableWavetables[wavetableName]);
					for (int i = 0; i < Voices.Length; i++)
					{
						if (i == 0)
							Voices[0].Osc4.SetWave(osc4Wavetable.Data);
						else
							Voices[i].Osc4.Wavetable = Voices[0].Osc4.Wavetable;
					}
					return;
			}
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

		public void NoteOn(int note, int velocity)
		{
			RemoveNote(note); // delete if already exists
			Notes.Add(new MidiNote(note, velocity));
			int voiceIndex = FindAvailableVoice(note);

			if (Polyphony > 1)
				Voices[voiceIndex].ResetEnvelopes();

			Voices[voiceIndex].NoteOn(note, velocity);
			Voices[voiceIndex].TriggerIndex = TriggerIndex++;
		}

		public void NoteOff(int note, int releaseVelocity)
		{
			RemoveNote(note);
			var newIndex = GetReplacementNoteIndex();

			for(int i = 0; i < Voices.Length; i++)
			{
				var voice = Voices[i];
				if(voice.Note == note)
				{
					if (newIndex >= 0)
						voice.NoteOn(Notes[newIndex].Note, Notes[newIndex].Velocity);
					else
						voice.NoteOff(releaseVelocity);
				}
			}
		}

		public void SetPitchWheel(double value)
		{
			for (int i = 0; i < Voices.Length; i++)
				Voices[i].MidiInput.PitchBend = value;
		}

		public void Process(double[][] buffer)
		{
			for(int i = 0; i < Voices.Length; i++)
				Voices[i].Process(buffer);
		}

		public void SetParameter(ModuleParams module, Enum parameter, object value)
		{
			Parameters[new ParameterKey(module, parameter)] = value;

			foreach (var voice in Voices)
				voice.SetParameter(module, parameter, value);
		}

		public object GetParameter(ModuleParams module, Enum parameter)
		{
			var idx = new ParameterKey(module, parameter);
			if(Parameters.ContainsKey(idx))
				return Parameters[idx];
			else
				return null;
		}

		private void SetDefaults()
		{
			var defaultParams = CreateDefaultParameters();
			foreach (var para in defaultParams)
				SetParameter(para.Key.Module, para.Key.Parameter, para.Value);

			SetWavetable(ModuleParams.Osc1, "Sawtooth Wave");
			SetWavetable(ModuleParams.Osc2, "Sawtooth Wave");
			SetWavetable(ModuleParams.Osc3, "Sawtooth Wave");
			SetWavetable(ModuleParams.Osc4, "Sawtooth Wave");
		}

		private int FindAvailableVoice(int note)
		{
			// First, check if this note is already playing
			for (int i = 0; i < Voices.Length; i++)
				if (Voices[i].Note == note)
					return i;

			// Next, look for inactive voices
			for (int i = 0; i < Voices.Length; i++)
				if (!Voices[i].IsActive)
					return i;

			// if none found, return oldest voice
			long oldestTriggerIndex = Voices[0].TriggerIndex;
			int oldestVoiceIndex = 0;

			for (int i = 1; i < Voices.Length; i++)
			{
				if (Voices[i].TriggerIndex < oldestTriggerIndex)
				{
					oldestTriggerIndex = Voices[i].TriggerIndex;
					oldestVoiceIndex = i;
				}
			}

			return oldestVoiceIndex;
		}

		private int GetReplacementNoteIndex()
		{
			if (Polyphony > 1)
				return -1;

			return Notes.Count - 1;
		}

		/// <summary>
		/// Removes a note from the Notes list and returns the index of the voice associated with it
		/// </summary>
		/// <param name="note"></param>
		/// <returns></returns>
		private void RemoveNote(int note)
		{
			for (int i = 0; i < Notes.Count; i++)
			{
				if (Notes[i].Note == note)
					Notes.RemoveAt(i);
			}
		}
	}
}
