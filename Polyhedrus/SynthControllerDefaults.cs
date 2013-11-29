using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public sealed partial class SynthController
	{
		private Dictionary<ParameterKey, object> CreateDefaultParameters()
		{
			var data = new Dictionary<ParameterKey, object>();

			// Oscillators
			for(int i = (int)ModuleParams.Osc1; i <= (int)ModuleParams.Osc4; i++)
			{
				ModuleParams module = (ModuleParams)i;
				data[new ParameterKey(module, OscParams.Octave)] = 0;
				data[new ParameterKey(module, OscParams.Semi)] = 0;
				data[new ParameterKey(module, OscParams.Cent)] = 0;
				data[new ParameterKey(module, OscParams.Position)] = 0.0;
				data[new ParameterKey(module, OscParams.Phase)] = 0.0;
				data[new ParameterKey(module, OscParams.Volume)] = (i == (int)ModuleParams.Osc1) ? 1.0 : 0.0;
				data[new ParameterKey(module, OscParams.FreePhase)] = true;
			}

			// Filters
			for (int i = (int)ModuleParams.Filter1; i <= (int)ModuleParams.Filter2; i++)
			{
				ModuleParams module = (ModuleParams)i;
				data[new ParameterKey(module, FilterParams.Cutoff)] = 0.5;
				data[new ParameterKey(module, FilterParams.Resonance)] = 0.0;
				data[new ParameterKey(module, FilterParams.Gain)] = 1.0;
				data[new ParameterKey(module, FilterParams.Tracking)] = 0.0;
				data[new ParameterKey(module, FilterParams.Envelope)] = 0.0;
				data[new ParameterKey(module, FilterParams.X)] = 0.0;
				data[new ParameterKey(module, FilterParams.A)] = 0.0;
				data[new ParameterKey(module, FilterParams.B)] = 0.0;
				data[new ParameterKey(module, FilterParams.C)] = 0.0;
				data[new ParameterKey(module, FilterParams.D)] = 1.0;
			}

			// Envs
			for (int i = (int)ModuleParams.AmpEnv; i <= (int)ModuleParams.Filter2Env; i++)
			{
				ModuleParams module = (ModuleParams)i;
				data[new ParameterKey(module, EnvParams.Attack)] = 0.05;
				data[new ParameterKey(module, EnvParams.Hold)] = 0.05;
				data[new ParameterKey(module, EnvParams.Decay)] = 0.5;
				data[new ParameterKey(module, EnvParams.Sustain)] = 0.5;
				data[new ParameterKey(module, EnvParams.Release)] = 0.5;
			}

			// Modulators
			for (int i = (int)ModuleParams.Modulator1; i <= (int)ModuleParams.Modulator6; i++)
			{
				ModuleParams module = (ModuleParams)i;
				data[new ParameterKey(module, ModulatorParams.Attack)] = 0.0;
				data[new ParameterKey(module, ModulatorParams.Hold)] = 0.0;
				data[new ParameterKey(module, ModulatorParams.Decay)] = 0.0;
				data[new ParameterKey(module, ModulatorParams.Sustain)] = 1.0;
				data[new ParameterKey(module, ModulatorParams.Release)] = 1.0;
				data[new ParameterKey(module, ModulatorParams.Frequency)] = 1.0;
				data[new ParameterKey(module, ModulatorParams.Phase)] = 0.0;
				data[new ParameterKey(module, ModulatorParams.Delay)] = 0.0;
				data[new ParameterKey(module, ModulatorParams.Offset)] = 0.0;
				data[new ParameterKey(module, ModulatorParams.Wave)] = AudioLib.Modules.LFO.Wave.Sine;
				data[new ParameterKey(module, ModulatorParams.FreePhase)] = true;
				data[new ParameterKey(module, ModulatorParams.TempoSync)] = false;
			}

			// Mixer
			for (int i = (int)ModuleParams.Mixer; i <= (int)ModuleParams.Mixer; i++)
			{
				ModuleParams module = (ModuleParams)i;
				data[new ParameterKey(module, MixerParams.Osc1Mix)] = 0.0;
				data[new ParameterKey(module, MixerParams.Osc2Mix)] = 0.0;
				data[new ParameterKey(module, MixerParams.Osc3Mix)] = 1.0;
				data[new ParameterKey(module, MixerParams.Osc4Mix)] = 1.0;
				data[new ParameterKey(module, MixerParams.F1ToF2)] = 0.0;
				data[new ParameterKey(module, MixerParams.F1Vol)] = 1.0;
				data[new ParameterKey(module, MixerParams.F2Vol)] = 1.0;
				data[new ParameterKey(module, MixerParams.F1Pan)] = 0.0;
				data[new ParameterKey(module, MixerParams.F2Pan)] = 0.0;
				data[new ParameterKey(module, MixerParams.OutputVolume)] = 1.0;
				data[new ParameterKey(module, MixerParams.OutputPan)] = 0.0;
				data[new ParameterKey(module, MixerParams.ParallelFX)] = false;
			}

			// Mod Matrix
			for (int i = (int)ModuleParams.ModMatrix; i <= (int)ModuleParams.ModMatrix; i++)
			{
				ModuleParams module = (ModuleParams)i;
				for (int j = 1; j <= 16; j++)
				{
					ModMatrixParams para = (ModMatrixParams)j;
					data[new ParameterKey(module, para)] = new ModRoute() { Index = j - 1 };
				}
			}

			// Settings
			for (int i = (int)ModuleParams.Settings; i <= (int)ModuleParams.Settings; i++)
			{
				ModuleParams module = (ModuleParams)i;
				data[new ParameterKey(module, SettingsParams.Volume)] = 1.0;
				data[new ParameterKey(module, SettingsParams.Pan)] = 0.0;
				data[new ParameterKey(module, SettingsParams.Stereo)] = true;
				data[new ParameterKey(module, SettingsParams.Unison)] = 1;
				data[new ParameterKey(module, SettingsParams.Voices)] = 6;
				data[new ParameterKey(module, SettingsParams.UnisonSpread)] = 0.0;
				data[new ParameterKey(module, SettingsParams.UnisonPan)] = 0.0;
				data[new ParameterKey(module, SettingsParams.Glide)] = 0.0;
			}

			return data;
		}
	}
}
