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
		const int Bufsize = 16;
		object lockObject = new object();

		public bool IsActive;
		public long TriggerIndex;
		public int Note;

		// ---------------------------- Synth Parts ----------------------------

		public BLOsc Osc1, Osc2, Osc3, Osc4;
		public CascadeFilter Filter1, Filter2;
		public Ahdsr AmpEnv, Filter1Env, Filter2Env;
		public Modulator Mod1, Mod2, Mod3, Mod4, Mod5, Mod6;
		public MidiInput MidiInput;
		public ModMatrix ModMatrix;
		public Mixer Mixer;

		public Dictionary<ModuleParams, object> ModuleMap;
		
		// ----------------------------Voice Specific Parameters ----------------------------

		double[] AmpEnvBuffer, Filter1EnvBuffer, Filter2EnvBuffer;
		double[] Path1Buffer, Path2Buffer;
		double[] OutputBuffer;

		public Voice()
		{
			double samplerate = 48000;

			AmpEnvBuffer = new double[Bufsize];
			Filter1EnvBuffer = new double[Bufsize];
			Filter2EnvBuffer = new double[Bufsize];
			
			Path1Buffer = new double[Bufsize];
			Path2Buffer = new double[Bufsize];
			OutputBuffer = new double[Bufsize];

			ModuleMap = new Dictionary<ModuleParams, object>();

			Osc1 = new BLOsc(samplerate, Bufsize);
			Osc2 = new BLOsc(samplerate, Bufsize);
			Osc3 = new BLOsc(samplerate, Bufsize);
			Osc4 = new BLOsc(samplerate, Bufsize);

			Filter1 = new CascadeFilter(samplerate, Bufsize);
			Filter2 = new CascadeFilter(samplerate, Bufsize);

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
			ModMatrix = new Modules.ModMatrix(this);
			Mixer = new Modules.Mixer();

			RegisterModules();

			Osc1.UpdateStepsize();
			Osc2.UpdateStepsize();
			Osc3.UpdateStepsize();
			Osc4.UpdateStepsize();
			Filter1.UpdateCoefficients();
			Filter2.UpdateCoefficients();
			Mod1.UpdateStepsize();
			Mod2.UpdateStepsize();
			Mod3.UpdateStepsize();
			Mod4.UpdateStepsize();
			Mod5.UpdateStepsize();
			Mod6.UpdateStepsize();
			Mixer.Update();
		}

		private void RegisterModules()
		{
			ModuleMap[ModuleParams.Osc1] = Osc1;
			ModuleMap[ModuleParams.Osc2] = Osc2;
			ModuleMap[ModuleParams.Osc3] = Osc3;
			ModuleMap[ModuleParams.Osc4] = Osc4;
			ModuleMap[ModuleParams.Filter1] = Filter1;
			ModuleMap[ModuleParams.Filter2] = Filter2;
			ModuleMap[ModuleParams.AmpEnv] = AmpEnv;
			ModuleMap[ModuleParams.Filter1Env] = Filter1Env;
			ModuleMap[ModuleParams.Filter2Env] = Filter2Env;
			ModuleMap[ModuleParams.Modulator1] = Mod1;
			ModuleMap[ModuleParams.Modulator2] = Mod2;
			ModuleMap[ModuleParams.Modulator3] = Mod3;
			ModuleMap[ModuleParams.Modulator4] = Mod4;
			ModuleMap[ModuleParams.Modulator5] = Mod5;
			ModuleMap[ModuleParams.Modulator6] = Mod6;
			ModuleMap[ModuleParams.ModMatrix] = ModMatrix;
			ModuleMap[ModuleParams.Mixer] = Mixer;
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
			Osc1.Note = note;
			Osc2.Note = note;
			Osc3.Note = note;
			Osc4.Note = note;

			Osc1.UpdateStepsize();
			Osc2.UpdateStepsize();
			Osc3.UpdateStepsize();
			Osc4.UpdateStepsize();

			SetGate(true);
		}

		public void NoteOff(int releaseVelocity)
		{
			MidiInput.ReleaseVelocity = releaseVelocity / 127.0;
			SetGate(false);
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

		public void Process(double[][] buffer)
		{
			lock (lockObject)
			{
				IsActive = (AmpEnv.Output > 0.00001 || AmpEnv.Gate);

				if(!IsActive)
				{
					for (int i = 0; i < buffer[0].Length; i++)
					{
						buffer[0][i] += 0;
						buffer[1][i] += 0;
					}

					return;
				}

				for (int i = 0; i < buffer[0].Length; i += Bufsize)
				{
					var f1Env = ModMatrix.Filter1EnvMod;
					var f2Env = ModMatrix.Filter2EnvMod;
					for (int n = 0; n < Bufsize; n++)
					{
						AmpEnvBuffer[n] = AmpEnv.Process(1);
						Filter1EnvBuffer[n] = Filter1Env.Process(1) * f1Env;
						Filter2EnvBuffer[n] = Filter2Env.Process(1) * f2Env;
					}

					// Process modulation
					Mod1.Process(Bufsize);
					Mod2.Process(Bufsize);
					Mod3.Process(Bufsize);
					Mod4.Process(Bufsize);
					Mod5.Process(Bufsize);
					Mod6.Process(Bufsize);
					ModMatrix.Process();

					Osc1.Process(Bufsize);
					Osc2.Process(Bufsize);
					Osc3.Process(Bufsize);
					Osc4.Process(Bufsize);

					for(int n = 0; n < Bufsize; n++)
					{
						Path1Buffer[n] = Osc1.OutputBuffer[n] * Mixer.Osc1Vol * (1.0 - Mixer.Osc1Mix)
										+ Osc2.OutputBuffer[n] * Mixer.Osc2Vol * (1.0 - Mixer.Osc2Mix)
										+ Osc3.OutputBuffer[n] * Mixer.Osc3Vol * (1.0 - Mixer.Osc3Mix) 
										+ Osc4.OutputBuffer[n] * Mixer.Osc4Vol * (1.0 - Mixer.Osc4Mix);

						Path2Buffer[n] = Osc1.OutputBuffer[n] * Mixer.Osc1Vol * Mixer.Osc1Mix
										+ Osc2.OutputBuffer[n] * Mixer.Osc2Vol * Mixer.Osc2Mix
										+ Osc3.OutputBuffer[n] * Mixer.Osc3Vol * Mixer.Osc3Mix
										+ Osc4.OutputBuffer[n] * Mixer.Osc4Vol * Mixer.Osc4Mix;
					}

					Filter1.Process(Path1Buffer, Filter1EnvBuffer);
					Filter2.Process(Path2Buffer, Filter2EnvBuffer);

					for (int n = 0; n < Bufsize; n++)
					{
						OutputBuffer[n] = Filter1.OutputBuffer[n] * Mixer.F1Vol + Filter2.OutputBuffer[n] * Mixer.F2Vol;
						OutputBuffer[n] *= Mixer.OutputVolume * AmpEnvBuffer[n];

						buffer[0][i + n] += OutputBuffer[n];
						buffer[1][i + n] += OutputBuffer[n];
					}
				}
			}
		}

		public void SetParameter(ModuleParams module, Enum parameter, object value)
		{
			lock (lockObject)
			{
				switch (module)
				{
					case ModuleParams.Osc1:
						SetParameterOsc(module, parameter, value);
						break;
					case ModuleParams.Osc2:
						SetParameterOsc(module, parameter, value);
						break;
					case ModuleParams.Osc3:
						SetParameterOsc(module, parameter, value);
						break;
					case ModuleParams.Osc4:
						SetParameterOsc(module, parameter, value);
						break;

					case ModuleParams.Filter1:
						SetParameterFilter(module, parameter, value);
						break;
					case ModuleParams.Filter2:
						SetParameterFilter(module, parameter, value);
						break;

					case ModuleParams.AmpEnv:
						SetParameterEnv(module, parameter, value);
						break;
					case ModuleParams.Filter1Env:
						SetParameterEnv(module, parameter, value);
						break;
					case ModuleParams.Filter2Env:
						SetParameterEnv(module, parameter, value);
						break;

					case ModuleParams.Mixer:
						SetParameterMixer(module, parameter, value);
						break;

					case ModuleParams.Modulator1:
						SetParameterModulator(module, parameter, value);
						break;
					case ModuleParams.Modulator2:
						SetParameterModulator(module, parameter, value);
						break;
					case ModuleParams.Modulator3:
						SetParameterModulator(module, parameter, value);
						break;
					case ModuleParams.Modulator4:
						SetParameterModulator(module, parameter, value);
						break;
					case ModuleParams.Modulator5:
						SetParameterModulator(module, parameter, value);
						break;
					case ModuleParams.Modulator6:
						SetParameterModulator(module, parameter, value);
						break;

					case ModuleParams.ModMatrix:
						SetParameterModMatrix(module, parameter, value);
						break;
				}
			}
		}

		private void SetParameterOsc(ModuleParams module, Enum parameter, object value)
		{
			BLOsc osc = ModuleMap[module] as BLOsc;
			OscParams para = (OscParams)parameter;
			double val = Convert.ToDouble(value);

			switch (para)
			{
				case OscParams.Octave:
					osc.Octave = (int)val;
					break;
				case OscParams.Semi:
					osc.Semi = (int)val;
					break;
				case OscParams.Cent:
					osc.Cent = (int)val;
					break;
				case OscParams.Position:
					osc.TablePosition = (double)val;
					break;
				case OscParams.Phase:
					// Todo
					break;
				case OscParams.Volume:
					switch(module)
					{
						case ModuleParams.Osc1:
							Mixer.Osc1VolParam = val;
							break;
						case ModuleParams.Osc2:
							Mixer.Osc2VolParam = val;
							break;
						case ModuleParams.Osc3:
							Mixer.Osc3VolParam = val;
							break;
						case ModuleParams.Osc4:
							Mixer.Osc4VolParam = val;
							break;
					}
					Mixer.Update();
					break;
			}

			osc.UpdateStepsize();
		}

		private void SetParameterFilter(ModuleParams module, Enum parameter, object value)
		{
			var filter = ModuleMap[module] as CascadeFilter;
			FilterParams para = (FilterParams)parameter;
			double val = Convert.ToDouble(value);

			switch (para)
			{
				case FilterParams.Cutoff:
					filter.CutoffKnob = val;
					break;
				case FilterParams.Resonance:
					filter.Resonance = val;
					break;
				case FilterParams.Tracking:
					//filter.
					break;
				case FilterParams.Envelope:
					if(module == ModuleParams.Filter1)
						ModMatrix.Filter1EnvMod = val;
					else
						ModMatrix.Filter2EnvMod = val;
					break;
				case FilterParams.X:
					filter.VX = val;
					break;
				case FilterParams.A:
					filter.VA = val;
					break;
				case FilterParams.B:
					filter.VB = val;
					break;
				case FilterParams.C:
					filter.VC = val;
					break;
				case FilterParams.D:
					filter.VD = val;
					break;
			}

			filter.UpdateCoefficients();
		}

		private void SetParameterEnv(ModuleParams module, Enum parameter, object value)
		{
			var env = ModuleMap[module] as Ahdsr;
			EnvParams para = (EnvParams)parameter;
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

		private void SetParameterMixer(ModuleParams module, Enum parameter, object value)
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

		private void SetParameterModulator(ModuleParams module, Enum parameter, object value)
		{
			var mod = ModuleMap[module] as Modulator;
			ModulatorParams para = (ModulatorParams)parameter;

			if (para == ModulatorParams.Attack || para == ModulatorParams.Hold || para == ModulatorParams.Decay || para == ModulatorParams.Release || para == ModulatorParams.Delay) // 2ms - 20sec range
				value = ValueTables.Get((double)value, ValueTables.Response4Dec) * 20000;
			
			if(para == ModulatorParams.Frequency)
				value = ValueTables.Get((double)value, ValueTables.Response3Dec) * 50;

			mod.SetParameter(para, value);
			mod.UpdateStepsize();
		}

		private void SetParameterModMatrix(ModuleParams module, Enum parameter, object value)
		{
			ModMatrixParams para = (ModMatrixParams)parameter;
			ModRoute route = (ModRoute)value;

			ModMatrix.UpdateRoute(route);
		}
	}
}
