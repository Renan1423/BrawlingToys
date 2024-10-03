using BrawlingToys.Actors;
using UnityEngine;
public interface IHitCommand {
    void Execute(Hitable target);
}
