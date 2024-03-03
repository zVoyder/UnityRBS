namespace RBS.Editor.Patterns.StateMachine
{
    using System.Linq;
    using System.Collections.Generic;

    public class RBSStateMachine
    {
        public Dictionary<string, RBSState> States { get; protected set; } = new Dictionary<string, RBSState>();

        public RBSState CurrentState { get; private set; }

        public void Start()
        {
            ChangeState(States.First().Key);
        }

        public void Stop()
        {
            CurrentState?.OnExit();
            CurrentState = null;
        }

        public void Update()
        {
            CurrentState?.OnProcess();
        }

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