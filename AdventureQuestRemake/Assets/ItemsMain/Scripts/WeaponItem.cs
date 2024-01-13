using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;
using UnityEngine.Events;

namespace MoreMountains.InventoryEngine
{	
	[CreateAssetMenu(fileName = "WeaponItem", menuName = "MoreMountains/InventoryEngine/WeaponItem", order = 2)]
	[Serializable]
	/// <summary>
	/// Demo class for a weapon item
	/// </summary>
	public class WeaponItem : InventoryItem 
	{
		[Header("Weapon")]
		/// the sprite to use to show the weapon when equipped
		public Sprite WeaponSprite;

		public UnityEvent weaponEquip;
		public UnityEvent weaponUnequip;

		/// <summary>
		/// What happens when the object is used 
		/// </summary>
		public override bool Equip(string playerID)
		{
			base.Equip(playerID);
			//TargetInventory(playerID).TargetTransform.GetComponent<InventoryDemoCharacter>().SetWeapon(WeaponSprite,this);
			weaponEquip.Invoke();
			return true;
		}

		/// <summary>
		/// What happens when the object is used 
		/// </summary>
		public override bool UnEquip(string playerID)
		{
			base.UnEquip(playerID);
			//TargetInventory(playerID).TargetTransform.GetComponent<InventoryDemoCharacter>().SetWeapon(null,this);
			weaponUnequip.Invoke();
			return true;
		}
		
	}
}