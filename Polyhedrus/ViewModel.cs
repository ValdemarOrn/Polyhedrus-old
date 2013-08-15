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
	class ViewModel : INotifyPropertyChanged
	{
		SynthController Ctrl;

		SynthModuleView _osc1View;
		SynthModuleView _ampEnvView;
		SynthModuleView _filterView;

		ObservableCollection<string> _oscNames;

		public ViewModel(SynthController ctrl)
		{
			Ctrl = ctrl;
			Init();
		}

		public void Init()
		{
			_osc1View = new BLOscView(Ctrl, ModuleParams.Osc1);
			_ampEnvView = new EnvelopeView(Ctrl, ModuleParams.AmpEnv);
			_filterView = new CascadeFilterView(Ctrl, ModuleParams.Filter1);
			OscNames = new ObservableCollection<string>(new string[] { "1", "2", "3", "4" });
		}

		public ObservableCollection<string> OscNames
		{
			get { return _oscNames; }
			set { _oscNames = value; NotifyChange(() => OscNames); }
		}

		public SynthModuleView OscView
		{
			get { return _osc1View; }
			set { _osc1View = value; NotifyChange(() => OscView); }
		}

		public SynthModuleView AmpEnvView
		{
			get { return _ampEnvView; }
			set { _ampEnvView = value; NotifyChange(() => AmpEnvView); }
		}

		public SynthModuleView FilterView
		{
			get { return _filterView; }
			set { _filterView = value; NotifyChange(() => FilterView); }
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
