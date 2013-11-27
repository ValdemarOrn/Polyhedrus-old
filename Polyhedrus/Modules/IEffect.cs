using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Modules
{
	public interface IEffect
	{
		double[] Output { get; }
		double Samplerate { get; set; }
		double[] Parameters { get; set; }
		void Update();

		void Process(double[] input);
	}
}
