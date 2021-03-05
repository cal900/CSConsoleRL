using System;
using System.Collections.Generic;
using System.Text;
using CSConsoleRL.Ai.Interfaces;
using CSConsoleRL.Helpers;
using CSConsoleRL.Logging;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Ai
{
  public delegate bool Condition(Entity entity, GameStateHelper gameStateHelper);
  public sealed class AiStateMachine
  {
    private Dictionary<string, IAiState> _states;
    private List<AiStateMachineNode> _nodes;
    public string CurrentState { get; private set; }

    public AiStateMachine()
    {

    }

    private List<AiStateMachineNode> GetAllNodesForCurrentState()
    {
      List<AiStateMachineNode> nodes = _nodes.FindAll(n => n.PreState == CurrentState);

      return nodes;
    }

    private void EvaluateConditions(Entity entity, GameStateHelper gameStateHelper)
    {
      List<AiStateMachineNode> nodes = GetAllNodesForCurrentState();

      foreach (var node in nodes)
      {
        if (node.Condition(entity, gameStateHelper))
        {
          CurrentState = node.PostState;
          GameLogger.Instance().LogDebug($"Switching entity ${entity.Id} from state {node.PreState} to {node.PostState}");
          return;
        }
      }
    }

    public void AddState(string name, IAiState state)
    {
      _states.Add(name, state);
    }

    public void AddStateChange(string preState, string postState, Condition condition)
    {
      if (!_states.ContainsKey(preState)) throw new Exception($"preState ${preState} does not exist in the State Machine");
      if (!_states.ContainsKey(postState)) throw new Exception($"postState ${postState} does not exist in the State Machine");

      _nodes.Add(new AiStateMachineNode(preState, postState, condition));
    }

    public void GetCurrentStateResponse(Entity entity, GameStateHelper gameStateHelper)
    {
      EvaluateConditions(entity, gameStateHelper);

      var currentState = _states[CurrentState];

      currentState.GetAiStateResponse(gameStateHelper);
    }

    private class AiStateMachineNode
    {
      public readonly string PreState;
      public readonly string PostState;
      public readonly Condition Condition;

      public AiStateMachineNode(string preState, string postState, Condition condition)
      {
        PreState = preState;
        PostState = postState;
        Condition = condition;
      }
    }
  }
}
