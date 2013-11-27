using Polyhedrus.Parameters;
using Polyhedrus.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Polyhedrus
{
	public class ViewModel : INotifyPropertyChanged
	{
		SynthController Ctrl;

		public ViewModel(SynthController ctrl)
		{
			Ctrl = ctrl;
			Init();
		}

		public void Init()
		{
			OscViews = new ObservableCollection<Control>(new []
			{
				new BLOscView(Ctrl, ModuleParams.Osc1),
				new BLOscView(Ctrl, ModuleParams.Osc2),
				new BLOscView(Ctrl, ModuleParams.Osc3),
				new BLOscView(Ctrl, ModuleParams.Osc4)
			});

			FilterViews = new ObservableCollection<Control>(new []
			{
				new CascadeFilterView(Ctrl, ModuleParams.Filter1),
				new CascadeFilterView(Ctrl, ModuleParams.Filter2)
			});

			AmpEnvViews = new ObservableCollection<Control>(new []
			{
				new EnvelopeView(Ctrl, ModuleParams.AmpEnv)
			});

			FilterEnvViews = new ObservableCollection<Control>(new []
			{
				new EnvelopeView(Ctrl, ModuleParams.Filter1Env),
				new EnvelopeView(Ctrl, ModuleParams.Filter2Env)
			});

			RoutingViews = new ObservableCollection<Control>(new Control[]
			{
				new MixerView(Ctrl, ModuleParams.Mixer),
				new ModMatrixView(Ctrl, ModuleParams.ModMatrix)
			});

			ModulatorViews = new ObservableCollection<Control>(new[]
			{
				new ModulatorView(Ctrl, ModuleParams.Modulator1),
				new ModulatorView(Ctrl, ModuleParams.Modulator2),
				new ModulatorView(Ctrl, ModuleParams.Modulator3),
				new ModulatorView(Ctrl, ModuleParams.Modulator4),
				new ModulatorView(Ctrl, ModuleParams.Modulator5),
				new ModulatorView(Ctrl, ModuleParams.Modulator6)
			});

			OscNames = new ObservableCollection<string>(new string[] { "Osc 1", "Osc 2", "Osc 3", "Osc 4" });
			AmpEnvNames = new ObservableCollection<string>(new string[] { "Amp Env" });
			InsertFXNames = new ObservableCollection<string>(new string[] { "Insert 1", "Insert 2" });
			FilterNames = new ObservableCollection<string>(new string[] { "Filter 1", "Filter 2" });
			FilterEnvNames = new ObservableCollection<string>(new string[] { "Filter 1 Envelope", "Filter 2 Envelope" });
			RoutingNames = new ObservableCollection<string>(new string[] { "Routing", "Modulation" });
			SettingsNames = new ObservableCollection<string>(new string[] { "Settings" });
			EffectNames = new ObservableCollection<string>(new string[] { "FX 1", "FX 2", "FX 3", "FX 4" });
			ModulatorNames = new ObservableCollection<string>(new string[] { "Mod 1", "Mod 2", "Mod 3", "Mod 4", "Mod 5", "Mod 6" });

			OscSelectors = new ObservableCollection<string>(new string[] { "1", "2", "3", "4" });
			AmpEnvSelectors = new ObservableCollection<string>(new string[] { "" });
			InsertSelectors = new ObservableCollection<string>(new string[] { "1", "2" });
			FilterSelectors = new ObservableCollection<string>(new string[] { "1", "2" });
			FilterEnvSelectors = new ObservableCollection<string>(new string[] { "1", "2" });
			RoutingSelectors = new ObservableCollection<string>(new string[] { "Routing", "Modulation" });
			SettingsSelectors = new ObservableCollection<string>(new string[] { "" });
			EffectsSelectors = new ObservableCollection<string>(new string[] { "1", "2", "3", "4" });
			ModulatorSelectors = new ObservableCollection<string>(new string[] { "1", "2", "3", "4", "5", "6" });
		}

		public ObservableCollection<string> OscNames { get; set; }
		public ObservableCollection<string> AmpEnvNames { get; set; }
		public ObservableCollection<string> InsertFXNames { get; set; }
		public ObservableCollection<string> FilterNames { get; set; }
		public ObservableCollection<string> FilterEnvNames { get; set; }
		public ObservableCollection<string> RoutingNames { get; set; }
		public ObservableCollection<string> SettingsNames { get; set; }
		public ObservableCollection<string> EffectNames { get; set; }
		public ObservableCollection<string> ModulatorNames { get; set; }

		public ObservableCollection<string> OscSelectors { get; set; }
		public ObservableCollection<string> AmpEnvSelectors { get; set; }
		public ObservableCollection<string> InsertSelectors { get; set; }
		public ObservableCollection<string> FilterSelectors { get; set; }
		public ObservableCollection<string> FilterEnvSelectors { get; set; }
		public ObservableCollection<string> RoutingSelectors { get; set; }
		public ObservableCollection<string> SettingsSelectors { get; set; }
		public ObservableCollection<string> EffectsSelectors { get; set; }
		public ObservableCollection<string> ModulatorSelectors { get; set; }

		private ObservableCollection<Control> _oscViews;
		public ObservableCollection<Control> OscViews
		{
			get { return _oscViews; }
			set { _oscViews = value; NotifyChange(() => OscViews); }
		}

		private ObservableCollection<Control> _ampEnvViews;
		public ObservableCollection<Control> AmpEnvViews
		{
			get { return _ampEnvViews; }
			set { _ampEnvViews = value; NotifyChange(() => AmpEnvViews); }
		}

		private ObservableCollection<Control> _filterViews;
		public ObservableCollection<Control> FilterViews
		{
			get { return _filterViews; }
			set { _filterViews = value; NotifyChange(() => FilterViews); }
		}

		private ObservableCollection<Control> _filterEnvViews;
		public ObservableCollection<Control> FilterEnvViews
		{
			get { return _filterEnvViews; }
			set { _filterEnvViews = value; NotifyChange(() => FilterEnvViews); }
		}

		private ObservableCollection<Control> _routingViews;
		public ObservableCollection<Control> RoutingViews
		{
			get { return _routingViews; }
			set { _routingViews = value; NotifyChange(() => RoutingViews); }
		}

		private ObservableCollection<Control> _modulatorViews;
		public ObservableCollection<Control> ModulatorViews
		{
			get { return _modulatorViews; }
			set { _modulatorViews = value; NotifyChange(() => ModulatorViews); }
		}


		private int _selectedOsc;
		public int SelectedOsc
		{
			get { return _selectedOsc; }
			set 
			{
				_selectedOsc = value; 
				NotifyChange(() => SelectedOsc); 
			}
		}

		private int _selectedFilter;
		public int SelectedFilter
		{
			get { return _selectedFilter; }
			set
			{
				_selectedFilter = value;
				NotifyChange(() => SelectedFilter);
			}
		}

		private int _selectedModulator;
		public int SelectedModulator
		{
			get { return _selectedModulator; }
			set
			{
				_selectedModulator = value;
				NotifyChange(() => SelectedModulator);
			}
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
