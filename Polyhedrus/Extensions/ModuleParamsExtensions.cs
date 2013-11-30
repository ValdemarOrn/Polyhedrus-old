using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public static class ModuleParamsExtensions
	{
		private static Dictionary<ModuleParams, string> Names = new Dictionary<ModuleParams, string>
		{
			{ModuleParams.Osc1, "Oscillator 1"},
			{ModuleParams.Osc2, "Oscillator 2"},
			{ModuleParams.Osc3, "Oscillator 3"},
			{ModuleParams.Osc4, "Oscillator 4"},

			{ModuleParams.Filter1, "Filter 1"},
			{ModuleParams.Filter2, "Filter 2"},

			{ModuleParams.AmpEnv, "Amplifier Envelope"},
			{ModuleParams.Filter1Env, "Filter 1 Envelope"},
			{ModuleParams.Filter2Env, "Filter 2 Envelope"},

			{ModuleParams.Modulator1, "Modulator 1"},
			{ModuleParams.Modulator2, "Modulator 2"},
			{ModuleParams.Modulator3, "Modulator 3"},
			{ModuleParams.Modulator4, "Modulator 4"},
			{ModuleParams.Modulator5, "Modulator 5"},
			{ModuleParams.Modulator6, "Modulator 6"},

			{ModuleParams.Step1, "Step Sequencer 1"},
			{ModuleParams.Step2, "Step Sequencer 2"},

			{ModuleParams.FX1, "Effect 1"},
			{ModuleParams.FX2, "Effect 2"},
			{ModuleParams.FX3, "Effect 3"},

			{ModuleParams.Mixer, "Mixer"},
			{ModuleParams.ModMatrix, "Modulation Matrix"},

			{ModuleParams.Arp, "Arpeggiator"},
			{ModuleParams.Settings, "Settings"}
		};

		public static string GetName(this ModuleParams param)
		{
			return Names.ContainsKey(param) ? Names[param] : param.ToString();
		}
	}
}
