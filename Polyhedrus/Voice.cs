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
		object lockObject = new object();

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

		int ModulationIterator;

		public Voice()
		{
			double samplerate = 48000;

			ModuleMap = new Dictionary<ModuleParams, object>();

			Osc1 = new BLOsc(samplerate);
			Osc2 = new BLOsc(samplerate);
			Osc3 = new BLOsc(samplerate);
			Osc4 = new BLOsc(samplerate);

			Filter1 = new CascadeFilter(samplerate);
			Filter2 = new CascadeFilter(samplerate);

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

		public void SetNote(int note, double velocity)
		{
			if (velocity > 0) // triggering new note
			{
				/*if(MidiInput.Gate == true)
				{
					AmpEnv.Retrigger();
					Filter1Env.Retrigger();
				}*/

				MidiInput.Velocity = velocity;
				MidiInput.Note = note;

				Osc1.Note = note;
				Osc2.Note = note;
				Osc3.Note = note;
				Osc4.Note = note;

				Osc1.UpdateStepsize();
				Osc2.UpdateStepsize();
				Osc3.UpdateStepsize();
				Osc4.UpdateStepsize();
			}

			// Set gate if we're releasing the current note, or if we just set the note
			if (note == MidiInput.Note)
			{
				MidiInput.Gate = (velocity > 0);
				AmpEnv.Gate = MidiInput.Gate;
				Filter1Env.Gate = MidiInput.Gate;
				Filter2Env.Gate = MidiInput.Gate;
				Mod1.SetGate(MidiInput.Gate);
				Mod2.SetGate(MidiInput.Gate);
				Mod3.SetGate(MidiInput.Gate);
				Mod4.SetGate(MidiInput.Gate);
				Mod5.SetGate(MidiInput.Gate);
				Mod6.SetGate(MidiInput.Gate);
			}
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
				for (int i = 0; i < buffer[0].Length; i++)
				{
					// Amp and Filter envs update every cycle
					AmpEnv.Process(1);
					Filter1Env.Process(1);
					Filter2Env.Process(1);

					if (ModulationIterator == 0)
					{
						ModulationIterator = 16;
						Mod1.Process(ModulationIterator);
						Mod2.Process(ModulationIterator);
						Mod3.Process(ModulationIterator);
						Mod4.Process(ModulationIterator);
						Mod5.Process(ModulationIterator);
						Mod6.Process(ModulationIterator);

						ModMatrix.Process();
					}

					// Calculate Amp and Filter modulation every cycle
					{
						Filter1.CutoffModulationEnv = Filter1Env.Output * ModMatrix.Filter1EnvMod;
						Filter1.UpdateCoefficients();

						Filter2.CutoffModulationEnv = Filter2Env.Output * ModMatrix.Filter2EnvMod;
						Filter2.UpdateCoefficients();
					}

					Osc1.Process();
					Osc2.Process();
					Osc3.Process();
					Osc4.Process();

					var val = Osc1.Output * Mixer.Osc1Vol
							+ Osc2.Output * Mixer.Osc2Vol
							+ Osc3.Output * Mixer.Osc3Vol
							+ Osc4.Output * Mixer.Osc4Vol;

					val = Filter1.Process(val);
					val = val * Mixer.OutputVolume * AmpEnv.Output;

					buffer[0][i] = val;
					buffer[1][i] = val;

					ModulationIterator--;
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
			double val = (double)value;

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
					// Todo
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
			double val = (double)value;

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
			double val = (double)value;

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

		private void SetParameterModulator(ModuleParams module, Enum parameter, object value)
		{
			var mod = ModuleMap[module] as Modulator;
			ModulatorParams para = (ModulatorParams)parameter;

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
