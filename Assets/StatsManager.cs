using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    private Stats chickenStat = new Stats(36, 1);

    private Text hungerText;

    private void Start()
    {
        hungerText = GameObject.Find("Canvas/HungerText").GetComponent<Text>();
    }
    private void Update()
    {
        hungerText.text = "Energy: " + GetHunger().ToString();
    }

    public void IncreaseHunger(float n)
    {
        chickenStat.hunger += n;
        chickenStat.age += 1;
        if (chickenStat.age >= 3)
        {
            Mate();
        }
    }

    private void Mate()
    {
        Debug.Log("Mating");
    }

    public void DecreaseHunger(float n)
    {
        if (chickenStat.hunger > 0)
        {
            chickenStat.hunger -= n;
            if (chickenStat.hunger == 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        Debug.Log("Chicken has died :/ ");
        Destroy(gameObject);
    }

    public void SetCanReproduce()
    {
        chickenStat.canReproduce = true;
    }

    public bool isHungry()
    {
        if (chickenStat.hunger < 30)
        {
            return true;
        }
        return false;

    }

    public float GetHunger()
    {
        return chickenStat.hunger;
    }
}

public class Stats
{
    public float hunger;
    public float age;
    public bool canReproduce;

    public Stats(float _hunger, float _age)
    {
        hunger = _hunger; 
        age = _age;
    }
}