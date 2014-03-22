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
		double[] OutputBuffer { get; set; }
		double TablePosition { get; set; }

		void SetParameter(OscParams parameter, object value);
		void Reset();
		void Process(int sampleCount);
	}
}
