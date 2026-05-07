using UnityEngine;

public enum SpecialItemCategory
{
    Joke,
    EasterEgg,
    Cheat

}
[CreateAssetMenu(fileName = "SpecialItem", menuName = "My Objects/SpecialItem")]
public class SpecialItem : BaseItem
{
    [Header("Special Item Properties")]
    [SerializeField]
    private SpecialItemCategory specialCategory;
    public SpecialItemCategory SpecialCategory => specialCategory;

    [SerializeField, TextArea]
    private string specialEffectDescription;
    public string SpecialEffectDescription => specialEffectDescription;
}
