using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Parameters
{
	public enum OscParams
	{
		Octave = 1,
		Semi,
		Cent,
		Position,
		Phase,
		Volume,
		FreePhase,
		MixFilters
	}

	public enum FilterParams
	{
		Cutoff = 1,
		Resonance,
		Tracking,
		Envelope,
		Volume,
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

	public enum SettingsParams
	{
		Volume = 1,
		Pan,
		Stereo,
		Unison,
		Voices,
		UnisonSpread,
		UnisonPan,
		Glide
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

	public enum LFOParams
	{
		Frequency = 1,
		Phase,
		Attack,
		Hold,
		Decay,
		Wave,
		FreePhase,
		TempoSync
	}
}
