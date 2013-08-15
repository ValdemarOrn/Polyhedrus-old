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
		// ---------------------------- Synth Parts ----------------------------

		public BLOsc Osc1, Osc2;
		public CascadeFilter Filter1, Filter2;
		public Ahdsr AmpEnv, Filter1Env, Filter2Env, ModEnv1, ModEnv2, ModEnv3, ModEnv4;
		public LFO Lfo1, Lfo2, Lfo3, Lfo4;
		public MidiInput MidiInput;
		public ModMatrix ModMatrix;

		public Dictionary<ModuleParams, object> ModuleMap;
		
		// ----------------------------Voice Specific Parameters ----------------------------

		int ModulationIterator;

		public Voice()
		{
			ModuleMap = new Dictionary<ModuleParams, object>();
			Osc1 = new BLOsc(48000);
			Filter1 = new CascadeFilter(48000);
			MidiInput = new MidiInput();
			AmpEnv = new Ahdsr(48000);
			Filter1Env = new Ahdsr(48000);

			RegisterModules();

			//ModMatrix = new ModMatrix(this);
			//CreateModRoutes();
		}

		private void RegisterModules()
		{
			ModuleMap[ModuleParams.Osc1] = Osc1;
			ModuleMap[ModuleParams.Osc2] = Osc2;
		}

		/*private void CreateModRoutes()
		{
			var ampRoute = new ModRouting();
			ampRoute.Source = ModSource.AmpEnv;
			ampRoute.Destination = ModDestination.Filter1Vol;
			ampRoute.Amount = 1.0;
			ModMatrix.Routes.Add(ampRoute);

			var filter1Route = new ModRouting();
			filter1Route.Source = ModSource.Filter1Env;
			filter1Route.Destination = ModDestination.Filter1Freq;
			filter1Route.Amount = 1.0;
			ModMatrix.Routes.Add(filter1Route);
		}*/


		public void SetNote(int note, double velocity)
		{
			MidiInput.Note = note;

			// triggering new note
			if(velocity > 0)
			{
				/*if(MidiInput.Gate == true)
				{
					AmpEnv.Retrigger();
					Filter1Env.Retrigger();
				}*/

				MidiInput.Velocity = velocity;
			}

			MidiInput.Gate = (velocity > 0);
			Osc1.Note = note;
			AmpEnv.Gate = MidiInput.Gate;
			Filter1Env.Gate = MidiInput.Gate;
		}

		double Process()
		{
			double val = Osc1.Process();
			val = Filter1.Process(val);

			val = val * MidiInput.Velocity * 0.5;// ((Gate > 0.01) ? 0.5 : 0.0);
			return val;
		}

		public void Process(double[][] buffer)
		{
			// copy data to outBuffer
			for (int i = 0; i < buffer[0].Length; i++)
			{
				if(ModulationIterator % 64 == 0)
				{
					Osc1.UpdateStepsize();
					Filter1.UpdateCoefficients();
				}

				double val = Process();
				buffer[0][i] = val;
				buffer[1][i] = val;

				ModulationIterator++;
			}
		}

		public void SetParameter(ModuleParams module, Enum parameter, object value)
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
			}
		}
	}
}
