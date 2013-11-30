using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Polyhedrus
{
	public class RealtimeDispatcher<T> : IDisposable
	{
		ManualResetEvent WorkCompleted;
		Semaphore Sema;
		Thread[] WorkerThreads;
		Action<T> Action;
		Queue<T> WorkItems;
		volatile int QueueLength;
		volatile bool Stopping;

		public int NumberOfWorkers { get; private set; }
		public int MaxQueueSize { get; private set; }
		public ThreadPriority WorkerPriority { get; private set; }

		public RealtimeDispatcher(int numberOfWorkers, int maxQueueSize, ThreadPriority workerPriority, Action<T> action)
		{
			WorkCompleted = new ManualResetEvent(false);
			Sema = new Semaphore(0, maxQueueSize);
			Action = action;
			WorkItems = new Queue<T>(maxQueueSize);
			NumberOfWorkers = numberOfWorkers;
			MaxQueueSize = maxQueueSize;
			WorkerPriority = workerPriority;
			Stopping = true;

			Start();
		}

		public void QueueWorkItems(T[] items)
		{
			lock (WorkItems)
			{
				WorkCompleted.Reset();

				for (int i = 0; i < items.Length; i++)
				{
					QueueLength++;
					WorkItems.Enqueue(items[i]);	
					Sema.Release();
				}
			}
		}

		public void QueueWorkItem(T item)
		{
			lock (WorkItems)
			{
				WorkCompleted.Reset();

				QueueLength++;
				WorkItems.Enqueue(item);
				Sema.Release();
			}
		}

		private void WorkLoop()
		{
			while (!Stopping)
			{
				Sema.WaitOne();

				if (Stopping)
					return;

				T item;

				lock (WorkItems)
				{
					item = WorkItems.Dequeue();
				}
				
				Action(item);

				int len = Interlocked.Add(ref QueueLength, -1);
				if (len == 0)
					WorkCompleted.Set();
			}
		}

		public void Start()
		{
			if (!Stopping)
				return;

			Stopping = false;

			WorkerThreads = Enumerable
				.Range(0, NumberOfWorkers)
				.Select(x => new Thread(WorkLoop) { Priority = WorkerPriority })
				.ToArray();

			foreach (var thread in WorkerThreads)
				thread.Start();
		}

		public void Stop()
		{
			if (Stopping)
				return;

			Stopping = true;

			for (int i = 0; i < WorkerThreads.Length; i++)
				Sema.Release();

			while (WorkerThreads.Any(x => x.ThreadState != ThreadState.Stopped))
				Thread.Sleep(1);
		}

		public void WaitAll()
		{
			WorkCompleted.WaitOne();
		}

		public void Dispose()
		{
			Stop();
		}

		~RealtimeDispatcher()
		{
			Dispose();
		}
	}
}
