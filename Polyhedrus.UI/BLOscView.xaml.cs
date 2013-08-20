using Polyhedrus.Parameters;
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
		private double _octave;
		private double _semi;
		private double _cent;
		private double _position;
		private double _phase;
		private double _volume;

		public BLOscView()
		{
			InitializeComponent();
		}

		public BLOscView(SynthController ctrl, ModuleParams moduleId) : this()
		{
			Ctrl = ctrl;
			ModuleId = moduleId;
		}

		public double Octave
		{
			get { return _octave; }
			set 
			{
				_octave = value; 
				Ctrl.SetParameter(ModuleId, OscParams.Octave, value); 
				NotifyChange(() => Octave); 
			}
		}

		public double Semi
		{
			get { return _semi; }
			set 
			{
				_semi = value;
				Ctrl.SetParameter(ModuleId, OscParams.Semi, value); 
				NotifyChange(() => Semi);
			}
		}

		public double Cent
		{
			get { return _cent; }
			set 
			{
				_cent = value;
				Ctrl.SetParameter(ModuleId, OscParams.Cent, value); 
				NotifyChange(() => Cent); 
			}
		}

		public double Position
		{
			get { return _position; }
			set
			{
				_position = value;
				Ctrl.SetParameter(ModuleId, OscParams.Position, value);
				NotifyChange(() => Position);
			}
		}

		public double Phase
		{
			get { return _phase; }
			set
			{
				_phase = value;
				Ctrl.SetParameter(ModuleId, OscParams.Phase, value);
				NotifyChange(() => Phase);
			}
		}

		public double Volume
		{
			get { return _volume; }
			set
			{
				_volume = value;
				Ctrl.SetParameter(ModuleId, OscParams.Volume, value);
				NotifyChange(() => Volume);
			}
		}
	}
}
