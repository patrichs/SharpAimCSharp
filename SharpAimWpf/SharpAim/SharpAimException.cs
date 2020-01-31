using System;
using System.Runtime.Serialization;

namespace SharpAimWpf.SharpAim
{
    [Serializable]
    public class SharpAimException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public SharpAimException()
        {
        }

        public SharpAimException(string message) : base(message)
        {
        }

        public SharpAimException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SharpAimException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
