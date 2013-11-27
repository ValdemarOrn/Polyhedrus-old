using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Polyhedrus.WT
{
	public class WavetableData
	{
		public string Name { get; set; }
		public double[][] Data { get; set; }

		public bool WriteFile(string path)
		{
			if (File.Exists(path))
				return false;

			var header = "Polyhedrus Wavetable, Format 1.0\n" + Name + "\n\n";

			var tableBytes = string.Join("\n\n",
				Data.Select(x => string.Join("\n", x.Select(y => y.ToString(CultureInfo.InvariantCulture)).ToArray()))
					.ToArray());

			var total = Compress(Encoding.UTF8.GetBytes(header + tableBytes));

			File.WriteAllBytes(path, total);
			return true;
		}

		public static WavetableData FromFile(string path)
		{
			try
			{
				var bytes = Decompress(File.ReadAllBytes(path));
				var text = Encoding.UTF8.GetString(bytes);
				var parts = text.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

				var header = parts[0].Split('\n');
				var format = header[0].Trim();
				var name = header[1].Trim();

				var data = parts.Skip(1).Select(x =>
				{
					var lines = x.Split('\n');
					var wave = lines.Select(y => Convert.ToDouble(y, CultureInfo.InvariantCulture)).ToArray();
					return wave;

				}).ToArray();

				return new WavetableData
				{
					Name = name,
					Data = data
				};
			}
			catch
			{
				return null;
			}
		}

		private static byte[] Compress(byte[] raw)
		{
			var ms = new MemoryStream(raw);
			ms.Position = 0;

			System.IO.MemoryStream outStream = new System.IO.MemoryStream();
			using (GZipStream tinyStream = new GZipStream(outStream, CompressionMode.Compress))
			{
				tinyStream.Write(raw, 0, raw.Length);
			}
			byte[] compressed = outStream.ToArray();
			return compressed;
		}

		private static byte[] Decompress(byte[] gzip)
		{
			const int size = 4096;
			byte[] buffer = new byte[size];

			using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
			using (MemoryStream memory = new MemoryStream())
			{
				int count = 1;
				while (count > 0)
				{
					count = stream.Read(buffer, 0, size);
					if (count > 0)
						memory.Write(buffer, 0, count);
				}

				return memory.ToArray();
			}
		}
	}
}
