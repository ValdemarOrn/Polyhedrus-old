using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public class MidiNote
	{
		public int Note;
		public int Velocity;

		public MidiNote(int note, int velocity)
		{
			Note = note;
			Velocity = velocity;
		}
	}
}
