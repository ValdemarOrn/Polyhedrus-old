using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using LowProfile.Fourier.Double;

namespace Polyhedrus.WT
{
	public static class WavetableContext
	{
		public const int WaveSize = 2048;

		public static Dictionary<string, double[][][]> wavetables = new Dictionary<string, double[][][]>();

		// Wavetable note and partial data
		public static int[] PartialsCount = new int[128];
		public static int[] WavetableNoteIndex = new int[128];
		public static int[] PartialsPerWave;
		public static int PartialsTableCount;

		// Wavetable file and folder data
		public static string TableDirectory { get; private set; }
		public static Dictionary<string, string> WavetableFiles { get; private set; }

		/// <summary>
		/// Generates any missing wavetable files using IWavetable classes. 
		/// Loads a list of wavetable files available on disk.
		/// </summary>
		public static void SetupWavetables()
		{
			var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			path = Path.Combine(path, "Polyhedrus", "Wavetables");
			Directory.CreateDirectory(path);
			TableDirectory = path;

			// Create waves if they are missing
			Assembly
				.GetExecutingAssembly()
				.GetExportedTypes()
				.Where(x => !x.IsInterface && typeof(IWavetable).IsAssignableFrom(x))
				.Select(x => x.GetConstructor(new Type[0]).Invoke(null) as IWavetable)
				.Select(x => x.Create(2048))
				.ToList()
				.ForEach(x => 
				{
					var filename = Path.Combine(TableDirectory, x.Name + ".wt.gz");
					if (!File.Exists(filename))
						x.WriteFile(filename);
				});

			WavetableFiles = Directory.GetFiles(TableDirectory)
				.Where(x => x.ToLower().EndsWith(".wt.gz"))
				.ToDictionary(x =>
				{
					var file = Path.GetFileName(x);
					return file.Substring(0, file.Length - 6);
				}, 
				x => x);
		}

		/// <summary>
		/// Calculates how many partials each note should have
		/// </summary>
		/// <param name="samplerate"></param>
		public static void CalculateIndexes(double samplerate)
		{
			var nyquist = samplerate * 0.5;
			int firstReduction = -1;

			for (var i = 0; i < 128; i++)
			{
				var hz = 440 * Math.Pow(2, (i - 69) / 12.0);
				int maxParts = (int)(nyquist / hz);
				if (maxParts >= 256)
					PartialsCount[i] = 256;
				else
				{
					PartialsCount[i] = maxParts;
					if (firstReduction == -1)
						firstReduction = i;
				}
			}

			// break notes into groups of 4
			for (var i = firstReduction + 3; i < 128 - 4; i += 4)
			{
				PartialsCount[i - 3] = PartialsCount[i];
				PartialsCount[i - 2] = PartialsCount[i];
				PartialsCount[i - 1] = PartialsCount[i];
			}

			int index = 0;
			// calculate note indexes
			for (var i = 1; i < 128; i++)
			{
				if (PartialsCount[i - 1] != PartialsCount[i])
					index++;

				WavetableNoteIndex[i] = index;
			}

			PartialsPerWave = PartialsCount.Distinct().ToArray();
			PartialsTableCount = PartialsPerWave.Length;
		}

		public static double[][][] TransformWavetable(double[][] wavetable)
		{
			var waveCount = wavetable.Length;
			var newWavetable = new double[waveCount][][];
			if (wavetable.Any(x => x.Length != WaveSize))
				throw new Exception("Wave length must be " + WaveSize);

			var trans = new TransformNative(WaveSize);

			for (var w = 0; w < waveCount; w++)
			{
				newWavetable[w] = new double[PartialsTableCount][];
				var baseWave = wavetable[w];

				var complexIn = baseWave.Select(x => new Complex(x, 0)).ToArray();
				var fft = new Complex[complexIn.Length];
				var ifft = new Complex[fft.Length];
				trans.FFT(complexIn, fft);

				for (var i = 0; i < PartialsTableCount; i++)
				{
					var partials = PartialsPerWave[i];
					for (var n = partials + 1; n < fft.Length - partials; n++)
					{
						fft[n].Real = 0;
						fft[n].Imag = 0;
					}

					trans.IFFT(fft, ifft);
					newWavetable[w][i] = ifft.Select(x => x.Real).ToArray();
				}
			}

			return newWavetable;
		}

		public static double[][][] GetWavetable(string wavetableName)
		{
			// Todo: unused tables never get unloaded
			if (wavetables.ContainsKey(wavetableName))
				return wavetables[wavetableName];

			var data = WavetableData.FromFile(WavetableFiles[wavetableName]);
			var transformed = TransformWavetable(data.Data);
			wavetables[wavetableName] = transformed;
			return wavetables[wavetableName];
		}
	}
}
