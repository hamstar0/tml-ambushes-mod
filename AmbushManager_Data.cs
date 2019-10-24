using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	partial class AmbushManager {
		internal void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			foreach( Ambush ambush in this.ActiveAmbushes ) {
				if( ambush.TriggeringPlayer == spawnInfo.player?.whoAmI ) {
					ambush.EditNPCSpawnPool( pool, spawnInfo );
				}
			}
		}


		internal void EditSpawnData( Player player, ref int spawnRate, ref int maxSpawns ) {
			foreach( Ambush ambush in this.ActiveAmbushes ) {
				if( ambush.TriggeringPlayer == player.whoAmI ) {
					ambush.EditNPCSpawnData( player, ref spawnRate, ref maxSpawns );
				}
			}
		}

		////

		internal void UpdateNPCForAllAmbushes( NPC npc ) {
			foreach( Ambush ambush in this.ActiveAmbushes ) {
				ambush.InternalUpdateNPCForAmbush( npc );
			}
		}
	}
}
