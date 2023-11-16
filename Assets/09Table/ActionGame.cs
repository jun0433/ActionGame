using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class ActionGame : ScriptableObject
{
	public List<ItemData_Entity> ItemData; // Replace 'EntityType' to an actual type that is serializable.
	public List<TipMess_Entity> TipMess; // Replace 'EntityType' to an actual type that is serializable.
	public List<Language> Language; // Replace 'EntityType' to an actual type that is serializable.
	public List<MonsterData> MonsterData; // Replace 'EntityType' to an actual type that is serializable.
	public List<LevelEXP> LevelEXP; // Replace 'EntityType' to an actual type that is serializable.

}
