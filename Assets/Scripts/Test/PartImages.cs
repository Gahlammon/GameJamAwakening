using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartImages : Singleton<PartImages>
{
    [Header("References")]
    [SerializeField]
    private Image image;
    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();


    private ServerInventory inventory;

    public void SetServerInventory(ServerInventory serverInventory)
    {
        inventory = serverInventory;
        inventory.Parts.OnValueChanged += (_, i) => ChangeSprite(i);
    }

    private void ChangeSprite(int i)
    {
        image.sprite = sprites[i];
    }
}
