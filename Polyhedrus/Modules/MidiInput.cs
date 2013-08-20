using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus.Modules
{
	public sealed class MidiInput
	{
		public bool Gate;
		public double Velocity;
		public double Pitch; // note + pitchbend
		public double Note;
		public double PitchBend;
		public double ModWheel;
		public double Aftertouch;
		public double Spread; // used for unison
	}
}
