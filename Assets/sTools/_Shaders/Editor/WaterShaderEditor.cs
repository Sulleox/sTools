using UnityEngine;
using UnityEditor;

public class WaterShaderEditor : ShaderGUI 
{
	//R PROPERTIES
    MaterialProperty RScrollXSpeed = null;
    MaterialProperty RScrollYSpeed = null;
    MaterialProperty RWaterNormal = null;
	MaterialProperty RWaterHeight = null;
	GUIContent RWaterNormalLabel;
	GUIContent RWaterHeightLabel;

	//G PROPERTIES
	MaterialProperty GScrollXSpeed = null;
    MaterialProperty GScrollYSpeed = null;
    MaterialProperty GWaterNormal = null;
	MaterialProperty GWaterHeight = null;
	GUIContent GWaterNormalLabel;
	GUIContent GWaterHeightLabel;

	//B PROPERTIES
	MaterialProperty BScrollXSpeed = null;
    MaterialProperty BScrollYSpeed = null;
    MaterialProperty BWaterNormal = null;
	MaterialProperty BWaterHeight = null;
	GUIContent BWaterNormalLabel;
	GUIContent BWaterHeightLabel;

	//BASE PROPERTIES
	MaterialProperty WaterColor = null;
    MaterialProperty WaveHeight = null;
    MaterialProperty DepthSize = null;

	//FLOW MAP PROPERTIES
	MaterialProperty FlowMap = null;
	MaterialProperty FlowDeformation = null;
	GUIContent FlowMapLabel;

    MaterialEditor m_MaterialEditor = null;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        FindProperties(properties);
        m_MaterialEditor = materialEditor;

		//Wave Properties
		m_MaterialEditor.ShaderProperty(WaterColor, "Water Color");
        m_MaterialEditor.ShaderProperty(WaveHeight, "Wave Height");
        m_MaterialEditor.ShaderProperty(DepthSize, "Depth Size");

		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

		//Flow Map Properties
		m_MaterialEditor.TexturePropertySingleLine(FlowMapLabel , FlowMap);
		m_MaterialEditor.ShaderProperty(FlowDeformation, "Flow Deformation");

		Material m_Mat = m_MaterialEditor.target as Material;
		if(FlowMap.textureValue == null) m_Mat.DisableKeyword("FLOW_MAP");
		else m_Mat.EnableKeyword("FLOW_MAP");

		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        //R Properties
        m_MaterialEditor.ShaderProperty(RScrollXSpeed, "R - X Scroll Speed");
        m_MaterialEditor.ShaderProperty(RScrollYSpeed, "R - Y Scroll Speed");
        m_MaterialEditor.TexturePropertySingleLine(RWaterNormalLabel,RWaterNormal);
		m_MaterialEditor.TextureScaleOffsetProperty(RWaterNormal);
		m_MaterialEditor.TexturePropertySingleLine(RWaterHeightLabel, RWaterHeight);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

		//G Properties
		m_MaterialEditor.ShaderProperty(GScrollXSpeed, "G - X Scroll Speed");
        m_MaterialEditor.ShaderProperty(GScrollYSpeed, "G - Y Scroll Speed");
        m_MaterialEditor.TexturePropertySingleLine(GWaterNormalLabel ,GWaterNormal);
		m_MaterialEditor.TextureScaleOffsetProperty(GWaterNormal);
		m_MaterialEditor.TexturePropertySingleLine(GWaterHeightLabel ,GWaterHeight);

		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

		//B Properties
		m_MaterialEditor.ShaderProperty(BScrollXSpeed, "B - X Scroll Speed");
        m_MaterialEditor.ShaderProperty(BScrollYSpeed, "B - Y Scroll Speed");
        m_MaterialEditor.TexturePropertySingleLine(BWaterNormalLabel ,BWaterNormal);
		m_MaterialEditor.TextureScaleOffsetProperty(BWaterNormal);
		m_MaterialEditor.TexturePropertySingleLine(BWaterHeightLabel ,BWaterHeight);
		
    }

    public void FindProperties(MaterialProperty[] properties)
    {
		//Wave Properties
		WaterColor = FindProperty("_WaterColor", properties);
		WaveHeight = FindProperty("_WaveHeight", properties);
		DepthSize = FindProperty("_DepthSize", properties);

		//Flow Map Properties
		FlowMap = FindProperty("_FlowMap", properties);
		FlowDeformation = FindProperty("_FlowDeformation", properties);
		FlowMapLabel = new GUIContent(FlowMap.displayName);

		//R Properties
        RScrollXSpeed = FindProperty("_RScrollXSpeed", properties);
		RScrollYSpeed = FindProperty("_RScrollYSpeed", properties);
		RWaterNormal = FindProperty("_RWaterNormal", properties);
		RWaterHeight = FindProperty("_RWaterHeight", properties);
		RWaterNormalLabel = new GUIContent(RWaterNormal.displayName);
		RWaterHeightLabel = new GUIContent(RWaterHeight.displayName);

		//G Properties
        GScrollXSpeed = FindProperty("_GScrollXSpeed", properties);
		GScrollYSpeed = FindProperty("_GScrollYSpeed", properties);
		GWaterNormal = FindProperty("_GWaterNormal", properties);
		GWaterHeight = FindProperty("_GWaterHeight", properties);
		GWaterNormalLabel = new GUIContent(GWaterNormal.displayName);
		GWaterHeightLabel = new GUIContent(GWaterHeight.displayName);

		//B Properties
        BScrollXSpeed = FindProperty("_BScrollXSpeed", properties);
		BScrollYSpeed = FindProperty("_BScrollYSpeed", properties);
		BWaterNormal = FindProperty("_BWaterNormal", properties);
		BWaterHeight = FindProperty("_BWaterHeight", properties);
		BWaterNormalLabel = new GUIContent(BWaterNormal.displayName);
		BWaterHeightLabel = new GUIContent(BWaterHeight.displayName);
    }
}
