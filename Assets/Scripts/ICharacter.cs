using UnityEngine;
interface ICharacter
{
    void TakeDamage(float dmg);
    void OnDeath();
    Meter GetHealth();
}