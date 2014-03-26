using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AudioLib;
using AudioLib.Modules;
using Polyhedrus.Parameters;

namespace Polyhedrus.Modules
{
	[ModuleName("Dual Filter")]
	public class DualFilter : IFilter
	{
		private double samplerate;
		private TwoPoleFilter filterA, filterB;

		public double[] OutputBuffer { get; private set; }

		private double cutoffKnob;
		private double cutoffModulation;
		private double cutoffModulationEnv;

		private double resonanceKnob;
		private double resonanceModulation;

		private bool fourPole;
		private double cutoffOffset;
		private double resonanceOffset;

		private bool coefficientsDirty;

		public double Samplerate
		{
			get { return samplerate; }
			set
			{
				samplerate = value;
				filterA.Samplerate = samplerate;
				filterB.Samplerate = samplerate;
			}
		}

		public DualFilter(double samplerate, int bufferSize)
		{
			OutputBuffer = new double[bufferSize];
			filterA = new TwoPoleFilter();
			filterB = new TwoPoleFilter();
			filterA.Fc = 1;
			filterB.Fc = 1;
			filterA.Q = 0.5;
			filterB.Q = 0.5;
			filterA.Mode = TwoPoleFilter.FilterMode.LowPass;
			filterB.Mode = TwoPoleFilter.FilterMode.LowPass;
			Samplerate = samplerate;
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
				case FilterParams.NumPoles:
					fourPole = value.Equals(4);
					break;

				case FilterParams.CutoffOffset:
					cutoffOffset = (double)value;
					break;
				case FilterParams.ResonanceOffset:
					resonanceOffset = (double)value;
					break;

				case FilterParams.FilterAMode:
					filterA.Mode = (TwoPoleFilter.FilterMode)value;
					break;
				case FilterParams.FilterBMode:
					filterB.Mode = (TwoPoleFilter.FilterMode)value;
					break;
			}

			coefficientsDirty = true;
		}

		public void Process(double[] input, double[] cutoffModEnv)
		{
			var outputBuf = OutputBuffer;
			for (var i = 0; i < input.Length; i++)
			{
				cutoffModulationEnv = cutoffModEnv[i];
				UpdateCoefficients();
				outputBuf[i] = Process(input[i]);
			}
		}

		private double Process(double value)
		{
			if (coefficientsDirty)
				UpdateCoefficients();

			value = filterA.Process(value);
			if (fourPole) value = filterB.Process(value);
			return value;
		}

		private void UpdateCoefficients()
		{
			var cutoffAVal = cutoffKnob + cutoffModulation + cutoffModulationEnv;
			if (cutoffAVal > 1) cutoffAVal = 1;
			else if (cutoffAVal < 0) cutoffAVal = 0;

			var cutoffBVal = cutoffKnob + cutoffModulation + cutoffModulationEnv + cutoffOffset;
			if (cutoffBVal > 1) cutoffBVal = 1;
			else if (cutoffBVal < 0) cutoffBVal = 0;

			var resoA = resonanceKnob + resonanceModulation;
			if (resoA > 1) resoA = 1;
			else if (resoA < 0.01) resoA = 0.01;

			var resoB = resonanceKnob + resonanceModulation + resonanceOffset;
			if (resoB > 1) resoB = 1;
			else if (resoB < 0.01) resoB = 0.01;

			var cutoffA = 10 + ValueTables.Get(cutoffAVal, ValueTables.Response3Dec) * 21000;
			var cutoffB = 10 + ValueTables.Get(cutoffBVal, ValueTables.Response3Dec) * 21000;

			filterA.Fc = cutoffA;
			filterB.Fc = cutoffB;
			filterA.Q = 0.5 + (ValueTables.Get(resoA, ValueTables.Response2Oct) - 0.25) * 10;
			filterB.Q = 0.5 + (ValueTables.Get(resoB, ValueTables.Response2Oct) - 0.25) * 10;
			filterA.Update();
			filterB.Update();
			coefficientsDirty = false;
		}
	}
}
