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
using Polyhedrus.Parameters;

namespace Polyhedrus.UI
{
	[ViewProviderFor(typeof(Modules.NoiseOsc))]
	public partial class NoiseOscView : SynthModuleView
	{
		public NoiseOscView() : base(null, (ModuleId)0)
		{
			InitializeComponent();
		}

		public NoiseOscView(SynthController ctrl, ModuleId moduleId) : base(ctrl, moduleId)
		{
			InitializeComponent();
		}

		public IEnumerable<string> NoiseTypes
		{
			get { return NoiseOsc.NoiseTypes; }
		}

		public string NoiseType
		{
			get
			{
				return (string)Ctrl.GetParameter(ModuleId, OscParams.Wavetable);
			}
			set
			{
				Ctrl.SetParameter(ModuleId, OscParams.Wavetable, value);
				NotifyChange(() => NoiseType);
			}
		}

		public double Rate
		{
			get { return (double)Ctrl.GetParameter(ModuleId, OscParams.Position); }
			set
			{
				Ctrl.SetParameter(ModuleId, OscParams.Position, value);
				NotifyChange(() => Rate);
			}
		}

		public double Mix
		{
			get { return 0.0; }
			set
			{

			}
		}

		public double Volume
		{
			get { return (double)Ctrl.GetParameter(ModuleId, OscParams.Volume); }
			set
			{
				Ctrl.SetParameter(ModuleId, OscParams.Volume, value);
				NotifyChange(() => Volume);
			}
		}

		public bool Keytrack
		{
			get { return (bool)Ctrl.GetParameter(ModuleId, OscParams.Keytrack); }
			set
			{
				Ctrl.SetParameter(ModuleId, OscParams.Keytrack, value);
				NotifyChange(() => Volume);
			}
		}
	}
}
