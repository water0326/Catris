using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Particle {
    public string name;
    public GameObject prefab;
    public int init_count;
}

public class ParticlePool : MonoBehaviour
{

    public static ParticlePool instance;

    [SerializeField]
    float effectSize = 0.4f;

    [SerializeField]
    Particle[] particles;
    [SerializeField]
    Transform parent;

    [SerializeField]
    Transform activedParticleTransform;

    Dictionary<string, Queue<GameObject>> queue;
    Dictionary<string, Transform> parents;
    List<GameObject> activedParticles;

    private void Awake() {
        effectSize = effectSize / 1440 * StaticDataManager.Instance.data.resolutionY;
        instance = this;
        queue = new Dictionary<string, Queue<GameObject>>();
        parents = new Dictionary<string, Transform>();
        activedParticles = new List<GameObject>();
        for(int i = 0 ; i < particles.Length ; i++) {
            queue.Add(particles[i].name, new Queue<GameObject>());
            GameObject tempParent = new GameObject(particles[i].name + " Pool");
            tempParent.transform.SetParent(parent);
            parents.Add(particles[i].name, tempParent.transform);
            for(int j = 0 ; j < particles[i].init_count ; j++) {
                CreateParticle(particles[i]);
            }
        }
    }

    void CreateParticle(Particle particle) {
        GameObject obj = Instantiate(particle.prefab);
        obj.transform.SetParent(parents[particle.name]);
        obj.SetActive(false);
        obj.name = particle.name;
        queue[particle.name].Enqueue(obj);
    }

    public void SetParticle(string name, int x, int y) {
        Particle particle = GetParticleByName(name);
        if(particle == null) {
            Debug.LogError("There isn't any particle which name is " + name);
            return;
        }
        if(queue[particle.name].Count == 0) {
            CreateParticle(particle);
        }
        GameObject obj = queue[particle.name].Dequeue();
        obj.transform.SetParent(activedParticleTransform);
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = BlockGrid.Instance.GetParticlePos(x, y-0.5f);
        rectTransform.localScale = new Vector3(effectSize, effectSize, effectSize);
        obj.SetActive(true);
        activedParticles.Add(obj);
    }

    Particle GetParticleByName(string name) {
        for(int i = 0 ; i < particles.Length ; i++) {
            if(particles[i].name == name) {
                return particles[i];
            }
        }
        return null;
    }

    void ReturnObject(GameObject obj) {
        obj.transform.SetParent(parents[obj.name]);
        queue[obj.name].Enqueue(obj);
    }

    private void Update() {
        for(int i = 0 ; i < activedParticles.Count ; i++) {
            if(!activedParticles[i].activeSelf) {
                ReturnObject(activedParticles[i]);
                activedParticles.RemoveAt(i);
                i--;
            }
        }
    }
}
