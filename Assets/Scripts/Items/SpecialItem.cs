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
    [SerializeField] private SpecialItemCategory _specialCategory;
    public SpecialItemCategory SpecialCategory => _specialCategory;

    [SerializeField, TextArea] private string _specialEffectDescription;
    public string SpecialEffectDescription => _specialEffectDescription;
}
