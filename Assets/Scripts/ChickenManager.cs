using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class ChickenManager : MonoBehaviour
{
    private ChickenState state;

    private NavMeshAgent agent;
    private StatsManager statsm;
    private Perception perception;
    private Animator anim;
    private GameObject npc;

    private Text text;
    private Text destinationText;


    private void Start()
    {
        npc = gameObject;
        
        perception = GetComponent<Perception>();    
        statsm = GetComponent<StatsManager>();
        agent = GetComponent<NavMeshAgent>();   
        anim = GetComponent<Animator>();

        text = GameObject.Find("Canvas/Text").GetComponent<Text>();
        destinationText = GameObject.Find("Canvas/Destination").GetComponent<Text>();

        state = new Wandering(npc, agent, anim, statsm, perception);
    }
    private void Update()
    {
        state = state.Process();
        text.text = "Current State: " + state.ToString();
        destinationText.text = "Destination : " + state.destination.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
