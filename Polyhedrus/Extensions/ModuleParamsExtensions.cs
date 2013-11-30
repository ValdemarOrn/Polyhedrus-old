using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public static class ModuleParamsExtensions
	{
		private static Dictionary<ModuleId, string> Names = new Dictionary<ModuleId, string>
		{
			{ModuleId.Osc1, "Oscillator 1"},
			{ModuleId.Osc2, "Oscillator 2"},
			{ModuleId.Osc3, "Oscillator 3"},
			{ModuleId.Osc4, "Oscillator 4"},

			{ModuleId.Insert1, "Insert FX 1"},
			{ModuleId.Insert2, "Insert FX 2"},

			{ModuleId.Filter1, "Filter 1"},
			{ModuleId.Filter2, "Filter 2"},

			{ModuleId.AmpEnv, "Amp Envelope"},
			{ModuleId.Filter1Env, "Filter 1 Envelope"},
			{ModuleId.Filter2Env, "Filter 2 Envelope"},

			{ModuleId.Modulator1, "Modulator 1"},
			{ModuleId.Modulator2, "Modulator 2"},
			{ModuleId.Modulator3, "Modulator 3"},
			{ModuleId.Modulator4, "Modulator 4"},
			{ModuleId.Modulator5, "Modulator 5"},
			{ModuleId.Modulator6, "Modulator 6"},

			{ModuleId.Step1, "Sequencer 1"},
			{ModuleId.Step2, "Sequencer 2"},

			{ModuleId.FX1, "Effect 1"},
			{ModuleId.FX2, "Effect 2"},
			{ModuleId.FX3, "Effect 3"},

			{ModuleId.Mixer, "Mixer"},
			{ModuleId.ModMatrix, "Modulation Matrix"},

			{ModuleId.Arp, "Arpeggiator"},
			{ModuleId.Settings, "Settings"}
		};

		public static string GetName(this ModuleId param)
		{
			return Names.ContainsKey(param) ? Names[param] : param.ToString();
		}
	}
}
