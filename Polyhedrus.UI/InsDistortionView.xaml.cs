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
	[ViewProviderFor(typeof(Modules.InsDistortion))]
	public partial class InsDistortionView : SynthModuleView
	{
		public InsDistortionView()
		{
			InitializeComponent();
		}

		public InsDistortionView(SynthController ctrl, ModuleId moduleId) : base(ctrl, moduleId)
		{
			InitializeComponent();
		}

		public double InGain
		{
			get { return (double)Ctrl.GetParameter(ModuleId, InsertParams.Param1); }
			set
			{
				Ctrl.SetParameter(ModuleId, InsertParams.Param1, value);
				NotifyChange(() => InGain);
			}
		}

		public double OutGain
		{
			get { return (double)Ctrl.GetParameter(ModuleId, InsertParams.Param2); }
			set
			{
				Ctrl.SetParameter(ModuleId, InsertParams.Param2, value);
				NotifyChange(() => OutGain);
			}
		}

		public double Bias
		{
			get { return (double)Ctrl.GetParameter(ModuleId, InsertParams.Param3); }
			set
			{
				Ctrl.SetParameter(ModuleId, InsertParams.Param3, value);
				NotifyChange(() => Bias);
			}
		}

		public double Mode
		{
			get { return (double)Ctrl.GetParameter(ModuleId, InsertParams.Param4); }
			set
			{
				Ctrl.SetParameter(ModuleId, InsertParams.Param4, value);
				NotifyChange(() => Mode);
			}
		}
	}
}
