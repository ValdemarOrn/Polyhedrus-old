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
		public static Dictionary<Type, Dictionary<Enum, object>> DefaultSettings { get; private set; }
		
		static ParameterDefaults()
		{
			DefaultSettings = new Dictionary<Type,Dictionary<Enum,object>>();

			var data = new Dictionary<Enum, object>();
			DefaultSettings[typeof(BlOsc)] = data;
			data[OscParams.Octave] = 0;
			data[OscParams.Semi] = 0;
			data[OscParams.Cent] = 0;
			data[OscParams.Position] = 0.0;
			data[OscParams.Phase] = 0.0;
			data[OscParams.Volume] = 0.0;
			data[OscParams.FreePhase] = true;
			data[OscParams.Keytrack] = true;
			data[OscParams.Wavetable] = "Sawtooth Wave";

			data = new Dictionary<Enum, object>();
			DefaultSettings[typeof(MultiOsc)] = data;
			data[OscParams.Octave] = 0;
			data[OscParams.Semi] = 0;
			data[OscParams.Cent] = 0;
			data[OscParams.Position] = 0.0;
			data[OscParams.Phase] = 0.0;
			data[OscParams.Volume] = 0.0;
			data[OscParams.FreePhase] = true;
			data[OscParams.Keytrack] = true;

			data = new Dictionary<Enum, object>();
			DefaultSettings[typeof(NoiseOsc)] = data;
			data[OscParams.Position] = 1.0;
			data[OscParams.Volume] = 0.0;
			data[OscParams.Keytrack] = false;
			data[OscParams.Wavetable] = "White Noise";

			data = new Dictionary<Enum, object>();
			DefaultSettings[typeof(InsRedux)] = data;
			data[InsertParams.Param1] = 1.0;
			data[InsertParams.Param2] = 16.0;

			data = new Dictionary<Enum, object>();
			DefaultSettings[typeof(InsDistortion)] = data;
			data[InsertParams.Param1] = 1.0;
			data[InsertParams.Param2] = 1.0;
			data[InsertParams.Param3] = 0.0;
			data[InsertParams.Param4] = 0.0;

			data = new Dictionary<Enum, object>();
			DefaultSettings[typeof(CascadeFilter)] = data;
			data[FilterParams.Cutoff] = 0.5;
			data[FilterParams.Resonance] = 0.0;
			data[FilterParams.Gain] = 1.0;
			data[FilterParams.Tracking] = 0.0;
			data[FilterParams.Envelope] = 0.0;
			data[FilterParams.X] = 0.0;
			data[FilterParams.A] = 0.0;
			data[FilterParams.B] = 0.0;
			data[FilterParams.C] = 0.0;
			data[FilterParams.D] = 1.0;

			data = new Dictionary<Enum, object>();
			DefaultSettings[typeof(Ahdsr)] = data;
			data[EnvParams.Attack] = 0.05;
			data[EnvParams.Hold] = 0.00;
			data[EnvParams.Decay] = 0.5;
			data[EnvParams.Sustain] = 0.5;
			data[EnvParams.Release] = 0.2;

			data = new Dictionary<Enum, object>();
			DefaultSettings[typeof(Modulator)] = data;
			data[ModulatorParams.Attack] = 0.0;
			data[ModulatorParams.Hold] = 0.0;
			data[ModulatorParams.Decay] = 0.0;
			data[ModulatorParams.Sustain] = 1.0;
			data[ModulatorParams.Release] = 1.0;
			data[ModulatorParams.Frequency] = 1.0;
			data[ModulatorParams.Phase] = 0.0;
			data[ModulatorParams.Delay] = 0.0;
			data[ModulatorParams.Offset] = 0.0;
			data[ModulatorParams.Shape] = 0.5;
			data[ModulatorParams.Wave] = LFO.Wave.Sine;
			data[ModulatorParams.FreePhase] = true;
			data[ModulatorParams.TempoSync] = false;

			data = new Dictionary<Enum, object>();
			DefaultSettings[typeof(Mixer)] = data;
			data[MixerParams.Osc1Mix] = 0.0;
			data[MixerParams.Osc2Mix] = 0.0;
			data[MixerParams.Osc3Mix] = 1.0;
			data[MixerParams.Osc4Mix] = 1.0;
			data[MixerParams.F1ToF2] = 0.0;
			data[MixerParams.F1Vol] = 1.0;
			data[MixerParams.F2Vol] = 1.0;
			data[MixerParams.F1Pan] = 0.0;
			data[MixerParams.F2Pan] = 0.0;
			data[MixerParams.OutputVolume] = 1.0;
			data[MixerParams.OutputPan] = 0.0;
			data[MixerParams.ParallelFX] = false;

			data = new Dictionary<Enum, object>();
			DefaultSettings[typeof(ModMatrix)] = data;
			for (int j = 1; j <= 16; j++)
				data[(ModMatrixParams)j] = new ModRoute() { Index = j - 1 };

			data = new Dictionary<Enum, object>();
			DefaultSettings[typeof(SynthController)] = data;
			data[SettingsParams.Stereo] = true;
			data[SettingsParams.Unison] = 1;
			data[SettingsParams.Voices] = 6;
			data[SettingsParams.UnisonSpread] = 0.0;
			data[SettingsParams.UnisonPan] = 0.0;
			data[SettingsParams.Glide] = 0.0;
		}
	}
}
