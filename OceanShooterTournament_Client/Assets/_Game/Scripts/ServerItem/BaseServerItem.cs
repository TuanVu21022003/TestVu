using UnityEngine;

public abstract class BaseServerItem : BaseGame, IGetPoolID
{
    public string itemID { get; private set; }
    public int itemType { get; private set; }
    public string itemPoolID;

    public virtual string GetPoolID()
    {
        return itemPoolID;
    }

    public virtual BaseServerItem Init()
    {
        Debug.Log("BaseServerItem Init");
        return this;
    }
    
    public virtual BaseServerItem SetDataServer(ItemDataServer itemData)
    {
        var position = itemData.position;
        var velocity = itemData.velocity;

        itemID = itemData.itemID;
        itemType = itemData.itemType;
        TF.position = new Vector3(position.x, position.y, 0);
        TF.rotation = Quaternion.Euler(
            0,
            0,
            Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg
        );

        return this;
    }
    
    public virtual void DestroyItem()
    {
        ObjectPoolManager.instance.Push(this);
    }
}