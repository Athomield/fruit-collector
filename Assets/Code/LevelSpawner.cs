using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject mPlatform_P;

    [SerializeField]
    GameObject mBlocker_P;

    [SerializeField]
    BasketController[] mBaskets;

    [SerializeField]
    GameObject mFruit_P;

    [SerializeField]
    Transform mFruitSpawner;

    int mBlockerCount;

    [SerializeField]
    Transform[] mInstructionText;

    void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                bool blockerSpawned = false;

                if(mBlockerCount < 4 && i != 0 && i != 6)
                {
                    if(Random.Range(0,1f) > 0.5f)
                    {
                        Instantiate<GameObject>(mBlocker_P, new Vector3(8 - (j * 4), 0.55f, 5 - (i * 4)), mBlocker_P.transform.rotation);
                        blockerSpawned = true;
                        mBlockerCount++;
                    }
                }

                if(!blockerSpawned)
                Instantiate<GameObject>(mPlatform_P, new Vector3(8 - (j * 4), 0.55f, 5 - (i * 4)), mPlatform_P.transform.rotation);
            }
        }
        List<int> nbrs = new List<int> { 0, 1, 2 };

        for (int i = 0; i < mBaskets.Length; i++)
        {
            int index = Random.Range(0, nbrs.Count);
            mBaskets[i].mFruitType = (FruitController.FruitType)nbrs[index];
            nbrs.RemoveAt(index);
            mBaskets[i].Init();
        }

        StartCoroutine(SpawnFruit());
        StartCoroutine(ShowInscructionText());
    }

    IEnumerator SpawnFruit()
    {
        for (int i = 0; i < 25; i++)
        {
            yield return new WaitForSeconds(0.25f);
            Instantiate<GameObject>(mFruit_P, mFruitSpawner.position + new Vector3(Random.Range(0,1f),0, Random.Range(0, 1f)), Quaternion.identity) ;
        }
    }

    IEnumerator ShowInscructionText()
    { 
        mInstructionText[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        mInstructionText[1].gameObject.SetActive(true);
        mInstructionText[0].gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        mInstructionText[1].gameObject.SetActive(false);
    }


}
