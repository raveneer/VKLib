using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using UnityEditorInternal;

/// <summary>
/// https://gist.github.com/allanolivei/8982445
/// 메시 랜더러의 경우 드로 순서 때문에 스프라이트와는 다른 그리기 순서를 가질 수 있다.
/// 소팅레이어를 이용해서 어떤 오브젝트를 먼저 그릴지를 결정하고 있다면 이게 매우 곤란해질 수 있는데,
/// 메시 랜더러에 소팅 레이어 정보를 노출하여 이 문제를 해결하는 유니티 에디터 확장이다.
/// </summary>
[CustomEditor(typeof(MeshRenderer)), CanEditMultipleObjects]
public class MeshRendererInspector : Editor 
{

	//Armazena Sorting Layer criadas no unity
	private string[] sortingLayerNames;

	//Order
	private int sortingOrder;

	//Layer
	private int sortingLayer;

	//Objetos selecionados
	private MeshRenderer[] renderer;

	//Se todos os objetos selecionado possuem os mesmos valores
	private bool sortingLayerEqual;
	private bool sortingOrderEqual;


	void OnEnable() 
	{
		//Cache de Sorting Layer criadas.
		sortingLayerNames = GetSortingLayerNames();

		//Recupera objetos selecionados
		System.Object[] objects = serializedObject.targetObjects;

		//Armazena valores iniciais
		MeshRenderer first = objects[0] as MeshRenderer;
		sortingOrder = first.sortingOrder;
		string layerName = first.sortingLayerName;
		sortingLayer = Mathf.Max(System.Array.IndexOf( sortingLayerNames, layerName ), 0);

		//Cast
		renderer = new MeshRenderer[objects.Length];
		//Igualdade entre multiobjects
		sortingLayerEqual = true;
		sortingOrderEqual = true;
		for( int i = 0 ; i < objects.Length ; i++ ) 
		{
			//Cast
			renderer[i] = objects[i] as MeshRenderer;
			//Verifica se todos os objetos possuem o mesmo valor
			if( renderer[i].sortingOrder != sortingOrder ) sortingOrderEqual = false;
			if( renderer[i].sortingLayerName != layerName ) sortingLayerEqual = false;
		}
	}

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		EditorGUILayout.Space();

		/**
		 * SORTING Layer
		 **/
		EditorGUI.BeginChangeCheck();
		
		//UI
		EditorGUI.showMixedValue = !sortingLayerEqual;
		sortingLayer = EditorGUILayout.Popup(sortingLayer, sortingLayerNames);
		
		//Aplicar modificacoes e igualar valores
		if( EditorGUI.EndChangeCheck() ) {
			foreach( MeshRenderer r in renderer )
			{
				r.sortingLayerName = sortingLayerNames[sortingLayer];
				EditorUtility.SetDirty(r);
			}
			sortingLayerEqual = true;
		}


		/**
		 * SORTING ORDER
		 **/
		EditorGUI.BeginChangeCheck();

		//UI
		EditorGUI.showMixedValue = !sortingOrderEqual;
		sortingOrder = EditorGUILayout.IntField("Order in Layer", sortingOrder);

		//Aplicar modificacoes e igualar valores
		if( EditorGUI.EndChangeCheck() ) 
		{
			foreach( MeshRenderer r in renderer )
			{
				r.sortingOrder = sortingOrder;
				EditorUtility.SetDirty(r);
			}
			sortingOrderEqual = true;
		}
	}

	public string[] GetSortingLayerNames() 
	{
		Type t = typeof(InternalEditorUtility);
		PropertyInfo prop = t.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[])prop.GetValue(null, null);
	}

}