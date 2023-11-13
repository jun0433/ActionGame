using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 다중 상속 불가(인터페이스는 허용)
public class Popup_Item : PopupBase, IPopup
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
