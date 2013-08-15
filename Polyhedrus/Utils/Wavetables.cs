using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Utils
{
	public static class Wavetables
	{
		public static int[] PartialsCount = new int[128];
		public static int[] WavetableIndex = new int[128];
		public static int[] PartialsPerWave;
		public static int WavetableCount;

		public static void CalculateIndexes(double samplerate)
		{
			var nyquist = samplerate * 0.5;
			int i = 0;
			int firstReduction = -1;

			for (i = 0; i < 128; i++)
			{
				var hz = 440 * Math.Pow(2, (i - 69) / 12.0);
				int maxParts = (int)(nyquist / hz);
				if (maxParts >= 256)
					PartialsCount[i] = 256;
				else
				{
					PartialsCount[i] = maxParts;
					if(firstReduction == -1)
						firstReduction = i;
				}
			}

			// break notes into groups of 4
			for (i = firstReduction + 3; i < 128 - 4; i += 4)
			{
				PartialsCount[i - 3] = PartialsCount[i];
				PartialsCount[i - 2] = PartialsCount[i];
				PartialsCount[i - 1] = PartialsCount[i];
			}

			int index = 0;
			// calculate note indexes
			for (i = 1; i < 128; i++)
			{
				if (PartialsCount[i - 1] != PartialsCount[i])
					index++;

				WavetableIndex[i] = index;
			}

			PartialsPerWave = PartialsCount.Distinct().ToArray();
			WavetableCount = PartialsPerWave.Length;
		}
	}
}
