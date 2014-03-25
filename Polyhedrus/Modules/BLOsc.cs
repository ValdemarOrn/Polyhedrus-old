using AudioLib;
using Polyhedrus.Parameters;
using Polyhedrus.WT;

namespace Polyhedrus.Modules
{
	[ModuleName("Wavetable Oscillator")]
	public sealed class BlOsc : IOscillator
	{
		private double fsInv;
		private double samplerate;
		
		private double startPhase;
		private bool keytrack;
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
			stepsizeDirty = true;
		}

		public double Samplerate
		{
			get { return samplerate; }
			set
			{
				samplerate = value;
				fsInv = 1.0 / samplerate;
			}
		}

		public double[] OutputBuffer { get; set; }
		
		private double TablePosition
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
				case OscParams.Keytrack:
					keytrack = (bool)val;
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

		public double[] Process(int sampleCount)
		{
			if (stepsizeDirty)
				UpdateStepsize();

			if (wavetable == null)
			{
				for (var i = 0; i < sampleCount; i++)
					OutputBuffer[i] = 0.0;

				return OutputBuffer;
			}

			for (var i = 0; i < sampleCount; i++)
			{
				double waveA = WavetableContext.Interpolate(wavetable[tableA][waveNumber], accumulator);
				double waveB = WavetableContext.Interpolate(wavetable[tableB][waveNumber], accumulator);
				var output = waveA * (1 - tableMix) + waveB * tableMix;
				accumulator += stepsize;
				if (accumulator > 1)
					accumulator -= 1;

				OutputBuffer[i] = output;
			}

			return OutputBuffer;
		}

		private void UpdateStepsize()
		{
			double pitch = note + octave * 12 + semi + cent * 0.01 + modulation * 24;
			if (pitch > 127) pitch = 127;
			else if (pitch < 0) pitch = 0;

			waveNumber = WavetableContext.WavetableNoteIndex[(int)pitch];
			var hz = Utils.Note2HzLookup(pitch);
			stepsize = hz * fsInv;
			stepsizeDirty = false;
		}
	}
}
