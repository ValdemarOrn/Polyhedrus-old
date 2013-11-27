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
		public EnvelopeView()
		{
			InitializeComponent();
			KnobA.ValueFormatter = Formatter;
			KnobH.ValueFormatter = Formatter;
			KnobD.ValueFormatter = Formatter;
			KnobR.ValueFormatter = Formatter;
		}

		public EnvelopeView(SynthController ctrl, ModuleParams moduleId)
		{
			Ctrl = ctrl;
			ModuleId = moduleId;

			InitializeComponent();
			KnobA.ValueFormatter = Formatter;
			KnobH.ValueFormatter = Formatter;
			KnobD.ValueFormatter = Formatter;
			KnobR.ValueFormatter = Formatter;
		}

		string Formatter(double val)
		{
			var value = ValueTables.Get(val, ValueTables.Response4Dec) * 20000;

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
			get { return (double)Ctrl.GetParameter(ModuleId, EnvParams.Attack); }
			set
			{
				Ctrl.SetParameter(ModuleId, EnvParams.Attack, value);
				NotifyChange(() => Attack);
			}
		}

		public double Hold
		{
			get { return (double)Ctrl.GetParameter(ModuleId, EnvParams.Hold); }
			set
			{
				Ctrl.SetParameter(ModuleId, EnvParams.Hold, value);
				NotifyChange(() => Hold);
			}
		}

		public double Decay
		{
			get { return (double)Ctrl.GetParameter(ModuleId, EnvParams.Decay); }
			set
			{
				Ctrl.SetParameter(ModuleId, EnvParams.Decay, value);
				NotifyChange(() => Decay);
			}
		}

		public double Sustain
		{
			get { return (double)Ctrl.GetParameter(ModuleId, EnvParams.Sustain); }
			set
			{
				Ctrl.SetParameter(ModuleId, EnvParams.Sustain, value);
				NotifyChange(() => Sustain);
			}
		}

		public double Release
		{
			get { return (double)Ctrl.GetParameter(ModuleId, EnvParams.Release); }
			set
			{
				Ctrl.SetParameter(ModuleId, EnvParams.Release, value);
				NotifyChange(() => Release);
			}
		}
	}
}
