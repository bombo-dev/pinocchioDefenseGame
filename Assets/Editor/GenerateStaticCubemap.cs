using UnityEngine;
using UnityEditor;
using System.Collections;

public class GenerateStaticCubemap : ScriptableWizard {


	public Transform renderPosition;
	public Cubemap cubemap;
	// Use this for initialization 
	void OnWizardUpdate () {


		helpString = "select transform to render" + "from and cubemap to render into";
		if (helpString != null && cubemap != null) 
		{
			isValid = true;
		}
		else 
		{
			isValid = false;
		}
	}


	void OnWizardCreate()
	{
		//렌더링을 위한 임시 카메라 생성 
		GameObject go = new GameObject ("CubeCam", typeof(Camera));

		//카메라를 렌더링 위치에 놓는다. 
		go.transform.position = renderPosition.position;
		go.transform.rotation = Quaternion.identity;

		//큐브맵 렌더링 
		go.GetComponent<Camera>().RenderToCubemap (cubemap);

		//임시카메라 제거 
		DestroyImmediate (go);
	}


	
	[MenuItem("Make Cubemap/ Render Cubemap")]
	static void RenderCubemap(){
		
		ScriptableWizard.DisplayWizard ("Render CubeMap", typeof(GenerateStaticCubemap), "Render!");
		
	}



	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
