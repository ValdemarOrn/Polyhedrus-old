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
		RealtimeDispatcher<Voice> Dispatcher;

		private WavetableData osc1Wavetable;
		private WavetableData osc2Wavetable;
		private WavetableData osc3Wavetable;
		private WavetableData osc4Wavetable;

		private volatile int ProcessSampleCount;

		public SynthController()
		{
			LowProfile.Fourier.Double.TransformNative.Setup();
			WavetableContext.Setup();

			Dispatcher = new RealtimeDispatcher<Voice>(4, 1000, System.Threading.ThreadPriority.Highest, x => x.Process(ProcessSampleCount));
			Parameters = new Dictionary<ParameterKey, object>();
			TriggerIndex = 1;
			Notes = new List<MidiNote>(32);
			Voices = new Voice[16];

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
			var wt = WavetableData.FromFile(WavetableContext.AvailableWavetables[wavetableName]);

			switch (module)
			{
				case ModuleParams.Osc1:
					osc1Wavetable = wt;
					break;
				case ModuleParams.Osc2:
					osc2Wavetable = wt;
					break;
				case ModuleParams.Osc3:
					osc3Wavetable = wt;
					break;
				case ModuleParams.Osc4:
					osc4Wavetable = wt;
					break;
			}

			for (int i = 0; i < Voices.Length; i++)
			{
				if (i == 0)
					Voices[0].GetOsc(module).SetWave(wt.Data);
				else
					Voices[i].GetOsc(module).Wavetable = Voices[0].GetOsc(module).Wavetable;
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
			var len = buffer[0].Length;
			ProcessSampleCount = len;
			Dispatcher.QueueWorkItems(Voices);
			Dispatcher.WaitAll();

			for (int i = 0; i < Voices.Length; i++)
			{
				var voice = Voices[i];

				for (int j = 0; j < len; j++)
				{
					buffer[0][j] += voice.OutputBuffer[0][j];
					buffer[1][j] += voice.OutputBuffer[1][j];
				}
			}

			//for (int i = 0; i < Voices.Length; i++)
				//Voices[i].Process(buffer);
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

			var wt = WavetableData.FromFile(WavetableContext.AvailableWavetables["Sawtooth Wave"]);
			osc1Wavetable = wt;
			osc2Wavetable = wt;
			osc3Wavetable = wt;
			osc4Wavetable = wt;
			Voices[0].Osc1.SetWave(wt.Data);
			var table = Voices[0].Osc1.Wavetable;

			for (int i = 0; i < Voices.Length; i++)
			{
				Voices[i].Osc1.Wavetable = table;
				Voices[i].Osc2.Wavetable = table;
				Voices[i].Osc3.Wavetable = table;
				Voices[i].Osc4.Wavetable = table;
			}
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

		public void Dispose()
		{
			Dispatcher.Dispose();
		}

		~SynthController()
		{
			Dispose();
		}
	}
}
