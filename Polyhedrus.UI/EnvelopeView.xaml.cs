using AudioLib;
using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
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
	/// <summary>
	/// Interaction logic for EnvelopeView.xaml
	/// </summary>
	public partial class EnvelopeView : SynthModuleView
	{
		double _attack;
		double _hold;
		double _decay;
		double _sustain;
		double _release;

		public EnvelopeView()
		{
			InitializeComponent();
			KnobA.ValueFormatter = Formatter;
			KnobH.ValueFormatter = Formatter;
			KnobD.ValueFormatter = Formatter;
			KnobR.ValueFormatter = Formatter;
		}

		public EnvelopeView(SynthController ctrl, ModuleParams moduleId) : this()
		{
			Ctrl = ctrl;
			ModuleId = moduleId;
		}

		string Formatter(double val)
		{
			double value = ValueTables.Get(val, ValueTables.Response4Dec) * 20000;
			if(value < 10)
				return String.Format("{0:0.00}ms", value);
			else if (value < 100)
				return String.Format("{0:0.0}ms", value);
			else if (value < 1000)
				return String.Format("{0:0}ms", value);
			else
				return String.Format("{0:0.00}s", value * 0.001);
		}

		

		public double Attack
		{
			get { return _attack; }
			set
			{
				_attack = value;
				var val = ValueTables.Get(value, ValueTables.Response4Dec) * 20000;
				Ctrl.SetParameter(ModuleId, EnvParams.Attack, val);
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
				Ctrl.SetParameter(ModuleId, EnvParams.Hold, val);
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
				Ctrl.SetParameter(ModuleId, EnvParams.Decay, val);
				NotifyChange(() => Decay);
			}
		}

		public double Sustain
		{
			get { return _sustain; }
			set
			{
				_sustain = value;
				Ctrl.SetParameter(ModuleId, EnvParams.Sustain, value);
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
				Ctrl.SetParameter(ModuleId, EnvParams.Release, val);
				NotifyChange(() => Release);
			}
		}
	}
}
