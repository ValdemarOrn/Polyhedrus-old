using System;
namespace Polyhedrus.Modules
{
	public interface IInsEffect
	{
		double[] OutputBuffer { get; }
		double[] Parameters { get; }
		double[] Process(double[] input);
		void Update();
	}
}
