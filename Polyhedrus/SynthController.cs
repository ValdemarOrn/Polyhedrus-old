using AudioLib;
using Polyhedrus.Modules;
using Polyhedrus.Parameters;
using Polyhedrus.WT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public sealed class SynthController
	{
		private int bufferSize;
		private Voice[] voices;
		private List<MidiNote> notes;
		private long triggerIndex;
		private ParameterMap parameters;
		private RealtimeDispatcher<Voice> dispatcher;

		private volatile int processSampleCount;

		public SynthController(double samplerate, int bufferSize)
		{
			Samplerate = samplerate;
			this.bufferSize = bufferSize;

			LowProfile.Fourier.Double.TransformNative.Setup();
			WavetableContext.SetupWavetables();

			dispatcher = new RealtimeDispatcher<Voice>(
				4,
				1000, 
				System.Threading.ThreadPriority.Highest, 
				x => x.Process(processSampleCount));

			parameters = new ParameterMap();
			triggerIndex = 1;
			notes = new List<MidiNote>(64);
			voices = new Voice[64];
			WavetableContext.CalculateIndexes(Samplerate);

			for(var i = 0; i < voices.Length; i++)
				voices[i] = new Voice(Samplerate, bufferSize);

			RefreshModuleTypes();
			SetDefaults();
		}

		public int Polyphony { get { return voices.Length; } }
		public double Samplerate { get; private set; }
		public Dictionary<ModuleId, Type> ModuleTypes { get; private set; }

		public void SetModuleType(ModuleId module, Type moduleType)
		{
			if (moduleType == ModuleTypes[module])
				return;

			foreach(var voice in voices)
			{
				switch (module)
				{
					case ModuleId.Insert1:
						voice.Ins1 = ModuleType.CreateNew<IInsEffect>(moduleType, Samplerate, bufferSize);
						break;
					case ModuleId.Insert2:
						voice.Ins2 = ModuleType.CreateNew<IInsEffect>(moduleType, Samplerate, bufferSize);
						break;
					case ModuleId.Osc1:
						voice.Osc1 = ModuleType.CreateNew<IOscillator>(moduleType, Samplerate, bufferSize);
						break;
					case ModuleId.Osc2:
						voice.Osc2 = ModuleType.CreateNew<IOscillator>(moduleType, Samplerate, bufferSize);
						break;
					case ModuleId.Osc3:
						voice.Osc3 = ModuleType.CreateNew<IOscillator>(moduleType, Samplerate, bufferSize);
						break;
					case ModuleId.Osc4:
						voice.Osc4 = ModuleType.CreateNew<IOscillator>(moduleType, Samplerate, bufferSize);
						break;
					case ModuleId.Filter1:
						voice.Filter1 = ModuleType.CreateNew<IFilter>(moduleType, Samplerate, bufferSize);
						break;
					case ModuleId.Filter2:
						voice.Filter2 = ModuleType.CreateNew<IFilter>(moduleType, Samplerate, bufferSize);
						break;
				}
			}

			RefreshModuleTypes();
			var para = parameters.GetParameters(module, moduleType);
			SetModuleParameters(module, para ?? ParameterDefaults.DefaultSettings[ModuleTypes[module]]);
		}

		public void SetSamplerate(double samplerate)
		{
			Samplerate = samplerate;

			foreach (var voice in voices)
				voice.SetSamplerate(samplerate);
		}

		public void NoteOn(int note, int velocity)
		{
			RemoveNote(note); // delete if already exists
			notes.Add(new MidiNote(note, velocity));
			int voiceIndex = FindAvailableVoice(note);

			if (Polyphony > 1)
				voices[voiceIndex].ResetEnvelopes();

			voices[voiceIndex].NoteOn(note, velocity);
			voices[voiceIndex].TriggerIndex = triggerIndex++;
		}

		public void NoteOff(int note, int releaseVelocity)
		{
			RemoveNote(note);
			var newIndex = GetReplacementNoteIndex();

			for(int i = 0; i < voices.Length; i++)
			{
				var voice = voices[i];
				if(voice.Note == note)
				{
					if (newIndex >= 0)
						voice.NoteOn(notes[newIndex].Note, notes[newIndex].Velocity);
					else
						voice.NoteOff(releaseVelocity);
				}
			}
		}

		public void SetPitchWheel(double value)
		{
			for (var i = 0; i < voices.Length; i++)
				voices[i].MidiInput.PitchBend = value;
		}

		public void Process(double[][] buffer)
		{
			var len = buffer[0].Length;
			processSampleCount = len;
			var activeVoices = new List<Voice>();

			for (var i = 0; i < voices.Length; i++)
			{
				if (voices[i].IsActive)
					activeVoices.Add(voices[i]);
			}

			var voiceArray = activeVoices.ToArray();
			var voiceCount = voiceArray.Length;
			if (voiceCount > 0)
			{
				dispatcher.QueueWorkItems(voiceArray);
				dispatcher.WaitAll();
			}

			for (var i = 0; i < voiceCount; i++)
			{
				var voice = voiceArray[i];
				for (var j = 0; j < len; j++)
				{
					buffer[0][j] += voice.OutputBuffer[0][j];
					buffer[1][j] += voice.OutputBuffer[1][j];
				}
			}
		}

		public void SetParameter(ModuleId module, Enum parameter, object value)
		{
			SetParameter(module, Convert.ToInt32(parameter), value);
		}

		public void SetParameter(ModuleId module, int parameter, object value)
		{
			parameters.SetParameter(module, ModuleTypes[module], parameter, value);

			foreach (var voice in voices)
				voice.SetParameter(module, parameter, value);
		}

		public object GetParameter(ModuleId module, Enum parameter)
		{
			return GetParameter(module, Convert.ToInt32(parameter));
		}

		public object GetParameter(ModuleId module, int parameter)
		{
			return parameters.GetParameter(module, ModuleTypes[module], parameter);
		}

		public void SetModuleParameters(ModuleId module, Dictionary<int, object> para, bool update = true)
		{
			parameters.SetParameters(module, ModuleTypes[module], para);

			if (!update)
				return;

			foreach (var p in para)
				foreach (var voice in voices)
					voice.SetParameter(module, p.Key, p.Value);
		}

		private void SetDefaults()
		{
			Action<ModuleId> setDefaultParameters = module =>
			{
				var defaults = ParameterDefaults.DefaultSettings[ModuleTypes[module]];
				parameters.SetParameters(module, ModuleTypes[module], defaults);
			};

			setDefaultParameters(ModuleId.Osc1);
			setDefaultParameters(ModuleId.Osc2);
			setDefaultParameters(ModuleId.Osc3);
			setDefaultParameters(ModuleId.Osc4);
			setDefaultParameters(ModuleId.Insert1);
			setDefaultParameters(ModuleId.Insert2);
			setDefaultParameters(ModuleId.Filter1);
			setDefaultParameters(ModuleId.Filter2);
			setDefaultParameters(ModuleId.AmpEnv);
			setDefaultParameters(ModuleId.Filter1Env);
			setDefaultParameters(ModuleId.Filter2Env);
			setDefaultParameters(ModuleId.Modulator1);
			setDefaultParameters(ModuleId.Modulator2);
			setDefaultParameters(ModuleId.Modulator3);
			setDefaultParameters(ModuleId.Modulator4);
			setDefaultParameters(ModuleId.Modulator5);
			setDefaultParameters(ModuleId.Modulator6);
			setDefaultParameters(ModuleId.Mixer);
			setDefaultParameters(ModuleId.ModMatrix);
			setDefaultParameters(ModuleId.Settings);

			var allParams = parameters.GetAllParameters();
			foreach (var para in allParams)
				SetParameter(para.Module, para.Key, para.Value);
		}

		private void RefreshModuleTypes()
		{
			foreach (var voice in voices)
				voice.RefreshModuleObjects();

			var v = voices[0];

			ModuleTypes = new Dictionary<ModuleId, Type>
			{
				{ ModuleId.AmpEnv, v.Modules[ModuleId.AmpEnv].GetType() },

				{ ModuleId.Filter1, v.Modules[ModuleId.Filter1].GetType() },
				{ ModuleId.Filter1Env, v.Modules[ModuleId.Filter1Env].GetType() },
				{ ModuleId.Filter2, v.Modules[ModuleId.Filter2].GetType() },
				{ ModuleId.Filter2Env, v.Modules[ModuleId.Filter2Env].GetType() },

				//{ ModuleParams.FX1, v.asd.GetType() },
				//{ ModuleParams.FX2, v.asd.GetType() },
				//{ ModuleParams.FX3, v.asd.GetType() },

				{ ModuleId.Insert1, v.Modules[ModuleId.Insert1].GetType() },
				{ ModuleId.Insert2, v.Modules[ModuleId.Insert2].GetType() },

				{ ModuleId.Mixer, v.Modules[ModuleId.Mixer].GetType() },
				{ ModuleId.ModMatrix, v.Modules[ModuleId.ModMatrix].GetType() },

				{ ModuleId.Modulator1, v.Modules[ModuleId.Modulator1].GetType() },
				{ ModuleId.Modulator2, v.Modules[ModuleId.Modulator2].GetType() },
				{ ModuleId.Modulator3, v.Modules[ModuleId.Modulator3].GetType() },
				{ ModuleId.Modulator4, v.Modules[ModuleId.Modulator4].GetType() },
				{ ModuleId.Modulator5, v.Modules[ModuleId.Modulator5].GetType() },
				{ ModuleId.Modulator6, v.Modules[ModuleId.Modulator6].GetType() },

				{ ModuleId.Osc1, v.Modules[ModuleId.Osc1].GetType() },
				{ ModuleId.Osc2, v.Modules[ModuleId.Osc2].GetType() },
				{ ModuleId.Osc3, v.Modules[ModuleId.Osc3].GetType() },
				{ ModuleId.Osc4, v.Modules[ModuleId.Osc4].GetType() },

				//{ ModuleParams.Step1, v.asd.GetType() },
				//{ ModuleParams.Step2, v.asd.GetType() }

				{ ModuleId.Settings, typeof(SynthController) },
			};
		}

		private int FindAvailableVoice(int note)
		{
			// Look for inactive voices
			for (var i = 0; i < voices.Length; i++)
				if (!voices[i].IsActive)
					return i;

			// if none found, return oldest voice
			var oldestTriggerIndex = voices[0].TriggerIndex;
			var oldestVoiceIndex = 0;

			for (var i = 1; i < voices.Length; i++)
			{
				if (voices[i].TriggerIndex < oldestTriggerIndex)
				{
					oldestTriggerIndex = voices[i].TriggerIndex;
					oldestVoiceIndex = i;
				}
			}

			return oldestVoiceIndex;
		}

		private int GetReplacementNoteIndex()
		{
			if (Polyphony > 1)
				return -1;

			return notes.Count - 1;
		}

		private void RemoveNote(int note)
		{
			for (var i = 0; i < notes.Count; i++)
			{
				if (notes[i].Note == note)
					notes.RemoveAt(i);
			}
		}

		public void Dispose()
		{
			dispatcher.Dispose();
		}

		~SynthController()
		{
			Dispose();
		}
	}
}
