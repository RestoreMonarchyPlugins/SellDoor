using UnityEngine;

namespace SDG.Unturned
{
	[CreateAssetMenu(fileName = "AssetBundleCustomData", menuName = "Unturned/Asset Bundle Custom Data")]
	public class AssetBundleCustomData : ScriptableObject
	{
		/// <summary>
		/// If Unturned is loading this asset bundle from a Steam workshop file but the file ID does not match then
		/// loading will be canceled. Prevents the asset bundle from being easily copied/stolen.
		/// </summary>
		public ulong ownerWorkshopFileId;
	}
}
