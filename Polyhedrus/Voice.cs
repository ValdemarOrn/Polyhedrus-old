using AudioLib;
using AudioLib.Modules;
using Polyhedrus.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polyhedrus.Parameters;

namespace Polyhedrus
{
	public sealed class Voice
	{
		private int bufsize;
		private double samplerate;
		private object lockObject = new object();
		
		public long TriggerIndex;
		public int Note;

		public Dictionary<ModuleId, object> Modules { get; private set; }

		// ---------------------------- Synth Parts ----------------------------

		public IOscillator Osc1, Osc2, Osc3, Osc4;
		public IInsEffect Ins1, Ins2;
		public IFilter Filter1, Filter2;
		public Ahdsr AmpEnv, Filter1Env, Filter2Env;
		public Modulator Mod1, Mod2, Mod3, Mod4, Mod5, Mod6;
		public MidiInput MidiInput;
		public ModMatrix ModMatrix;
		public Mixer Mixer;

		
		public double[][] OutputBuffer;
		// ----------------------------Voice Specific Parameters ----------------------------

		double[] ampEnvBuffer, filter1EnvBuffer, filter2EnvBuffer;
		double[] path1Buffer, path2Buffer;
		double[] processBuffer;

		public bool IsActive { get { return AmpEnv.Output > 0.00001 || AmpEnv.Gate; } }

		public Voice(double samplerate, int bufferSize)
		{
			this.samplerate = samplerate;
			bufsize = bufferSize;

			ampEnvBuffer = new double[bufsize];
			filter1EnvBuffer = new double[bufsize];
			filter2EnvBuffer = new double[bufsize];
			
			path1Buffer = new double[bufsize];
			path2Buffer = new double[bufsize];
			processBuffer = new double[bufsize];
			OutputBuffer = new double[2][];
			OutputBuffer[0] = new double[48000];
			OutputBuffer[1] = new double[48000];

			Modules = new Dictionary<ModuleId, object>();

			Osc1 = new MultiOsc(samplerate, bufsize);
			Osc2 = new BlOsc(samplerate, bufsize);
			Osc3 = new BlOsc(samplerate, bufsize);
			Osc4 = new BlOsc(samplerate, bufsize);

			//Ins1 = new InsRedux(samplerate, Bufsize);
			//Ins2 = new InsRedux(samplerate, Bufsize);
			Ins1 = new InsDistortion(samplerate, bufsize);
			Ins2 = new InsDistortion(samplerate, bufsize);

			Filter1 = new DualFilter(samplerate, bufsize);
			Filter2 = new DualFilter(samplerate, bufsize);

			AmpEnv = new Ahdsr(samplerate);
			Filter1Env = new Ahdsr(samplerate);
			Filter2Env = new Ahdsr(samplerate);

			Mod1 = new Modulator(samplerate);
			Mod2 = new Modulator(samplerate);
			Mod3 = new Modulator(samplerate);
			Mod4 = new Modulator(samplerate);
			Mod5 = new Modulator(samplerate);
			Mod6 = new Modulator(samplerate);

			MidiInput = new MidiInput();
			ModMatrix = new ModMatrix(this);
			Mixer = new Mixer();

			RefreshModuleObjects();

			Mod1.UpdateStepsize();
			Mod2.UpdateStepsize();
			Mod3.UpdateStepsize();
			Mod4.UpdateStepsize();
			Mod5.UpdateStepsize();
			Mod6.UpdateStepsize();
			Mixer.Update();
		}

		public void RefreshModuleObjects()
		{
			Modules[ModuleId.Osc1] = Osc1;
			Modules[ModuleId.Osc2] = Osc2;
			Modules[ModuleId.Osc3] = Osc3;
			Modules[ModuleId.Osc4] = Osc4;
			Modules[ModuleId.Insert1] = Ins1;
			Modules[ModuleId.Insert2] = Ins2;
			Modules[ModuleId.Filter1] = Filter1;
			Modules[ModuleId.Filter2] = Filter2;
			Modules[ModuleId.AmpEnv] = AmpEnv;
			Modules[ModuleId.Filter1Env] = Filter1Env;
			Modules[ModuleId.Filter2Env] = Filter2Env;
			Modules[ModuleId.Modulator1] = Mod1;
			Modules[ModuleId.Modulator2] = Mod2;
			Modules[ModuleId.Modulator3] = Mod3;
			Modules[ModuleId.Modulator4] = Mod4;
			Modules[ModuleId.Modulator5] = Mod5;
			Modules[ModuleId.Modulator6] = Mod6;
			Modules[ModuleId.ModMatrix] = ModMatrix;
			Modules[ModuleId.Mixer] = Mixer;
		}

		public void ResetEnvelopes()
		{
			AmpEnv.Reset();
			Filter1Env.Reset();
			Filter2Env.Reset();
			Mod1.Reset();
			Mod2.Reset();
			Mod3.Reset();
			Mod4.Reset();
			Mod5.Reset();
			Mod6.Reset();
		}

		public void NoteOn(int note, int velocity)
		{
			MidiInput.Velocity = velocity / 127.0;

			Note = note;
			MidiInput.Note = note;
			Osc1.SetParameter(OscParams.Note, note);
			Osc2.SetParameter(OscParams.Note, note);
			Osc3.SetParameter(OscParams.Note, note);
			Osc4.SetParameter(OscParams.Note, note);
			SetGate(true);
		}

		public void NoteOff(int releaseVelocity)
		{
			MidiInput.ReleaseVelocity = releaseVelocity / 127.0;
			SetGate(false);
		}

		public void SetSamplerate(double samplerate)
		{
			Osc1.Samplerate = samplerate;
			Osc2.Samplerate = samplerate;
			Osc3.Samplerate = samplerate;
			Osc4.Samplerate = samplerate;
			Ins1.Samplerate = samplerate;
			Ins2.Samplerate = samplerate;
			Filter1.Samplerate = samplerate;
			Filter2.Samplerate = samplerate;
			AmpEnv.Samplerate = samplerate;
			Filter1Env.Samplerate = samplerate;
			Filter2Env.Samplerate = samplerate;
		}

		void SetGate(bool gate)
		{
			MidiInput.Gate = gate;
			AmpEnv.Gate = gate;
			Filter1Env.Gate = gate;
			Filter2Env.Gate = gate;
			Mod1.SetGate(gate);
			Mod2.SetGate(gate);
			Mod3.SetGate(gate);
			Mod4.SetGate(gate);
			Mod5.SetGate(gate);
			Mod6.SetGate(gate);
		}

		public double GetModSource(ModSource source)
		{
			switch (source)
			{
				case (ModSource.Aftertouch):
					return MidiInput.Aftertouch;
				case (ModSource.AmpEnv):
					return AmpEnv.Output;
				case (ModSource.Filter1Env):
					return Filter1Env.Output;
				case (ModSource.Filter2Env):
					return Filter2Env.Output;
				case (ModSource.Mod1):
					return Mod1.Output;
				case (ModSource.Mod2):
					return Mod2.Output;
				case (ModSource.Mod3):
					return Mod3.Output;
				case (ModSource.Mod4):
					return Mod4.Output;
				case (ModSource.Mod5):
					return Mod5.Output;
				case (ModSource.Mod6):
					return Mod6.Output;
				case (ModSource.ModWheel):
					return MidiInput.ModWheel;
				case (ModSource.Note):
					return MidiInput.Note;
				case (ModSource.Pitch):
					return MidiInput.Pitch;
				case (ModSource.PitchBend):
					return MidiInput.PitchBend;

				case (ModSource.Seq1):
					return 0.0;
				case (ModSource.Seq2):
					return 0.0;
				case (ModSource.Spread):
					return MidiInput.Spread;
				case (ModSource.Velocity):
					return MidiInput.Velocity;


				default:
					return 0.0;
			}
		}

		public void Process(int length)
		{
			lock (lockObject)
			{
				if (OutputBuffer[0].Length < length)
				{
					OutputBuffer[0] = new double[2 * length];
					OutputBuffer[1] = new double[2 * length];
				}

				for (int i = 0; i < length; i += bufsize)
				{
					var f1Env = ModMatrix.Filter1EnvMod;
					var f2Env = ModMatrix.Filter2EnvMod;
					for (int n = 0; n < bufsize; n++)
					{
						ampEnvBuffer[n] = AmpEnv.Process(1);
						filter1EnvBuffer[n] = Filter1Env.Process(1) * f1Env;
						filter2EnvBuffer[n] = Filter2Env.Process(1) * f2Env;
					}

					// Process modulation
					Mod1.Process(bufsize);
					Mod2.Process(bufsize);
					Mod3.Process(bufsize);
					Mod4.Process(bufsize);
					Mod5.Process(bufsize);
					Mod6.Process(bufsize);
					ModMatrix.Process();

					Osc1.Process(bufsize);
					Osc2.Process(bufsize);
					Osc3.Process(bufsize);
					Osc4.Process(bufsize);

					for(int n = 0; n < bufsize; n++)
					{
						path1Buffer[n] =  Osc1.OutputBuffer[n] * Mixer.Osc1Vol * (1.0 - Mixer.Osc1Mix)
										+ Osc2.OutputBuffer[n] * Mixer.Osc2Vol * (1.0 - Mixer.Osc2Mix)
										+ Osc3.OutputBuffer[n] * Mixer.Osc3Vol * (1.0 - Mixer.Osc3Mix) 
										+ Osc4.OutputBuffer[n] * Mixer.Osc4Vol * (1.0 - Mixer.Osc4Mix);

						path2Buffer[n] =  Osc1.OutputBuffer[n] * Mixer.Osc1Vol * Mixer.Osc1Mix
										+ Osc2.OutputBuffer[n] * Mixer.Osc2Vol * Mixer.Osc2Mix
										+ Osc3.OutputBuffer[n] * Mixer.Osc3Vol * Mixer.Osc3Mix
										+ Osc4.OutputBuffer[n] * Mixer.Osc4Vol * Mixer.Osc4Mix;
					}

					path1Buffer = Ins1.Process(path1Buffer);
					path2Buffer = Ins2.Process(path2Buffer);

					Filter1.Process(path1Buffer, filter1EnvBuffer);
					Filter2.Process(path2Buffer, filter2EnvBuffer);

					for (int n = 0; n < bufsize; n++)
					{
						processBuffer[n] = Filter1.OutputBuffer[n] * Mixer.F1Vol + Filter2.OutputBuffer[n] * Mixer.F2Vol;
						processBuffer[n] *= Mixer.OutputVolume * ampEnvBuffer[n];

						OutputBuffer[0][i + n] = processBuffer[n];
						OutputBuffer[1][i + n] = processBuffer[n];
					}
				}
			}
		}

		public void SetParameter(ModuleId module, int parameter, object value)
		{
			lock (lockObject)
			{
				switch (module)
				{
					case ModuleId.Osc1:
					case ModuleId.Osc2:
					case ModuleId.Osc3:
					case ModuleId.Osc4:
						SetParameterOsc(module, parameter, value);
						break;

					case ModuleId.Insert1:
					case ModuleId.Insert2:
						SetParameterInsert(module, parameter, value);
						break;

					case ModuleId.Filter1:
					case ModuleId.Filter2:
						SetParameterFilter(module, parameter, value);
						break;

					case ModuleId.AmpEnv:
					case ModuleId.Filter1Env:
					case ModuleId.Filter2Env:
						SetParameterEnv(module, parameter, value);
						break;

					case ModuleId.Mixer:
						SetParameterMixer(module, parameter, value);
						break;

					case ModuleId.Modulator1:
					case ModuleId.Modulator2:
					case ModuleId.Modulator3:
					case ModuleId.Modulator4:
					case ModuleId.Modulator5:
					case ModuleId.Modulator6:
						SetParameterModulator(module, parameter, value);
						break;

					case ModuleId.ModMatrix:
						SetParameterModMatrix(module, parameter, value);
						break;
				}
			}
		}

		private void SetParameterOsc(ModuleId module, int parameter, object value)
		{
			var osc = (IOscillator)Modules[module];
			var para = (OscParams)parameter;

			switch (para)
			{
				case OscParams.Volume:
					switch(module)
					{
						case ModuleId.Osc1:
							Mixer.Osc1VolParam = Convert.ToDouble(value);
							break;
						case ModuleId.Osc2:
							Mixer.Osc2VolParam = Convert.ToDouble(value);
							break;
						case ModuleId.Osc3:
							Mixer.Osc3VolParam = Convert.ToDouble(value);
							break;
						case ModuleId.Osc4:
							Mixer.Osc4VolParam = Convert.ToDouble(value);
							break;
					}
					Mixer.Update();
					break;
				default:
					osc.SetParameter(para, value);
					break;
			}
		}

		private void SetParameterInsert(ModuleId module, int parameter, object value)
		{
			var insEffect = (IInsEffect)Modules[module];
			insEffect.SetParameter(parameter, value);
		}

		private void SetParameterFilter(ModuleId module, int parameter, object value)
		{
			var filter = (IFilter)Modules[module];
			var para = (FilterParams)parameter;

			switch (para)
			{
				case FilterParams.Envelope:
					if(module == ModuleId.Filter1)
						ModMatrix.Filter1EnvMod = Convert.ToDouble(value);
					else
						ModMatrix.Filter2EnvMod = Convert.ToDouble(value);
					break;
				case FilterParams.Tracking:
					//filter.
					break;
				default:
					filter.SetParameter(para, value);
					break;
			}
		}

		private void SetParameterEnv(ModuleId module, int parameter, object value)
		{
			var env = (Ahdsr)Modules[module];
			var para = (EnvParams)parameter;
			double val = Convert.ToDouble(value);

			if(para != EnvParams.Sustain) // 2ms - 20sec range
				val = ValueTables.Get(val, ValueTables.Response4Dec) * 20000;

			switch (para)
			{
				case EnvParams.Attack:
					env.SetParameter(Ahdsr.StageAttack, val);
					break;
				case EnvParams.Hold:
					env.SetParameter(Ahdsr.StageHold, val);
					break;
				case EnvParams.Decay:
					env.SetParameter(Ahdsr.StageDecay, val);
					break;
				case EnvParams.Sustain:
					env.SetParameter(Ahdsr.StageSustain, val);
					break;
				case EnvParams.Release:
					env.SetParameter(Ahdsr.StageRelease, val);
					break;
			}
		}

		private void SetParameterMixer(ModuleId module, int parameter, object value)
		{
			var para = (MixerParams)parameter;
			double val = Convert.ToDouble(value);

			switch (para)
			{
				case MixerParams.Osc1Mix:
					Mixer.Osc1MixParam = val;
					break;
				case MixerParams.Osc2Mix:
					Mixer.Osc2MixParam = val;
					break;
				case MixerParams.Osc3Mix:
					Mixer.Osc3MixParam = val;
					break;
				case MixerParams.Osc4Mix:
					Mixer.Osc4MixParam = val;
					break;
				case MixerParams.F1ToF2:
					Mixer.F1ToF2Param = val;
					break;
				case MixerParams.F1Vol:
					Mixer.F1VolParam = val;
					break;
				case MixerParams.F2Vol:
					Mixer.F2VolParam = val;
					break;
				case MixerParams.ParallelFX:
					Mixer.ParallelFX = (bool)value;
					break;
			}

			Mixer.Update();
		}

		private void SetParameterModulator(ModuleId module, int parameter, object value)
		{
			var mod = (Modulator)Modules[module];
			var para = (ModulatorParams)parameter;

			if (para == ModulatorParams.Attack || para == ModulatorParams.Hold || para == ModulatorParams.Decay || para == ModulatorParams.Release || para == ModulatorParams.Delay) // 2ms - 20sec range
				value = ValueTables.Get((double)value, ValueTables.Response4Dec) * 20000;

			mod.SetParameter(para, value);
			mod.UpdateStepsize();
		}

		private void SetParameterModMatrix(ModuleId module, int parameter, object value)
		{
			var para = (ModMatrixParams)parameter;
			var route = (ModRoute)value;

			ModMatrix.UpdateRoute(route);
		}
	}
}
