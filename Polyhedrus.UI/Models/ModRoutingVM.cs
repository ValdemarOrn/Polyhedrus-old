using Polyhedrus.Modules;
using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.UI.Models
{
	/// <summary>
	/// Control wrapper for ModRouting, because WPF needs properties, not fields
	/// </summary>
	public class ModRoutingVM
	{
		/// <summary>
		/// ModRoute struct that gets sent to the synthController whenever a change occurs
		/// </summary>
		public ModRoute Model;
		public SynthController Ctrl;
		public ModuleParams ModuleId;

		public ModRoutingVM(SynthController ctrl, ModuleParams moduleId)
		{
			Model = new ModRoute();
			Ctrl = ctrl;
			ModuleId = moduleId;
		}

		public ModSource Source 
		{
			get 
			{ 
				return Model.Source;
			} 
			set 
			{
				Model.Source = value;
				Ctrl.SetParameter(ModuleId, (ModMatrixParams)(Model.Index + 1), Model);
			} 
		}

		public ModDestination Destination
		{
			get
			{
				return Model.Destination;
			}
			set
			{
				Model.Destination = value;
				Ctrl.SetParameter(ModuleId, (ModMatrixParams)(Model.Index + 1), Model);
			}
		}


		public ModSource Control
		{
			get
			{
				return Model.Control;
			}
			set
			{
				Model.Control = value;
				Ctrl.SetParameter(ModuleId, (ModMatrixParams)(Model.Index + 1), Model);
			}
		}


		public double ControlAmount
		{
			get
			{
				return Model.ControlAmount;
			}
			set
			{
				Model.ControlAmount = value;
				Ctrl.SetParameter(ModuleId, (ModMatrixParams)(Model.Index + 1), Model);
			}
		}

		public double Amount
		{
			get
			{
				return Model.Amount;
			}
			set
			{
				Model.Amount = value;
				Ctrl.SetParameter(ModuleId, (ModMatrixParams)(Model.Index + 1), Model);
			}
		}


		public bool Enabled
		{
			get
			{
				return Model.Enabled;
			}
			set
			{
				Model.Enabled = value;
				Ctrl.SetParameter(ModuleId, (ModMatrixParams)(Model.Index + 1), Model);
			}
		}

	}
}
