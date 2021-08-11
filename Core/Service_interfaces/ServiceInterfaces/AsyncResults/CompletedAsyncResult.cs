using System;
using System.Threading;

namespace LSOne.Services.Interfaces.AsyncResults
{
    /// <summary>
    /// Basic implementation of the IAsyncResult
    /// </summary>
    /// <typeparam name="T">Generic data type used for return</typeparam>
    public class CompletedAsyncResult<T> : IAsyncResult
    {
        T data;
        private object asyncState = null;
        private WaitHandle waitHandle = null;
        private bool completedSynchronously = false;
        private bool isCompleted = true;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Data returned by the async operation</param>
        /// <param name="asyncState">Task state identifier</param>
        public CompletedAsyncResult(T data, object asyncState)
        {
            this.asyncState = asyncState;
            this.data = data;
        }

        /// <summary>
        /// Generic data returned by the async operation
        /// </summary>
        public T Data
        {
            get { return data; }
            set { data = value; }
        }

        /// <summary>
        /// True if the async operation completed succesfully. This must be true in order for the End event to be triggered
        /// </summary>
        public bool IsCompleted
        {
            get { return isCompleted; }
            set { isCompleted = value; }
        }

        /// <summary>
        /// Handler used to wait for a task
        /// </summary>
        public WaitHandle AsyncWaitHandle
        {
            get { return waitHandle; }
            set { waitHandle = value; }
        }

        /// <summary>
        /// Task async state identifier
        /// </summary>
        public object AsyncState
        {
            get { return asyncState; }
            set { asyncState = value; }
        }

        /// <summary>
        /// True if a synchronous version of a method was found and called instead of the Begin method of the async pattern
        /// </summary>
        public bool CompletedSynchronously
        {
            get { return completedSynchronously; }
            set { completedSynchronously = value; }
        }
        
    }
}
