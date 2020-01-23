using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
public void RestartNOW(){
	SceneManager.LoadScene(0);
}
 public void Update(){
 if (Input.GetKeyDown(KeyCode.Alpha1)){
RestartNOW();	
}
}
}
