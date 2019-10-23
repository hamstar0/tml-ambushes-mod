using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	partial class AmbushManager {
		public void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			foreach( Ambush ambush in this.ActiveAmbushes ) {
				if( ambush.TriggeringPlayer == spawnInfo.player?.whoAmI ) {
					ambush.EditNPCSpawnPool( pool, spawnInfo );
				}
			}
		}


		public void EditSpawnData( Player player, ref int spawnRate, ref int maxSpawns ) {
			foreach( Ambush ambush in this.ActiveAmbushes ) {
				if( ambush.TriggeringPlayer == player.whoAmI ) {
					ambush.EditNPCSpawnData( player, ref spawnRate, ref maxSpawns );
				}
			}
		}

		////

		public void PreAI( NPC npc ) {
			int minDistSqr = ( 34 * 16 ) * ( 34 * 16 );

			foreach( Ambush ambush in this.ActiveAmbushes ) {
				var wldPos = new Vector2( ambush.TileX<<4, ambush.TileY<<4 );
				var diff = new Vector2(
					npc.Center.X - (ambush.TileX<<4), 
					npc.Center.Y - (ambush.TileY<<4)
				);

				if( diff.LengthSquared() < minDistSqr ) {
					ambush.InternalNPCPreAI( npc );
				}
			}
		}
	}
}
