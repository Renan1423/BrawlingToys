using UnityEngine;

public interface ICommandManager {

    public void SetShootingCommand(ICommand command);

    public void SetMeleeCommand(ICommand command);
}
