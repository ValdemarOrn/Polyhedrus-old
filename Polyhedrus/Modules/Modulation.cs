using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Modules
{
	public enum ModDestination
	{
		None = 0,

		Osc1Pitch = 100,
		Osc1Vol,
		Osc2Pitch,
		Osc2Vol,

		Filter1Freq,
		Filter1Res,
		Filter1Vol,

		Filter2Freq,
		Filter2Res,
		Filter2Vol,

		Lfo1Speed,
		Lfo2Speed
	}

	public enum ModSource
	{
		None = 0,

		Pitch = 200,
		Velocity,
		ModWheel,

		AmpEnv,
		Filter1Env,
		Filter2Env,
		ModEnv1,
		ModEnv2,
		ModEnv3,
		ModEnv4,
		Lfo1,
		Lfo2

	}

	public static class Mod
	{
		public static string Name(this ModDestination src)
		{
			switch (src)
			{
				case (ModDestination.Osc1Pitch):
					return "Osc. 1 Pitch";
				case (ModDestination.Osc2Pitch):
					return "Osc. 2 Pitch";
				case (ModDestination.Filter1Freq):
					return "Filter 1 Freq.";
				case (ModDestination.Filter1Res):
					return "Filter 1 Res.";
				case (ModDestination.Filter1Vol):
					return "Filter 1 Amp.";
				case (ModDestination.Filter2Freq):
					return "Fitler 2 Freq.";
				case (ModDestination.Filter2Res):
					return "Filter 2 Res.";
				case (ModDestination.Filter2Vol):
					return "Filter 2 Amp.";
				case (ModDestination.Lfo1Speed):
					return "LFO 1 Speed";
				case (ModDestination.Lfo2Speed):
					return "LFO 2 Speed";
			}

			return "";
		}

		public static string Name(this ModSource src)
		{
			switch (src)
			{
				case (ModSource.Pitch):
					return "Pitch";
				case (ModSource.Velocity):
					return "Velocity";
				case (ModSource.ModWheel):
					return "ModWheel";
				case (ModSource.AmpEnv):
					return "Amplifier Envelope";
				case (ModSource.Filter1Env):
					return "Filter 1 Envelope";
				case (ModSource.Filter2Env):
					return "Filter 2 Envelope";
				case (ModSource.ModEnv1):
					return "Mod. Envelope 1";
				case (ModSource.Lfo1):
					return "LFO 1";
				case (ModSource.Lfo2):
					return "LFO 2";
			}

			return "";
		}
	}

	public sealed class ModRouting
	{
		public ModSource Source;
		public ModDestination Destination;

		public double Amount;

		/// <summary>
		/// True if visible in GUI
		/// </summary>
		public bool Visible;
	}
}
