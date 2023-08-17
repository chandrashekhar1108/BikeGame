using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour 
{
	public static LoadingManager myScript;

	public static string SceneName = "";

	AsyncOperation AsyncOp;
	bool Is_ReadyToLoad = false;
	
	float timerVal = 0;
	float targeTimer = 1;

	public Text Text_Loading;
	public float LoadingPer;

	[SerializeField]
	public LoadingProgressBar ProgressBar;

	public GameObject BG;
	public static int LoadingBGCount=0;
	public bool Is_ChangeSprite;
	public Sprite[] LaodingSprites;

	public string DefaultScene;


	public Text Text_Hint;
	public string[] _sHintStrings;
	public string _sCurrentHint;


	void Awake()
	{
		if (Time.timeScale == 0)
		{
			Time.timeScale = 1;
		}
		myScript =this;

		
		if (PlayerPrefs.GetInt("CurrentScene") == 1)
		{
			DefaultScene = "Menu";
			PlayerPrefs.DeleteKey("CurrentScene");
		}
		else if (PlayerPrefs.GetInt("CurrentScene") == 2)
		{
			DefaultScene = "Gameplay";
			PlayerPrefs.DeleteKey("CurrentScene");
		}
		else
		{
			DefaultScene = "Menu";
		}

	}

	void Start ()
	{
		Is_ReadyToLoad = false;
		timerVal = 0f;
		
		Text_Loading.text = "LOADING ";
		Invoke ("LoadNextScene", 2f);
		LoadingPer = 0;
		ProgressBar.mf_Percentage = 0;

		if (LaodingSprites.Length != 0)
		{
			checkSprite ();
		}

		if (_sHintStrings.Length!=0)
		{
			Check_Hint ();
		}
    }

	void Check_Hint()
	{
		int _iRandomName = Random.Range (0, 3);
		_sCurrentHint = _sHintStrings [_iRandomName];
		Text_Hint.text=""+_sCurrentHint;
	}

	void LoadNextScene ()
	{
		StartCoroutine (loadLEvelTest ());
	}

	void checkSprite()
	{			
		BG.GetComponent<Image>().sprite=LaodingSprites[LoadingBGCount-1];
		print("Changing Here");
	}
	
	IEnumerator loadLEvelTest ()
	{
		if (SceneName == DefaultScene)
		{
//			AsyncOp = Application.LoadLevelAsync (DefaultScene);
			AsyncOp = SceneManager.LoadSceneAsync (DefaultScene);
			AsyncOp.allowSceneActivation = false;
			
			yield return AsyncOp; 
//			yield break;

		}
		else
		{
//			AsyncOp = Application.LoadLevelAsync (SceneName);
			AsyncOp = SceneManager.LoadSceneAsync (DefaultScene);
			AsyncOp.allowSceneActivation = false;
			
			yield return AsyncOp; 
		}
	}

	void Update ()
	{
		if (Is_ReadyToLoad == false && AsyncOp != null) 
		{
			if (AsyncOp.progress >= 0.85f) 
			{
				AsyncOp.allowSceneActivation = true;
				Is_ReadyToLoad = true;
			}
			LoadingPer = ((int)(AsyncOp.progress * 100));
			ProgressBar.mf_Percentage = ((int)(AsyncOp.progress * 100));


		}
		
		return;
		timerVal += Time.deltaTime;
		
		if (timerVal <= targeTimer)
			return;
		
		Text_Loading.text = "LOADING " + Application.GetStreamProgressForLevel (SceneName);
		
		if (Application.GetStreamProgressForLevel (SceneName) >= 1 && !Is_ReadyToLoad) 
		{
			Is_ReadyToLoad = true;	
			SceneManager.LoadScene (SceneName);
		}
	}
}
