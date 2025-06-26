using System;
using UnityEngine;
using System.Collections.Generic;
using CustomSystem.Pooling;
using CustomSystem.Singleton;
using System.Linq;

#if UNITY_EDITOR
using CustomSystem.GenerateEnum;
using UnityEditor;
#endif

/// <summary>
/// Class pour remplacer les fonctions Instantiate & Destroy par des fonctions de poolling
/// </summary>
public class PoollingSystem : SingletonSystem<PoollingSystem>
{
    public ScriptablePoolling[] ScriptObject = new ScriptablePoolling[0]; // Liste des différant pool d'entité
    public Poolling[] poolling = new Poolling[0];

    [Serializable]
    public class Poolling
    {
        [HideInInspector] public string Name;
        public List<GameObject> allEntities = new List<GameObject>(); // La liste contenant toute les entit�s pouvant �tre pool
        [HideInInspector] public Transform holder;
        [HideInInspector] public PoollingEntityType entityType;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    #region Private Function

    /// <summary>
    /// Créer chaque différentes entités pouvant être pool dans un holder 
    /// </summary>
    public void CreateEatchPoolling()
    {
#if UNITY_EDITOR
        poolling = new Poolling[ScriptObject.Length];

        GameObject allHolder = new GameObject($"Poolling Holder");
        allHolder.transform.Cast<Transform>().ToList().ForEach(t => DestroyImmediate(t.gameObject));

        for (int i = 0; i < ScriptObject.Length; i++)
        {
            GameObject holder = new GameObject($"Holder of {ScriptObject[i].customEntityType} ({ScriptObject[i].nmbOfEntity}) ");
            holder.transform.parent = allHolder.transform;

            poolling[i] = new Poolling();
            poolling[i].Name = ScriptObject[i].Name;
            poolling[i].holder = holder.transform;
            poolling[i].entityType = ScriptObject[i].entityType;

            for (int a = 0; a < ScriptObject[i].nmbOfEntity; a++)
            {
                GameObject entity = (GameObject)PrefabUtility.InstantiatePrefab(ScriptObject[i].entity, poolling[i].holder);
                poolling[i].allEntities.Add(entity);
                entity.SetActive(false);
            }
        }
#endif
    }

    /// <summary>
    /// Cherche le type d'entité (Enum) correspondant au string
    /// </summary>
    /// <param name="enumName">Le type d'entité voulue</param>
    /// <returns>Le type  d'entité (Enum) correspondant au string (retourne DEFAULT si non trouvé)</returns>
    PoollingEntityType GetEnumType(string enumName)
    {
        for (int i = 0; i < Enum.GetValues(typeof(PoollingEntityType)).Length; i++)
        {
            if (((PoollingEntityType)i).ToString() == enumName)
            {
                return (PoollingEntityType)i;
            }
        }
        Debug.LogWarning($"{typeof(PoollingSystem).Name} : The type {enumName} not exist, the type DEFAULT is attribuate");
        return PoollingEntityType.DEFAULT;
    }

    /// <summary>
    /// Retourne le type d'entité voulue
    /// </summary>
    /// <param name="entityType">Le type d'entité voulue</param>
    /// <returns>La première entité activé (Retourne null si en dehors de la range accesible)</returns>
    GameObject GetEntity(PoollingEntityType entityType)
    {
        for (int i = 0; i < poolling.Length; i++)
        {
            if (poolling[i].entityType == entityType)
            {
                for (int a = 0; a < poolling[i].allEntities.Count; a++)
                {
                    if (!poolling[i].allEntities[a].activeSelf)
                    {
                        return poolling[i].allEntities[a];
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Retourne le conteneur de l'entité renseigné
    /// </summary>
    /// <param name="entity">L'entité voulant trouver le conteneur</param>
    /// <returns>Le transform contenant l'entité (si non trouvé retourne Null)</returns>
    Transform GetHolder(GameObject entity)
    {
        for (int i = 0; i < poolling.Length; i++)
        {
            if (poolling[i].allEntities.Contains(entity))
            {
                return poolling[i].holder;
            }
        }
        return null;
    }

    #endregion

    #region Public Function

    /// <summary>
    /// Désactive toute les entités 
    /// </summary>
    public void ClearAllEntities()
    {
        for (int i = 0; i < ScriptObject.Length; i++)
        {
            for (int a = 0; a < poolling[i].allEntities.Count; a++)
            {
                Merge(poolling[i].allEntities[a]);
            }
        }
    }

    /// <summary>
    /// Récupère une entité diponible 
    /// </summary>
    /// <param name="entityType">Le type d'entité</param>
    public GameObject Pool(PoollingEntityType entityType)
    {
        GameObject entity = GetEntity(entityType);
        if (entity == null) { Debug.LogWarning($"{typeof(PoollingSystem).Name} : The pooling of {entityType} is out of range, can't not be pool "); return null; }
        entity.transform.position = Vector3.zero;
        entity.SetActive(true);
        return entity;
    }

    /// <summary>
    /// Récupère une entité diponible 
    /// </summary>
    /// <param name="entityType">Le type d'entité</param>
    /// <param name="newParent">Le nouveau parent assigné a l'entité</param>
    public GameObject Pool(PoollingEntityType entityType, Transform newParent)
    {
        GameObject entity = GetEntity(entityType);
        if (entity == null) { Debug.LogWarning($"{typeof(PoollingSystem).Name} : The pooling of {entityType} is out of range, can't not be pool "); return null; }
        entity.transform.position = Vector3.zero;
        entity.transform.parent = newParent;
        entity.SetActive(true);
        return entity;
    }

    /// <summary>
    /// Récupère une entité diponible 
    /// </summary>
    /// <param name="entityType">Le type d'entité</param>
    /// <param name="newPos">La nouvelle positions de l'entité</param>
    public GameObject Pool(PoollingEntityType entityType, Vector3 newPos)
    {
        GameObject entity = GetEntity(entityType);
        if (entity == null) { Debug.LogWarning($"{typeof(PoollingSystem).Name} : The pooling of {entityType} is out of range, can't not be pool "); return null; }
        entity.transform.position = newPos;
        entity.SetActive(true);
        return entity;
    }

    /// <summary>
    /// Récupère une entité diponible 
    /// </summary>
    /// <param name="entityType">Le type d'entité</param>
    /// <param name="newPos">La nouvelle positions de l'entité</param>
    /// <param name="newRot">La nouvelle rotations de l'entité</param>
    public GameObject Pool(PoollingEntityType entityType, Vector3 newPos, Quaternion newRot)
    {
        GameObject entity = GetEntity(entityType);
        if (entity == null) { Debug.LogWarning($"{typeof(PoollingSystem).Name} : The pooling of {entityType} is out of range, can't not be pool "); return null; }
        entity.transform.position = newPos;
        entity.transform.rotation = newRot;
        entity.SetActive(true);
        return entity;
    }

    /// <summary>
    /// Récupère une entité diponible 
    /// </summary>
    /// <param name="entityType">Le type d'entité</param>
    /// <param name="newPos">La nouvelle positions de l'entité</param>
    /// <param name="newRot">La nouvelle rotations de l'entité</param>
    /// <param name="newParent">Le nouveau parent assigné a l'entité</param>
    public GameObject Pool(PoollingEntityType entityType, Vector3 newPos, Quaternion newRot, Transform newParent)
    {
        GameObject entity = GetEntity(entityType);
        if (entity == null) { Debug.LogWarning($"{typeof(PoollingSystem).Name} : The pooling of {entityType} is out of range, can't not be pool "); return null; }
        entity.transform.parent = newParent;
        entity.transform.position = newPos;
        entity.transform.rotation = newRot;
        entity.SetActive(true);
        return entity;
    }

    /// <summary>
    /// Range l'entité renseignée
    /// </summary>
    /// <param name="entity">L'entité a ranger</param>
    public void Merge(GameObject entity)
    {
        entity.transform.position = Vector3.zero;
        entity.transform.rotation = Quaternion.identity;
        Transform holder = GetHolder(entity);
        if (holder == null) { Debug.LogWarning($"{typeof(PoollingSystem).Name} : The holder of the {entity} is not found, can't not be merge "); return; }
        entity.transform.parent = holder;
        entity.SetActive(false);
    }

    #endregion
}


