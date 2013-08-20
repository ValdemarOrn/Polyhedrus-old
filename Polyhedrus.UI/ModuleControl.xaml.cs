using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	public class SelectorVisible : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var val = (value != null && value is string && ((string)value).Trim().Length > 0) ? Visibility.Visible : Visibility.Collapsed;
			return val;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Interaction logic for ModuleControl.xaml
	/// </summary>
	public partial class ModuleControl : UserControl
	{
		static internal DependencyProperty PanelProperty = DependencyProperty.Register("Panel", typeof(Control), typeof(ModuleControl),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		static internal DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ModuleControl),
			new FrameworkPropertyMetadata("Temp", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		static internal DependencyProperty ShowComboBoxProperty = DependencyProperty.Register("ShowComboBox", typeof(Visibility), typeof(ModuleControl),
			new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		static internal DependencyProperty SelectorsProperty = DependencyProperty.Register("Selectors", typeof(ObservableCollection<string>), typeof(ModuleControl),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		static internal DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(ModuleControl),
			new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public Control Panel
		{
			get { return (Control)base.GetValue(PanelProperty); }
			set { SetValue(PanelProperty, value); }
		}

		public string Title
		{
			get { return (string)base.GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public Visibility ShowComboBox
		{
			get { return (Visibility)base.GetValue(ShowComboBoxProperty); }
			set { SetValue(ShowComboBoxProperty, value); }
		}

		public ObservableCollection<string> Selectors
		{
			get { return (ObservableCollection<string>)base.GetValue(SelectorsProperty); }
			set { SetValue(SelectorsProperty, value); }
		}

		public int SelectedIndex
		{
			get { return (int)base.GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		public ModuleControl()
		{
			InitializeComponent();
			Selectors = new ObservableCollection<string>(new string[] { null, null, null, null, null, null });
		}

		private void Label_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (sender == Selector0)
				SelectedIndex = 0;
			else if (sender == Selector1)
				SelectedIndex = 1;
			else if (sender == Selector2)
				SelectedIndex = 2;
			else if (sender == Selector3)
				SelectedIndex = 3;
			else if (sender == Selector4)
				SelectedIndex = 4;
			else if (sender == Selector5)
				SelectedIndex = 5;
		}

	}
}
