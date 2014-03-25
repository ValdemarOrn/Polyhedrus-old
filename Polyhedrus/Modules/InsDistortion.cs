using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Modules
{
	[ModuleName("Distortion")]
	public sealed class InsDistortion : IInsEffect
	{
		public enum GainMode
		{
			None = 0,
			Tanh = 1,
			Diode,
			Tube,
			Sin,
			Clip
		};

		private const int LimitLow = -3;
		private const int LimitHigh = 3;
		private const int ScaleFactor = (1 << 16);

		private static double[] tanhData;
		private static double[] diodeData;
		private static double[] tubeData;
		private static double[] sinData;
		private static int count;

		static InsDistortion()
		{
			count = ScaleFactor * (LimitHigh - LimitLow);
			var source = AudioLib.Utils.Linspace(LimitLow, LimitHigh, count);

			tanhData = source.Select(x => Math.Tanh(x)).ToArray();
			
			diodeData = source.Select(x => (x > 0) 
				? Math.Tanh(Math.Exp(x) - 1)
				: -Math.Tanh(Math.Exp(-x) - 1))
				.ToArray();

			tubeData = source.Select(x => (x > 0)
				? Math.Tanh(Math.Exp(x) - 1)
				: Math.Exp(x) - 1)
				.ToArray();

			sinData = source.Select(x =>
			{
				var val = (x < -1) ? -1 : (x > 1) ? 1 : x;
				val = val * 0.5 * Math.PI;
				return Math.Sin(val);
			}).ToArray();
		}

		public double Samplerate { get; set; }
		public double[] OutputBuffer { get { return output; } }
		public double[] Parameters { get; private set; }

		private double inGain;
		private double outGain;
		private double bias;
		private GainMode mode;

		private double[] output;
		private double[] temp;
		private int[] intValues;

		public InsDistortion(double samplerate, int bufferSize)
		{
			Samplerate = samplerate;
			Parameters = new double[4];
			output = new double[bufferSize];
			temp = new double[bufferSize];
			intValues = new int[bufferSize];
			Parameters[0] = 1;
			Parameters[1] = 1;
			Update();
		}

		public void SetParameter(Parameters.InsertParams parameter, object value)
		{
			var idx = (int)parameter - 1;
			if (idx < 0 || idx >= Parameters.Length)
				return;

			Parameters[idx] = (double)value;
			Update();
		}

		public double[] Process(double[] input)
		{
			var len = input.Length;

			if (mode == GainMode.None)
			{
				for (int i = 0; i < len; i++)
					output[i] = input[i];
				return output;
			}
			
			for (int i = 0; i < len; i++)
				temp[i] = (input[i] + bias) * inGain;

			switch(mode)
			{
				case GainMode.None:
					ProcessNone(temp);
					break;
				case GainMode.Clip:
					ProcessClip(temp);
					break;
				case GainMode.Diode:
					Scale(temp);
					ProcessDiode(intValues);
					break;
				case GainMode.Sin:
					Scale(temp);
					ProcessSin(intValues);
					break;
				case GainMode.Tanh:
					Scale(temp);
					ProcessTanh(intValues);
					break;
				case GainMode.Tube:
					Scale(temp);
					ProcessTube(intValues);
					break;
			}

			for (var i = 0; i < len; i++)
				output[i] = temp[i] * outGain;

			return output;
		}

		private void Update()
		{
			inGain = Parameters[0];
			outGain = Parameters[1];
			bias = Parameters[2];
			mode = (GainMode)(int)Parameters[3];
		}

		private void Scale(double[] input)
		{
			var offset = count >> 1;
			var factor = ScaleFactor - 1;
			var len = input.Length;
			for (var i = 0; i < len; i++)
				intValues[i] = offset + (int)(factor * ((input[i] < LimitLow) ? LimitLow : (input[i] > LimitHigh) ? LimitHigh : input[i]));
		}

		private void ProcessNone(double[] input)
		{
			var len = input.Length;
			for (var i = 0; i < len; i++)
				temp[i] = input[i];
		}

		private void ProcessTube(int[] input)
		{
			var len = input.Length;
			for (var i = 0; i < len; i++)
				temp[i] = tubeData[input[i]];
		}

		private void ProcessTanh(int[] input)
		{
			var len = input.Length;
			for (int i = 0; i < len; i++)
				temp[i] = tanhData[input[i]];
		}

		private void ProcessSin(int[] input)
		{
			var len = input.Length;
			for (var i = 0; i < len; i++)
				temp[i] = sinData[input[i]];
		}

		private void ProcessDiode(int[] input)
		{
			var len = input.Length;
			for (var i = 0; i < len; i++)
				temp[i] = diodeData[input[i]];
		}

		private void ProcessClip(double[] input)
		{
			var len = input.Length;
			for (var i = 0; i < len; i++)
				temp[i] = (input[i] < -1) ? -1 : (input[i] > 1) ? 1 : input[i];
		}
	}
}
