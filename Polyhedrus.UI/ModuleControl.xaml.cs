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
	public partial class ModuleControl : UserControl, INotifyPropertyChanged
	{
		static internal DependencyProperty PanelsProperty = DependencyProperty.Register("Panels", typeof(ObservableCollection<Control>), typeof(ModuleControl),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		static internal DependencyProperty SelectorsProperty = DependencyProperty.Register("Selectors", typeof(ObservableCollection<string>), typeof(ModuleControl),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		static internal DependencyProperty TitlesProperty = DependencyProperty.Register("Titles", typeof(ObservableCollection<string>), typeof(ModuleControl),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		static internal DependencyProperty ShowComboBoxProperty = DependencyProperty.Register("ShowComboBox", typeof(Visibility), typeof(ModuleControl),
			new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		static internal DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(ModuleControl),
			new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public static readonly DependencyProperty TitleVisibilityProperty =
			DependencyProperty.Register("TitleVisibility", typeof(Visibility), typeof(ModuleControl), new PropertyMetadata(Visibility.Visible));

		public static readonly DependencyProperty AvailableOptionsProperty =
			DependencyProperty.Register("AvailableOptions", typeof(ObservableCollection<string>), typeof(ModuleControl), new PropertyMetadata(null));

		public static readonly DependencyProperty SelectedOptionProperty =
			DependencyProperty.Register("SelectedOption", typeof(string), typeof(ModuleControl), new PropertyMetadata(null));

		public ModuleControl()
		{
			InitializeComponent();

			var prop = DependencyPropertyDescriptor.FromProperty(PanelsProperty, this.GetType());
			prop.AddValueChanged(this, (s, e) => 
			{
				Panels.CollectionChanged += (a, b) => 
					NotifyChange(() => Panel);
				NotifyChange(() => Panel);
			});

			prop = DependencyPropertyDescriptor.FromProperty(SelectorsProperty, this.GetType());

			prop = DependencyPropertyDescriptor.FromProperty(TitlesProperty, this.GetType());
			prop.AddValueChanged(this, (s, e) => 
				NotifyChange(() => Title));

			prop = DependencyPropertyDescriptor.FromProperty(SelectedIndexProperty, this.GetType());
			prop.AddValueChanged(this, (s, e) => 
				NotifyChange(() => Title));
			prop.AddValueChanged(this, (s, e) => 
				NotifyChange(() => Panel));
		}

		public ObservableCollection<Control> Panels
		{
			get { return (ObservableCollection<Control>)base.GetValue(PanelsProperty); }
			set { SetValue(PanelsProperty, value); }
		}

		public ObservableCollection<string> Selectors
		{
			get { return (ObservableCollection<string>)base.GetValue(SelectorsProperty); }
			set { SetValue(SelectorsProperty, value); }
		}

		public ObservableCollection<string> Titles
		{
			get { return (ObservableCollection<string>)base.GetValue(TitlesProperty); }
			set { SetValue(TitlesProperty, value); }
		}

		public Visibility ShowComboBox
		{
			get { return (Visibility)base.GetValue(ShowComboBoxProperty); }
			set { SetValue(ShowComboBoxProperty, value); }
		}

		public int SelectedIndex
		{
			get { return (int)base.GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		public Visibility TitleVisibility
		{
			get { return (Visibility)GetValue(TitleVisibilityProperty); }
			set { SetValue(TitleVisibilityProperty, value); }
		}

		public ObservableCollection<string> AvailableOptions
		{
			get { return (ObservableCollection<string>)GetValue(AvailableOptionsProperty); }
			set { SetValue(AvailableOptionsProperty, value); }
		}

		public string SelectedOption
		{
			get { return (string)GetValue(SelectedOptionProperty); }
			set { SetValue(SelectedOptionProperty, value); }
		}

		public string Title
		{
			get { return Titles.Eval(x => x[SelectedIndex], ""); }
		}

		public Control Panel
		{
			get { return Panels.Eval(x => x[SelectedIndex], null); }
		}
	
		#region Notify Change

		public event PropertyChangedEventHandler PropertyChanged;

		// I used this and GetPropertyName to avoid having to hard-code property names
		// into the NotifyChange events. This makes the application much easier to refactor
		// leter on, if needed.
		protected void NotifyChange<T>(System.Linq.Expressions.Expression<Func<T>> exp)
		{
			var name = GetPropertyName(exp);
			NotifyChange(name);
		}

		protected void NotifyChange(string property)
		{
			if (PropertyChanged != null)
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
		}

		protected static string GetPropertyName<T>(System.Linq.Expressions.Expression<Func<T>> exp)
		{
			return (((System.Linq.Expressions.MemberExpression)(exp.Body)).Member).Name;
		}

		#endregion

		private void Label_MouseDown(object sender, MouseButtonEventArgs e)
		{
			SelectedIndex = Selectors.IndexOf((sender as Label).Content as string);
		}
	}
}
