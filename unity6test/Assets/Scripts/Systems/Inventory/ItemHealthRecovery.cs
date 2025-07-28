using UnityEngine;

[CreateAssetMenu]
public class ItemHealthRecovery : Item
{
    public int Value = 1;
    public override bool UseItem()
    {
        HealthManager.instance.TakeDamage(-Value);
        Debug.Log("used health recovery item");
        return true;
    }
}
