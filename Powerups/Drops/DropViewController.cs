using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DropViewController : MonoBehaviour
{
    [SerializeField] private List<DropView> dropViewPanels;

    public void EnableViewPanel(float time,Sprite dropIcon)
    {
        foreach (DropView dropView in dropViewPanels)
        {
            if (!dropView.gameObject.activeSelf)
            {
                dropView.gameObject.SetActive(true);
                dropView.ActivateDrop(time);
                dropView.SetSprite(dropIcon);
                return;
            }
        }
    }

    public void DisableFireworkViewPanel()
    {
        foreach (DropView dropView in dropViewPanels)
        {
            if (dropView.GetSprite() == SpriteCollection.Instance.BoosterFirework)
            {
                dropView.gameObject.SetActive(false);
            }
        }

    }
}
