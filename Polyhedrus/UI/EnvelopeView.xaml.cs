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
		}

		public EnvelopeView(SynthController ctrl, ModuleParams moduleId) : this()
		{
			Ctrl = ctrl;
			ModuleId = moduleId;
		}

		public double Attack
		{
			get { return _attack; }
			set
			{
				_attack = value;
				Ctrl.SetParameter(ModuleId, EnvParams.Attack, value);
				NotifyChange(() => Attack);
			}
		}

		public double Hold
		{
			get { return _hold; }
			set
			{
				_hold = value;
				Ctrl.SetParameter(ModuleId, EnvParams.Hold, value);
				NotifyChange(() => Hold);
			}
		}

		public double Decay
		{
			get { return _decay; }
			set
			{
				_decay = value;
				Ctrl.SetParameter(ModuleId, EnvParams.Decay, value);
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
				Ctrl.SetParameter(ModuleId, EnvParams.Release, value);
				NotifyChange(() => Release);
			}
		}
	}
}
