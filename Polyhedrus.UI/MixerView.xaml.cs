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
	/// Interaction logic for MixerView.xaml
	/// </summary>
	public partial class MixerView : SynthModuleView
	{
		public MixerView()
		{
			InitializeComponent();
		}

		public MixerView(SynthController ctrl, ModuleParams moduleId)
		{
			Ctrl = ctrl;
			ModuleId = moduleId;
			Func<double?, string> formatter = x =>
			{
				x = x ?? 1.0;
				return string.Format("{0:0}/{1:0}%", (1 - x) * 100, x * 100);
			};

			InitializeComponent();
			Osc1MixSpinner.Formatting = formatter;
			Osc2MixSpinner.Formatting = formatter;
			Osc3MixSpinner.Formatting = formatter;
			Osc4MixSpinner.Formatting = formatter;
		}

		public double Osc1Mix
		{
			get { return (double)Ctrl.GetParameter(ModuleId, MixerParams.Osc1Mix); }
			set
			{
				Ctrl.SetParameter(ModuleId, MixerParams.Osc1Mix, value);
				NotifyChange(() => Osc1Mix);
			}
		}

		public double Osc2Mix
		{
			get { return (double)Ctrl.GetParameter(ModuleId, MixerParams.Osc2Mix); }
			set
			{
				Ctrl.SetParameter(ModuleId, MixerParams.Osc2Mix, value);
				NotifyChange(() => Osc2Mix);
			}
		}

		public double Osc3Mix
		{
			get { return (double)Ctrl.GetParameter(ModuleId, MixerParams.Osc3Mix); }
			set
			{
				Ctrl.SetParameter(ModuleId, MixerParams.Osc3Mix, value);
				NotifyChange(() => Osc3Mix);
			}
		}

		public double Osc4Mix
		{
			get { return (double)Ctrl.GetParameter(ModuleId, MixerParams.Osc4Mix); }
			set
			{
				Ctrl.SetParameter(ModuleId, MixerParams.Osc4Mix, value);
				NotifyChange(() => Osc4Mix);
			}
		}

		public double F1ToF2
		{
			get { return (double)Ctrl.GetParameter(ModuleId, MixerParams.F1ToF2); }
			set
			{
				Ctrl.SetParameter(ModuleId, MixerParams.F1ToF2, value);
				NotifyChange(() => F1ToF2);
			}
		}

		public double Filter1Vol
		{
			get { return (double)Ctrl.GetParameter(ModuleId, MixerParams.F1Vol); }
			set
			{
				Ctrl.SetParameter(ModuleId, MixerParams.F1Vol, value);
				NotifyChange(() => Filter1Vol);
			}
		}

		public double Filter2Vol
		{
			get { return (double)Ctrl.GetParameter(ModuleId, MixerParams.F2Vol); }
			set
			{
				Ctrl.SetParameter(ModuleId, MixerParams.F2Vol, value);
				NotifyChange(() => Filter2Vol);
			}
		}

		public bool ParallelFX
		{
			get { return (bool)Ctrl.GetParameter(ModuleId, MixerParams.ParallelFX); }
			set
			{
				Ctrl.SetParameter(ModuleId, MixerParams.ParallelFX, value);
				NotifyChange(() => ParallelFX);
			}
		}
	}
}
