using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    public enum FruitType { Apple, Banana, Coconut}

    public FruitType mFruitType;

    private void Awake()
    {
        mFruitType = (FruitType) Random.Range(0, 3);
        transform.GetChild((int)mFruitType).gameObject.SetActive(true);
    }
}
