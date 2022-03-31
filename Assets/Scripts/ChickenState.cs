using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenState 
{
    public enum States
    {
        Wandering,
        Hungry,
        Eating,
        Mating,
        Fleeing
    }

    public enum Stages
    {
        Enter,
        Update,
        Exit
    }

    protected ChickenState nextState;
    protected GameObject npc;
    protected Perception perception;
    protected NavMeshAgent agent;
    protected Animator anim;
    protected Stages stage;
    protected States state;
    protected StatsManager cs;
    public Vector3 destination = new Vector3(39, 0, 28);

    public ChickenState(GameObject _npc, NavMeshAgent _agent, Animator _anim, StatsManager _cS, Perception _perception)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        cs = _cS;
        perception = _perception;
    }

    protected bool CalculatePath()
    {

        NavMeshPath path = new NavMeshPath();

        float _x = Random.Range(0, 50.0f);
        float _z = Random.Range(0, 50.0f);

        Vector3 dest = new Vector3(_x, 3.0f, _z);

        bool hasPath = agent.CalculatePath(dest, path);

        if (hasPath)
        {
            destination = dest;
            return true;
        }

        return false;

    }

    protected virtual void OnEnter()
    {
        stage = Stages.Update;
    }

    protected virtual void OnUpdate()
    {
        stage = Stages.Update;
    }

    protected virtual void OnExit()
    {
        stage = Stages.Exit;
    }

    public ChickenState Process()
    {
        if (stage == Stages.Enter)
            OnEnter();

        if(stage == Stages.Update)
            OnUpdate();

        if (stage == Stages.Exit)
        {
            OnExit();
            return nextState;
        }

        return this;
    }
}

public class Wandering : ChickenState
{
    public Wandering(GameObject _npc, NavMeshAgent _agent, Animator _anim, StatsManager _stats, Perception _perception) : base(_npc, _agent, _anim, _stats, _perception)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        cs = _stats;
        perception = _perception;
    }

    protected override void OnEnter()
    {
        state = States.Wandering;
        agent.isStopped = false;
        base.OnEnter();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();


        float distanceLeft = (destination - npc.transform.position).magnitude;

        if (distanceLeft < 3)
        {
            cs.DecreaseHunger(2.0f);
            CalculatePath();
            Debug.Log("Calculating path");
        }

        else
        {
            agent.SetDestination(destination);
        }
        
        if (cs.isHungry())
        {
            Debug.Log("Chicken is hungey!!");
            nextState = new Hungry(npc, agent, anim, cs, perception);
            stage = Stages.Exit;
        }
    }

    protected override void OnExit()
    {
        Debug.Log("Has Exitted Wander");
        base.OnExit();
    }
}

public class Hungry : ChickenState
{
    public Hungry(GameObject _npc, NavMeshAgent _agent, Animator _anim, StatsManager _stats, Perception _perception) : base(_npc, _agent, _anim, _stats, _perception)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        cs = _stats;
        perception = _perception;
    }

    protected override void OnEnter()
    {
        state = States.Hungry;
        agent.isStopped = false;
        base.OnEnter();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        bool hasFound = perception.SearchForFood();
        
        if (hasFound)
        {
            Debug.Log("Has found food");
            nextState = new Eating(npc, agent, anim, cs, perception);
            stage = Stages.Exit;
        }

    }

    protected override void OnExit()
    {
        Debug.Log("Has Exitted");
        base.OnExit();
    }
}
public class Eating : ChickenState
{
    public Eating(GameObject _npc, NavMeshAgent _agent, Animator _anim, StatsManager _stats, Perception _perception) : base(_npc, _agent, _anim, _stats, _perception)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        cs = _stats;
        perception = _perception;
    }

    protected override void OnEnter()
    {
        state = States.Eating;
        Debug.Log("Chicken is now eating");
        agent.isStopped = false;
        
        base.OnEnter();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        //Debug.Log(cs.GetHunger());

        agent.SetDestination(perception.getDestination());

        if (Vector3.Distance(npc.transform.position, perception.getDestination()) < 2)
        {
            cs.IncreaseHunger(5);
            nextState = new Wandering(npc, agent, anim, cs, perception);
            stage = Stages.Exit;    
        }

    }

    protected override void OnExit()
    {
        Debug.Log("Has Exited Eating");
        base.OnExit();
    }
}
