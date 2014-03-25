using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polyhedrus.Parameters;

namespace Polyhedrus.Modules
{
	public interface IOscillator
	{
		double Samplerate { get; set; }
		double[] OutputBuffer { get; }

		void SetParameter(OscParams parameter, object value);
		void Reset();
		double[] Process(int sampleCount);
	}
}
