using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float destroyTime = 5;
    [SerializeField]
    public ItemData item;
    [SerializeField]
    private int quantity = 10;
    public int Extract(int q)
    {
        if (quantity <= 0 || q <= 0)
        {
            return 0;
        }
        else if (quantity < q)
        {
            int aux = quantity;
            quantity = 0;
            ChangeToEmpty();
            return aux;
        }
        quantity -= q;
        if (quantity <= 0)
        {
            ChangeToEmpty();
        }
        return q;
    }
    public void ChangeToEmpty()
    {

    }
}
