using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GlobalRawAnimator : MonoBehaviour
{
    public void addButtonOutline(Image pButtonImage)
    {
        Outline mOutline = pButtonImage.gameObject.AddComponent<Outline>();
        mOutline.effectColor = new Color(9 / 255f, 126 / 255f, 57 / 255f, 0f);
        mOutline.effectDistance = new Vector2(0, 0);

        Sequence mSequence = DOTween.Sequence();
        mSequence.Append(mOutline.DOColor(new Color(9 / 255f, 126 / 255f, 57 / 255f, 1f), 0.25f));
        mSequence.Join(DOVirtual.Float(0, 0f, 0.25f, value => mOutline.effectDistance = new Vector2(0, value)));
        mSequence.Append(DOVirtual.Float(0f, 0, 0.25f, value => mOutline.effectDistance = new Vector2(0, value)));
    }

    public void removeButtonOutline(Image pButtonImage)
    {
        Outline[] mOutlines = pButtonImage.GetComponents<Outline>();
        if (mOutlines.Length > 0)
        {
            Outline mOutlineToRemove = mOutlines[mOutlines.Length - 1];
            Sequence mSequence = DOTween.Sequence();
            mSequence.Append(mOutlineToRemove.DOColor(new Color(mOutlineToRemove.effectColor.r, mOutlineToRemove.effectColor.g, mOutlineToRemove.effectColor.b, 0f), 0.25f));
            mSequence.Join(DOVirtual.Float(mOutlineToRemove.effectDistance.y, 0, 0.25f, value => mOutlineToRemove.effectDistance = new Vector2(0, value)));
            mSequence.OnComplete(() => Destroy(mOutlineToRemove));
        }
    }

    public void WobbleObject(GameObject pAppObject)
    {
        GlobalAnimator.Instance.WobbleObject(pAppObject);
    }
}
