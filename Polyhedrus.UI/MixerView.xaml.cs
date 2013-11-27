using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
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
	/// Interaction logic for MixerView.xaml
	/// </summary>
	public partial class MixerView : SynthModuleView
	{
		public MixerView()
		{
			InitializeComponent();
		}

		public MixerView(SynthController ctrl, ModuleParams moduleId)
		{
			Ctrl = ctrl;
			ModuleId = moduleId;

			InitializeComponent();
		}		
	}
}
