using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Polyhedrus.WT
{
	public interface IWavetable
	{
		WavetableData Create(int samplesPerWave);
	}
}
