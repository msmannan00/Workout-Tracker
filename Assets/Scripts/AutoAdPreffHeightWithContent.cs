using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AutoAdPreffHeightWithContent : MonoBehaviour
{

    public RectTransform ChildRootTransform;

     LayoutElement LayoutElementxComponent;


    private void OnEnable()
    {
        LayoutElementxComponent =  GetComponent<LayoutElement>();
    
    }

    private void Update()
    {
        if(ChildRootTransform && LayoutElementxComponent)
        {
            LayoutElementxComponent.preferredHeight = ChildRootTransform.rect.height;

        }
    }


}
