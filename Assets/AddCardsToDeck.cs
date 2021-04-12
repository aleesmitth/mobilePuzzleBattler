using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddCardsToDeck : MonoBehaviour {
    public TMP_InputField inputNumber;
    public FloatValue handSize;

    private void OnEnable() {
        inputNumber.text = handSize.value.ToString(CultureInfo.InvariantCulture);
    }

    public void ChangeInputNumber(TMP_InputField inputField) {
        this.inputNumber = inputField;
    }

    public void AddCards() {
        GameManager.instance.AddCardsToDeck(inputNumber);
    }
}
