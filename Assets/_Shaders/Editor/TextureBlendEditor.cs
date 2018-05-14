using UnityEngine;
using UnityEditor;

public class TextureBlendEditor : ShaderGUI
{
    //Properties
    MaterialProperty blendAmout = null;
    MaterialProperty blendMask = null;

    MaterialProperty firstTexture = null;
    MaterialProperty firstMetallicMap = null;
    MaterialProperty firstGlossinessRange = null;
    MaterialProperty firstMetallicRange = null;
    MaterialProperty firstAO = null;
    MaterialProperty firstBumpMap = null;

    MaterialProperty secondTexture = null;
    MaterialProperty secondMetallicMap = null;
    MaterialProperty secondGlossinessRange = null;
    MaterialProperty secondMetallicRange = null;
    MaterialProperty secondAO = null;
    MaterialProperty secondBumpMap = null;

    MaterialEditor m_MaterialEditor = null;

    bool showFirstProperties = false;
    bool showSecondProperties = false;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        FindProperties(properties);
        m_MaterialEditor = materialEditor;

        //Blend Properties
        m_MaterialEditor.ShaderProperty(blendAmout, "Blend Amout");
        m_MaterialEditor.ShaderProperty(blendMask, "Blend Mask");

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        //Show First Properties
        showFirstProperties = EditorGUILayout.Foldout(showFirstProperties, "Show First Texture Properties");
        if (showFirstProperties)
        {
            m_MaterialEditor.ShaderProperty(firstTexture, "First Texture Map");
            m_MaterialEditor.ShaderProperty(firstMetallicMap, "First Metallic Map");
            m_MaterialEditor.ShaderProperty(firstMetallicRange, "First Metallic Range");
            m_MaterialEditor.ShaderProperty(firstGlossinessRange, "First Glossiness Range");
            m_MaterialEditor.ShaderProperty(firstAO, "First Ambient Occlusion Map");
            m_MaterialEditor.ShaderProperty(firstBumpMap, "First Normal Map");
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        //Show Second Properties
        showSecondProperties = EditorGUILayout.Foldout(showSecondProperties, "Show Second Texture Properties");
        if (showSecondProperties)
        {
            m_MaterialEditor.ShaderProperty(secondTexture, "Second Texture Map");
            m_MaterialEditor.ShaderProperty(secondMetallicMap, "Second Metallic Map");
            m_MaterialEditor.ShaderProperty(secondMetallicRange, "Second Metallic Range");
            m_MaterialEditor.ShaderProperty(secondGlossinessRange, "Second Glossiness Range");
            m_MaterialEditor.ShaderProperty(secondAO, "Second Ambient Occlusion Map");
            m_MaterialEditor.ShaderProperty(secondBumpMap, "Second Normal Map");
        }

    }

    public void FindProperties(MaterialProperty[] properties)
    {
        blendAmout = FindProperty("_blendAmount", properties);
        blendMask = FindProperty("_blendMask", properties);

        firstTexture = FindProperty("_FirstTexture", properties);
        firstMetallicMap = FindProperty("_FirstMetallicMap", properties);
        firstGlossinessRange = FindProperty("_FirstGlossinessRange", properties);
        firstMetallicRange = FindProperty("_FirstMetallicRange", properties);
        firstAO = FindProperty("_FirstAO", properties);
        firstBumpMap = FindProperty("_FirstBumpMap", properties);

        secondTexture = FindProperty("_SecondTexture", properties);
        secondMetallicMap = FindProperty("_SecondMetallicMap", properties);
        secondGlossinessRange = FindProperty("_SecondGlossinessRange", properties);
        secondMetallicRange = FindProperty("_SecondMetallicRange", properties);
        secondAO = FindProperty("_SecondAO", properties);
        secondBumpMap = FindProperty("_SecondBumpMap", properties);
    }
}
