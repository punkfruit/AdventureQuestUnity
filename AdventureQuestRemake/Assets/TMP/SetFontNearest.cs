using System.Collections;
using System.Collections.Generic;
using MoreMountains.InventoryEngine;
using UnityEngine;

public class SetFontNearest : MonoBehaviour
{

	[SerializeField] Font[] fonts;

	public Inventory inv;

	void Start()
	{
		foreach (Font font in fonts)
		{
			var mat = font.material;
			var txtr = mat.mainTexture;
			txtr.filterMode = FilterMode.Point;
		}



		//set late to avoid errors. i might move this stuff somewhere else later
		inv.LoadSavedInventory();



	}

    private void OnValidate()
    {
        
    }







}
