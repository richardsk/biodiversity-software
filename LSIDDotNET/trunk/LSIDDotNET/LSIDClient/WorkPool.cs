using System;
using System.Collections;
using System.Threading;

namespace LSIDClient
{
	/**
	 * 
	 * This workpool manages a set of threads that can handle runnables. 
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class WorkPool 
	{
		public delegate void Runnable(object[] args);

		private class Job
		{
			public Runnable method;
			public object[] args;

			public Job(Runnable method, object[] args)
			{
				this.method = method;
				this.args = args;
			}
		}

		public WorkPool() 
		{
		}

		public void AddJob(Runnable job, object[] args)
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(RunJob), new Job(job, args));
		}

		private void RunJob(object stateInfo)
		{
			Job j = (Job)stateInfo;
			j.method.DynamicInvoke(new object[]{j.args});
		}

//		private int numthreads = 0;
//		private ProducerConsumerBuffer workqueue;
//		// wait one minute for threads to clean themselves up before interrupting them
//		private static long CLEANUP_WAIT_TIME = 4000;
//		private int maxthreads;
//		private ArrayList workers = new ArrayList();
//
//		/**
//		 * Get the number of threads in this pool.
//		 * @return int the number of threads
//		 */
//		public int workerCount() 
//		{
//			lock(typeof(WorkPool))
//			{
//				return numthreads;
//			}
//		}
//	
//		/**
//		 * Create a new work pool
//		 * @param int the number of threads that the pool should have.
//		 */
//		public WorkPool(int maxthreads) 
//		{
//			this.maxthreads = maxthreads;
//			workqueue = new ProducerConsumerBuffer();
//		}
//	
//		/**
//		 * Add a job to the pool
//		 * @param Runnable the job to run
//		 */
//		public void addJob(Runnable job) 
//		{
//			lock(typeof(WorkPool))
//			{
//				if (numthreads < maxthreads && !(job is KillJob)) 
//				{
//					Thread worker = new Thread(new ThreadStart(job));
//					workers.addElement(worker);
//					
//					numthreads++;
//					worker.Start();
//				}
//				workqueue.put(job);
//			}
//		}
//	
//		/**
//		 * destroy the work pool
//		 */
//		public void destroy() 
//		{
//			lock(typeof(WorkPool))
//			{
//				for (int i = 0; i < numthreads; i++) 
//				{
//					//addJob(new KillJob());
//				}
//				long waittime = 0;
//				while (waittime < CLEANUP_WAIT_TIME) 
//				{
//					try 
//					{
//						//wait(1000);
//					} 
//					catch (Exception e) 
//					{
//						//ben.debug.Debug.println(ben.debug.Debug.SYSTEM_ERR,2,"Work Pool Destruction interrupted");
//					}
//					waittime += 1000;
//					if (numthreads == 0)
//						waittime = CLEANUP_WAIT_TIME;
//				}
//				for (int i = 0; i < numthreads; i++) 
//				{
////					WorkerThread worker = (WorkerThread) workers.elementAt(i);
////					worker.interrupt();
//				}
//			} 
//		}
//	
//		/**
//		 * Remove a given worker from the pool...this is useless right now as there is no public way to 
//		 * get an instance of a worker that is in the pool.
//		 * @param WorkerThread the worker to remove
//		 */
//		public void removeWorker(WorkerThread worker) 
//		{
//			lock(typeof(WorkPool))
//			{
//				workers.Remove(worker);
//				numthreads--;
//			}
//		}
	}
}