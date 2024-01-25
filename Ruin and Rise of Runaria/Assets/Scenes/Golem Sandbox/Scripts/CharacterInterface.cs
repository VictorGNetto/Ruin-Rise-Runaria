using UnityEngine;

public interface ICharacter
{
    ICharacter Target();

    int GetSortingOrder();
    Vector3 TargetPosition();
    Vector3 Position();

    void TakeDamage(float amount);
    void Die();
    int GUID();
    // void Heal(float amount);
}