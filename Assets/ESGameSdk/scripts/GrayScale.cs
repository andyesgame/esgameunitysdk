using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GrayScale))]
public class GrayScaleEditor : Editor
{
    private SerializedProperty grayScale;
    private void OnEnable()
    {
        grayScale = serializedObject.FindProperty("grayScale");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(grayScale, new GUIContent("grayScale"));
        serializedObject.ApplyModifiedProperties();
        ((GrayScale)target).validate();
    }
}
#endif

public class GrayScale : MonoBehaviour
{
    private bool gScale;
    public bool grayScale;
    private Material grayScaleMaterial;
    private Image image;

    public void EnableGrayScale(bool isGrayScale)
    {
        SetGrayScale(isGrayScale);
    }

    public void validate()
    {
        if (image == null) image = GetComponent<Image>();
        if (image != null)
        {
            if (!grayScale) image.material = null;
            else
            {
                if (grayScaleMaterial == null)
                    grayScaleMaterial = Resources.Load<Material>("ESShader/Grayscale");
                
                image.material = grayScaleMaterial;
            }
        }
    }

    public bool SetGrayScale(bool isGrayScale)
    {
        if (gScale != isGrayScale)
        {
            gScale = isGrayScale;
            validate();
            return true;
        }
        else return false;
    }
}
