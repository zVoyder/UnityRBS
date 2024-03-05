namespace RBS.Editor.Patterns.StateMachine
{
    using System;

    public abstract class RBSState
    {
        public string StateKey { get; protected set; }

        public RBSState(string stateKey)
        {
            StateKey = stateKey;
        }

        /// <summary>
        /// Called when entering the state.
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// Called when exiting the state.
        /// </summary>
        public abstract void OnExit();

        /// <summary>
        /// Called to process the state's logic each frame.
        /// </summary>
        public abstract void OnProcess();
    }
}