using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private PlayerManager player;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject arrowPosition;
    

    private void Start()
    {
        player = GetComponentInParent<PlayerManager>();       
    }

    public void AttackOver()
    {
        player.AttackOver();
    }

    public void SetActiveArrow() 
    {
        arrowPosition.SetActive(true);
    }

    public void InstantiateArrow()
    {
        arrowPosition.SetActive(false);
        GameObject arrow = Instantiate(arrowPrefab, arrowPosition.transform.position, Quaternion.Euler(0, 0, -90));
        
    }

    public void GiveDamage()
    {
        player.SwordDamage();        

    }
   


}
