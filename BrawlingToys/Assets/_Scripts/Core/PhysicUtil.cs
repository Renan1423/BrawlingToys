using BrawlingToys.Core;
using UnityEngine;

public class PhysicUtil 
{
    private CountdownTimer timer;

    public CountdownTimer Timer { get { return timer; } private set { } }

    public PhysicUtil(float timeToCount)
    {
        timer = new(timeToCount);
    }

    public void AddForce(Transform subject, Vector3 direction, float power, float deltaTime)
    {
        timer.Tick(deltaTime);

        if (timer.IsRunning)
        {
            subject.position = Vector3.Lerp(subject.position, subject.position + direction, power * deltaTime);
        }
    }
}
