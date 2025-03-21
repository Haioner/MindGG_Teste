using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FloatNumber_DOTAnim : MonoBehaviour
{
    [SerializeField] private List<DOTweenAnimation> dotAnimations;

    private void Awake() 
        => GetComponent<FloatNumber>().OnInitiateFloatNumber += () 
            => dotAnimations.ForEach(anim => anim.DORestart());
}
