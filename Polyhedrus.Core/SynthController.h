#ifndef SYNTH_CONTROLLER
#define SYNTH_CONTROLLER

#include<list>

class SynthController
{
public:
	void* Voices;
	std::list<void*> Notes;

	void Test();
};

#endif