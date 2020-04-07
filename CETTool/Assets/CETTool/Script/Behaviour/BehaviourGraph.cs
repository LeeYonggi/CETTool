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

        public void Init()
        {
            stateNodesDict.Clear();
            stateDict.Clear();
        }

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
        }

        public void AddStateNode(StateNode stateNode, Saved_StateNode savedStateNode)
        {
            stateNodesDict.Add(stateNode, savedStateNode);
            stateDict.Add(savedStateNode.state, stateNode);
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
    }

    [System.Serializable]
    public class Saved_StateNode
    {
        public State state;
        public Vector2 position;

    }

    [System.Serializable]
    public class Saved_Trasition
    {

    }
}
