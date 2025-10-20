using UnityEngine;
using System.Collections;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public int numberOfArrows = 5;   // חצים בכל סבב
    public float maxDelay = 0.5f;    // דיליי רנדומלי בין חץ לחץ
    public float roundInterval = 60f; // סבב חדש כל דקה
    public float spawnHeightOffset = 2f; // כמה מעל המסך החץ מתחיל

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnArrowRoundsForever());
    }

    IEnumerator SpawnArrowRoundsForever()
    {
        while (true)
        {
            // התחלת סבב חצים חדש
            for (int i = 0; i < numberOfArrows; i++)
            {
                float minX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
                float maxX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
                float spawnY = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + spawnHeightOffset;

                float randomX = Random.Range(minX, maxX);
                Vector3 spawnPos = new Vector3(randomX, spawnY, 0);

                float randomDelay = Random.Range(0f, maxDelay);
                StartCoroutine(SpawnArrowWithDelay(spawnPos, randomDelay));
            }

            // מחכים עד לסבב הבא
            yield return new WaitForSeconds(roundInterval);
        }
    }

    IEnumerator SpawnArrowWithDelay(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        Quaternion arrowRotation = Quaternion.Euler(0, 0, -90f);
        Instantiate(arrowPrefab, position, Quaternion.identity);
    }
}
