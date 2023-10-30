using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombController : MonoBehaviour
{
    public GameObject bombPrefab;
    public KeyCode inputKey = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombsRemaining;

    //object pooling
    private List<GameObject> bombPool = new List<GameObject>();
    public int maxBombPoolSize = 2;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;

        for(int i = 0; i < maxBombPoolSize; i++)
        {
            GameObject bomb = Instantiate(bombPrefab);
            bomb.SetActive(false);
            bombPool.Add(bomb);
        }
    }

    private void Update()
    {
        if(bombsRemaining > 0 && Input.GetKeyDown(inputKey))
        {
            PlaceBomb();
        }
    }

    private void PlaceBomb()
    {
        for(int i = 0; i < bombPool.Count; i++)
        {
            GameObject bomb = bombPool[i];
            if(!bomb.activeSelf)
            {
                bomb.GetComponent<Collider2D>().isTrigger = true;
                Vector2 position = transform.position;
                position.x = Mathf.Round(position.x);
                position.y = Mathf.Round(position.y);

                bomb.transform.position = position;
                bomb.SetActive(true);
                bombsRemaining--;

                StartCoroutine(ExplodeBomb(bomb));
                break;
            }
        }
    }

    private IEnumerator ExplodeBomb(GameObject bomb)
    {
        yield return new WaitForSeconds(bombFuseTime);
        bomb.SetActive(false);
        bombsRemaining++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }

}
