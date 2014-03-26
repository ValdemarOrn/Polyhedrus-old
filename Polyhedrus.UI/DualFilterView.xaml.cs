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

		public TwoPoleFilter.FilterMode FilterAMode
		{
			get { return (TwoPoleFilter.FilterMode)Ctrl.GetParameter(ModuleId, FilterParams.FilterAMode); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.FilterAMode, value);
				NotifyChange(() => FilterAMode);
			}
		}

		public TwoPoleFilter.FilterMode FilterBMode
		{
			get { return (TwoPoleFilter.FilterMode)Ctrl.GetParameter(ModuleId, FilterParams.FilterBMode); }
			set
			{
				Ctrl.SetParameter(ModuleId, FilterParams.FilterBMode, value);
				NotifyChange(() => FilterBMode);
			}
		}

		private void FilterTypeClicked(object sender, RoutedEventArgs e)
		{
			if (sender.Equals(ToggleFALowPass))
			{
				FilterAMode = TwoPoleFilter.FilterMode.LowPass;
				ToggleFAHighPass.IsChecked = false;
				ToggleFABandPass.IsChecked = false;
			}
			else if (sender.Equals(ToggleFAHighPass))
			{
				FilterAMode = TwoPoleFilter.FilterMode.HighPass;
				ToggleFALowPass.IsChecked = false;
				ToggleFABandPass.IsChecked = false;
			}
			else if (sender.Equals(ToggleFABandPass))
			{
				FilterAMode = TwoPoleFilter.FilterMode.BandPass;
				ToggleFALowPass.IsChecked = false;
				ToggleFAHighPass.IsChecked = false;
			}

			else if (sender.Equals(ToggleFBLowPass))
			{
				FilterBMode = TwoPoleFilter.FilterMode.LowPass;
				ToggleFBHighPass.IsChecked = false;
				ToggleFBBandPass.IsChecked = false;
			}
			else if (sender.Equals(ToggleFBHighPass))
			{
				FilterBMode = TwoPoleFilter.FilterMode.HighPass;
				ToggleFBLowPass.IsChecked = false;
				ToggleFBBandPass.IsChecked = false;
			}
			else if (sender.Equals(ToggleFBBandPass))
			{
				FilterBMode = TwoPoleFilter.FilterMode.BandPass;
				ToggleFBLowPass.IsChecked = false;
				ToggleFBHighPass.IsChecked = false;
			}
		}
	}
}
