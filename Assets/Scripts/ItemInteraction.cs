using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour, IInteraction {
    public GameObject dropdownButtons;
    private bool imBeingUsed;
    private void OnEnable() {
        imBeingUsed = false;
        EventManager.onItemSelected += HideDropdownButtons;
    }

    private void OnDisable() {
        EventManager.onItemSelected -= HideDropdownButtons;
    }

    private void HideDropdownButtons() {
        if (!imBeingUsed)
            dropdownButtons.SetActive(false);
        imBeingUsed = false;
    }

    public void Interact() {
        imBeingUsed = true;
        dropdownButtons.SetActive(true);
        EventManager.OnItemSelected();
    }
}
