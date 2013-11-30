using Polyhedrus.Modules;
using Polyhedrus.Parameters;
using Polyhedrus.UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	public class ModSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ModMatrixView._sources.Single(x => x.Value == (ModSource)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ((ListItem<ModSource>)value).Value;
		}
	}

	public class ModDestinationConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ModMatrixView._destinations.Single(x => x.Value == (ModDestination)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ((ListItem<ModDestination>)value).Value;
		}
	}

	[ViewProviderFor(typeof(Modules.ModMatrix))]
	public partial class ModMatrixView : SynthModuleView
	{
		internal static ObservableCollection<Models.ListItem<ModSource>> _sources;
		internal static ObservableCollection<Models.ListItem<ModDestination>> _destinations;

		static ModMatrixView()
		{
			var sources = Mod.SourceNames.Select(x => new Models.ListItem<ModSource>(x.Key, x.Value)).ToArray();
			var destinations = Mod.DestNames.Select(x => new Models.ListItem<ModDestination>(x.Key, x.Value)).ToArray();

			_sources = new ObservableCollection<Models.ListItem<ModSource>>(sources);
			_destinations = new ObservableCollection<Models.ListItem<ModDestination>>(destinations);
		}

		ObservableCollection<ModRoutingVM> _routes;
		int _page;

		public ModMatrixView() : base(null, (ModuleId)0)
		{
			InitializeComponent();
		}

		public ModMatrixView(SynthController ctrl, ModuleId moduleId) : base(ctrl, moduleId)
		{
			var routes = new ObservableCollection<ModRoutingVM>(new ModRoutingVM[5]);
			for (int i = 0; i < routes.Count; i++)
			{
				routes[i] = new ModRoutingVM(Ctrl, moduleId);
				routes[i].Model.Index = i;
			}
			Routes = routes;

			InitializeComponent();
		}		

		public ObservableCollection<Models.ListItem<ModSource>> Sources
		{
			get { return _sources; }
			set { _sources = value; NotifyChange(() => Sources); }
		}

		public ObservableCollection<Models.ListItem<ModDestination>> Destinations
		{
			get { return _destinations; }
			set { _destinations = value; NotifyChange(() => Destinations); }
		}

		public ObservableCollection<ModRoutingVM> Routes
		{
			get { return _routes; }
			set { _routes = value; NotifyChange(() => Routes); }
		}

		public int Page
		{
			get { return _page; }
			set 
			{ 
				_page = value;
				UpdatePage();
				NotifyChange(() => Page);
				NotifyChange(() => Routes);
			}
		}

		void UpdatePage()
		{

		}

		private void PageClick(object sender, RoutedEventArgs e)
		{
			if (sender == Page1)
				Page = 0;
			else if (sender == Page2)
				Page = 1;
			else if (sender == Page3)
				Page = 2;
			else if (sender == Page4)
				Page = 3;
		}
	}
}
