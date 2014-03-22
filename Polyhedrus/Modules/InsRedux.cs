using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Modules
{
	public sealed class InsRedux : IInsEffect
	{
		public double Samplerate { get; set; }
		public double[] OutputBuffer { get { return Output; } }
		public double[] Parameters { get; private set; }

		private int Redux { get { return (int)(Parameters[0] + 0.001); } }
		private int Bits { get { return (int)(Parameters[1] + 0.001); } }
		private double[] Output;
		private double CurrentVal;
		private volatile uint Increment;
		private uint Counter;

		public InsRedux(double samplerate, int bufferSize)
		{
			Samplerate = samplerate;
			Parameters = new double[2];
			Output = new double[bufferSize];
			Parameters[0] = 1;
			Parameters[1] = 16;
			Update();
		}

		public void Update()
		{
			if (Redux < 1) // Redux must be 1....n
				return;

			Increment = (uint)(UInt32.MaxValue / Redux) + 1;
		}

		public double[] Process(double[] input)
		{
			var redux = Redux;
			var bitMultiplier = 1 << Bits;
			var bitMultuplierInv = 1.0 / bitMultiplier;
			var incr = Increment;

			for (int i = 0; i < input.Length; i++)
			{
				var sample = input[i];

				uint counterPrev = Counter;
				Counter += incr;
				if (Counter <= counterPrev)
				{
					CurrentVal = ((int)(sample * bitMultiplier)) * bitMultuplierInv;
				}

				Output[i] = CurrentVal;
			}

			return Output;
		}
	}
}
