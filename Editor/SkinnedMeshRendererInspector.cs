using System;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// https://gist.github.com/allanolivei/8982445
/// 메시 랜더러의 경우 드로 순서 때문에 스프라이트와는 다른 그리기 순서를 가질 수 있다.
/// 소팅레이어를 이용해서 어떤 오브젝트를 먼저 그릴지를 결정하고 있다면 이게 매우 곤란해질 수 있는데,
/// 메시 랜더러에 소팅 레이어 정보를 노출하여 이 문제를 해결하는 유니티 에디터 확장이다.
/// hack : SkinnedMeshRendererInspector 의 복붙임. 둘을 합칠 수 있으면 좋겠는데. 아마 합칠 수 있을 것이다. 일단 되나부터 보자.
/// todo : 둘을 합칠 것. 근데 어떻게 합칠지는 잘 모르겠다... mesh renderer와 skinned mesh renderer가 render 에서 상속받기 때문에,
/// todo : 그냥 renderer에 대해 이 코드를 수행하면 '모든 렌더러' 에 소팅레이어 표시가 추가되므로 부작용이 우려된다. 
/// </summary>
[CustomEditor(typeof(SkinnedMeshRenderer)), CanEditMultipleObjects]
public class SkinnedMeshRendererInspector : Editor 
{
    //Armazena Sorting Layer criadas no unity
    private string[] sortingLayerNames;

    //Order
    private int sortingOrder;

    //Layer
    private int sortingLayer;

    //Objetos selecionados
    private SkinnedMeshRenderer[] renderer;

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
        SkinnedMeshRenderer first = objects[0] as SkinnedMeshRenderer;
        sortingOrder = first.sortingOrder;
        string layerName = first.sortingLayerName;
        sortingLayer = Mathf.Max(System.Array.IndexOf( sortingLayerNames, layerName ), 0);

        //Cast
        renderer = new SkinnedMeshRenderer[objects.Length];
        //Igualdade entre multiobjects
        sortingLayerEqual = true;
        sortingOrderEqual = true;
        for( int i = 0 ; i < objects.Length ; i++ ) 
        {
            //Cast
            renderer[i] = objects[i] as SkinnedMeshRenderer;
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
            foreach( SkinnedMeshRenderer r in renderer )
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
            foreach( SkinnedMeshRenderer r in renderer )
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