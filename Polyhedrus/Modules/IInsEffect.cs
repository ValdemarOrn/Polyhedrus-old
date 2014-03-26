using System;
using Polyhedrus.Parameters;

namespace Polyhedrus.Modules
{
	public interface IInsEffect
	{
		double Samplerate { get; set; }
		double[] OutputBuffer { get; }

		void SetParameter(int parameter, object value);
		double[] Process(double[] input);
	}
}
