using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AudioLib;
using AudioLib.Modules;
using Polyhedrus.Parameters;
using Polyhedrus.WT;

namespace Polyhedrus.Modules
{
	[ModuleName("Multi Oscillator")]
	public sealed class MultiOsc : IOscillator
	{
		private double fsInv;
		private double samplerate;

		private bool keytrack;
		private int octave;
		private int semi;
		private int cent;
		private int note;
		private double modulation;
		private double spread;

		private double output, alpha;
		private double[][] wavetable;
		private int waveNumber;

		private double[] iterators;
		private double[] increments;

		private bool incrementsDirty;

		public MultiOsc(double samplerate, int bufferSize)
		{
			OutputBuffer = new double[bufferSize];
			Samplerate = samplerate;
			iterators = new double[9];
			increments = new double[9];
			incrementsDirty = true;
			wavetable = WavetableContext.GetWavetable(Sawtooth.WavetableName)[0];

			var rand = new Random();
			for (int i = 0; i < iterators.Length; i++)
				iterators[i] = rand.NextDouble();
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
					spread = (double)val;
					break;
			}
			incrementsDirty = true;
		}

		public void Reset()
		{
		}

		public double[] Process(int sampleCount)
		{
			if (incrementsDirty)
				UpdateIncrements();

			for (var i = 0; i < sampleCount; i++)
			{
				var sum = 0.0;
				for (var j = 0; j < iterators.Length; j++)
				{
					var it = iterators[j];
					var sample = WavetableContext.Interpolate(wavetable[waveNumber], it);
					sum += sample;

					it += increments[j];
					if (it >= 1.0)
						it -= 1.0;

					iterators[j] = it;
				}

				output = (alpha - 1) * sum - alpha * output;
				OutputBuffer[i] = output * 0.33;
			}

			return OutputBuffer;
		}

		private void UpdateIncrements()
		{
			double pitch = note + octave * 12 + semi + cent * 0.01 + modulation * 24;
			if (pitch > 127) pitch = 127;
			else if (pitch < 0) pitch = 0;
			var max = increments.Length / 2;

			waveNumber = WavetableContext.WavetableNoteIndex[(int)pitch];
			var x = (2 * Utils.Note2HzLookup(pitch) / samplerate);
			alpha = x * x;

			for (var i = 0; i < increments.Length; i++)
			{
				var n = i - max;
				var part = pitch + n / (double)max * spread / 64;
				if (i < 0) // detune lower partials slightly lower to minimize beating
					part = - 0.02;

				var hz = Utils.Note2HzLookup(part);
				increments[i] = hz * fsInv;
			}

			incrementsDirty = false;
		}
	}
}
