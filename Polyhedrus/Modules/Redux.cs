using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Modules
{
	public class Redux : IEffect
	{
		double[] _output;
		double[] _parameters;

		int Counter;
		double LastSample;
		double Multiplier;

		int Bitcrush;
		int Downsample;

		public double[] Output { get { return _output; } }
		public double Samplerate { get; set; }

		public Redux()
		{
			_output = new double[16];
			_parameters = new double[2];
		}

		public double[] Parameters
		{
			get { return _parameters; }
			set { _parameters = value; }
		}

		public void Update()
		{
			Bitcrush = (int)Math.Pow(2, (int)_parameters[0]);
			Multiplier = 1.0 / Bitcrush;
			Downsample = (int)_parameters[1];
		}

		public void Process(double[] input)
		{
			for(int i = 0; i < input.Length; i++)
			{
				if ((Counter % Downsample) == 0)
				{
					var sample = input[i] * Bitcrush * Multiplier;
					Output[i] = input[i];
					LastSample = input[i];
				}
				else
				{
					Output[i] = LastSample;
				}

				Counter++;
			}
		}
	}
}
