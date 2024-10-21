using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerFeedbacks : MonoBehaviour {
    [SerializeField] private MMF_Player _stepFeedback;
    [SerializeField] private MMF_Player _shootFeedback;
    [SerializeField] private MMF_Player _meleeFeedback;
    [SerializeField] private MMF_Player _dashFeedback;
    [SerializeField] private MMF_Player _getHitFeedback;
    [SerializeField] private MMF_Player _dieFeedback;

    // Essa classe está vazia por agora, o sistema de feedbacks pode ser implementado
    // por aqui ou utilizando os eventos dos states, ficará a critério de cada implementação
    // individual.
}
