using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.WT
{
	public class Pulse : IWavetable
	{
		public WavetableData Create(int samplesPerWave)
		{
			var tableCount = 64;
			var waveLen = samplesPerWave;
			var output = new WavetableData { Name = "Pulse Wave", Data = new double[tableCount][] };

			for (int num = 0; num < tableCount; num++)
			{
				var wave = new double[waveLen];
				var breakpoint = (int)((0.5 + num / 64.0 * 0.5) * waveLen);

				for (int i = 0; i < waveLen; i++)
					wave[i] = (i <= breakpoint) ? 1.0 : -1.0;

				output.Data[num] = wave;
			}

			return output;
		}
	}
}
