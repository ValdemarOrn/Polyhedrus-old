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
	/// Interaction logic for Slider.xaml
	/// </summary>
	public partial class Slider : UserControl
	{
		public static readonly DependencyProperty FillProperty =
			DependencyProperty.Register("Fill", typeof(Brush), typeof(Slider), new PropertyMetadata(Brushes.Red));

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(double), typeof(Slider), new PropertyMetadata(0.0));

		public static readonly DependencyProperty SliderLengthProperty =
			DependencyProperty.Register("SliderLength", typeof(double), typeof(Slider), new PropertyMetadata(0.0));


		public Slider()
		{
			InitializeComponent();

			var prop = DependencyPropertyDescriptor.FromProperty(ValueProperty, this.GetType());
			prop.AddValueChanged(this, (s, e) => SliderLength = Value * ActualWidth);
			this.SizeChanged += (s, e) => SliderLength = Value * ActualWidth;
		}

		public Brush Fill
		{
			get { return (Brush)GetValue(FillProperty); }
			set { SetValue(FillProperty, value); }
		}

		public double Value
		{
			get { return (double)GetValue(ValueProperty); }
			set 
			{ 
				SetValue(ValueProperty, value);
				SliderLength = value * ActualWidth;
			}
		}

		protected double SliderLength
		{
			get { return (double)GetValue(SliderLengthProperty); }
			set { SetValue(SliderLengthProperty, value); }
		}

		
	}
}
