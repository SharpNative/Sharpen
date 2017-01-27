namespace Sharpen.Exec
{
    public unsafe class SignalAction
    {
        public struct SigAction
        {
            public int Flags;
            public uint Mask;
            public void* Handler;
        }

        public int SignalNumber { get; private set; }
        public SigAction Sigaction;

        /// <summary>
        /// Initializes action
        /// </summary>
        /// <param name="signal">The signal where this action is for</param>
        public SignalAction(int signal)
        {
            SignalNumber = signal;
            Sigaction.Flags = 0;
            Sigaction.Mask = 0;
            Sigaction.Handler = null;
        }

        /// <summary>
        /// Clones this signal handler
        /// </summary>
        /// <returns></returns>
        public SignalAction Clone()
        {
            SignalAction clone = new SignalAction(SignalNumber);
            clone.Sigaction.Flags = Sigaction.Flags;
            clone.Sigaction.Mask = Sigaction.Mask;
            clone.Sigaction.Handler = Sigaction.Handler;
            return clone;
        }

        /// <summary>
        /// Internal return from signal handler (calls the corresponding syscall)
        /// </summary>
        public static extern void ReturnFromSignal();
    }
}
