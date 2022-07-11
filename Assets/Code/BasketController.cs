using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    public FruitController.FruitType mFruitType;//type of fruit can be thrown in the basket

    Transform mFruitHolder;
    public void Init()
    {
        mFruitHolder = transform.Find("basket_mesh").GetChild(0);

        mFruitHolder.GetChild((int)mFruitType).gameObject.SetActive(true);
    }

    private void Update()
    {
        if(mFruitHolder)
        mFruitHolder.Rotate(Vector3.up * 60 * Time.deltaTime );
    }
}
