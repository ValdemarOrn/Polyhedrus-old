using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polyhedrus.Parameters;

namespace Polyhedrus.Modules
{
	public interface IFilter
	{
		double Samplerate { get; set; }
		double[] OutputBuffer { get; }

		void SetParameter(FilterParams parameter, object value);
		void Process(double[] input, double[] cutoffModulationEnv);
	}
}
