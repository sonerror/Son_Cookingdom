using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.FoodStall
{
    public class MaterialInstancer : MonoBehaviour
    {
        [Header("Material gốc (source)")]
        public Material sourceMaterial;

        // Bản sao tạo ra lúc Start
        private Material instanceMaterial;

        public void InitMaterial()
        {
            if (sourceMaterial == null)
            {
                Debug.LogWarning("Chưa gán Source Material!");
                return;
            }

            // Tạo bản sao riêng
            instanceMaterial = new Material(sourceMaterial);
            
            // Lấy tất cả SpriteRenderer trong object và con của nó
            SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>(true);

            foreach (var r in renderers)
            {
                r.material = instanceMaterial;
            }
        }

        /// <summary>
        /// Trả lại material mặc định (Sprites-Default) cho tất cả SpriteRenderer con
        /// </summary>

        [Button]
        public void ResetToDefaultMaterial()
        {
            SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>(true);

            foreach (var r in renderers)
            {
                // Cách 1: Gán null -> Unity sẽ tự dùng "Sprites-Default"
                r.material = null;

                // Nếu muốn chắc chắn:
                // r.material = r.sharedMaterial; // sharedMaterial thường là Sprites-Default
            }

            // Giải phóng instanceMaterial nếu cần
            if (instanceMaterial != null)
            {
                Destroy(instanceMaterial);
                instanceMaterial = null;
            }
        }

        /// <summary>
        /// Cho phép thay đổi tham số material ở ngoài code
        /// </summary>
        public void SetFloat(string property, float value)
        {
            if (instanceMaterial != null)
                instanceMaterial.SetFloat(property, value);
        }

        public void SetColor(string property, Color value)
        {
            if (instanceMaterial != null)
                instanceMaterial.SetColor(property, value);
        }

        public void SetTexture(string property, Texture value)
        {
            if (instanceMaterial != null)
                instanceMaterial.SetTexture(property, value);
        }

        public Material GetInstanceMaterial()
        {
            return instanceMaterial;
        }
    }
}
