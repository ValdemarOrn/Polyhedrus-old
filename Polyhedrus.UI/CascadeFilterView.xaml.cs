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
using Polyhedrus.Modules;

namespace Polyhedrus.UI
{
	[ViewProviderFor(typeof(CascadeFilter))]
	public partial class CascadeFilterView : SynthModuleView
	{
		public CascadeFilterView() : base(null, (ModuleId)0)
		{
			InitializeComponent();
		}

		public CascadeFilterView(SynthController ctrl, ModuleId moduleId) : base(ctrl, moduleId)
		{
			InitializeComponent();
		}


		public double Cutoff
		{
			get { return (double)Ctrl.GetParameter(ModuleId, FilterParams.Cutoff); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.Cutoff, value);
				NotifyChange(() => Cutoff);
			}
		}

		public double Resonance
		{
			get { return (double)Ctrl.GetParameter(ModuleId, FilterParams.Resonance); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.Resonance, value);
				NotifyChange(() => Resonance);
			}
		}

		public double Gain
		{
			get { return (double)Ctrl.GetParameter(ModuleId, FilterParams.Gain); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.Gain, value);
				NotifyChange(() => Gain);
			}
		}

		public double Tracking
		{
			get { return (double)Ctrl.GetParameter(ModuleId, FilterParams.Tracking); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.Tracking, value);
				NotifyChange(() => Tracking);
			}
		}

		public double Envelope
		{
			get { return (double)Ctrl.GetParameter(ModuleId, FilterParams.Envelope); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.Envelope, value);
				NotifyChange(() => Envelope);
			}
		}

		public double X
		{
			get { return (double)Ctrl.GetParameter(ModuleId, FilterParams.X); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.X, value);
				NotifyChange(() => X);
			}
		}

		public double A
		{
			get { return (double)Ctrl.GetParameter(ModuleId, FilterParams.A); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.A, value);
				NotifyChange(() => A);
			}
		}

		public double B
		{
			get { return (double)Ctrl.GetParameter(ModuleId, FilterParams.B); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.B, value);
				NotifyChange(() => B);
			}
		}

		public double C
		{
			get { return (double)Ctrl.GetParameter(ModuleId, FilterParams.C); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.C, value);
				NotifyChange(() => C);
			}
		}

		public double D
		{
			get { return (double)Ctrl.GetParameter(ModuleId, FilterParams.D); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.D, value);
				NotifyChange(() => D);
			}
		}
	}
}
