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
	/// Interaction logic for LadderFilter.xaml
	/// </summary>
	public partial class CascadeFilterView : SynthModuleView
	{
		public double _cutoff;
		public double _resonance;
		public double _tracking;
		public double _envelope;
		public double _volume;
		public double _x;
		public double _a;
		public double _b;
		public double _c;
		public double _d;

		public CascadeFilterView()
		{
			InitializeComponent();
		}

		public CascadeFilterView(SynthController ctrl, ModuleParams moduleId) : this()
		{
			Ctrl = ctrl;
			ModuleId = moduleId;
			D = 1.0;
		}


		public double Cutoff
		{
			get { return _cutoff; }
			set
			{
				_cutoff = value;
				Ctrl.SetParameter(ModuleId, FilterParams.Cutoff, value);
				NotifyChange(() => Cutoff);
			}
		}

		public double Resonance
		{
			get { return _resonance; }
			set
			{
				_resonance = value;
				Ctrl.SetParameter(ModuleId, FilterParams.Resonance, value);
				NotifyChange(() => Resonance);
			}
		}

		public double Tracking
		{
			get { return _tracking; }
			set
			{
				_tracking = value;
				Ctrl.SetParameter(ModuleId, FilterParams.Tracking, value);
				NotifyChange(() => Tracking);
			}
		}

		public double Envelope
		{
			get { return _envelope; }
			set
			{
				_envelope = value;
				Ctrl.SetParameter(ModuleId, FilterParams.Envelope, value);
				NotifyChange(() => Envelope);
			}
		}

		public double Volume
		{
			get { return _volume; }
			set
			{
				_volume = value;
				Ctrl.SetParameter(ModuleId, FilterParams.Volume, value);
				NotifyChange(() => Volume);
			}
		}

		public double X
		{
			get { return _x; }
			set
			{
				_x = value;
				Ctrl.SetParameter(ModuleId, FilterParams.X, value);
				NotifyChange(() => X);
			}
		}

		public double A
		{
			get { return _a; }
			set
			{
				_a = value;
				Ctrl.SetParameter(ModuleId, FilterParams.A, value);
				NotifyChange(() => A);
			}
		}

		public double B
		{
			get { return _b; }
			set
			{
				_b = value;
				Ctrl.SetParameter(ModuleId, FilterParams.B, value);
				NotifyChange(() => B);
			}
		}

		public double C
		{
			get { return _c; }
			set
			{
				_c = value;
				Ctrl.SetParameter(ModuleId, FilterParams.C, value);
				NotifyChange(() => C);
			}
		}

		public double D
		{
			get { return _d; }
			set
			{
				_d = value;
				Ctrl.SetParameter(ModuleId, FilterParams.D, value);
				NotifyChange(() => D);
			}
		}
	}
}
