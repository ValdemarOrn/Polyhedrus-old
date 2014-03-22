using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Parameters
{
	public enum OscParams
	{
		Note = 1,
		Octave,
		Semi,
		Cent,
		Modulation,
		Position,
		Phase,
		Volume,
		FreePhase,
		Wavetable
	}

	public enum FilterParams
	{
		Cutoff = 1,
		Resonance,
		Gain,
		Tracking,
		Envelope,
		X,
		A,
		B,
		C,
		D
	}

	public enum EnvParams
	{
		Attack = 1,
		Hold,
		Decay,
		Sustain,
		Release
	}

	public enum InsertParams
	{
		Param1 = 1,
		Param2,
		Param3,
		Param4,
		Param5
	}

	public enum ModulatorParams
	{
		Attack = 1,
		Hold,
		Decay,
		Sustain,
		Release,

		Frequency,
		Phase,
		Delay,
		Offset,
		Shape,

		Wave,
		FreePhase,
		TempoSync
	}

	public enum EffectsParams
	{
		Param1 = 1,
		Param2,
		Param3,
		Param4,
		Param5,
		Param6,
		Param7,
		Param8,
		Wet,
		Enabled
	}

	public enum MixerParams
	{
		Osc1Mix = 1,
		Osc2Mix,
		Osc3Mix,
		Osc4Mix,
		F1ToF2,
		F1Vol,
		F2Vol,
		F1Pan,
		F2Pan,
		OutputVolume,
		OutputPan,
		ParallelFX
	}

	public enum ModMatrixParams
	{
		Modroute1 = 1,
		Modroute2,
		Modroute3,
		Modroute4,
		Modroute5,
		Modroute6,
		Modroute7,
		Modroute8,
		Modroute9,
		Modroute10,
		Modroute11,
		Modroute12,
		Modroute13,
		Modroute14,
		Modroute15,
		Modroute16,
		Modroute17,
		Modroute18,
		Modroute19,
		Modroute20,
	}

	public enum SettingsParams
	{
		Stereo = 1,
		Unison,
		Voices,
		UnisonSpread,
		UnisonPan,
		Glide
	}
}
