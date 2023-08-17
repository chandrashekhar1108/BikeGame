using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour 
{

	Rect _TextureCompleteRect,_TextureCurrentRect;
	public float mf_Percentage	= 0f;
	float f_PercentageVisible	= 1;

	private bool Is_True;

    public Text LoadingPer;
	public Image LoadingBar;

	void Start()
	{
		Invoke("SetBasicRect",0f);
        SetPercentage(0);
    }
	void SetBasicRect()
	{
 		//_TextureCompleteRect	= GetComponent<GUITexture>().pixelInset;
		_TextureCurrentRect		= _TextureCompleteRect;
		Is_True = true;

	}
	
	void Update()
	{
		if(f_PercentageVisible != mf_Percentage && Is_True)
		{
			f_PercentageVisible	= mf_Percentage;
			SetPercentage(f_PercentageVisible);
		}
       LoadingPer.text = "Loading... " + (int)mf_Percentage+"%";


		LoadingBar.gameObject.GetComponent<Image>().fillAmount=(mf_Percentage/100);

    }
	
	void SetPercentage(float percentageToShow)
	{
		float xOffSet	= _TextureCompleteRect.width*(percentageToShow/100);
		
		if(xOffSet < 0)
		{
			xOffSet	= 0;
		}
		else 
			if(xOffSet > _TextureCompleteRect.width)
		{
			xOffSet	= _TextureCompleteRect.width;	
		}
		
		_TextureCurrentRect	= new Rect(0,0,xOffSet,_TextureCompleteRect.height);
//		GetComponent<GUITexture>().pixelInset	= _TextureCurrentRect;
	}
}
