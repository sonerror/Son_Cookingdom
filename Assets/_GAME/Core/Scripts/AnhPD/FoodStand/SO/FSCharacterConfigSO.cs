using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AnhPD.FoodStall
{
    [CreateAssetMenu(menuName = "AnhPD/FoodStall/CharacterConfigSO")]
    public class FSCharacterConfigSO : ScriptableObject
    {
        [SerializeField] private AssetReference[] _assetRef;
        public int AssetRefNumber => _assetRef.Length;

        public AssetReference GetAssetReferenceById(int id)
        {
            if (id >= _assetRef.Length) return null;
            return _assetRef[id];
        }

    }
    //---------------list----------------
    //0 : snake
    //1 : bear
    //2 : sheep
    //3 : shiba
    //4 : capy - bro
    //5 : capy - dad
    //6 : capy - mom
    //7 : capy - sis
    //8 : capy - uncle
    //9 : crocodine
    //10 : fox
    //11 : rabiit - pricess
    //12 : dog - magician
    //13 : cat - fighter
    //14 : bear - prisoner
    //15 : cat - confucian
    //16 : capy2 - normal
    //17 : capy2 - bro
    //18 : capy2 - dad
    //19 : capy2 - mom
    //-----------------------------------
}
