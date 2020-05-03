using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 100;
    [SerializeField] bool isFriendly = false;

    public int GetDamage() => damage;
    public bool IsFriendly() => isFriendly;

    public void Hit() => Destroy(gameObject);
}
