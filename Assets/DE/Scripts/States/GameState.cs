using NPP.DE.Core.Signal;
using Stateless;

namespace NPP.DE.Core.State
{

    public class GameState
    {
        public enum State { Initialization, Menu, Game };
        public enum Trigger { ChangeState };

        private StateMachine<State, Trigger> _machine;
        private static GameState _states;

        public event System.Action OnInitialization;
        public event System.Action OnMenu;
        public event System.Action OnGame;

        public static void Initialize()
        {
            _states = new GameState();
        }

        public static GameState States
        {
            get
            {
                if (_states == null)
                    _states = new GameState();
                return _states;
            }
        }

        public State Current
        {
            get
            {
                return _machine.State;
            }
        }

        public GameState()
        {
            #region App State

            _machine = new StateMachine<State, Trigger>(State.Initialization);
            _machine.Configure(State.Initialization).Permit(Trigger.ChangeState, State.Menu);
            _machine.Configure(State.Menu).Permit(Trigger.ChangeState, State.Game);
            _machine.Configure(State.Game).Permit(Trigger.ChangeState, State.Menu);

            _machine.OnTransitioned(OnStateChange);

            Signals.Hub.Get<SignalCollection.AppState.GameStateTransitionSignal>().AddListener(OnReadyTransition);

            #endregion
        }

        private void OnStateChange(StateMachine<State, Trigger>.Transition transition)
        {
            if (transition.Destination == State.Initialization)
                OnInitialization?.Invoke();
            if (transition.Destination == State.Menu)
                OnMenu?.Invoke();
            if (transition.Destination == State.Game)
                OnGame?.Invoke();
        }

        private void OnReadyTransition()
        {
            Fire();
        }

        private void Fire()
        {
            _machine.Fire(Trigger.ChangeState);
        }

        ~GameState()
        {
            Signals.Hub.Get<SignalCollection.AppState.GameStateTransitionSignal>().RemoveListener(OnReadyTransition);
        }
    }
}