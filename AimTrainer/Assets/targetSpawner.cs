using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class targetSpawner : MonoBehaviour
{
    List<GameObject> childs;
    TargetManager targetManager;
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject myPrefab;
    private int timer;
    private float far = 29.41f;
    public float minX = -6.92f;
    public float maxX = 11.7f;
    public float minY = 0f;
    public float maxY = 5.97f;
    private int spawnDelay = 4;
    private bool added;
    private float timeVar;
    public TextMeshProUGUI time;
    public TextMeshProUGUI targetsLabel;
    public TextMeshProUGUI remaining;
    public TextMeshProUGUI dificultadLabel;

    // Start is called before the first frame update
    void Start()
    {
        targetManager = new TargetManager(this.gameObject);
        childs = GetChildren(this.gameObject);
        Debug.Log(childs.Count);
        int dificultad = PlayerPrefs.GetInt("dificultad");
        if (dificultad == 0)
        {
            dificultadLabel.text = "Dificultad: Facil";
            spawnDelay = 3;
            far += 5f;
        }
        else if (dificultad == 1)
        {
            dificultadLabel.text = "Dificultad: Medio";
            spawnDelay = 3;
            far += 10f;
        }
        else if (dificultad == 2)
        {
            dificultadLabel.text = "Dificultad: Dificil";
            spawnDelay = 4;
            far += 15f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeVar += Time.deltaTime;
        time.text = string.Format("Time: {0}", Math.Floor(timeVar));

        if (targetManager.getLabel() == null)
        {
            targetManager.setTargetsLabel(targetsLabel, remaining);
        }
        timer += 1;
        double timeToSpawn = Math.Floor(timeVar) % spawnDelay;
        if (timeToSpawn == 0 && !added)
        {
            targetManager.addTarget(new Target(targetManager, new Vector3(minX, minY, far), myPrefab, 6, 
                new Vector3(minX, minY, far),
                new Vector3(maxX, maxY, far)));
            timer = 0;
            added = true;
        }
        else if(timeToSpawn != 0)
        {
            added = false;
        }
    }

    void FixedUpdate()
    {
        targetManager.tick();
    }

    public float lerp(float start, float end, float percent)
    {
        return (start + percent * (end - start));
    }

    public TargetManager getTargetManager()
    {
        return targetManager;
    }

    public class TargetManager
    {
        List<Target> targets;
        GameObject parent;
        System.Random random;
        TextMeshProUGUI targetsLabel;
        private TextMeshProUGUI remaining;
        private int remainingTargets;
        private int currentTargetsCount;
        private int totalEnemies;
        private float speed;
        private float progress;
        private float progressSpeed;
        private float savedProgressSpeed;
        private float duration;
        private bool pauseV;

        public TargetManager(GameObject parent)
        {
            targets = new List<Target>();
            this.speed = 1;
            this.duration = 10;
            this.remainingTargets = 20;
            this.totalEnemies = 20;
            int dificultad = PlayerPrefs.GetInt("dificultad");
            this.progressSpeed = 5;
            if (dificultad == 0)
            {
                this.progressSpeed = 3;
            }
            else if(dificultad == 1)
            {
                this.progressSpeed = 6;
            } 
            else if(dificultad == 2)
            {
                this.progressSpeed = 10;
            }
            Debug.Log("Dificultad: " + dificultad);
            this.parent = parent;
            random = new System.Random();
            random.Next(10, 20);
        }

        public void setTargetsLabel(TextMeshProUGUI targetsLabel, TextMeshProUGUI remainingTargetsLabel)
        {
            this.targetsLabel = targetsLabel;
            this.remaining = remainingTargetsLabel;
        }

        public TextMeshProUGUI getLabel()
        {
            return targetsLabel;
        }

        public void pause()
        {
            this.savedProgressSpeed = progressSpeed;
            this.progressSpeed = 0;
            pauseV = true;
            Debug.Log("Pause");
        }

        public void resume()
        {
            pauseV = false;
            this.progressSpeed = savedProgressSpeed;
            Debug.Log("Resume");
        }

        public void tick()
        {
            if (!pauseV)
            {
                List<int> toRemove = new List<int>();
                if (progress < duration)
                {
                    progress += Time.deltaTime * progressSpeed;
                    if (progress > duration)
                    {
                        progress = duration;
                    }
                }
                else
                {
                    for (int i = 0; i < targets.Count; i++)
                    {
                        if (targets[i] != null)
                        {
                            targets[i].onCycleEnds();
                        }
                    }
                    progress = 0;
                }
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i] != null)
                    {
                        targets[i].tick();
                    }
                    if (targets[i].isDead())
                    {
                        toRemove.Add(i);
                    }
                }
                for (int i = 0; i < toRemove.Count; i++)
                {
                    GameObject obj = targets[toRemove[i]].getGameObject();
                    targets.RemoveAt(toRemove[i]);
                    Destroy(obj);
                    targetsLabel.text = string.Format("Targets: {0}", targets.Count);
                }
            }
        }

        public void addTarget(Target target)
        {
            if (!pauseV && currentTargetsCount < totalEnemies) {
                currentTargetsCount++;
                remainingTargets--;
                remaining.text = string.Format("Remaining Targets: {0}", remainingTargets);
                target.getGameObject().transform.parent = parent.transform;
                targets.Add(target);
                targetsLabel.text = string.Format("Targets: {0}", targets.Count);
            }
        }

        public System.Random getRandom()
        {
            return random;
        }

        public float getSpeed()
        {
            return speed;
        }

        public float getProgress()
        {
            return progress / duration;
        }

        public float getDuration()
        {
            return duration;
        }

    }

    public class Target
    {
        TargetManager manager;
        Vector3 pos;
        List<Vector3> pattern;
        GameObject gmo;
        Vector3 current;
        Vector3 old;
        int currentIndex;

        public Target(TargetManager manager, GameObject prefab, int patternSize, Vector3 min, Vector3 max)
        {
            this.manager = manager;
            pattern = generatePattern(patternSize, min, max);
            pos = pattern[0];
            // 17.46f, 0f, 39.41f non visible position
            // Instantiate at a given position and zero rotation.
            gmo = Instantiate(prefab, pos, Quaternion.identity);
            
        }

        public Target(TargetManager manager, Vector3 vec, GameObject prefab, int patternSize, Vector3 min, Vector3 max)
        {
            this.manager = manager;
            pos = new Vector3(17.46f, 0f, 39.41f);//new Vector3(vec.x, vec.y, vec.z);
            pattern = generatePattern(patternSize, min, max);
            // Instantiate at a given position and zero rotation.
            gmo = Instantiate(prefab, pos, Quaternion.identity);
            if (patternSize >= 2)
            {
                old = new Vector3(pos.x, pos.y, pos.z);
                current = pattern[currentIndex];
            }
        }

        public void tick()
        {
            if(old != null && current != null)
            {
                pos = Vector3.Lerp(old, current, manager.getProgress());
            }
            //pos.x += Time.deltaTime * manager.getSpeed();
            gmo.GetComponent<Rigidbody>().MovePosition(pos);
        }

        public void onCycleEnds()
        {
            currentIndex++;
            if(currentIndex >= pattern.Count)
            {
                currentIndex = 0;
            }
            selectNewPatternTarget();
        }

        private void selectNewPatternTarget()
        {
            old = new Vector3(current.x, current.y, current.z);
            current = pattern[currentIndex];
        }

        public bool isDead()
        {
            return gmo.GetComponent<TargetScript>().isDead();
        }

        public List<Vector3> generatePattern(int patternSize, Vector3 min, Vector3 max)
        {
            List<Vector3> pattern = new List<Vector3>();
            for (int i = 0; i < patternSize; i++)
            {
                int x = manager.getRandom().Next((int)min.x, (int)max.x);
                int y = manager.getRandom().Next((int)min.y, (int)max.y);
                int z = manager.getRandom().Next((int)min.z, (int)max.z);
                Vector3 vec = new Vector3(x, y, z);
                pattern.Add(vec);
            }
            return pattern;
        }

        public GameObject getGameObject()
        {
            return gmo;
        }
    }

    public List<GameObject> GetChildren(GameObject go)
    {
        List<GameObject> list = new List<GameObject>();
        return GetChildrenHelper(go, list);
    }

    private List<GameObject> GetChildrenHelper(GameObject go, List<GameObject> list)
    {
        if (go == null || go.transform.childCount == 0)
        {
            return list;
        }
        foreach (Transform t in go.transform)
        {
            list.Add(t.gameObject);
            GetChildrenHelper(t.gameObject, list);
        }
        return list;
    }
}