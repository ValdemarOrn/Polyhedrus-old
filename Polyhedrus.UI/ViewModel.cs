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
		SynthController ctrl;

		public ViewModel(SynthController ctrl)
		{
			this.ctrl = ctrl;
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

			OscNames = new [] { "Osc 1", "Osc 2", "Osc 3", "Osc 4" };
			AmpEnvNames = new [] { "Amp Env" };
			InsertEffectNames = new [] { "Insert 1", "Insert 2" };
			FilterNames = new [] { "Filter 1", "Filter 2" };
			FilterEnvNames = new [] { "Filter 1 Envelope", "Filter 2 Envelope" };
			RoutingNames = new [] { "Routing", "Modulation" };
			SettingsNames = new [] { "Settings" };
			EffectNames = new [] { "FX 1", "FX 2", "FX 3", "FX 4" };
			ModulatorNames = new [] { "Modulator 1", "Modulator 2", "Modulator 3", "Modulator 4", "Modulator 5", "Modulator 6" };
			
			OscSelectors = new [] { "1", "2", "3", "4" };
			AmpEnvSelectors = new [] { "" };
			InsertEffectSelectors = new [] { "1", "2" };
			FilterSelectors = new [] { "1", "2" };
			FilterEnvSelectors = new [] { "1", "2" };
			RoutingSelectors = new [] { "Routing", "Modulation" };
			SettingsSelectors = new [] { "" };
			EffectsSelectors = new [] { "1", "2", "3", "4" };
			ModulatorSelectors = new [] { "1", "2", "3", "4", "5", "6" };

			InsertEffectOptions = ModuleType.InsertEffectTypes.Select(ModuleType.GetName).ToArray();
			OscillatorOptions = ModuleType.OscillatorTypes.Select(ModuleType.GetName).ToArray();
			FilterOptions = ModuleType.FilterTypes.Select(ModuleType.GetName).ToArray();

			SetViews();
		}

		#region Properties

		public string SelectedInsertEffectOption
		{
			get 
			{
				var module = (ModuleId)((int)ModuleId.Insert1 + SelectedInsertEffect);
				var type = ctrl.ModuleTypes[module];
				return ModuleType.GetName(type);
			} 
			set
			{
				var module = (ModuleId)((int)ModuleId.Insert1 + SelectedInsertEffect);
				var type = ModuleType.GetType(value);
				if (type == null)
					return;

				ctrl.SetModuleType(module, type);
				InsertEffectViews[SelectedInsertEffect] = ViewProvider.GetView(type, ctrl, module);
				NotifyChange(() => SelectedInsertEffectOption);
			}
		}

		public string SelectedOscillatorOption
		{
			get
			{
				var module = (ModuleId)((int)ModuleId.Osc1 + SelectedOsc);
				var type = ctrl.ModuleTypes[module];
				return ModuleType.GetName(type);
			}
			set
			{
				var module = (ModuleId)((int)ModuleId.Osc1 + SelectedOsc);
				var type = ModuleType.GetType(value);
				if (type == null)
					return;

				ctrl.SetModuleType(module, type);
				OscViews[SelectedOsc] = ViewProvider.GetView(type, ctrl, module);
				NotifyChange(() => SelectedOscillatorOption);
			}
		}

		public string SelectedFilterOption
		{
			get
			{
				var module = (ModuleId)((int)ModuleId.Filter1 + SelectedFilter);
				var type = ctrl.ModuleTypes[module];
				return ModuleType.GetName(type);
			}
			set
			{
				var module = (ModuleId)((int)ModuleId.Filter1 + SelectedFilter);
				var type = ModuleType.GetType(value);
				if (type == null)
					return;

				ctrl.SetModuleType(module, type);
				FilterViews[SelectedFilter] = ViewProvider.GetView(type, ctrl, module);
				NotifyChange(() => SelectedFilterOption);
			}
		}

		public string[] OscNames { get; private set; }
		public string[] AmpEnvNames { get; private set; }
		public string[] InsertEffectNames { get; private set; }
		public string[] FilterNames { get; private set; }
		public string[] FilterEnvNames { get; private set; }
		public string[] RoutingNames { get; private set; }
		public string[] SettingsNames { get; private set; }
		public string[] EffectNames { get; private set; }
		public string[] ModulatorNames { get; private set; }

		public string[] OscSelectors { get; private set; }
		public string[] AmpEnvSelectors { get; private set; }
		public string[] InsertEffectSelectors { get; private set; }
		public string[] FilterSelectors { get; private set; }
		public string[] FilterEnvSelectors { get; private set; }
		public string[] RoutingSelectors { get; private set; }
		public string[] SettingsSelectors { get; private set; }
		public string[] EffectsSelectors { get; private set; }
		public string[] ModulatorSelectors { get; private set; }

		public string[] InsertEffectOptions { get; private set; }
		public string[] OscillatorOptions { get; private set; }
		public string[] FilterOptions { get; private set; }

		private ObservableCollection<Control> oscViews;
		public ObservableCollection<Control> OscViews
		{
			get { return oscViews; }
			set { oscViews = value; NotifyChange(() => OscViews); }
		}

		private ObservableCollection<Control> ampEnvViews;
		public ObservableCollection<Control> AmpEnvViews
		{
			get { return ampEnvViews; }
			set { ampEnvViews = value; NotifyChange(() => AmpEnvViews); }
		}

		private ObservableCollection<Control> insertEffectViews;
		public ObservableCollection<Control> InsertEffectViews
		{
			get { return insertEffectViews; }
			set { insertEffectViews = value; NotifyChange(() => InsertEffectViews); }
		}

		private ObservableCollection<Control> filterViews;
		public ObservableCollection<Control> FilterViews
		{
			get { return filterViews; }
			set { filterViews = value; NotifyChange(() => FilterViews); }
		}

		private ObservableCollection<Control> filterEnvViews;
		public ObservableCollection<Control> FilterEnvViews
		{
			get { return filterEnvViews; }
			set { filterEnvViews = value; NotifyChange(() => FilterEnvViews); }
		}

		private ObservableCollection<Control> routingViews;
		public ObservableCollection<Control> RoutingViews
		{
			get { return routingViews; }
			set { routingViews = value; NotifyChange(() => RoutingViews); }
		}

		private ObservableCollection<Control> modulatorViews;
		public ObservableCollection<Control> ModulatorViews
		{
			get { return modulatorViews; }
			set { modulatorViews = value; NotifyChange(() => ModulatorViews); }
		}

		private int selectedOsc;
		public int SelectedOsc
		{
			get { return selectedOsc; }
			set 
			{
				selectedOsc = value; 
				NotifyChange(() => SelectedOsc);
				NotifyChange(() => SelectedOscillatorOption);
			}
		}

		private int selectedFilter;
		public int SelectedFilter
		{
			get { return selectedFilter; }
			set
			{
				selectedFilter = value;
				NotifyChange(() => SelectedFilter);
			}
		}

		private int selectedInsertEffect;
		public int SelectedInsertEffect
		{
			get { return selectedInsertEffect; }
			set
			{
				selectedInsertEffect = value;
				NotifyChange(() => SelectedInsertEffect);
				NotifyChange(() => SelectedInsertEffectOption);
			}
		}

		private int selectedModulator;
		public int SelectedModulator
		{
			get { return selectedModulator; }
			set
			{
				selectedModulator = value;
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
			Func<ModuleId, SynthModuleView> findView = x => ViewProvider.GetView(ctrl.ModuleTypes[x], ctrl, x);

			OscViews.Add(findView(ModuleId.Osc1));
			OscViews.Add(findView(ModuleId.Osc2));
			OscViews.Add(findView(ModuleId.Osc3));
			OscViews.Add(findView(ModuleId.Osc4));

			AmpEnvViews.Add(findView(ModuleId.AmpEnv));
			((EnvelopeView)AmpEnvViews[0]).DelayVisibility = System.Windows.Visibility.Collapsed;

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
