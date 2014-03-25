using System;
using System.Collections.Generic;
using AudioLib;
using Polyhedrus.Parameters;

namespace Polyhedrus.Modules
{
	public interface IFilter
	{
		double Samplerate { get; set; }
		double[] OutputBuffer { get; }

		void SetParameter(FilterParams parameter, object value);
		void Process(double[] input, double[] cutoffModulationEnv);
	}

	public sealed class CascadeFilter : IFilter
	{
		const double Oversample = 4;

		public double[] OutputBuffer { get; private set; }

		private double CutoffKnob;
		private double CutoffModulation;
		private double CutoffModulationEnv;

		private double ResonanceKnob;
		private double ResonanceModulation;

		private double Vx;
		private double Va;
		private double Vb;
		private double Vc;
		private double Vd;

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
			CutoffKnob = 1;
			Vd = 1;
			coefficientsDirty = true;
		}

		public void SetParameter(FilterParams parameter, object value)
		{
			switch (parameter)
			{
				case FilterParams.Cutoff:
					CutoffKnob = (double)value;
					break;
				case FilterParams.CutoffModulation:
					CutoffModulation = (double)value;
					break;
				case FilterParams.Resonance:
					ResonanceKnob = (double)value;
					break;
				case FilterParams.ResonanceModulation:
					ResonanceModulation = (double)value;
					break;
				case FilterParams.A:
					Va = (double)value;
					break;
				case FilterParams.B:
					Vb = (double)value;
					break;
				case FilterParams.C:
					Vc = (double)value;
					break;
				case FilterParams.D:
					Vd = (double)value;
					break;
				case FilterParams.X:
					Vx = (double)value;
					break;
			}
			
			coefficientsDirty = true;
		}

		public void Process(double[] input, double[] cutoffModulationEnv)
		{
			var outputBuf = OutputBuffer;
			for (int i = 0; i < input.Length; i++)
			{
				CutoffModulationEnv = cutoffModulationEnv[i];
				UpdateCoefficients();
				outputBuf[i] = Process(input[i]);
			}
		}

		private double Process(double input)
		{
			if (coefficientsDirty)
				UpdateCoefficients();

			var reso = ResonanceKnob + ResonanceModulation;
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

			var sample = (Vx * x + Va * a + Vb * b + Vc * c + Vd * d) * (1 - reso * 0.5);
			return sample;
		}

		private void UpdateCoefficients()
		{
			var value = CutoffKnob + CutoffModulation + CutoffModulationEnv;
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
