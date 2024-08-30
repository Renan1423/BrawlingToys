using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurpriseBox : MonoBehaviour
{
    private BuffDebuffTestScriptable _buffDebuffInsideBox;

    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private Transform _drawnPartContainerTrans;
    [SerializeField]
    private GameObject _surpriseBoxModel;

    public void SetupSurpriseBox(BuffDebuffTestScriptable buffDebuffInsideBox) 
    {
        _buffDebuffInsideBox = buffDebuffInsideBox;

        Renderer modelMesh = _surpriseBoxModel.GetComponent<Renderer>();
        Color newMatColor = (buffDebuffInsideBox.EffectType == EffectType.Buff) ? Color.green : Color.red;
        modelMesh.material.color = newMatColor;
    }

    public void OpenSurpriseBox() 
    {
        _anim.SetTrigger("Open");
        Destroy(this.gameObject, 5f);
    }

    public BuffDebuffTestScriptable GetBuffDebuffInsideBox() { return _buffDebuffInsideBox; }
}
