using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityHexPlanet
{
    [ExecuteAlways]
    public class HexPlanetManagerSaver : MonoBehaviour
    {
        [SerializeField]
        public HexPlanet hexPlanet;
        private HexPlanet _prevHexPlanet;

        // Called when the whole sphere must be regenerated
        public void UpdateRenderObjects()
        {
            // Delete all children
            foreach (Transform child in transform)
            {
                StartCoroutine(Destroy(child.gameObject));
            }

            if (hexPlanet == null)
            {
                return;
            }

            HexPlanetHexGenerator.GeneratePlanetTilesAndChunks(hexPlanet);

            for (int i = 0; i < hexPlanet.chunks.Count; i++)
            {
                GameObject chunkGO = new GameObject("Chunk " + i);
                chunkGO.transform.SetParent(transform);
                chunkGO.transform.localPosition = Vector3.zero;
                MeshFilter mf = chunkGO.AddComponent<MeshFilter>();
                MeshCollider mc = chunkGO.AddComponent<MeshCollider>();

                MeshRenderer mr = chunkGO.AddComponent<MeshRenderer>();
                mr.sharedMaterial = hexPlanet.chunkMaterial;

                HexChunkRenderer hcr = chunkGO.AddComponent<HexChunkRenderer>();
                hcr.SetHexChunk(hexPlanet, i);
                hcr.UpdateMesh();

                // Set layer
                int hexPlanetLayer = LayerMask.NameToLayer("HexPlanet");
                if (hexPlanetLayer == -1)
                {
                    throw new UnassignedReferenceException("Layer \"HexPlanet\" must be created in the Layer Manager!");
                }
                chunkGO.layer = hexPlanetLayer;
            }

            // Create prefab asset
            CreatePrefab();
        }

        IEnumerator Destroy(GameObject go)
        {
            yield return new WaitForSeconds(0.1f);
            DestroyImmediate(go);
        }

        private void CreatePrefab()
        {
            // Get the selected path in the Project window
            string selectedPath = GetSelectedPathOrFallback();
            string prefabPath = $"{selectedPath}/{hexPlanet.name}_HexPlanet.prefab";

            // Create a temporary GameObject to store the hierarchy
            GameObject tempGO = new GameObject("HexPlanetPrefab");
            foreach (Transform child in transform)
            {
                GameObject childCopy = Instantiate(child.gameObject, tempGO.transform);
                childCopy.name = child.name;
            }

            // Save as a prefab
            PrefabUtility.SaveAsPrefabAsset(tempGO, prefabPath);
            DestroyImmediate(tempGO);

            Debug.Log($"Prefab saved at: {prefabPath}");
        }

        private string GetSelectedPathOrFallback()
        {
            // Get the current selection in the Project window
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(path))
            {
                path = "Assets"; // Fallback to Assets root folder
            }
            else if (!System.IO.Directory.Exists(path))
            {
                path = System.IO.Path.GetDirectoryName(path); // Use parent directory if a file is selected
            }

            return path;
        }
    }
}
