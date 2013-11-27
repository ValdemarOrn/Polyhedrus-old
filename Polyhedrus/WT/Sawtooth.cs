using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.WT
{
	public class Sawtooth : IWavetable
	{
		public WavetableData Create(int samplesPerWave)
		{
			var tableCount = 64;
			var waveLen = samplesPerWave;
			var output = new WavetableData { Name = "Sawtooth Wave", Data = new double[tableCount][] };

			for (int num = 0; num < tableCount; num++)
			{
				var wave = new double[waveLen];
				var breakpoint = (int)(num / 64.0 * waveLen);
				var slope = 1.0 / waveLen;
				var val = 1.0;

				for (int i = 0; i < waveLen; i++)
				{
					if (i == breakpoint)
						val = 1.0;

					wave[i] = val;
					val -= slope;
				}

				output.Data[num] = AudioLib.Utils.Normalize(wave);
			}

			return output;
		}
	}
}
