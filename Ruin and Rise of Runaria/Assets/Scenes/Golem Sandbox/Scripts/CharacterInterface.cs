using UnityEngine;

public interface ICharacter
{
    ICharacter Target();
    int GetSortingOrder();
    Vector3 TargetPosition();
    Vector3 Position();
    void TakeDamage(float amount);
    void Heal(float amount);
    float MaxHealth();
    bool Alive();
    void Die();
    int GUID();
    SelectedAndTargetUI SelectedAndTargetUI();

    void IncreaseDefense(float amount);
    void IncreaseAttack(float amount);
}
