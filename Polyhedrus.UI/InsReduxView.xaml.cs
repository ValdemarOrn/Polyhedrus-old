using AudioLib;
using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
using System.Globalization;
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
	[ViewProviderFor(typeof(Modules.InsRedux))]
	public partial class InsReduxView : SynthModuleView
	{
		public InsReduxView() : base(null, (ModuleId)0)
		{
			InitializeComponent();
		}

		public InsReduxView(SynthController ctrl, ModuleId moduleId) : base(ctrl, moduleId)
		{
			InitializeComponent();
		}

		public double Reduce
		{
			get { return (double)Ctrl.GetParameter(ModuleId, InsertParams.Param1); }
			set
			{
				Ctrl.SetParameter(ModuleId, InsertParams.Param1, value);
				NotifyChange(() => Reduce);
			}
		}

		public double Bits
		{
			get { return (double)Ctrl.GetParameter(ModuleId, InsertParams.Param2); }
			set
			{
				Ctrl.SetParameter(ModuleId, InsertParams.Param2, value);
				NotifyChange(() => Bits);
			}
		}
	}
}
