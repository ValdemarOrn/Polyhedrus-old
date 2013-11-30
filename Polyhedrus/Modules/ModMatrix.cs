using AudioLib.Modules;
using Polyhedrus.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Modules
{
	public sealed class ModMatrix
	{
		public static int[] AvailableSources;
		public static int[] AvailableDestinations;

		public Voice Voice;
		public ModRoute[] Routes;

		public double Filter1Tracking;
		public double Filter2Tracking;
		public double Filter1EnvMod;
		public double Filter2EnvMod;

		double[] PreviousValues;
		double[] Values;

		volatile bool UpdateMixer;

		int[] ActiveRoutes;
		ModDestination[] ActiveDestinations;

		public ModMatrix(Voice voice)
		{
			Voice = voice;
			int count = GetDestinationCount();
			Values = new double[count];
			PreviousValues = new double[count];
			ActiveRoutes = new int[0];
			ActiveDestinations = new ModDestination[0];

			Routes = new ModRoute[20]; // structs get initialized
		}

		public void UpdateRoute(ModRoute route)
		{
			Routes[route.Index] = route;

			var dests = new List<ModDestination>();
			var activeRoutes = new List<int>();

			for (int i = 0; i < Routes.Length; i++)
			{
				if (!Routes[i].Enabled || Routes[i].Amount == 0 || Routes[i].Source == ModSource.None || Routes[i].Destination == ModDestination.None)
					continue;

				if(!dests.Contains(Routes[i].Destination))
					dests.Add(Routes[i].Destination);

				activeRoutes.Add(i);
			}

			// Todo: Zero out current modulation value after turning off a mod route 

			ActiveDestinations = dests.ToArray();
			ActiveRoutes = activeRoutes.ToArray();

			ProcessAllRoutes();
		}

		public void ProcessAllRoutes()
		{
			UpdateMixer = false;

			// zero out old values
			for (int i = 0; i < Values.Length; i++)
				Values[i] = 0;

			// loop through ALL routes and add together all the values
			for (int i = 0; i < Routes.Length; i++)
			{
				var route = Routes[i];

				double src = Voice.GetModSource(route.Source);
				double ctrl = Voice.GetModSource(route.Control);
				double ctrlAmt = route.ControlAmount;
				double amt = route.Amount * (route.Enabled ? 1 : 0);

				double result = src * (1 - ctrlAmt + ctrlAmt * ctrl) * amt;
				Values[(int)route.Destination] += result;
			}

			// apply the values to the destination
			for (int i = 0; i < Values.Length; i++)
			{
				var value = Values[i];
				PreviousValues[i] = value;
				UpdateModDestination((ModDestination)i, value);
			}

			if (UpdateMixer)
				Voice.Mixer.Update();
		}

		public void Process()
		{
			UpdateMixer = false;

			// zero out old values
			for (int i = 0; i < ActiveDestinations.Length; i++)
			{
				var dest = ActiveDestinations[i];
				Values[(int)dest] = 0;
			}

			// loop through active routes and add together all the values
			for (int i = 0; i < ActiveRoutes.Length; i++)
			{
				var route = Routes[ActiveRoutes[i]];

				double src = Voice.GetModSource(route.Source);
				double ctrl = Voice.GetModSource(route.Control);
				double ctrlAmt = route.ControlAmount;
				double amt = route.Amount;

				double result = src * (1 - ctrlAmt + ctrlAmt * ctrl) * amt;
				Values[(int)route.Destination] += result;
			}

			// apply the values to the destination
			for (int i = 0; i < ActiveDestinations.Length; i++)
			{
				var dest = ActiveDestinations[i];

				var previousOutput = PreviousValues[(int)dest];
				var value = Values[(int)dest];
				PreviousValues[(int)dest] = value;

				if (previousOutput == value)
					continue; // no need for an update

				UpdateModDestination(dest, value);
			}

			if (UpdateMixer)
				Voice.Mixer.Update();
		}

		void UpdateModDestination(ModDestination dest, double value)
		{
			// If you're reading this, try to come up with a better idea :)
			switch (dest)
			{
				case ModDestination.Filter1Freq:
					Voice.Filter1.CutoffModulation = value;
					// Filters get updated after ModMatrix finishes, because the filter envs run every cycle
					break;
				case ModDestination.Filter1Pan:
					Voice.Mixer.F1PanModulation = value;
					UpdateMixer = true;
					break;
				case ModDestination.Filter1Res:
					Voice.Filter1.ResonanceModulation = value;
					break;
				case ModDestination.Filter1Vol:
					Voice.Mixer.F1VolModulation = value;
					UpdateMixer = true;
					break;
				case ModDestination.Filter2Freq:
					Voice.Filter2.CutoffModulation = value;
					// Filters get updated after ModMatrix finishes, because the filter envs run every cycle
					break;
				case ModDestination.Filter2Pan:
					Voice.Mixer.F2PanModulation = value;
					UpdateMixer = true;
					break;
				case ModDestination.Filter2Res:
					Voice.Filter2.ResonanceModulation = value;
					break;
				case ModDestination.Filter2Vol:
					Voice.Mixer.F2VolModulation = value;
					UpdateMixer = true;
					break;
				case ModDestination.FX1Wet:
					// Add route
					break;
				case ModDestination.FX2Wet:
					// Add route
					break;
				case ModDestination.FX3Wet:
					// Add route
					break;
				case ModDestination.FX4Wet:
					// Add route
					break;
				case ModDestination.Insert1Param1:
					// Add route
					break;
				case ModDestination.Insert1Param2:
					// Add route
					break;
				case ModDestination.Insert1Param3:
					// Add route
					break;
				case ModDestination.Insert1Param4:
					// Add route
					break;
				case ModDestination.Insert1Wet:
					// Add route
					break;
				case ModDestination.Insert2Param1:
					// Add route
					break;
				case ModDestination.Insert2Param2:
					// Add route
					break;
				case ModDestination.Insert2Param3:
					// Add route
					break;
				case ModDestination.Insert2Param4:
					// Add route
					break;
				case ModDestination.Insert2Wet:
					// Add route
					break;
				case ModDestination.Mod1Speed:
					// Add route
					break;
				case ModDestination.Mod2Speed:
					// Add route
					break;
				case ModDestination.Mod3Speed:
					// Add route
					break;
				case ModDestination.Mod4Speed:
					// Add route
					break;
				case ModDestination.Mod5Speed:
					// Add route
					break;
				case ModDestination.Mod6Speed:
					// Add route
					break;
				case ModDestination.MasterPan:
					Voice.Mixer.OutputPanModulation = value;
					UpdateMixer = true;
					break;
				case ModDestination.MasterVolume:
					Voice.Mixer.OutputVolumeModulation = value;
					UpdateMixer = true;
					break;
				case ModDestination.Osc1Pitch:
					Voice.Osc1.Modulation = value;
					Voice.Osc1.UpdateStepsize();
					break;
				case ModDestination.Osc1Vol:
					Voice.Mixer.Osc1VolModulation = value;
					UpdateMixer = true;
					break;
				case ModDestination.Osc1Pos:
					// Add route
					break;
				case ModDestination.Osc2Pitch:
					Voice.Osc2.Modulation = value;
					Voice.Osc2.UpdateStepsize();
					break;
				case ModDestination.Osc2Vol:
					Voice.Mixer.Osc2VolModulation = value;
					UpdateMixer = true;
					break;
				case ModDestination.Osc2Pos:
					// Add route
					break;
				case ModDestination.Osc3Pitch:
					Voice.Osc3.Modulation = value;
					Voice.Osc3.UpdateStepsize();
					break;
				case ModDestination.Osc3Vol:
					Voice.Mixer.Osc3VolModulation = value;
					UpdateMixer = true;
					break;
				case ModDestination.Osc3Pos:
					// Add route
					break;
				case ModDestination.Osc4Pitch:
					Voice.Osc4.Modulation = value;
					Voice.Osc4.UpdateStepsize();
					break;
				case ModDestination.Osc4Vol:
					Voice.Mixer.Osc4VolModulation = value;
					UpdateMixer = true;
					break;
				case ModDestination.Osc4Pos:
					// Add route
					break;
			}
		}

		private int GetDestinationCount()
		{
			var destinations = new List<int>();

			foreach (var s in Enum.GetValues(typeof(ModDestination)))
				destinations.Add((int)s);

			return destinations.Max();
		}
	}
}
