using System;
using System.Collections.Generic;
using AudioLib;
using Polyhedrus.Parameters;

namespace Polyhedrus.Modules
{
	[ModuleName("Cascade Filter")]
	public sealed class CascadeFilter : IFilter
	{
		const double Oversample = 4;

		public double[] OutputBuffer { get; private set; }

		private double cutoffKnob;
		private double cutoffModulation;
		private double cutoffModulationEnv;

		private double resonanceKnob;
		private double resonanceModulation;

		private double vx;
		private double va;
		private double vb;
		private double vc;
		private double vd;

		private double fsinv;
		private double samplerate;

		double p;

		double x;
		double a;
		double b;
		double c;
		double d;
		double feedback;
		private bool coefficientsDirty;

		public double Samplerate
		{
			get { return samplerate; }
			set
			{
				samplerate = value;
				fsinv = 1 / (Oversample * value);
			}
		}

		public CascadeFilter(double samplerate, int bufferSize)
		{
			Samplerate = samplerate;
			OutputBuffer = new double[bufferSize];
			cutoffKnob = 1;
			vd = 1;
			coefficientsDirty = true;
		}

		public void SetParameter(FilterParams parameter, object value)
		{
			switch (parameter)
			{
				case FilterParams.Cutoff:
					cutoffKnob = (double)value;
					break;
				case FilterParams.CutoffModulation:
					cutoffModulation = (double)value;
					break;
				case FilterParams.Resonance:
					resonanceKnob = (double)value;
					break;
				case FilterParams.ResonanceModulation:
					resonanceModulation = (double)value;
					break;
				case FilterParams.A:
					va = (double)value;
					break;
				case FilterParams.B:
					vb = (double)value;
					break;
				case FilterParams.C:
					vc = (double)value;
					break;
				case FilterParams.D:
					vd = (double)value;
					break;
				case FilterParams.X:
					vx = (double)value;
					break;
			}
			
			coefficientsDirty = true;
		}

		public void Process(double[] input, double[] cutoffModEnv)
		{
			var outputBuf = OutputBuffer;
			for (int i = 0; i < input.Length; i++)
			{
				cutoffModulationEnv = cutoffModEnv[i];
				UpdateCoefficients();
				outputBuf[i] = Process(input[i]);
			}
		}

		private double Process(double input)
		{
			if (coefficientsDirty)
				UpdateCoefficients();

			var reso = resonanceKnob + resonanceModulation;
			if (reso > 1)
				reso = 1;
			else if (reso < 0)
				reso = 0;

			for (int i = 0; i < Oversample; i++)
			{
				double fb = reso * 5 * (feedback - 0.5 * input);
				double val = input - fb;
				x = val;

				// 4 cascaded low pass stages
				a = (1 - p) * val + p * a;
				val = a;
				b = (1 - p) * val + p * b;
				val = b;
				c = (1 - p) * val + p * c;
				val = c;
				d = (1 - p) * val + p * d;
				val = d;

				feedback = Utils.TanhApprox(val);
			}

			var sample = (vx * x + va * a + vb * b + vc * c + vd * d) * (1 - reso * 0.5);
			return sample;
		}

		private void UpdateCoefficients()
		{
			var value = cutoffKnob + cutoffModulation + cutoffModulationEnv;
			if (value > 1)
				value = 1;
			else if (value < 0)
				value = 0;

			var cutoff = 10 + ValueTables.Get(value, ValueTables.Response3Dec) * 21000;
			p = (1 - 2 * cutoff * fsinv) * (1 - 2 * cutoff * fsinv);
			coefficientsDirty = false;
		}
	}
}
