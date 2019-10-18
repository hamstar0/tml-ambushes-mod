using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	partial class AmbushManager {
		public void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			foreach( Ambush ambush in this.ActiveAmbushes ) {
				ambush.EditSpawnPoolWhileRunning( pool, spawnInfo );
			}
		}


		public void EditSpawnData( Player player, ref int spawnRate, ref int maxSpawns ) {
			foreach( Ambush ambush in this.ActiveAmbushes ) {
				if( ambush.TriggeringPlayer == player.whoAmI ) {
					ambush.EditSpawnDataWhileRunning( player, ref spawnRate, ref maxSpawns );
				}
			}
		}
	}
}
