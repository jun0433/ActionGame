using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Forge : PopupBase, IPopup
{
    public void PopupClose()
    {
        GetComponent<Image>().enabled = false;
    }

    public void PopupOpen()
    {
        GetComponent<Image>().enabled = true;
    }
}
