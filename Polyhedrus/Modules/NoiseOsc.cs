using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polyhedrus.Parameters;

namespace Polyhedrus.Modules
{
	[ModuleName("Noise Generator")]
	public sealed class NoiseOsc : IOscillator
	{
		public const string TypeWhiteNoise = "White Noise";
		public const string TypePinkNoise = "Pink Noise";
		public const string TypeBrownNoise = "Brown Noise";
		public const string TypeRandomNoise = "Random Noise";

		public static IEnumerable<string> NoiseTypes
		{
			get { return new[] { TypeWhiteNoise, TypePinkNoise, TypeBrownNoise, TypeRandomNoise }; }
		}

		private double samplerate;

		private double index;
		private double stepsize;
		private double lastOutput;

		private double modulation;
		private bool keytrack;
		private int note;

		private string noiseType;

		public NoiseOsc(double samplerate, int bufferSize)
		{
			OutputBuffer = new double[bufferSize];
			Samplerate = samplerate;
			index = 0;
			stepsize = 1;
		}

		public double Samplerate
		{
			get { return samplerate; }
			set { samplerate = value; }
		}

		public double[] OutputBuffer { get; set; }

		public void SetParameter(OscParams parameter, object val)
		{
			switch (parameter)
			{
				case OscParams.Modulation:
					modulation = (double)val;
					break;
				case OscParams.Keytrack:
					keytrack = (bool)val;
					break;
				case OscParams.Note:
					note = (int)val;
					break;
				case OscParams.Position:
					stepsize = (double)val;
					break;
				case OscParams.Wavetable:
					noiseType = (string)val;
					break;
			}
		}

		public void Reset()
		{
			
		}

		public double[] Process(int sampleCount)
		{
			double[] source = null;
			double brown1 = 0.0;
			double brown2 = 1.0;
			switch (noiseType)
			{
				case TypeWhiteNoise:
					source = AudioLib.Noise.White;
					break;
				case TypePinkNoise:
					source = AudioLib.Noise.Pink;
					break;
				case TypeBrownNoise:
					brown1 = 0.99;
					brown2 = 0.1;
					source = AudioLib.Noise.Random;
					break;
				case TypeRandomNoise:
					source = AudioLib.Noise.Random;
					break;
				default:
					source = AudioLib.Noise.White;
					break;
			}

			var len = source.Length;
			var noteTrack = !keytrack ? 60 : note;
			var pitch = AudioLib.Utils.Note2HzLookup(noteTrack + modulation * 24) / AudioLib.Utils.Note2HzLookup(60);
			var inc = stepsize * pitch;

			if (inc < 0) inc = 0;
			if (inc > 1) inc = 1;

			for (var i = 0; i < sampleCount; i++)
			{
				var sample = source[(int)index];

				index += inc;
				if (index >= len)
					index -= len;

				lastOutput = (sample + lastOutput * brown1);
				OutputBuffer[i] = lastOutput * brown2;
			}

			return OutputBuffer;
		}
	}
}
