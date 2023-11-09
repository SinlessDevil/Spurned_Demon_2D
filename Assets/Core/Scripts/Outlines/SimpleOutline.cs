using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Outlines
{
    [DisallowMultipleComponent]
    public class SimpleOutline : MonoBehaviour
    {
        private static HashSet<Mesh> _registeredMeshes = new();

        public Mode OutlineMode
        {
            get { return _outlineMode; }
            set
            {
                _outlineMode = value;
                _needsUpdate = true;
            }
        }
        public Color OutlineColor
        {
            get { return _outlineColor; }
            set
            {
                _outlineColor = value;
                _needsUpdate = true;
            }
        }
        public float OutlineWidth
        {
            get { return _outlineWidth; }
            set
            {
                _outlineWidth = value;
                _needsUpdate = true;
            }
        }

        [Serializable]
        private class ListVector3
        {
            public List<Vector3> data;
        }

        [Header("---- Optional ----")]
        [SerializeField] private Mode _outlineMode;
        [SerializeField] private Color _outlineColor = Color.white;
        [SerializeField, Range(0f, 10f)] private float _outlineWidth = 2f;

        [SerializeField, Tooltip("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. "
        + "Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")]
        private bool _precomputeOutline;

        [SerializeField, HideInInspector] private List<Mesh> _bakeKeys = new();
        [SerializeField, HideInInspector] private List<ListVector3> _bakeValues = new();

        private Renderer[] _renderers;
        private Material _outlineMaskMaterial;
        private Material _outlineFillMaterial;

        private bool _needsUpdate;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<Renderer>();

            _outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineMask"));
            _outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineFill"));

            _outlineMaskMaterial.name = "OutlineMask (Instance)";
            _outlineFillMaterial.name = "OutlineFill (Instance)";

            LoadSmoothNormals();

            _needsUpdate = true;
        }
        private void LoadSmoothNormals()
        {
            foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
            {
                if (!_registeredMeshes.Add(meshFilter.sharedMesh)){
                    continue;
                }

                var index = _bakeKeys.IndexOf(meshFilter.sharedMesh);
                var smoothNormals = (index >= 0) ? _bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);
                meshFilter.sharedMesh.SetUVs(3, smoothNormals);


                var renderer = meshFilter.GetComponent<Renderer>();
                if (renderer != null){
                    CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
                }
            }

            foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (!_registeredMeshes.Add(skinnedMeshRenderer.sharedMesh)){
                    continue;
                }

                skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
                CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);
            }
        }
        private List<Vector3> SmoothNormals(Mesh mesh)
        {
            var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);
            var smoothNormals = new List<Vector3>(mesh.normals);

            foreach (var group in groups){
                if (group.Count() == 1){
                    continue;
                }

                var smoothNormal = Vector3.zero;

                foreach (var pair in group){
                    smoothNormal += smoothNormals[pair.Value];
                }

                smoothNormal.Normalize();

                foreach (var pair in group){
                    smoothNormals[pair.Value] = smoothNormal;
                }
            }

            return smoothNormals;
        }
        private void CombineSubmeshes(Mesh mesh, Material[] materials)
        {
            if (mesh.subMeshCount == 1){
                return;
            }

            if (mesh.subMeshCount > materials.Length){
                return;
            }

            mesh.subMeshCount++;
            mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
        }

        private void OnEnable()
        {
            foreach (var renderer in _renderers)
            {
                var materials = renderer.sharedMaterials.ToList();

                materials.Add(_outlineMaskMaterial);
                materials.Add(_outlineFillMaterial);

                renderer.materials = materials.ToArray();
            }
        }

        private void OnValidate()
        {
            _needsUpdate = true;

            if (!_precomputeOutline && _bakeKeys.Count != 0 || _bakeKeys.Count != _bakeValues.Count){
                _bakeKeys.Clear();
                _bakeValues.Clear();
            }

            if (_precomputeOutline && _bakeKeys.Count == 0){
                Bake();
            }
        }
        private void Bake()
        {
            var bakedMeshes = new HashSet<Mesh>();

            foreach (var meshFilter in GetComponentsInChildren<MeshFilter>()){
                if (!bakedMeshes.Add(meshFilter.sharedMesh)){
                    continue;
                }
                var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

                _bakeKeys.Add(meshFilter.sharedMesh);
                _bakeValues.Add(new ListVector3() { data = smoothNormals });
            }
        }

        private void Update()
        {
            if (_needsUpdate)
            {
                _needsUpdate = false;

                UpdateMaterialProperties();
            }
        }
        private void UpdateMaterialProperties()
        {
            _outlineFillMaterial.SetColor("_OutlineColor", _outlineColor);

            switch (_outlineMode)
            {
                case Mode.OutlineAll:
                    _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat("_OutlineWidth", _outlineWidth);
                    break;

                case Mode.OutlineVisible:
                    _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    _outlineFillMaterial.SetFloat("_OutlineWidth", _outlineWidth);
                    break;

                case Mode.OutlineHidden:
                    _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                    _outlineFillMaterial.SetFloat("_OutlineWidth", _outlineWidth);
                    break;

                case Mode.OutlineAndSilhouette:
                    _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat("_OutlineWidth", _outlineWidth);
                    break;

                case Mode.SilhouetteOnly:
                    _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                    _outlineFillMaterial.SetFloat("_OutlineWidth", 0f);
                    break;
            }
        }

        private void OnDisable()
        {
            foreach (var renderer in _renderers)
            {
                var materials = renderer.sharedMaterials.ToList();

                materials.Remove(_outlineMaskMaterial);
                materials.Remove(_outlineFillMaterial);

                renderer.materials = materials.ToArray();
            }
        }
        private void OnDestroy()
        {
            Destroy(_outlineMaskMaterial);
            Destroy(_outlineFillMaterial);
        }
    }
}