using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField]
    Color mBaseColor;

    [SerializeField]
    Color mRightPlatformColor;

    [SerializeField]
    Color mWrongColor;

    Renderer mRenderer;

    void Start()
    {
        mRenderer = GetComponent<Renderer>();
    }

    public void ShineRight() 
    {
        
        mRenderer.material.SetColor("Color_59e840e7b59442a2b4f9c593d8104a11", mRightPlatformColor);
        mRenderer.material.SetColor("Color_b864b3fbad5a427f8db72518c61157d7", mRightPlatformColor);
    }
    public void ShineWrong()
    {
        mRenderer.material.SetColor("Color_59e840e7b59442a2b4f9c593d8104a11", mWrongColor);
        mRenderer.material.SetColor("Color_b864b3fbad5a427f8db72518c61157d7", mWrongColor);
    }

    public void UnShine()
    {
        mRenderer.material.SetColor("Color_59e840e7b59442a2b4f9c593d8104a11", mBaseColor);
        mRenderer.material.SetColor("Color_b864b3fbad5a427f8db72518c61157d7", Color.black);

    }
}
