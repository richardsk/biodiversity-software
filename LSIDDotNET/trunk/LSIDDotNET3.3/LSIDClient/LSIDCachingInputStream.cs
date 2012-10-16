using System;
using System.IO;

namespace LSIDClient
{
	/**
	 *
	 * An input stream for LSID data.  As the data is read from the stream, it is also written to the LSID cache.
	 * When the input stream is closed, all remaining data in the object stream is written to the cache.
	 * 
	 * @author Kevin Richards (<a href="mailto:richardsk@landcareresearch.co.nz">richardsk@landcareresearch.co.nz</a>)
	 */
	class LSIDCachingInputStream : Stream 
	{	
		private static int BUFFER_SIZE=1024;

		private Stream objectData;
		private Stream cacheOut;
		private String cacheFile;
		private String tempFile;
		private LSID lsid;
		private LSIDAuthority authority;
		private LSIDCachingInputStreamListener listener;
		private LSIDCache lsidCache;
		private Boolean closed = false;
		private int len = 0;

		/**
		 * Create a cache input stream from the LSID objectData.  cacheFile is the location where the 
		 * cache is to be written.
		 * @param InputStream the source input stream to cache and pipe out.
		 * @param File the file to cache the input stream to.
		 * @param LSIDAuthority the authority associated with the data we are piping.
		 * @param LSID the lsid associated with the data we are piping
		 */
		public LSIDCachingInputStream(Stream objectData, String cacheFile, LSIDAuthority authority, LSID lsid) 
		{
			this.objectData = objectData;
			this.cacheFile = cacheFile;
			this.authority = authority;
			this.lsid = lsid;
		
			if (Directory.Exists(Path.GetDirectoryName(cacheFile))) Directory.CreateDirectory(Path.GetDirectoryName(cacheFile));

			this.lsidCache = LSIDCache.getSingletonInstance();
			if (this.lsidCache == null) 
			{
				throw new LSIDCacheException("Error opening Data cache");	
			}	

			String cacheDir = LSIDCache.getLocation() +  "\\temp";
			Directory.CreateDirectory(cacheDir);

			try 
			{
				tempFile = cacheDir + "\\cache.tmp";
				cacheOut = File.OpenWrite(tempFile);
				len = 0;
			}
			catch (IOException ) 
			{
				throw new LSIDCacheException("Error opening temporary cache file");	
			}
		}
	
		/**
		 * Closes the input stream.  Retrieves remaining data from the LSID object and writes it to the cache. 
		 * Closes the object data input stream as well.
		 * For the caching to work properly, users must call this function when finished with the stream.
		 */
		public override void Close() 
		{
			if (closed)
				return;
			closed = true;
			try 
			{
				// write out whatever is in the stream to the cache
				// re-use the buffer
			
				byte[] buffer = new byte[BUFFER_SIZE];
				int numbytes = objectData.Read(buffer, 0, BUFFER_SIZE);
				while (numbytes > 0) 
				{
					cacheOut.Write(buffer, 0, numbytes);
					numbytes = objectData.Read(buffer, 0, BUFFER_SIZE);
				}
			
				cacheOut.Flush();
				cacheOut.Close();
				listener.inputStreamClosed(authority, lsid);
			
				// move this file into the cache, delete existing file if it exists.
				if (File.Exists(cacheFile))
					File.Delete(cacheFile);
				File.Move(tempFile, cacheFile);
			
				File.Delete(tempFile);  // if the rename failed of file is present

				len = 0;
			} 
			catch (IOException e) 
			{
				if (tempFile != null)
					File.Delete(tempFile);
				throw e;
			} 
			finally 
			{
				try 
				{
					if (cacheOut != null)
						cacheOut.Close();
				} 
				catch (IOException e) 
				{
					LSIDException.PrintStackTrace(e);
				}
				try 
				{
					if (objectData != null)
						objectData.Close();
				} 
				catch (IOException e) 
				{
					LSIDException.PrintStackTrace(e);
				}
				try 
				{
					base.Close();
				} 
				catch (IOException e) 
				{
					LSIDException.PrintStackTrace(e);
				} 
			}
		}

		/**
		 * Reads an array of bytes from the object stream into the bytes[] argument.  The array is written out to the
		 * cache.  
		 * @param byte[] the allocated array in which to read the bytes.
		 */
//		public int read(byte[] bytes) 
//		{
//			if (closed)
//				throw new IOException("stream is closed");
//			return read(bytes,0,bytes.length);
//		}
	
		/**
		 * Set the listener who will be notified when this stream closes.  There can be only one.
		 * @param LSIDCachingInputStreamListener the listener to set
		 */
		public void setInputStreamListener(LSIDCachingInputStreamListener listener) 
		{
			this.listener = listener;
		}

	
		public override void Flush()
		{
			if (!closed && cacheOut != null)  cacheOut.Flush();
		}
	

		/**
		 * Reads an array of bytes from the object stream into the bytes[] argument.  The array is written out to the
		 * cache. 
		 * @param byte[] the allocated array in which to read the bytes.
		 * @param int the location in the array in which to start writing.
		 * @param int the number of bytes to read into the array
		 */
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (closed)
				throw new IOException("stream is closed");
			try 
			{
				int numbytes = objectData.Read(buffer, offset, count);
				if (numbytes > 0)
					cacheOut.Write(buffer, offset, numbytes);
				return numbytes;
			} 
			catch (IOException e) 
			{
				if (tempFile != null)
					File.Delete(tempFile);
				throw e;
			}
		}
	
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new Exception("Cant seek on theis stream");
		}
	
		public override long Length
		{
			get
			{
				return len;
			}
		}
	
		public override bool CanRead
		{
			get
			{
				return !closed;
			}
		}
	
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}
	
		public override void SetLength(long value)
		{
            throw new Exception("Cant set length of stream");
		}
	
		public override bool CanWrite
		{
			get
			{
				return !closed;
			}
		}
	
		/**
		 * Reads a single byte of data from the object input stream.  The byte is stored in a buffer.  When this buffer
		 * is full or the stream is closed, it gets written to the cache.
		 * @return the byte of data read.
		 */
		public override int ReadByte()
		{
			if (closed)
				throw new IOException("stream is closed");
			try 
			{
				int b = objectData.ReadByte();
				if (b == -1) 
				{
					cacheOut.Flush();
				} 
				else 
				{
					len += 1;
					cacheOut.WriteByte((Byte)b);
				}
				return b;
			} 
			catch (IOException e) 
			{
				if (tempFile != null)
					File.Delete(tempFile);
				throw e;
			}
		}
	
		public override long Position
		{
			get
			{
				return len;
			}
			set
			{
				throw new Exception("Cant seek on this stream");
			}
		}
	
		public override void Write(byte[] buffer, int offset, int count)
		{
		}
	}

}
