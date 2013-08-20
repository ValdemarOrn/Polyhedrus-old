using AudioLib;
using LowProfile.Fourier.Double;
using Polyhedrus.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Modules
{
	public sealed class BLOsc
	{
		public const int WaveSize = 2048;

		double _fsInv;
		double _samplerate;
		public double Samplerate
		{
			get { return _samplerate; }
			set
			{
				_samplerate = value;
				_fsInv = 1.0 / _samplerate;
				Wavetables.CalculateIndexes(value);
			}
		}
		
		public double Output;

		public double StartPhase;
		public int Octave;
		public int Semi;
		public int Cent;
		public int Note;
		public double Modulation;

		private double[][] Wavetable;
		private double[] WavetableCurrent;
		private double Accumulator;
		private double Stepsize;

		public BLOsc(double samplerate)
		{
			Samplerate = samplerate;
			Accumulator = 0;
			Wavetable = new double[Wavetables.WavetableCount][];
			for (int i = 0; i < Wavetable.Length; i++)
				Wavetable[i] = new double[WaveSize];

			SetWave(AudioLib.Utils.Saw(WaveSize, 1));
		}

		public void Reset()
		{
			Accumulator = StartPhase;
		}

		public void SetWave(double[] baseWave)
		{
			if (baseWave.Length != WaveSize)
				throw new Exception("Wave length must be 2048");

			var trans = new Transform(baseWave.Length);
			var complexIn = baseWave.Select(x => new Complex(x, 0)).ToArray();
			var fft = new Complex[complexIn.Length];
			var ifft = new Complex[fft.Length];
			trans.FFT(complexIn, fft);

			for (int i = 0; i < Wavetables.WavetableCount; i++)
			{
				var partials = Wavetables.PartialsPerWave[i];
				for (int n = partials + 1; n < fft.Length - partials; n++)
				{
					fft[n].Real = 0;
					fft[n].Imag = 0;
				}

				trans.IFFT(fft, ifft);
				Wavetable[i] = ifft.Select(x => x.Real).ToArray();
			}
		}

		public double Process()
		{
			double output = Interpolate(WavetableCurrent, Accumulator);
			Accumulator += Stepsize;
			if (Accumulator > 1)
				Accumulator -= 1;

			Output = output;
			return output;
		}

		public void UpdateStepsize()
		{
			double note = Note + Octave * 12 + Semi + Cent * 0.01 + Modulation * 24;
			if (note > 127)
				note = 127;
			else if (note < 0)
				note = 0;

			int waveNumber = Wavetables.WavetableIndex[(int)note];
			WavetableCurrent = Wavetable[waveNumber];

			var hz = AudioLib.Utils.Note2HzLookup(note);
			double increment = hz * _fsInv;
			Stepsize = increment;
		}

		/// <summary>
		/// Linear Interpolation between samples
		/// </summary>
		private double Interpolate(double[] table, double accumulator)
		{
			try
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
			catch (Exception e)
			{
				return 0;
			}

		}
		
	}
}
