using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrawlingToys.Actors;

public class SurpriseBox : MonoBehaviour
{
    private ModifierScriptable _buffDebuffInsideBox;

    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private Transform _drawnPartContainerTrans;
    [SerializeField]
    private Transform _modelTrans;
    [SerializeField]
    private GameObject _buffSurpriseBoxPrefab;
    [SerializeField]
    private GameObject _debuffSurpriseBoxPrefab;

    public void SetupSurpriseBox(ModifierScriptable buffDebuffInsideBox) 
    {
        _buffDebuffInsideBox = buffDebuffInsideBox;

        GameObject surpriseBoxModelPrefab = (buffDebuffInsideBox.EffectType == EffectType.Buff) ? _buffSurpriseBoxPrefab : _debuffSurpriseBoxPrefab;
        GameObject surpriseBoxModel = Instantiate(surpriseBoxModelPrefab, _modelTrans);
    }

    public void OpenSurpriseBox() 
    {
        _anim.SetTrigger("Open");
        Destroy(this.gameObject, 5f);
    }

    public ModifierScriptable GetBuffDebuffInsideBox() { return _buffDebuffInsideBox; }
}
