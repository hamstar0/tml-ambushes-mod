using Ambushes.Tiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class CleanupOnlyAmbush : Ambush {
		public CleanupOnlyAmbush( int tileX, int tileY ) : base( tileX, tileY ) {
		}


		////////////////

		public override bool OnActivate( int openTileX, int openTileY ) {
			return true;
		}

		public override void OnDeactivate() {
		}

		////

		public override void EditSpawnData( Player player, ref int spawnRate, ref int maxSpawns ) {
		}

		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
		}
	}
}
