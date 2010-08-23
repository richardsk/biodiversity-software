using System;
using System.Collections;

namespace LSIDClient
{

	/**
	 * 
	 * A simple prod-cons buffer.  Items are atted at the rear of the queue.  the get method blocks until 
	 * an item is available in the queue.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 * 
	 */
	public class ProducerConsumerBuffer 
	{	
	
		private ArrayList buffer = new ArrayList();
	
		/**
		 * Construct a new buffer
		 */
		public ProducerConsumerBuffer() : base()
		{
		}
		/**
		 * 
		 * Get an object from the queue, blocks until an object is ready.
		 * @return Object the object from the queue
		 */
		public Object get()
		{
			while (buffer.Count == 0) 
			{
				//this.wait();
			}
			Object ret = buffer[buffer.Count - 1];
			buffer.RemoveAt(buffer.Count - 1);
			return ret;

		}
		/**
		 * 
		 * Puts an object into the queue, notify's  the first waiting process that an obect is available
		 * 
		 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
		 * 
		 */
		public void put(Object obj) 
		{
			buffer.Insert(0, obj);
			//this.notify();
		}
	}
}