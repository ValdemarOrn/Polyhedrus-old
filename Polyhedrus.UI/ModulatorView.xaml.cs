using AudioLib;
using AudioLib.Modules;
using Polyhedrus.Parameters;
using Polyhedrus.UI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Polyhedrus.UI
{
	[ViewProviderFor(typeof(Modules.Modulator))]
	public partial class ModulatorView : SynthModuleView
	{
		public ModulatorView() : base(null, (ModuleId)0)
		{
			InitializeComponent();
			KnobDelay.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobA.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobH.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobD.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobR.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobFreq.ValueFormatter = FreqFormatter;
		}

		public ModulatorView(SynthController ctrl, ModuleId moduleId) : base(ctrl, moduleId)
		{
			Waveforms = LFO.WaveNames.Select(x => new ListItem<LFO.Wave>(x.Key, x.Value)).ToArray();

			InitializeComponent();
			KnobDelay.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobA.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobH.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobD.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobR.ValueFormatter = Utils.Formatters.FormatEnvelope;
			
			KnobFreq.ValueFormatter = FreqFormatter;
		}

		private static string FreqFormatter(double val)
		{
			var value = ValueTables.Get(val, ValueTables.Response3Dec) * 50;
			return String.Format(CultureInfo.InvariantCulture, "{0:0.00} Hz", value);
		}

		public IEnumerable<ListItem<LFO.Wave>> Waveforms { get; set; }

		public ListItem<LFO.Wave> SelectedWaveform
		{
			get 
			{ 
				var wave = (LFO.Wave)Ctrl.GetParameter(ModuleId, ModulatorParams.Wave);
				return Waveforms.Single(x => x.Value == wave);
			}
			set
			{
				Ctrl.SetParameter(ModuleId, ModulatorParams.Wave, value.Value);
				NotifyChange(() => SelectedWaveform);
				NotifyChange(() => WaveformData);
			}
		}

		public IEnumerable<double> WaveformData
		{
			get
			{
				if (SelectedWaveform == null)
					return null;

				return Enumerable.Range(0, 256).Select(x => LFO.GetSample(SelectedWaveform.Value, x / 255.0,
					(SelectedWaveform.Value == LFO.Wave.SampleAndHold) ? x >> 4 : Shape));
			}
		}

		public double Delay
		{
			get { return (double)Ctrl.GetParameter(ModuleId, ModulatorParams.Delay); }
			set
			{
				Ctrl.SetParameter(ModuleId, ModulatorParams.Delay, value);
				NotifyChange(() => Delay);
			}
		}

		public double Attack
		{
			get { return (double)Ctrl.GetParameter(ModuleId, ModulatorParams.Attack); }
			set
			{
				Ctrl.SetParameter(ModuleId, ModulatorParams.Attack, value);
				NotifyChange(() => Attack);
			}
		}

		public double Hold
		{
			get { return (double)Ctrl.GetParameter(ModuleId, ModulatorParams.Hold); }
			set
			{
				Ctrl.SetParameter(ModuleId, ModulatorParams.Hold, value);
				NotifyChange(() => Hold);
			}
		}

		public double Decay
		{
			get { return (double)Ctrl.GetParameter(ModuleId, ModulatorParams.Decay); }
			set
			{
				Ctrl.SetParameter(ModuleId, ModulatorParams.Decay, value);
				NotifyChange(() => Decay);
			}
		}

		public double Sustain
		{
			get { return (double)Ctrl.GetParameter(ModuleId, ModulatorParams.Sustain); }
			set
			{
				Ctrl.SetParameter(ModuleId, ModulatorParams.Sustain, value);
				NotifyChange(() => Sustain);
			}
		}

		public double Release
		{
			get { return (double)Ctrl.GetParameter(ModuleId, ModulatorParams.Release); }
			set
			{
				Ctrl.SetParameter(ModuleId, ModulatorParams.Release, value);
				NotifyChange(() => Release);
			}
		}

		public double Frequency
		{
			get
			{ 
				var actual = (double)Ctrl.GetParameter(ModuleId, ModulatorParams.Frequency);
				return ValueTables.FindIndex(actual / 50, ValueTables.Response3Dec);
			}
			set
			{
				var val = ValueTables.Get((double)value, ValueTables.Response3Dec) * 50;
				Ctrl.SetParameter(ModuleId, ModulatorParams.Frequency, val);
				NotifyChange(() => Frequency);
			}
		}

		public double Phase
		{
			get { return (double)Ctrl.GetParameter(ModuleId, ModulatorParams.Phase); }
			set
			{
				Ctrl.SetParameter(ModuleId, ModulatorParams.Phase, value);
				NotifyChange(() => Phase);
			}
		}

		public double Offset
		{
			get { return (double)Ctrl.GetParameter(ModuleId, ModulatorParams.Offset); }
			set
			{
				Ctrl.SetParameter(ModuleId, ModulatorParams.Offset, value);
				NotifyChange(() => Offset);
			}
		}

		public double Shape
		{
			get { return (double)Ctrl.GetParameter(ModuleId, ModulatorParams.Shape); }
			set
			{
				Ctrl.SetParameter(ModuleId, ModulatorParams.Shape, value);
				NotifyChange(() => Shape);
				NotifyChange(() => WaveformData);
			}
		}

		public bool FreePhase
		{
			get { return (bool)Ctrl.GetParameter(ModuleId, ModulatorParams.FreePhase); }
			set
			{
				Ctrl.SetParameter(ModuleId, ModulatorParams.FreePhase, value);
				NotifyChange(() => FreePhase);
			}
		}

		public bool TempoSync
		{
			get { return (bool)Ctrl.GetParameter(ModuleId, ModulatorParams.TempoSync); }
			set
			{
				Ctrl.SetParameter(ModuleId, ModulatorParams.TempoSync, value);
				NotifyChange(() => TempoSync);
			}
		}
	}
}
