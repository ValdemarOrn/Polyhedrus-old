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
		double _attack;
		double _hold;
		double _decay;
		double _sustain;
		double _release;

		double _frequency;
		double _phase;
		double _delay;
		double _offset;

		bool _freePhase;
		bool _tempoSync;

		ObservableCollection<ListItem<LFO.Wave>> _waveforms;
		ListItem<LFO.Wave> _selectedWave;

		public ModulatorView()
		{
			InitializeComponent();
			KnobA.ValueFormatter = EnvFormatter;
			KnobH.ValueFormatter = EnvFormatter;
			KnobD.ValueFormatter = EnvFormatter;
			KnobR.ValueFormatter = EnvFormatter;
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

		public ModulatorView(SynthController ctrl, ModuleParams moduleId) : this()
		{
			Ctrl = ctrl;
			ModuleId = moduleId;
			var waveformItems = LFO.WaveNames.Select(x => new ListItem<LFO.Wave>(x.Key, x.Value)).ToArray();
			Waveforms = new ObservableCollection<ListItem<LFO.Wave>>(waveformItems);
		}

		public ObservableCollection<ListItem<LFO.Wave>> Waveforms
		{
			get { return _waveforms; }
			set
			{
				_waveforms = value;
				NotifyChange(() => Waveforms);
			}
		}

		public ListItem<LFO.Wave> SelectedWaveform
		{
			get { return _selectedWave; }
			set
			{
				_selectedWave = value;
				Ctrl.SetParameter(ModuleId, ModulatorParams.Wave, value.Value);
				NotifyChange(() => SelectedWaveform);
			}
		}

		public double Attack
		{
			get { return _attack; }
			set
			{
				_attack = value;
				var val = ValueTables.Get(value, ValueTables.Response4Dec) * 20000;
				Ctrl.SetParameter(ModuleId, ModulatorParams.Attack, val);
				NotifyChange(() => Attack);
			}
		}

		public double Hold
		{
			get { return _hold; }
			set
			{
				_hold = value;
				var val = ValueTables.Get(value, ValueTables.Response4Dec) * 20000;
				Ctrl.SetParameter(ModuleId, ModulatorParams.Hold, val);
				NotifyChange(() => Hold);
			}
		}

		public double Decay
		{
			get { return _decay; }
			set
			{
				_decay = value;
				var val = ValueTables.Get(value, ValueTables.Response4Dec) * 20000;
				Ctrl.SetParameter(ModuleId, ModulatorParams.Decay, val);
				NotifyChange(() => Decay);
			}
		}

		public double Sustain
		{
			get { return _sustain; }
			set
			{
				_sustain = value;
				Ctrl.SetParameter(ModuleId, ModulatorParams.Sustain, value);
				NotifyChange(() => Sustain);
			}
		}

		public double Release
		{
			get { return _release; }
			set
			{
				_release = value;
				var val = ValueTables.Get(value, ValueTables.Response4Dec) * 20000;
				Ctrl.SetParameter(ModuleId, ModulatorParams.Release, val);
				NotifyChange(() => Release);
			}
		}

		public double Frequency
		{
			get { return _frequency; }
			set
			{
				_frequency = value;
				var val = ValueTables.Get(value, ValueTables.Response3Dec) * 50;
				Ctrl.SetParameter(ModuleId, ModulatorParams.Frequency, val);
				NotifyChange(() => Frequency);
			}
		}

		public double Phase
		{
			get { return _phase; }
			set
			{
				_phase = value;
				Ctrl.SetParameter(ModuleId, ModulatorParams.Phase, value);
				NotifyChange(() => Phase);
			}
		}

		public double Delay
		{
			get { return _delay; }
			set
			{
				_delay = value;
				Ctrl.SetParameter(ModuleId, ModulatorParams.Delay, value);
				NotifyChange(() => Delay);
			}
		}

		public double Offset
		{
			get { return _offset; }
			set
			{
				_offset = value;
				Ctrl.SetParameter(ModuleId, ModulatorParams.Offset, value);
				NotifyChange(() => Offset);
			}
		}


		public bool FreePhase
		{
			get { return _freePhase; }
			set
			{
				_freePhase = value;
				Ctrl.SetParameter(ModuleId, ModulatorParams.FreePhase, value);
				NotifyChange(() => FreePhase);
			}
		}

		public bool TempoSync
		{
			get { return _tempoSync; }
			set
			{
				_tempoSync = value;
				Ctrl.SetParameter(ModuleId, ModulatorParams.TempoSync, value);
				NotifyChange(() => TempoSync);
			}
		}
	}
}
