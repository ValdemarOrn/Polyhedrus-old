using Polyhedrus.Modules;
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
			OscViews = new ObservableCollection<Control>();
			AmpEnvViews = new ObservableCollection<Control>();
			InsertEffectViews = new ObservableCollection<Control>();
			FilterViews = new ObservableCollection<Control>();
			FilterEnvViews = new ObservableCollection<Control>();
			RoutingViews = new ObservableCollection<Control>();
			ModulatorViews = new ObservableCollection<Control>();

			OscNames = new ObservableCollection<string>(new string[] { "Osc 1", "Osc 2", "Osc 3", "Osc 4" });
			AmpEnvNames = new ObservableCollection<string>(new string[] { "Amp Env" });
			InsertEffectNames = new ObservableCollection<string>(new string[] { "Insert 1", "Insert 2" });
			FilterNames = new ObservableCollection<string>(new string[] { "Filter 1", "Filter 2" });
			FilterEnvNames = new ObservableCollection<string>(new string[] { "Filter 1 Envelope", "Filter 2 Envelope" });
			RoutingNames = new ObservableCollection<string>(new string[] { "Routing", "Modulation" });
			SettingsNames = new ObservableCollection<string>(new string[] { "Settings" });
			EffectNames = new ObservableCollection<string>(new string[] { "FX 1", "FX 2", "FX 3", "FX 4" });
			ModulatorNames = new ObservableCollection<string>(new string[] { "Modulator 1", "Modulator 2", "Modulator 3", "Modulator 4", "Modulator 5", "Modulator 6" });

			OscSelectors = new ObservableCollection<string>(new string[] { "1", "2", "3", "4" });
			AmpEnvSelectors = new ObservableCollection<string>(new string[] { "" });
			InsertEffectSelectors = new ObservableCollection<string>(new string[] { "1", "2" });
			FilterSelectors = new ObservableCollection<string>(new string[] { "1", "2" });
			FilterEnvSelectors = new ObservableCollection<string>(new string[] { "1", "2" });
			RoutingSelectors = new ObservableCollection<string>(new string[] { "Routing", "Modulation" });
			SettingsSelectors = new ObservableCollection<string>(new string[] { "" });
			EffectsSelectors = new ObservableCollection<string>(new string[] { "1", "2", "3", "4" });
			ModulatorSelectors = new ObservableCollection<string>(new string[] { "1", "2", "3", "4", "5", "6" });

			SetViews();
		}

		#region Properties

		public ObservableCollection<string> OscNames { get; private set; }
		public ObservableCollection<string> AmpEnvNames { get; private set; }
		public ObservableCollection<string> InsertEffectNames { get; private set; }
		public ObservableCollection<string> FilterNames { get; private set; }
		public ObservableCollection<string> FilterEnvNames { get; private set; }
		public ObservableCollection<string> RoutingNames { get; private set; }
		public ObservableCollection<string> SettingsNames { get; private set; }
		public ObservableCollection<string> EffectNames { get; private set; }
		public ObservableCollection<string> ModulatorNames { get; private set; }

		public ObservableCollection<string> OscSelectors { get; private set; }
		public ObservableCollection<string> AmpEnvSelectors { get; private set; }
		public ObservableCollection<string> InsertEffectSelectors { get; private set; }
		public ObservableCollection<string> FilterSelectors { get; private set; }
		public ObservableCollection<string> FilterEnvSelectors { get; private set; }
		public ObservableCollection<string> RoutingSelectors { get; private set; }
		public ObservableCollection<string> SettingsSelectors { get; private set; }
		public ObservableCollection<string> EffectsSelectors { get; private set; }
		public ObservableCollection<string> ModulatorSelectors { get; private set; }

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

		private ObservableCollection<Control> _insertEffectViews;
		public ObservableCollection<Control> InsertEffectViews
		{
			get { return _insertEffectViews; }
			set { _insertEffectViews = value; NotifyChange(() => InsertEffectViews); }
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

		private string announcerCaption;
		public string AnnouncerCaption
		{
			get { return announcerCaption; }
			set { announcerCaption = value; NotifyChange(() => AnnouncerCaption); }
		}

		private string announcerValue;
		public string AnnouncerValue
		{
			get { return announcerValue; }
			set { announcerValue = value; NotifyChange(() => AnnouncerValue); }
		}

		#endregion

		private void SetViews()
		{
			var types = Ctrl.ModuleTypes;
			Func<ModuleId, SynthModuleView> findView = x => ViewProvider.GetView(types[x], Ctrl, x);

			
			OscViews.Add(findView(ModuleId.Osc1));
			OscViews.Add(findView(ModuleId.Osc2));
			OscViews.Add(findView(ModuleId.Osc3));
			OscViews.Add(findView(ModuleId.Osc4));

			AmpEnvViews.Add(findView(ModuleId.AmpEnv));
			(AmpEnvViews[0] as EnvelopeView).DelayVisibility = System.Windows.Visibility.Collapsed;

			InsertEffectViews.Add(findView(ModuleId.Insert1));
			InsertEffectViews.Add(findView(ModuleId.Insert2));

			FilterViews.Add(findView(ModuleId.Filter1));
			FilterViews.Add(findView(ModuleId.Filter2));

			FilterEnvViews.Add(findView(ModuleId.Filter1Env));
			FilterEnvViews.Add(findView(ModuleId.Filter2Env));

			RoutingViews.Add(findView(ModuleId.Mixer));
			RoutingViews.Add(findView(ModuleId.ModMatrix));

			ModulatorViews.Add(findView(ModuleId.Modulator1));
			ModulatorViews.Add(findView(ModuleId.Modulator2));
			ModulatorViews.Add(findView(ModuleId.Modulator3));
			ModulatorViews.Add(findView(ModuleId.Modulator4));
			ModulatorViews.Add(findView(ModuleId.Modulator5));
			ModulatorViews.Add(findView(ModuleId.Modulator6));
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
