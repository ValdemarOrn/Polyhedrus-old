using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Modules
{
	public enum ModDestination
	{
		None = 0,

		Osc1Pitch = 1,
		Osc1Vol,
		Osc1Wave,

		Osc2Pitch,
		Osc2Vol,
		Osc2Wave,

		Osc3Pitch,
		Osc3Vol,
		Osc3Wave,

		Osc4Pitch,
		Osc4Vol,
		Osc4Wave,

		Filter1Freq = 30,
		Filter1Res,
		Filter1Vol,
		Filter1Pan,

		Filter2Freq,
		Filter2Res,
		Filter2Vol,
		Filter2Pan,

		Insert1Wet = 50,
		Insert1Param1,
		Insert1Param2,
		Insert1Param3,
		Insert1Param4,

		Insert2Wet,
		Insert2Param1,
		Insert2Param2,
		Insert2Param3,
		Insert2Param4,

		Mod1Speed = 70,
		Mod2Speed,
		Mod3Speed,
		Mod4Speed,
		Mod5Speed,
		Mod6Speed,

		FX1Wet = 80,
		FX2Wet,
		FX3Wet,
		FX4Wet,

		MasterVolume = 110,
		MasterPan
	}

	public enum ModSource
	{
		None = 0,

		Note = 1,
		Pitch, // note + pitchbend
		Velocity,
		PitchBend,
		ModWheel,
		Aftertouch,

		AmpEnv = 20,
		Filter1Env,
		Filter2Env,
		Mod1,
		Mod2,
		Mod3,
		Mod4,
		Mod5,
		Mod6,
		Seq1,
		Seq2,

		Spread = 40

	}

	public static class Mod
	{
		public static Dictionary<ModDestination, string> DestNames;
		public static Dictionary<ModSource, string> SourceNames;

		static Mod()
		{
			DestNames = new Dictionary<ModDestination, string>();
			SourceNames = new Dictionary<ModSource, string>();

			DestNames[ModDestination.None] = "None";
			DestNames[ModDestination.Osc1Pitch] = "Oscillator 1 Pitch";
			DestNames[ModDestination.Osc1Vol] = "Oscillator 1 Vol";
			DestNames[ModDestination.Osc1Wave] = "Oscillator 1 Wave";
			DestNames[ModDestination.Osc2Pitch] = "Oscillator 2 Pitch";
			DestNames[ModDestination.Osc2Vol] = "Oscillator 2 Vol";
			DestNames[ModDestination.Osc2Wave] = "Oscillator 2 Wave";
			DestNames[ModDestination.Osc3Pitch] = "Oscillator 3 Pitch";
			DestNames[ModDestination.Osc3Vol] = "Oscillator 3 Vol";
			DestNames[ModDestination.Osc3Wave] = "Oscillator 3 Wave";
			DestNames[ModDestination.Osc4Pitch] = "Oscillator 4 Pitch";
			DestNames[ModDestination.Osc4Vol] = "Oscillator 4 Vol";
			DestNames[ModDestination.Osc4Wave] = "Oscillator 4 Wave";
			DestNames[ModDestination.Filter1Freq] = "Filter 1 Freq";
			DestNames[ModDestination.Filter1Res] = "Filter 1 Resonance";
			DestNames[ModDestination.Filter1Vol] = "Filter 1 Volume";
			DestNames[ModDestination.Filter1Pan] = "Filter 1 Pan";
			DestNames[ModDestination.Filter2Freq] = "Filter 2 Freq";
			DestNames[ModDestination.Filter2Res] = "Filter 2 Resonance";
			DestNames[ModDestination.Filter2Vol] = "Filter 2 Volume";
			DestNames[ModDestination.Filter2Pan] = "Filter 2 Pan";
			DestNames[ModDestination.Insert1Wet] = "Insert 1 Wet";
			DestNames[ModDestination.Insert1Param1] = "Insert 1 Param1";
			DestNames[ModDestination.Insert1Param2] = "Insert 1 Param2";
			DestNames[ModDestination.Insert1Param3] = "Insert 1 Param3";
			DestNames[ModDestination.Insert1Param4] = "Insert 1 Param4";
			DestNames[ModDestination.Insert2Wet] = "Insert 2 Wet";
			DestNames[ModDestination.Insert2Param1] = "Insert 2 Param1";
			DestNames[ModDestination.Insert2Param2] = "Insert 2 Param2";
			DestNames[ModDestination.Insert2Param3] = "Insert 2 Param3";
			DestNames[ModDestination.Insert2Param4] = "Insert 2 Param4";
			DestNames[ModDestination.Mod1Speed] = "Lfo 1 Speed";
			DestNames[ModDestination.Mod2Speed] = "Lfo 2 Speed";
			DestNames[ModDestination.Mod3Speed] = "Lfo 3 Speed";
			DestNames[ModDestination.Mod4Speed] = "Lfo 4 Speed";
			DestNames[ModDestination.Mod5Speed] = "Lfo 5 Speed";
			DestNames[ModDestination.Mod6Speed] = "Lfo 6 Speed";
			DestNames[ModDestination.FX1Wet] = "FX 1 Wet";
			DestNames[ModDestination.FX2Wet] = "FX 2 Wet";
			DestNames[ModDestination.FX3Wet] = "FX 3 Wet";
			DestNames[ModDestination.FX4Wet] = "FX 4 Wet";
			DestNames[ModDestination.MasterVolume] = "Master Volume";
			DestNames[ModDestination.MasterPan] = "Master Pan";

			SourceNames[ModSource.None] = "None";
			SourceNames[ModSource.Note] = "Note";
			SourceNames[ModSource.Pitch] = "Pitch";
			SourceNames[ModSource.Velocity] = "Velocity";
			SourceNames[ModSource.PitchBend] = "PitchBend";
			SourceNames[ModSource.ModWheel] = "Modulation Wheel";
			SourceNames[ModSource.Aftertouch] = "Aftertouch";
			SourceNames[ModSource.AmpEnv] = "Amp Envevlope";
			SourceNames[ModSource.Filter1Env] = "Filter 1 Envelope";
			SourceNames[ModSource.Filter2Env] = "Filter 2 Envelope";
			SourceNames[ModSource.Mod1] = "Modulator 1";
			SourceNames[ModSource.Mod2] = "Modulator 2";
			SourceNames[ModSource.Mod3] = "Modulator 3";
			SourceNames[ModSource.Mod4] = "Modulator 4";
			SourceNames[ModSource.Mod5] = "Modulator 5";
			SourceNames[ModSource.Mod6] = "Modulator 6";
			SourceNames[ModSource.Seq1] = "Sequencer 1";
			SourceNames[ModSource.Seq2] = "Sequencer 2";
			SourceNames[ModSource.Spread] = "Spread";
		}
	}
}
