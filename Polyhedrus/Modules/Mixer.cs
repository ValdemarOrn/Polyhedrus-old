using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Modules
{
	public class Mixer
	{
		/// <summary>
		/// Knob Values
		/// </summary>
		public double Osc1VolParam;
		public double Osc2VolParam;
		public double Osc3VolParam;
		public double Osc4VolParam;
		public double Osc1MixParam;
		public double Osc2MixParam;
		public double Osc3MixParam;
		public double Osc4MixParam;
		public double F1ToF2Param;
		public double F1VolParam;
		public double F2VolParam;
		public double F1PanParam;
		public double F2PanParam;
		public double OutputVolumeParam;
		public double OutputPanParam;

		/// <summary>
		/// Modulation
		/// </summary>
		public double Osc1VolModulation;
		public double Osc2VolModulation;
		public double Osc3VolModulation;
		public double Osc4VolModulation;
		public double Osc1MixModulation;
		public double Osc2MixModulation;
		public double Osc3MixModulation;
		public double Osc4MixModulation;
		public double F1ToF2Modulation;
		public double F1VolModulation;
		public double F2VolModulation;
		public double F1PanModulation;
		public double F2PanModulation;
		public double OutputVolumeModulation;
		public double OutputPanModulation;

		/// <summary>
		/// Total
		/// </summary>
		public double Osc1Vol;
		public double Osc2Vol;
		public double Osc3Vol;
		public double Osc4Vol;
		public double Osc1Mix;
		public double Osc2Mix;
		public double Osc3Mix;
		public double Osc4Mix;
		public double F1ToF2;
		public double F1Vol;
		public double F2Vol;
		public double F1Pan;
		public double F2Pan;
		public double OutputVolume;
		public double OutputPan;

		public Mixer()
		{
			Osc1VolParam = 1;
			Osc2VolParam = 0;
			Osc3VolParam = 0;
			Osc4VolParam = 0;
			Osc1MixParam = 0;
			Osc2MixParam = 0;
			Osc3MixParam = 1;
			Osc4MixParam = 1;
			F1ToF2Param = 0;
			F1VolParam = 1;
			F2VolParam = 1;
			F1PanParam = 0;
			F2PanParam = 0;
			OutputVolumeParam = 1;
			OutputPan = 0;
		}

		public void Update()
		{
			Osc1Vol = Osc1VolParam + Osc1VolModulation;
			Osc2Vol = Osc2VolParam + Osc2VolModulation;
			Osc3Vol = Osc3VolParam + Osc3VolModulation;
			Osc4Vol = Osc4VolParam + Osc4VolModulation;
			Osc1Mix = Osc1MixParam + Osc1MixModulation;
			Osc2Mix = Osc2MixParam + Osc2MixModulation;
			Osc3Mix = Osc3MixParam + Osc3MixModulation;
			Osc4Mix = Osc4MixParam + Osc4MixModulation;
			F1ToF2 = F1ToF2Param + F1ToF2Modulation;
			F1Vol = F1VolParam + F1VolModulation;
			F2Vol = F2VolParam + F2VolModulation;
			F1Pan = F1PanParam + F1PanModulation;
			F2Pan = F2PanParam + F2PanModulation;
			OutputVolume = OutputVolumeParam + OutputVolumeModulation;
			OutputPan = OutputPanParam + OutputPanModulation;
		}
	}
}
