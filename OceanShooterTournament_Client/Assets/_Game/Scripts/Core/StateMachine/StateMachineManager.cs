using System;
using System.Collections.Generic;

namespace Core.StateMachine
{
    public class StateMachineManager
    {
        private IState _currentState;
        private Dictionary<Type, List<Transition>> _transitions = new();
        private List<Transition> _currentTransitions = new();
        private List<Transition> _fromAnyTransitions = new();
        private Dictionary<string, IState> _states = new();

        private static List<Transition> EmptyTransitions = new(0);

        public void Tick()
        {
            var transition = GetTransition();
            if (transition != null) SetState(transition.NextState);

            _currentState?.Action();
        }

        public void SetState(IState state)
        {
            if (_currentState == state) return;

            _currentState?.ExitState();
            _currentState = state;

            _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
            if (_currentTransitions == null) _currentTransitions = EmptyTransitions;

            _currentState.EnterState();
        }

        public IState GetCurrentState()
        {
            return _currentState;
        }

        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            _states[from.GetType().Name] = from;
            _states[to.GetType().Name] = to;

            if (!_transitions.TryGetValue(from.GetType(), out var transitions))
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(IState to, Func<bool> predicate)
        {
            _fromAnyTransitions.Add(new Transition(to, predicate));
        }

        public IState GetStateByName(string stateName)
        {
            return _states.GetValueOrDefault(stateName, null);
        }

        private Transition GetTransition()
        {
            foreach (var transition in _fromAnyTransitions)
            {
                if (transition.ConditionSatisfied()) return transition;
            }

            foreach (var transition in _currentTransitions)
            {
                if (transition.ConditionSatisfied()) return transition;
            }

            return null;
        }

        private class Transition
        {
            public IState NextState { get; }
            public Func<bool> ConditionSatisfied { get; }

            public Transition(IState nextState, Func<bool> condition)
            {
                NextState = nextState;
                ConditionSatisfied = condition;
            }
        }
    }
}