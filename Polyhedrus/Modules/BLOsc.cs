using AudioLib;
using LowProfile.Fourier.Double;
using Polyhedrus.WT;
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
				WavetableContext.CalculateIndexes(value);
			}
		}

		public double[][][] Wavetable;
		public double Output;
		public double[] OutputBuffer;

		public double StartPhase;
		public int Octave;
		public int Semi;
		public int Cent;
		public int Note;
		public double Modulation;

		private int WaveNumber;
		private int TableA;
		private int TableB;
		private double TableMix;
		private double Accumulator;
		private double Stepsize;

		public BLOsc(double samplerate, int bufferSize)
		{
			OutputBuffer = new double[bufferSize];
			Samplerate = samplerate;
			Accumulator = 0;
		}

		double _tablePosition;
		public double TablePosition
		{
			get { return _tablePosition; }
			set
			{
				_tablePosition = value;
				TableA = (int)_tablePosition;
				TableB = (int)(_tablePosition + 1);
				TableMix = _tablePosition - (double)TableA;
			}
		}

		public int WaveCount { get; private set; }

		public void Reset()
		{
			Accumulator = StartPhase;
		}

		public void SetWave(double[][] wavetable)
		{
			var WaveCount = wavetable.Length;
			var newWavetable = new double[WaveCount][][];
			if(wavetable.Any(x => x.Length != WaveSize))
				throw new Exception("Wave length must be 2048");

			var trans = new TransformNative(WaveSize);

			for (int w = 0; w < WaveCount; w++)
			{
				newWavetable[w] = new double[WavetableContext.PartialsTableCount][];
				var baseWave = wavetable[w];

				var complexIn = baseWave.Select(x => new Complex(x, 0)).ToArray();
				var fft = new Complex[complexIn.Length];
				var ifft = new Complex[fft.Length];
				trans.FFT(complexIn, fft);

				for (int i = 0; i < WavetableContext.PartialsTableCount; i++)
				{
					var partials = WavetableContext.PartialsPerWave[i];
					for (int n = partials + 1; n < fft.Length - partials; n++)
					{
						fft[n].Real = 0;
						fft[n].Imag = 0;
					}

					trans.IFFT(fft, ifft);
					newWavetable[w][i] = ifft.Select(x => x.Real).ToArray();
				}
			}

			// replace after we have created the new table, no interruption in sound
			Wavetable = newWavetable;
		}

		public double Process()
		{
			if (Wavetable == null)
				return 0.0;

			double waveA = Interpolate(Wavetable[TableA][WaveNumber], Accumulator);
			double waveB = Interpolate(Wavetable[TableB][WaveNumber], Accumulator);
			var output = waveA * (1 - TableMix) + waveB * TableMix;
			Accumulator += Stepsize;
			if (Accumulator > 1)
				Accumulator -= 1;

			Output = output;
			return output;
		}

		public void Process(int sampleCount)
		{
			if (Wavetable == null)
			{
				for (int i = 0; i < sampleCount; i++)
					OutputBuffer[i] = 0.0;
				return;
			}

			for (int i = 0; i < sampleCount; i++)
			{
				double waveA = Interpolate(Wavetable[TableA][WaveNumber], Accumulator);
				double waveB = Interpolate(Wavetable[TableB][WaveNumber], Accumulator);
				var output = waveA * (1 - TableMix) + waveB * TableMix;
				Accumulator += Stepsize;
				if (Accumulator > 1)
					Accumulator -= 1;

				OutputBuffer[i] = output;
			}
		}

		public void UpdateStepsize()
		{
			double note = Note + Octave * 12 + Semi + Cent * 0.01 + Modulation * 24;
			if (note > 127)
				note = 127;
			else if (note < 0)
				note = 0;

			WaveNumber = WavetableContext.WavetableNoteIndex[(int)note];
			var hz = AudioLib.Utils.Note2HzLookup(note);
			double increment = hz * _fsInv;
			Stepsize = increment;
		}

		/// <summary>
		/// Linear Interpolation between samples
		/// </summary>
		private double Interpolate(double[] table, double accumulator)
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
