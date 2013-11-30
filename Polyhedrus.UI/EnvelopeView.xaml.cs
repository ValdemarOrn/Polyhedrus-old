using AudioLib;
using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
using System.Globalization;
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
	[ViewProviderFor(typeof(AudioLib.Modules.Ahdsr))]
	public partial class EnvelopeView : SynthModuleView
	{
		public EnvelopeView() : base(null, (ModuleId)0)
		{
			InitializeComponent();
			KnobA.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobH.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobD.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobR.ValueFormatter = Utils.Formatters.FormatEnvelope;
		}

		public EnvelopeView(SynthController ctrl, ModuleId moduleId) : base(ctrl, moduleId)
		{
			InitializeComponent();
			KnobA.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobH.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobD.ValueFormatter = Utils.Formatters.FormatEnvelope;
			KnobR.ValueFormatter = Utils.Formatters.FormatEnvelope;
		}

		private Visibility delayVisibility;
		public Visibility DelayVisibility
		{
			get { return delayVisibility; }
			set { delayVisibility = value; NotifyChange(() => DelayVisibility); }
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
