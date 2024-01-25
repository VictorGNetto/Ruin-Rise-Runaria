using UnityEngine;

public interface ICharacter
{
    // Vector3 TargetPosition();
    // Vector3 Position();

    void TakeDamage(float amount);
    void Die();
    int GUID();
    // void Heal(float amount);
}