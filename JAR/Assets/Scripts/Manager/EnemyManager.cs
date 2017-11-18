using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : ManagerTemplate<EnemyManager> {

    [SerializeField]
    List<Transform> enemyList = new List<Transform>();

    public Transform enemySpawnPos1;

	// Use this for initialization
	void Start () {
		for(int i=0; i< transform.childCount; i++)
        {
            enemyList.Add(transform.GetChild(i));
        }
	}
	
	// Update is called once per frame
	void Update () {

        updateEnemy();


        if (enemyList.Count <= 0)
        {
            SpawnEnemy("Enemy_slime", enemySpawnPos1.position);
        }
	}

    void SpawnEnemy(string prefabName, Vector3 pos)
    {
        GameObject enemy = Resources.Load("Prefabs/Enemy/" + prefabName) as GameObject;

        GameObject enemyInstance = MonoBehaviour.Instantiate(enemy) as GameObject;

        enemyInstance.transform.position = pos;
        enemyList.Add(enemyInstance.transform);

        // Effect
        GameObject effectPrefab = Resources.Load("Prefabs/Effects/SpawnRed") as GameObject;

        GameObject SpawnRed = MonoBehaviour.Instantiate(effectPrefab) as GameObject;

        Vector3 particleSpawnPos = pos;

        SpawnRed.GetComponent<Transform>().position = particleSpawnPos;
        SpawnRed.transform.SetParent(this.transform);
    }

    void updateEnemy()
    {
        for (int i = enemyList.Count - 1; i >= 0; i--)
        {
            if (enemyList[i].GetComponent<EnemyUnitInfo>().getIsDead())
            {

                //TODO: EFFECTMANAGER
                GameObject effectPrefab = Resources.Load("Prefabs/Effects/CubeExplosionGreen") as GameObject;

                GameObject ExplosionGreen = MonoBehaviour.Instantiate(effectPrefab) as GameObject;

                Vector3 particleSpawnPos = enemyList[i].transform.position;

                ExplosionGreen.GetComponent<Transform>().position = particleSpawnPos;

                GameObject haveToDel = enemyList[i].gameObject;
                enemyList.Remove(enemyList[i]); // Safe to do
                Destroy(haveToDel);

            }
        }
    }
}
