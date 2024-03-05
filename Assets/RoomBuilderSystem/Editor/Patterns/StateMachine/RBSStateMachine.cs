namespace RBS.Editor.Patterns.StateMachine
{
    using System.Linq;
    using System.Collections.Generic;

    public class RBSStateMachine
    {
        public Dictionary<string, RBSState> States { get; protected set; } = new Dictionary<string, RBSState>();

        public RBSState CurrentState { get; private set; }

        /// <summary>
        /// Initializes the State Machine.
        /// </summary>
        public void Start()
        {
            ChangeState(States.First().Key);
        }

        /// <summary>
        /// Stops the State Machine.
        /// </summary>
        public void Stop()
        {
            CurrentState?.OnExit();
            CurrentState = null;
        }

        /// <summary>
        /// Updates the State Machine.
        /// </summary>
        public void Update()
        {
            CurrentState?.OnProcess();
        }

        /// <summary>
        /// Changes the state of the State Machine.
        /// </summary>
        /// <param name="stateKey"> The key of the state to change to. </param>
        public void ChangeState(string stateKey)
        {
            if (States[stateKey] != CurrentState)
            {
                CurrentState?.OnExit();
                CurrentState = States[stateKey];
                CurrentState?.OnEnter();
            }
        }
    }
}