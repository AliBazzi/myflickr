using System;
using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;
using MyFlickr.Core;
using MyFlickr.Rest;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;

namespace APITest
{
    /// <summary>
    /// Provides support for Serializing and Deserializing data to and from XML representation , Synchronously and Asynchronously 
    /// </summary>
    /// <typeparam name="T">the Type of Object which is going to be Serialized or Deserialized</typeparam>
    public sealed class Serialization<T>
    {
        //fields //////////////////////////////////////////////////////////////

        private bool _IsBussy = false;
        private object _LockObject = new object();

        //Properties ///////////////////////////////////////////////////////

        /// <summary>
        /// Get the Value indicating if the Class is Busy with Doing a Process
        /// </summary>
        public bool IsBussy
        {
            get
            {
                return this._IsBussy;
            }
        }

        //Constructers //////////////////////////////////////////////

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// 
        public Serialization() { }

        //Methods /////////////////////////////////////////////////////

        /// <summary>
        /// Serialize the Given Data to the Specified Path
        /// </summary>
        /// <param name="data">The Data of Type T to Be Serialized</param>
        /// <param name="path">The Path to Serialize the data to</param>
        public void Serialize(T data, string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("path");

            using (var fs = new FileStream(path, FileMode.Create))
                this.Serialize(data, fs);
        }

        /// <summary>
        /// Serialize the Given Data to the Specified Path
        /// </summary>
        /// <param name="data">The Data of Type T to Be Serialized</param>
        /// <param name="stream">The stream to Serialize the data to</param>
        public void Serialize(T data, Stream stream)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (stream == null)
                throw new ArgumentNullException("stream");

            new XmlSerializer(typeof(T)).Serialize(stream, data);
        }

        /// <summary>
        /// The method which is responsible about desrerializing data from XML Path
        /// </summary>
        /// <param Name="Path">the Path which the XML Path is exist</param>
        /// <param Name="path">The path which the serializer will extract data from</param>
        public T Deserialize(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("path");

            using (var fs = new FileStream(path, FileMode.Open))
                return Deserialize(fs);
        }

        /// <summary>
        /// The method which is responsible about desrerializing data from XML Path
        /// </summary>
        /// <param Name="stream">a Stream that contains the Serialized Object</param>
        public T Deserialize(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            return (T)new XmlSerializer(typeof(T)).Deserialize(stream);
        }

        //Async Methods/////////////////////////////////////////////////

        /// <summary>
        /// Call the Serializing Asynchronously
        /// </summary>
        /// <param name="data">The Data to Be Serialized</param>
        /// <param name="path">The Path that the data will be serialized to</param>
        public void SerializingAsync(T data, string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path");
            if (data == null)
                throw new ArgumentNullException("data");
            this.VerifyAndLockOperation();

            new Action<T, Stream, AsyncOperation, bool>(this._Serialize).BeginInvoke(data, new FileStream(path, FileMode.Create),
                AsyncOperationManager.CreateOperation(null), true, null, null);
        }

        /// <summary>
        /// Call the Serializing Asynchronously
        /// </summary>
        /// <param name="data">The Data to Be Serialized</param>
        /// <param name="stream">The stream that the data will be serialized to</param>
        public void SerializingAsync(T data, Stream stream)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            this.VerifyAndLockOperation();

            new Action<T, Stream, AsyncOperation, bool>(this._Serialize).BeginInvoke(data, stream, AsyncOperationManager.CreateOperation(null), false, null, null);
        }

        /// <summary>
        /// Call the Deserializing Asynchronously
        /// </summary>
        public void DeserializingAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            this.VerifyAndLockOperation();

            new Action<Stream, AsyncOperation, bool>(this._Deserialize).BeginInvoke(new FileStream(path, FileMode.Open),
                AsyncOperationManager.CreateOperation(null), true, null, null);
        }

        /// <summary>
        /// Call the Deserializing Asynchronously
        /// </summary>
        public void DeserializingAsync(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            this.VerifyAndLockOperation();

            new Action<Stream, AsyncOperation, bool>(this._Deserialize).BeginInvoke(stream, AsyncOperationManager.CreateOperation(null), false, null, null);
        }

        //Internal Async Methods//////////////////////////////////////////////

        private void _Serialize(T data, Stream stream, AsyncOperation asyncOperation, bool disposeStream)
        {
            try
            {
                this.Serialize(data, stream);
                if (disposeStream)
                    stream.Dispose();
                asyncOperation.PostOperationCompleted((obj) => { this.InvokeFinishEvent((AsyncCompletedEventArgs)obj); }, new AsyncCompletedEventArgs(null, false, data));
            }
            catch (Exception exc)
            {
                asyncOperation.PostOperationCompleted((obj) => { this.InvokeFinishEvent((AsyncCompletedEventArgs)obj); }, new AsyncCompletedEventArgs(exc, false, data));
            }
            this.UnlockAccess();
        }

        private void _Deserialize(Stream stream, AsyncOperation asyncOperation, bool disposeStream)
        {
            try
            {
                T DeserializedObject = this.Deserialize(stream);
                if (disposeStream)
                    stream.Dispose();
                asyncOperation.PostOperationCompleted((obj) => { this.InvokeFinishEvent((AsyncCompletedEventArgs)obj); }, new AsyncCompletedEventArgs(null, false, DeserializedObject));
            }
            catch (Exception exc)
            {
                asyncOperation.PostOperationCompleted((obj) => { this.InvokeFinishEvent((AsyncCompletedEventArgs)obj); }, new AsyncCompletedEventArgs(exc, false, null));
            }
            this.UnlockAccess();
        }

        private void InvokeFinishEvent(AsyncCompletedEventArgs args)
        {
            if (this.SerializationFinished != null)
            {
                this.SerializationFinished.Invoke(this, args);
            }
        }

        //Events ////////////////////////////////////////////////////////////////

        /// <summary>
        /// This DeleteUC will be Raised when Serializing or Deserializing Complete
        /// </summary>
        /// <remarks>you don't have to use the Dispatcher (In WPF) or Invoke or BeginInvoke (Windows Forms) cause the DeleteUC will be Raised on the Correct Thread (Thanks for Asynchronous Design Pattern)</remarks>
        public event EventHandler<AsyncCompletedEventArgs> SerializationFinished;

        //Internal Stuff /////////////////////////////////////////////////////////

        private void VerifyAndLockOperation()
        {
            lock (this._LockObject)
            {
                if (this._IsBussy) throw new InvalidOperationException("Can't Start New Process Until the Current one is Finish");
                this._IsBussy = true;
            }
        }

        private void UnlockAccess()
        {
            lock (this._LockObject)
            {
                this._IsBussy = false;
            }
        }
    }

    [XmlRoot]
    public class Data
    {
        [XmlElement]
        public string apiKey;

        [XmlElement]
        public string sharedSecret;

        [XmlElement]
        public string token;

    }

    class Program
    {
        static void Main(string[] args)
        {
            Data data = new Serialization<Data>().Deserialize("C:\\data.xml");
            var user = new Authenticator(data.apiKey, data.sharedSecret).CheckToken(data.token).CreateUserInstance();
            foreach (var photo in user.GetPhotos())
            {
                photo.SetSafetyLevelCompleted+=new EventHandler<EventArgs<NoReply>>((o,e)=>Console.WriteLine(string.Format("{0},{1}",((Photo)o).Title,e.Successful)));
                photo.SetSafetyLevelAsync(SafetyLevel.Safe, false);
            }
            Console.ReadLine();
        }
    }
}
