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
		public Voice Voice;
		public List<ModRouting> Routes;

		public static int[] AvailableSources;
		public static int[] AvailableDestinations;

		static ModMatrix()
		{
			var sources = new List<int>();
			var destinations = new List<int>();
			
			foreach (var s in Enum.GetValues(typeof(ModDestination)))
				destinations.Add((int)s);

			foreach (var s in Enum.GetValues(typeof(ModSource)))
				sources.Add((int)s);

			AvailableSources = sources.ToArray();
			AvailableDestinations = destinations.ToArray();
		}

		public ModMatrix(Voice voice)
		{
			Voice = voice;
			Routes = new List<ModRouting>();
		}

		public void Process()
		{
			// Note: don't use foreach because another thread change modify the collection during the looping
			for(int i=0; i < Routes.Count; i++)
			{
				// one in a billion chance the user can remove the last route at the very momeny we try to read it
				// highly improbably, but if this code causes exceptions, it'll probably be that
				// probably requires locking to get rid of, or replacing the old route with null and performing null check
				var route = Routes[i];

				double src = 0.0;
				double amt = route.Amount;

				switch (route.Source)
				{
					case(ModSource.AmpEnv):
						src = Voice.AmpEnv.Output;
						break;
					case(ModSource.Filter1Env):
						src = Voice.Filter1Env.Output;
						break;
					case (ModSource.Filter2Env):
						src = Voice.Filter2Env.Output;
						break;
					case (ModSource.Lfo1):
						src = Voice.Lfo1.Output;
						break;
					case (ModSource.Lfo2):
						src = Voice.Lfo2.Output;
						break;
					case (ModSource.ModEnv1):
						src = Voice.ModEnv1.Output;
						break;
					case (ModSource.ModWheel):
						src = 0.0;
						break;
					case (ModSource.Pitch):
						src = 0.0;
						break;
					case (ModSource.Velocity):
						src = 0.0;
						break;
				}

				/*switch(route.Destination)
				{
					case (ModDestination.Filter1Vol):
						Voice.Filter1Vol = Voice.Filter1Vol * (amt * src + (1 - amt));
						break;
					case (ModDestination.Filter1Freq):
						Voice.Filter1.Cutoff += src * amt;
						break;
					case (ModDestination.Filter1Res):
						Voice.Filter1.Resonance += src * amt;
						break;

					case (ModDestination.Filter2Vol):
						Voice.Filter2Vol = Voice.Filter2Vol * (amt * src + (1 - amt));
						break;
					case (ModDestination.Filter2Freq):
						Voice.Filter2.Cutoff += src * amt;
						break;
					case (ModDestination.Filter2Res):
						Voice.Filter2.Resonance += src * amt;
						break;

					case (ModDestination.Lfo1Speed):
						Voice.Lfo1.FreqHz += src * amt * 15.0; // full modulation gives +-15Hz swing
						break;
					case (ModDestination.Lfo2Speed):
						Voice.Lfo2.FreqHz += src * amt * 15.0; // full modulation gives +-15Hz swing
						break;

					case (ModDestination.Osc1Pitch):
						Voice.Osc1.Pitch += src * amt * 36.0; // full modulation gives +-3 octaves
						break;
					case (ModDestination.Osc2Pitch):
						Voice.Osc2.Pitch += src * amt * 36.0; // full modulation gives +-3 octaves
						break;
				}*/
			}
		}
	}
}
