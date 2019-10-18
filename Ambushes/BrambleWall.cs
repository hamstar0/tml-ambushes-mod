using Ambushes.Tiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class BrambleWallAmbush : BrambleAmbush {
		public BrambleWallAmbush( int tileX, int tileY ) : base( tileX, tileY ) {
		}


		////////////////

		public override bool OnActivate( int openTileX, int openTileY ) {
			int radius = AmbushesMod.Config.AmbushEntrapmentRadius;
			IDictionary<int, ISet<int>> tileTrace = CursedBrambleTile.TraceTileEnclosure( openTileX, openTileY, radius );

			CursedBrambleTile.CreateBramblesAt( tileTrace );

			return true;
		}

		public override void OnDeactivate() {
		}


		////////////////

		public override bool Run() {
			return base.Run();
		}


		////////////////

		public override void EditSpawnData( Player player, ref int spawnRate, ref int maxSpawns ) {
		}

		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
		}
	}
}
