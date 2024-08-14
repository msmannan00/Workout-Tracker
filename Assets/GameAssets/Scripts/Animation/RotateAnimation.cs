using UnityEngine;
using DG.Tweening;

public class RotateAnimation : MonoBehaviour
{
    public RectTransform mImageTransform;
    public float mRotationSpeed = 0f;
    private bool mIsActive = true;

    private void Start()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (!mIsActive)
        {
            return;
        }

        float mRotationAngle = -360f;
        float mRotationTime = Mathf.Abs(mRotationAngle) / mRotationSpeed;

        mImageTransform.DORotate(new Vector3(0, 0, mRotationAngle), mRotationTime * 3, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .OnComplete(Rotate);
    }

    private void OnDisable()
    {
        mIsActive = false;
    }

    private void OnEnable()
    {
        mIsActive = true;
        Rotate();
    }
}
