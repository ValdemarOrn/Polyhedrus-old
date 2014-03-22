using System;
namespace Polyhedrus.Modules
{
	public interface IInsEffect
	{
		double Samplerate { get; set; }
		double[] OutputBuffer { get; }
		double[] Parameters { get; }
		double[] Process(double[] input);
		void Update();
	}
}
