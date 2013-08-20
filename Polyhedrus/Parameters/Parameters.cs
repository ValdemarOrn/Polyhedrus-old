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
	}

	public enum FilterParams
	{
		Cutoff = 1,
		Resonance,
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

		Wave,
		FreePhase,
		TempoSync
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
		Modroute16
	}
}
