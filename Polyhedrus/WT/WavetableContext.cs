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
		public static string TableDirectory { get; private set; }
		public static Dictionary<string, string> AvailableWavetables { get; private set; }

		public static void Setup()
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

			/*
			var tables = Directory.GetFiles(TableDirectory)
				.Select(x => WavetableData.FromFile(x))
				.ToList();*/

		}
	}
}
