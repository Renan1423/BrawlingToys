using BrawlingToys.Actors;
using UnityEngine;
public interface IHitCommand {

    public void SetSender(GameObject sender);

    public GameObject GetSender();

    void Execute(IHitable target);
}
