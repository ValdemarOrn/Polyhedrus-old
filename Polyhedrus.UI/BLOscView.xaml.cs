using Polyhedrus.Parameters;
using Polyhedrus.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
	/// Interaction logic for Oscillator.xaml
	/// </summary>
	public partial class BLOscView : SynthModuleView
	{
		public BLOscView()
		{
			InitializeComponent();
		}

		public BLOscView(SynthController ctrl, ModuleParams moduleId)
		{
			Ctrl = ctrl;
			ModuleId = moduleId;
			Wavetables = Ctrl.Wavetables.ToArray();

			InitializeComponent();

			KnobOctave.ValueFormatter = Formatter;
			KnobSemi.ValueFormatter = Formatter;
			KnobCent.ValueFormatter = Formatter;
		}

		string Formatter(double val)
		{
			return val.ToString("+0;-0");
		}

		IEnumerable<string> _wavetables;
		public IEnumerable<string> Wavetables
		{
			get { return _wavetables; }
			set
			{
				_wavetables = value;
				NotifyChange(() => Wavetables);
			}
		}

		string _selectedWavetable;
		public string SelectedWavetable
		{
			get
			{
				return Ctrl.GetWavetable(ModuleId).Name;
			}
			set
			{
				_selectedWavetable = value;

				new System.Threading.Thread(() =>
				{
					Ctrl.SetWavetable(ModuleId, value);
					Action update = () =>
					{
						NotifyChange(() => SelectedWavetable);
						NotifyChange(() => WavetableData);
					};

					this.Dispatcher.Invoke(update);

				}).Start();
			}
		}

		public IEnumerable<double> WavetableData
		{
			get { return Ctrl.GetWavetable(ModuleId).Data[(int)Position]; }
		}
	

		public double Octave
		{
			get { return (double)(int)Ctrl.GetParameter(ModuleId, OscParams.Octave); }
			set 
			{
				Ctrl.SetParameter(ModuleId, OscParams.Octave, (int)value); 
				NotifyChange(() => Octave); 
			}
		}

		public double Semi
		{
			get { return (double)(int)Ctrl.GetParameter(ModuleId, OscParams.Semi); }
			set 
			{
				Ctrl.SetParameter(ModuleId, OscParams.Semi, (int)value); 
				NotifyChange(() => Semi);
			}
		}

		public double Cent
		{
			get { return (double)(int)Ctrl.GetParameter(ModuleId, OscParams.Cent); }
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
