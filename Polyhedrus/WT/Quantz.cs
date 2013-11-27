using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.WT
{
	public class Quantz : IWavetable
	{
		static List<double[]> Waves = new List<double[]>
		{
			new double[8] { 10, 0, 0, 0, 0, 0, 0, 0 },
			new double[8] { 10, 10, 0, 0, 0, 0, 0, 0 },
			new double[8] { 10, 10, 10, 0, 0, 0, 0, 0 },
			new double[8] { 10, 10, 10, 10, 0, 0, 0, 0 },
			new double[8] { 10, 10, 10, 10, 10, 0, 0, 0 },
			new double[8] { 10, 10, 10, 10, 10, 10, 0, 0 },
			new double[8] { 10, 10, 10, 10, 10, 10, 10, 0 },
			
			new double[8] { 10, 10, 2, 2, 10, 10, 0, 0 },
			new double[8] { 10, 10, 4, 4, 10, 10, 0, 0 },
			new double[8] { 10, 10, 6, 6, 10, 10, 0, 0 },
			new double[8] { 10, 10, 8, 8, 10, 10, 0, 0 },

			new double[8] { 10, 0, 5, 5, 5, 5, 5, 5 },
			new double[8] { 10, 5, 0, 5, 5, 5, 5, 5 },
			new double[8] { 10, 6.6666, 3.3333, 0, 5, 5, 5, 5 },
			new double[8] { 10, 7.5, 5, 2.5, 0, 5, 5, 5 },
			new double[8] { 10, 8, 6, 4, 2, 0, 5, 5 },
			new double[8] { 10, 8.3333, 6.6666, 5, 3.3333, 1.6666, 0, 5 },
			new double[8] { 10, 8.5714, 7.1429, 5.7143, 4.2857, 2.8571, 1.4286, 0 },

			new double[8] { 0, 2, 4, 6, 8, 10, 7, 3 },
			new double[8] { 0, 3, 6, 9, 10, 9, 6, 3 },
			new double[8] { 5, 8, 10, 8, 5, 2, 0, 2 }
		};

		public WavetableData Create(int samplesPerWave)
		{
			var tableCount = 64;
			var waveLen = samplesPerWave;
			var output = new WavetableData { Name = "Quantz Waves", Data = new double[tableCount][] };

			var rand = new Random(42);
			var waves = Waves.Select(x => x).ToList();
			while(waves.Count < tableCount)
			{
				var randomWave = Enumerable.Range(0, 8).Select(x => rand.NextDouble() * 10).ToArray();
				waves.Add(randomWave);
			}

			var upsample = waveLen / 8;

			for (int w = 0; w < waves.Count; w++)
			{
				var wave = waves[w];
				var newWave = new List<double>();
				for (int i = 0; i < wave.Length; i++)
				{
					for (int j = 0; j < upsample; j++)
						newWave.Add(wave[i]);
				}

				output.Data[w] = AudioLib.Utils.Normalize(newWave.ToArray());
			}

			return output;
		}
	}
}
