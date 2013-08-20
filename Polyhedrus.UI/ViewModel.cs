using Polyhedrus.Parameters;
using Polyhedrus.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public class ViewModel : INotifyPropertyChanged
	{
		SynthController Ctrl;

		SynthModuleView _osc1View;
		SynthModuleView _osc2View;
		SynthModuleView _osc3View;
		SynthModuleView _osc4View;
		SynthModuleView _filter1View;
		SynthModuleView _filter2View;
		SynthModuleView _ampEnvView;
		SynthModuleView _filter1EnvView;
		SynthModuleView _filter2EnvView;
		SynthModuleView _modulator1View;
		SynthModuleView _modulator2View;
		SynthModuleView _modulator3View;
		SynthModuleView _modulator4View;
		SynthModuleView _modulator5View;
		SynthModuleView _modulator6View;
		SynthModuleView _modMatrixView;
		
		int _selectedOsc;
		int _selectedModulator;

		ObservableCollection<string> _oscNames;
		ObservableCollection<string> _modulatorNames;

		public ViewModel(SynthController ctrl)
		{
			Ctrl = ctrl;
			Init();
		}

		public void Init()
		{
			_osc1View = new BLOscView(Ctrl, ModuleParams.Osc1);
			_osc2View = new BLOscView(Ctrl, ModuleParams.Osc2);
			_osc3View = new BLOscView(Ctrl, ModuleParams.Osc3);
			_osc4View = new BLOscView(Ctrl, ModuleParams.Osc4);
			_filter1View = new CascadeFilterView(Ctrl, ModuleParams.Filter1);
			_filter2View = new CascadeFilterView(Ctrl, ModuleParams.Filter2);
			_ampEnvView = new EnvelopeView(Ctrl, ModuleParams.AmpEnv);
			_filter1EnvView = new EnvelopeView(Ctrl, ModuleParams.Filter1Env);
			_filter2EnvView = new EnvelopeView(Ctrl, ModuleParams.Filter2Env);
			_modMatrixView = new ModMatrixView(Ctrl, ModuleParams.ModMatrix);
			_modulator1View = new ModulatorView(Ctrl, ModuleParams.Modulator1);
			_modulator2View = new ModulatorView(Ctrl, ModuleParams.Modulator2);
			_modulator3View = new ModulatorView(Ctrl, ModuleParams.Modulator3);
			_modulator4View = new ModulatorView(Ctrl, ModuleParams.Modulator4);
			_modulator5View = new ModulatorView(Ctrl, ModuleParams.Modulator5);
			_modulator6View = new ModulatorView(Ctrl, ModuleParams.Modulator6);

			OscNames = new ObservableCollection<string>(new string[] { "1", "2", "3", "4", null, null });
			ModulatorNames = new ObservableCollection<string>(new string[] { "1", "2", "3", "4", "5", "6" });
		}

		public ObservableCollection<string> OscNames
		{
			get { return _oscNames; }
			set { _oscNames = value; NotifyChange(() => OscNames); }
		}

		public ObservableCollection<string> ModulatorNames
		{
			get { return _modulatorNames; }
			set { _modulatorNames = value; NotifyChange(() => ModulatorNames); }
		}

		public SynthModuleView OscView
		{
			get 
			{
				switch(SelectedOsc)
				{
					case 0:
						return _osc1View;
					case 1:
						return _osc2View;
					case 2:
						return _osc3View;
					case 3:
						return _osc4View;
					default:
						return null;
				}
			}
		}

		public SynthModuleView AmpEnvView
		{
			get { return _ampEnvView; }
			set { _ampEnvView = value; NotifyChange(() => AmpEnvView); }
		}

		public SynthModuleView FilterView
		{
			get { return _filter1View; }
			set { _filter1View = value; NotifyChange(() => FilterView); }
		}

		public SynthModuleView FilterEnvView
		{
			get { return _filter1EnvView; }
			set { _filter1EnvView = value; NotifyChange(() => FilterEnvView); }
		}

		public SynthModuleView RoutingView
		{
			get { return _modMatrixView; }
			set { _modMatrixView = value; NotifyChange(() => RoutingView); }
		}

		public SynthModuleView ModulatorView
		{
			get
			{
				switch (SelectedModulator)
				{
					case 0:
						return _modulator1View;
					case 1:
						return _modulator2View;
					case 2:
						return _modulator3View;
					case 3:
						return _modulator4View;
					case 4:
						return _modulator5View;
					case 5:
						return _modulator6View;
					default:
						return null;
				}
			}
		}
		
		public int SelectedOsc
		{
			get { return _selectedOsc; }
			set 
			{ 
				_selectedOsc = value; 
				NotifyChange(() => SelectedOsc); 
				NotifyChange(() => SelectedOscName);
				NotifyChange(() => OscView); 
			}
		}

		public int SelectedModulator
		{
			get { return _selectedModulator; }
			set
			{
				_selectedModulator = value;
				NotifyChange(() => SelectedModulator);
				NotifyChange(() => SelectedModulatorName);
				NotifyChange(() => ModulatorView);
			}
		}

		public string SelectedOscName
		{ 
			get { return "Oscillator " + (_selectedOsc + 1); }
		}

		public string SelectedModulatorName
		{
			get { return "Modulator " + (_selectedModulator + 1); }
		}

		#region Notify Change

		public event PropertyChangedEventHandler PropertyChanged;

		// I used this and GetPropertyName to avoid having to hard-code property names
		// into the NotifyChange events. This makes the application much easier to refactor
		// leter on, if needed.
		private void NotifyChange<T>(System.Linq.Expressions.Expression<Func<T>> exp)
		{
			var name = GetPropertyName(exp);
			NotifyChange(name);
		}

		private void NotifyChange(string property)
		{
			if (PropertyChanged != null)
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
		}

		private static string GetPropertyName<T>(System.Linq.Expressions.Expression<Func<T>> exp)
		{
			return (((System.Linq.Expressions.MemberExpression)(exp.Body)).Member).Name;
		}

		#endregion
	}
}
