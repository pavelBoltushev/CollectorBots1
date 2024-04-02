using System.Collections;
using UnityEngine;

public class MineralSpawner : MonoBehaviour
{
    [SerializeField] Mineral _template;
    [SerializeField] float _spawnZoneWidth;
    [SerializeField] float _spawnPeriodicity;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        var wait = new WaitForSeconds(_spawnPeriodicity);

        while (true)
        {
            float spawnXPosition = Random.Range(transform.position.x - _spawnZoneWidth, transform.position.x + _spawnZoneWidth);
            float spawnZPosition = Random.Range(transform.position.z - _spawnZoneWidth, transform.position.z + _spawnZoneWidth);
            Vector3 spawnPosition = new Vector3(spawnXPosition, transform.position.y, spawnZPosition);
            Instantiate(_template, spawnPosition, Quaternion.identity);
            yield return wait;
        }
    }
}
