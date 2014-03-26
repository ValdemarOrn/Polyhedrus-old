using AudioLib.Modules;
using Polyhedrus.Modules;
using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public static class ParameterDefaults
	{
		public static Dictionary<Type, Dictionary<int, object>> DefaultSettings { get; private set; }
		
		static ParameterDefaults()
		{
			DefaultSettings = new Dictionary<Type, Dictionary<int, object>>();

			var data = new Dictionary<int, object>();
			DefaultSettings[typeof(BlOsc)] = data;
			data[(int)OscParams.Octave] = 0;
			data[(int)OscParams.Semi] = 0;
			data[(int)OscParams.Cent] = 0;
			data[(int)OscParams.Position] = 0.0;
			data[(int)OscParams.Phase] = 0.0;
			data[(int)OscParams.Volume] = 0.0;
			data[(int)OscParams.FreePhase] = true;
			data[(int)OscParams.Keytrack] = true;
			data[(int)OscParams.Wavetable] = "Sawtooth Wave";

			data = new Dictionary<int, object>();
			DefaultSettings[typeof(MultiOsc)] = data;
			data[(int)OscParams.Octave] = 0;
			data[(int)OscParams.Semi] = 0;
			data[(int)OscParams.Cent] = 0;
			data[(int)OscParams.Position] = 0.0;
			data[(int)OscParams.Phase] = 0.0;
			data[(int)OscParams.Volume] = 0.0;
			data[(int)OscParams.FreePhase] = true;
			data[(int)OscParams.Keytrack] = true;

			data = new Dictionary<int, object>();
			DefaultSettings[typeof(NoiseOsc)] = data;
			data[(int)OscParams.Position] = 1.0;
			data[(int)OscParams.Volume] = 0.0;
			data[(int)OscParams.Keytrack] = false;
			data[(int)OscParams.Wavetable] = "White Noise";

			data = new Dictionary<int, object>();
			DefaultSettings[typeof(InsRedux)] = data;
			data[(int)InsertParams.Param1] = 1.0;
			data[(int)InsertParams.Param2] = 16.0;

			data = new Dictionary<int, object>();
			DefaultSettings[typeof(InsDistortion)] = data;
			data[(int)InsertParams.Param1] = 1.0;
			data[(int)InsertParams.Param2] = 1.0;
			data[(int)InsertParams.Param3] = 0.0;
			data[(int)InsertParams.Param4] = 0.0;

			data = new Dictionary<int, object>();
			DefaultSettings[typeof(CascadeFilter)] = data;
			data[(int)FilterParams.Cutoff] = 0.5;
			data[(int)FilterParams.Resonance] = 0.0;
			data[(int)FilterParams.Gain] = 1.0;
			data[(int)FilterParams.Tracking] = 0.0;
			data[(int)FilterParams.Envelope] = 0.0;
			data[(int)FilterParams.NumPoles] = 4;
			data[(int)FilterParams.X] = 0.0;
			data[(int)FilterParams.A] = 0.0;
			data[(int)FilterParams.B] = 0.0;
			data[(int)FilterParams.C] = 0.0;
			data[(int)FilterParams.D] = 1.0;

			data = new Dictionary<int, object>();
			DefaultSettings[typeof(DualFilter)] = data;
			data[(int)FilterParams.Cutoff] = 0.5;
			data[(int)FilterParams.Resonance] = 0.0;
			data[(int)FilterParams.Gain] = 1.0;
			data[(int)FilterParams.Tracking] = 0.0;
			data[(int)FilterParams.Envelope] = 0.0;
			data[(int)FilterParams.NumPoles] = 4;
			data[(int)FilterParams.CutoffOffset] = 0.0;
			data[(int)FilterParams.ResonanceOffset] = 0.0;
			data[(int)FilterParams.FilterAMode] = TwoPoleFilter.FilterMode.LowPass;
			data[(int)FilterParams.FilterBMode] = TwoPoleFilter.FilterMode.LowPass;

			data = new Dictionary<int, object>();
			DefaultSettings[typeof(Ahdsr)] = data;
			data[(int)EnvParams.Attack] = 0.05;
			data[(int)EnvParams.Hold] = 0.00;
			data[(int)EnvParams.Decay] = 0.5;
			data[(int)EnvParams.Sustain] = 0.5;
			data[(int)EnvParams.Release] = 0.2;

			data = new Dictionary<int, object>();
			DefaultSettings[typeof(Modulator)] = data;
			data[(int)ModulatorParams.Attack] = 0.0;
			data[(int)ModulatorParams.Hold] = 0.0;
			data[(int)ModulatorParams.Decay] = 0.0;
			data[(int)ModulatorParams.Sustain] = 1.0;
			data[(int)ModulatorParams.Release] = 1.0;
			data[(int)ModulatorParams.Frequency] = 1.0;
			data[(int)ModulatorParams.Phase] = 0.0;
			data[(int)ModulatorParams.Delay] = 0.0;
			data[(int)ModulatorParams.Offset] = 0.0;
			data[(int)ModulatorParams.Shape] = 0.5;
			data[(int)ModulatorParams.Wave] = LFO.Wave.Sine;
			data[(int)ModulatorParams.FreePhase] = true;
			data[(int)ModulatorParams.TempoSync] = false;

			data = new Dictionary<int, object>();
			DefaultSettings[typeof(Mixer)] = data;
			data[(int)MixerParams.Osc1Mix] = 0.0;
			data[(int)MixerParams.Osc2Mix] = 0.0;
			data[(int)MixerParams.Osc3Mix] = 1.0;
			data[(int)MixerParams.Osc4Mix] = 1.0;
			data[(int)MixerParams.F1ToF2] = 0.0;
			data[(int)MixerParams.F1Vol] = 1.0;
			data[(int)MixerParams.F2Vol] = 1.0;
			data[(int)MixerParams.F1Pan] = 0.0;
			data[(int)MixerParams.F2Pan] = 0.0;
			data[(int)MixerParams.OutputVolume] = 1.0;
			data[(int)MixerParams.OutputPan] = 0.0;
			data[(int)MixerParams.ParallelFX] = false;

			data = new Dictionary<int, object>();
			DefaultSettings[typeof(ModMatrix)] = data;
			for (int j = 1; j <= 16; j++)
				data[j] = new ModRoute() { Index = j - 1 };

			data = new Dictionary<int, object>();
			DefaultSettings[typeof(SynthController)] = data;
			data[(int)SettingsParams.Stereo] = true;
			data[(int)SettingsParams.Unison] = 1;
			data[(int)SettingsParams.Voices] = 6;
			data[(int)SettingsParams.UnisonSpread] = 0.0;
			data[(int)SettingsParams.UnisonPan] = 0.0;
			data[(int)SettingsParams.Glide] = 0.0;
		}
	}
}
