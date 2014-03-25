using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Polyhedrus
{
	public class RealtimeDispatcher<T> : IDisposable
	{
		ManualResetEvent workCompleted;
		Semaphore sema;
		Thread[] workerThreads;
		Action<T> action;
		Queue<T> workItems;
		int queueLength;
		volatile bool stopping;

		public int NumberOfWorkers { get; private set; }
		public int MaxQueueSize { get; private set; }
		public ThreadPriority WorkerPriority { get; private set; }

		public RealtimeDispatcher(int numberOfWorkers, int maxQueueSize, ThreadPriority workerPriority, Action<T> action)
		{
			workCompleted = new ManualResetEvent(true);
			sema = new Semaphore(0, maxQueueSize);
			this.action = action;
			workItems = new Queue<T>(maxQueueSize);
			NumberOfWorkers = numberOfWorkers;
			MaxQueueSize = maxQueueSize;
			WorkerPriority = workerPriority;
			stopping = true;

			Start();
		}

		public void QueueWorkItems(T[] items)
		{
			lock (workItems)
			{
				var len = items.Length;
				workCompleted.Reset();
				Interlocked.Add(ref queueLength, len);

				for (int i = 0; i < len; i++)
					workItems.Enqueue(items[i]);

				sema.Release(len);
			}
		}

		public void QueueWorkItem(T item)
		{
			lock (workItems)
			{
				workCompleted.Reset();

				Interlocked.Add(ref queueLength, 1);
				workItems.Enqueue(item);
				sema.Release();
			}
		}

		private void WorkLoop()
		{
			while (!stopping)
			{
				sema.WaitOne();

				if (stopping)
					return;

				T item;

				lock (workItems)
				{
					item = workItems.Dequeue();
				}
				
				action(item);

				int len = Interlocked.Add(ref queueLength, -1);
				if (len == 0)
					workCompleted.Set();
			}
		}

		public void Start()
		{
			if (!stopping)
				return;

			stopping = false;

			workerThreads = Enumerable
				.Range(0, NumberOfWorkers)
				.Select(x => new Thread(WorkLoop) { Priority = WorkerPriority })
				.ToArray();

			foreach (var thread in workerThreads)
				thread.Start();
		}

		public void Stop()
		{
			if (stopping)
				return;

			stopping = true;

			for (int i = 0; i < workerThreads.Length; i++)
				sema.Release();

			while (workerThreads.Any(x => x.ThreadState != ThreadState.Stopped))
				Thread.Sleep(1);
		}

		public void WaitAll()
		{
			workCompleted.WaitOne();
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
