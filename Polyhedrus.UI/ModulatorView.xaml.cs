using AudioLib;
using AudioLib.Modules;
using Polyhedrus.Parameters;
using Polyhedrus.UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Polyhedrus.UI
{
	public partial class ModulatorView : SynthModuleView
	{
		public ModulatorView()
		{
			InitializeComponent();
			KnobA.ValueFormatter = EnvFormatter;
			KnobH.ValueFormatter = EnvFormatter;
			KnobD.ValueFormatter = EnvFormatter;
			KnobR.ValueFormatter = EnvFormatter;
			KnobFreq.ValueFormatter = FreqFormatter;
		}

		public ModulatorView(SynthController ctrl, ModuleParams moduleId)
		{
			Ctrl = ctrl;
			ModuleId = moduleId;
			Waveforms = Ctrl.LFOWaves.Select(x => new ListItem<LFO.Wave>(x.Key, x.Value)).ToArray();

			InitializeComponent();
			KnobA.ValueFormatter = EnvFormatter;
			KnobH.ValueFormatter = EnvFormatter;
			KnobD.ValueFormatter = EnvFormatter;
			KnobR.ValueFormatter = EnvFormatter;
			KnobDelay.ValueFormatter = EnvFormatter;
			KnobFreq.ValueFormatter = FreqFormatter;
		}

		string EnvFormatter(double val)
		{
			double value = ValueTables.Get(val, ValueTables.Response4Dec) * 20000;
			if (value < 10)
				return String.Format("{0:0.00}ms", value);
			else if (value < 100)
				return String.Format("{0:0.0}ms", value);
			else if (value < 1000)
				return String.Format("{0:0}ms", value);
			else
				return String.Format("{0:0.00}s", value * 0.001);
		}

		string FreqFormatter(double val)
		{
			double value = ValueTables.Get(val, ValueTables.Response3Dec) * 50;
			return String.Format("{0:0.00}", value);
		}

		IEnumerable<ListItem<LFO.Wave>> _waveforms;
		public IEnumerable<ListItem<LFO.Wave>> Waveforms
		{
			get { return _waveforms; }
			set
			{
				_waveforms = value;
				NotifyChange(() => Waveforms);
			}
		}

		ListItem<LFO.Wave> _selectedWave;
		public ListItem<LFO.Wave> SelectedWaveform
		{
			get 
			{ 
				var wave = (LFO.Wave)Ctrl.GetParameter(ModuleId, ModulatorParams.Wave);
				return Waveforms.Single(x => x.Value == wave);
			}
			set
			{
				_selectedWave = value;
				Ctrl.SetParameter(ModuleId, ModulatorParams.Wave, value.Value);
				NotifyChange(() => SelectedWaveform);
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
			get { return (double)Ctrl.GetParameter(ModuleId, ModulatorParams.Frequency); }
			set
			{
				Ctrl.SetParameter(ModuleId, ModulatorParams.Frequency, value);
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

		public double Delay
		{
			get { return (double)Ctrl.GetParameter(ModuleId, ModulatorParams.Delay); }
			set
			{
				Ctrl.SetParameter(ModuleId, ModulatorParams.Delay, value);
				NotifyChange(() => Delay);
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
