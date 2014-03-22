using AudioLib;
using Polyhedrus.Parameters;
using Polyhedrus.WT;

namespace Polyhedrus.Modules
{
	public sealed class BlOsc : IOscillator
	{
		private double fsInv;
		private double samplerate;
		
		private double startPhase;
		private int octave;
		private int semi;
		private int cent;
		private int note;
		private double modulation;

		private double[][][] wavetable;
		double tablePosition;
		private int waveNumber;
		private int tableA;
		private int tableB;
		private double tableMix;
		private double accumulator;
		private double stepsize;

		private bool stepsizeDirty;

		public BlOsc(double samplerate, int bufferSize)
		{
			OutputBuffer = new double[bufferSize];
			Samplerate = samplerate;
			accumulator = 0;
		}

		public double Samplerate
		{
			get { return samplerate; }
			set
			{
				samplerate = value;
				fsInv = 1.0 / samplerate;
				WavetableContext.CalculateIndexes(value);
			}
		}

		public double[] OutputBuffer { get; set; }
		
		public double TablePosition
		{
			get { return tablePosition; }
			set
			{
				tablePosition = value;
				tableA = (int)tablePosition;
				tableB = (int)(tablePosition + 1);
				tableMix = tablePosition - tableA;
			}
		}

		public void SetParameter(OscParams parameter, object val)
		{
			switch (parameter)
			{
				case OscParams.Modulation:
					modulation = (double)val;
					break;
				case OscParams.Note:
					note = (int)val;
					break;
				case OscParams.Octave:
					octave = (int)val;
					break;
				case OscParams.Semi:
					semi = (int)val;
					break;
				case OscParams.Cent:
					cent = (int)val;
					break;
				case OscParams.Position:
					TablePosition = (double)val;
					break;
				case OscParams.Phase:
					startPhase = (double)val;
					break;
				case OscParams.Wavetable:
					wavetable = WavetableContext.GetWavetable((string)val);
					break;
			}
			stepsizeDirty = true;
		}

		public void Reset()
		{
			accumulator = startPhase;
		}

		public void Process(int sampleCount)
		{
			if (stepsizeDirty)
				UpdateStepsize();

			if (wavetable == null)
			{
				for (var i = 0; i < sampleCount; i++)
					OutputBuffer[i] = 0.0;
				return;
			}

			for (var i = 0; i < sampleCount; i++)
			{
				double waveA = Interpolate(wavetable[tableA][waveNumber], accumulator);
				double waveB = Interpolate(wavetable[tableB][waveNumber], accumulator);
				var output = waveA * (1 - tableMix) + waveB * tableMix;
				accumulator += stepsize;
				if (accumulator > 1)
					accumulator -= 1;

				OutputBuffer[i] = output;
			}
		}

		private void UpdateStepsize()
		{
			double pitch = note + octave * 12 + semi + cent * 0.01 + modulation * 24;
			if (pitch > 127) pitch = 127;
			else if (pitch < 0) pitch = 0;

			waveNumber = WavetableContext.WavetableNoteIndex[(int)pitch];
			var hz = Utils.Note2HzLookup(pitch);
			stepsize = hz * fsInv;
		}

		/// <summary>
		/// Linear Interpolation between samples
		/// </summary>
		private static double Interpolate(double[] table, double accumulator)
		{
			var len = table.Length;
			int indexA = (int)(accumulator * len);
			int indexB = (indexA == len - 1) ? 0 : indexA + 1;
			double subsampleIndex = accumulator * len - indexA;

			double a = table[indexA];
			double b = table[indexB];
			double val = a * (1 - subsampleIndex) + b * subsampleIndex;
			return val;
		}
		
	}
}
