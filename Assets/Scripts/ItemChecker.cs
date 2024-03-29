using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ItemChecker : MonoBehaviour, IInteractable
{
    [Serializable]
    private struct ItemAction
    {
        public Item item;
        public UnityEvent action;
    }

    [SerializeField] private List<ItemAction> checkList;

    public void Interact()
    {
        foreach (var itemAction in checkList.Where(itemAction => Inventory.instance.CheckItem(itemAction.item)))
        {
            itemAction.action.Invoke();
            Inventory.instance.RemoveItem(itemAction.item);
            break;
        }
    }
}
