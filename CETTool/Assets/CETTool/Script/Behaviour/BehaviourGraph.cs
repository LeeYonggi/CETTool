using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CET
{
    [CreateAssetMenu]
    public class BehaviourGraph : ScriptableObject
    {
        public List<Saved_StateNode> savedStateNodes = new List<Saved_StateNode>();
        Dictionary<StateNode, Saved_StateNode> stateNodesDict = new Dictionary<StateNode, Saved_StateNode>();
        Dictionary<State, StateNode> stateDict = new Dictionary<State, StateNode>();

        public List<Saved_Transition> transitionNodes = new List<Saved_Transition>();
        Dictionary<TransitionNode, Saved_Transition> transitionNodeDict = new Dictionary<TransitionNode, Saved_Transition>();


        public void Init()
        {
            stateNodesDict.Clear();
            stateDict.Clear();
        }

        public void SetNode(BaseNode node)
        {
            if(node is StateNode)
            {
                SetStateNode(node as StateNode);
            }
            if(node is TransitionNode)
            {

            }
            if(node is CommentNode)
            {

            }
        }

        #region
        private void SetTransitionNode(TransitionNode node)
        {
            Saved_Transition savedNode = GetSavedTransition(node);

            if(savedNode == null)
            {
                savedNode = new Saved_Transition();

                transitionNodes.Add(savedNode);
                transitionNodeDict.Add(node, savedNode);
            }

            savedNode.position = new Vector2(node.windowRect.x, node.windowRect.y);
            savedNode.transition = node.targetTransition;
            savedNode.enterState = node.enterState.currentState;
            savedNode.targetState = node.targetState.currentState;
        }

        private Saved_Transition GetSavedTransition(TransitionNode node)
        {
            Saved_Transition savedTransition = null;

            transitionNodeDict.TryGetValue(node, out savedTransition);

            return savedTransition;
        }

        public void RemoveTransitionNode(TransitionNode node)
        {
            Saved_Transition savedTransitionNode = GetSavedTransition(node);

            if(savedTransitionNode != null)
            {
                transitionNodes.Remove(savedTransitionNode);
                transitionNodeDict.Remove(node);
            }
        }
        #endregion

        #region STATE_NODE

        public void SetStateNode(StateNode node)
        {
            Saved_StateNode savedStateNode = GetSavedState(node);

            if(savedStateNode == null)
            {
                savedStateNode = new Saved_StateNode();

                savedStateNodes.Add(savedStateNode);
                stateNodesDict.Add(node, savedStateNode);
            }

            savedStateNode.state = node.currentState;
            savedStateNode.position = new Vector2(node.windowRect.x, node.windowRect.y);
            savedStateNode.isCollaps = node.collapse;
        }

        public void RemoveStateNode(StateNode node)
        {
            Saved_StateNode savedStateNode = GetSavedState(node);

            if(savedStateNode != null)
            {
                savedStateNodes.Remove(savedStateNode);
                stateNodesDict.Remove(node);
            }
        }

        public Saved_StateNode GetSavedState(StateNode node)
        {
            Saved_StateNode saveStateNode = null;

            stateNodesDict.TryGetValue(node, out saveStateNode);
            return saveStateNode;
        }

        public StateNode GetStateNode(State state)
        {
            StateNode stateNode = null;

            stateDict.TryGetValue(state, out stateNode);
            return stateNode;
        }
        #endregion
    }

    [System.Serializable]
    public class Saved_StateNode
    {
        public State state;
        public Vector2 position;
        public bool isCollaps;

    }

    [System.Serializable]
    public class Saved_Transition
    {
        public State targetState;
        public State enterState;
        public Transition transition;
        public Vector2 position;
    }
}
