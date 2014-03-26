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
using AudioLib.Modules;

namespace Polyhedrus.UI
{
	[ViewProviderFor(typeof(DualFilter))]
	public partial class DualFilterView : SynthModuleView
	{
		public DualFilterView() : base(null, (ModuleId)0)
		{
			InitializeComponent();
		}

		public DualFilterView(SynthController ctrl, ModuleId moduleId) : base(ctrl, moduleId)
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

		public double CutoffOffset
		{
			get { return (double)Ctrl.GetParameter(ModuleId, FilterParams.CutoffOffset); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.CutoffOffset, value);
				NotifyChange(() => CutoffOffset);
			}
		}

		public double ResonanceOffset
		{
			get { return (double)Ctrl.GetParameter(ModuleId, FilterParams.ResonanceOffset); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.ResonanceOffset, value);
				NotifyChange(() => ResonanceOffset);
			}
		}

		public bool FourPole
		{
			get { return Ctrl.GetParameter(ModuleId, FilterParams.NumPoles).Equals(4); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.NumPoles, value ? 4 : 2);
				NotifyChange(() => FourPole);
			}
		}

		public TwoPoleFilter.FilterMode FilterAMode
		{
			get { return (TwoPoleFilter.FilterMode)Ctrl.GetParameter(ModuleId, FilterParams.FilterAMode); }
			set
			{
				if (FilterAMode == value)
					return;

				Ctrl.SetParameter(ModuleId, FilterParams.FilterAMode, value);
				NotifyChange(() => FilterAMode);
				NotifyChange(() => F1Lp);
				NotifyChange(() => F1Hp);
				NotifyChange(() => F1Bp);
			}
		}

		public TwoPoleFilter.FilterMode FilterBMode
		{
			get { return (TwoPoleFilter.FilterMode)Ctrl.GetParameter(ModuleId, FilterParams.FilterBMode); }
			set
			{
				if (FilterBMode == value)
					return;

				Ctrl.SetParameter(ModuleId, FilterParams.FilterBMode, value);
				NotifyChange(() => FilterBMode);
				NotifyChange(() => F2Lp);
				NotifyChange(() => F2Hp);
				NotifyChange(() => F2Bp);
			}
		}

		public bool F1Lp
		{
			get { return FilterAMode == TwoPoleFilter.FilterMode.LowPass; }
			set { FilterAMode = TwoPoleFilter.FilterMode.LowPass; }
		}

		public bool F1Hp
		{
			get { return FilterAMode == TwoPoleFilter.FilterMode.HighPass; }
			set { FilterAMode = TwoPoleFilter.FilterMode.HighPass; }
		}

		public bool F1Bp
		{
			get { return FilterAMode == TwoPoleFilter.FilterMode.BandPass; }
			set { FilterAMode = TwoPoleFilter.FilterMode.BandPass; }
		}


		public bool F2Lp
		{
			get { return FilterBMode == TwoPoleFilter.FilterMode.LowPass; }
			set { FilterBMode = TwoPoleFilter.FilterMode.LowPass; }
		}

		public bool F2Hp
		{
			get { return FilterBMode == TwoPoleFilter.FilterMode.HighPass; }
			set { FilterBMode = TwoPoleFilter.FilterMode.HighPass; }
		}

		public bool F2Bp
		{
			get { return FilterBMode == TwoPoleFilter.FilterMode.BandPass; }
			set { FilterBMode = TwoPoleFilter.FilterMode.BandPass; }
		}
	}
}
