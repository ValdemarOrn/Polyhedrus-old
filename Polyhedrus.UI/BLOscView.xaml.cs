using System.Linq;
using Polyhedrus.Parameters;
using Polyhedrus.UI.Models;
using System;
using System.Collections.Generic;
using Polyhedrus.WT;

namespace Polyhedrus.UI
{
	[ViewProviderFor(typeof(Modules.BlOsc))]
	[ViewProviderFor(typeof(Modules.MultiOsc))]
	public partial class BlOscView : SynthModuleView
	{
		public BlOscView() : base(null, (ModuleId)0)
		{
			InitializeComponent();
		}

		public BlOscView(SynthController ctrl, ModuleId moduleId) : base(ctrl, moduleId)
		{
			InitializeComponent();
			
			KnobOctave.ValueFormatter = Formatter;
			KnobSemi.ValueFormatter = Formatter;
			KnobCent.ValueFormatter = Formatter;
		}

		string Formatter(double val)
		{
			return val.ToString("+0;-0");
		}

		public IEnumerable<string> Wavetables
		{
			get { return WavetableContext.WavetableFiles.Keys.ToArray(); }
		}

		public string SelectedWavetable
		{
			get
			{
				return (string)Ctrl.GetParameter(ModuleId, OscParams.Wavetable);
			}
			set
			{
				new System.Threading.Thread(() =>
				{
					Ctrl.SetParameter(ModuleId, OscParams.Wavetable, value);
					Action update = () =>
					{
						NotifyChange(() => SelectedWavetable);
						NotifyChange(() => WavetableData);
					};

					Dispatcher.Invoke(update);

				}).Start();
			}
		}

		public IEnumerable<double> WavetableData
		{
			get
			{
				var wt = SelectedWavetable != null ? WavetableContext.wavetables[SelectedWavetable] : null;
				return wt != null ? wt[(int)Position][0] : null;
			}
		}

		public double Octave
		{
			get { return (int)Ctrl.GetParameter(ModuleId, OscParams.Octave); }
			set 
			{
				Ctrl.SetParameter(ModuleId, OscParams.Octave, (int)value); 
				NotifyChange(() => Octave); 
			}
		}

		public double Semi
		{
			get { return (int)Ctrl.GetParameter(ModuleId, OscParams.Semi); }
			set 
			{
				Ctrl.SetParameter(ModuleId, OscParams.Semi, (int)value); 
				NotifyChange(() => Semi);
			}
		}

		public double Cent
		{
			get { return (int)Ctrl.GetParameter(ModuleId, OscParams.Cent); }
			set 
			{
				Ctrl.SetParameter(ModuleId, OscParams.Cent, (int)value); 
				NotifyChange(() => Cent); 
			}
		}

		public double Position
		{
			get { return (double)Ctrl.GetParameter(ModuleId, OscParams.Position); }
			set
			{
				Ctrl.SetParameter(ModuleId, OscParams.Position, value);
				NotifyChange(() => Position);
				NotifyChange(() => WavetableData);
			}
		}

		public double Phase
		{
			get { return (double)Ctrl.GetParameter(ModuleId, OscParams.Phase); }
			set
			{
				Ctrl.SetParameter(ModuleId, OscParams.Phase, value);
				NotifyChange(() => Phase);
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
	}
}
