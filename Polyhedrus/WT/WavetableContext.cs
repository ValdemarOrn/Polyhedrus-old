using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Polyhedrus.WT
{
	public static class WavetableContext
	{
		// Wavetable note and partial data
		public static int[] PartialsCount = new int[128];
		public static int[] WavetableNoteIndex = new int[128];
		public static int[] PartialsPerWave;
		public static int PartialsTableCount;

		// Wavetable file and folder data
		public static string TableDirectory { get; private set; }
		public static Dictionary<string, string> AvailableWavetables { get; private set; }

		/// <summary>
		/// Generates any missing wavetable files using IWavetable classes. 
		/// Loads a list of wavetable files available on disk.
		/// </summary>
		public static void SetupWavetables()
		{
			var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			path = Path.Combine(path, "Polyhedrus\\Wavetables");
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

			AvailableWavetables = Directory.GetFiles(TableDirectory)
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
					if (firstReduction == -1)
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

				WavetableNoteIndex[i] = index;
			}

			PartialsPerWave = PartialsCount.Distinct().ToArray();
			PartialsTableCount = PartialsPerWave.Length;
		}
	}
}
