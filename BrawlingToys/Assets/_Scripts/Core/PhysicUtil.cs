using System;
using BrawlingToys.Core;
using UnityEngine;

public class PhysicUtil 
{
    private CountdownTimer timer;

    public CountdownTimer Timer { get { return timer; } private set { } }

    private Guid _id; 

    public PhysicUtil(float timeToCount)
    {
        timer = new(timeToCount);
        _id = Guid.NewGuid(); 
    }

    public void AddForce(Transform subject, Vector3 direction, float power, float deltaTime)
    {
        timer.Tick(deltaTime);

        if (timer.IsRunning)
        {
            Debug.Log($"Running knock back on instance id: {_id.ToString()}");
            subject.position = Vector3.Lerp(subject.position, subject.position + direction, power * deltaTime);
        }
    }
}
