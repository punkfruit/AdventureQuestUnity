using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFontNearest : MonoBehaviour
{

	[SerializeField] Font[] fonts;

	void Start()
	{
		foreach (Font font in fonts)
		{
			var mat = font.material;
			var txtr = mat.mainTexture;
			txtr.filterMode = FilterMode.Point;
		}
	}

    private void OnValidate()
    {
        
    }
}
