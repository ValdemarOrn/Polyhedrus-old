﻿using Polyhedrus.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public struct ParameterKey
	{
		public ModuleId Module;
		public Enum Parameter;

		public int Key
		{
			get
			{
				return (Convert.ToInt32(Module) << 16) | Convert.ToInt32(Parameter);
			}
		}

		public ParameterKey(ModuleId module, Enum parameter)
		{
			Module = module;
			Parameter = parameter;
		}

		public override int GetHashCode()
		{
			return Key;
		}

		public override bool Equals(object obj)
		{
			return (obj is ParameterKey) && Key == ((ParameterKey)obj).Key;
		}

		public override string ToString()
		{
			return Module.ToString() + " " + Parameter.ToString();
		}
	}
}
