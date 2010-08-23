using System;
using System.Threading;

using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace LSIDClient
{
	/**
	 * 
	 * A worker thread lives inside the workpool and takes on jobs as they become available.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class WorkerThread 
	{
		private ProducerConsumerBuffer workqueue;
		private WorkPool pool;

		/**
		* This is the run method of the worker thread.  It waits on the work queue and does a job when
		* one becomes available
		*/
		public void run() 
		{
			Boolean alive = true;
			try 
			{
//				CSharpCodeProvider p = new CSharpCodeProvider();
//				ICodeCompiler cc = p.CreateCompiler();
//				cc.CompileAssemblyFromSource(
				while (alive) 
				{
//					Runnable job = (Runnable) workqueue.get();
//					if (job is KillJob)
//						alive = false;
//					else
//						job.run();
				}
			} 
			catch (Exception) 
			{				
			}
			//pool.removeWorker(this);
		}
	
		/**
		* Create a new worker.
		* @param ProducerConsumerBuffer workqueue the queue of work
		* @param WorkPool the pool of workers to which this worker will belong to
		*/
		WorkerThread(ProducerConsumerBuffer workqueue, WorkPool pool) //: base("JMB Worker Thread")
		{			
			this.workqueue = workqueue;
			this.pool = pool;
		}
	}
}