using Ambushes.Tiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class BrambleWallAmbush : BrambleAmbush {
		protected bool IsHorizontal;



		////////////////

		public BrambleWallAmbush( int tileX, int tileY, bool isHorizontal ) : base( tileX, tileY ) {
			this.IsHorizontal = isHorizontal;
		}


		////////////////

		public override bool OnActivate( int clearTileX, int clearTileY ) {
			int radius = AmbushesMod.Config.AmbushEntrapmentRadius;
			IDictionary<int, ISet<int>> tileTrace = CursedBrambleTile.TraceTileWall( clearTileX, clearTileY, radius, this.IsHorizontal );

			CursedBrambleTile.CreateBramblesAt( tileTrace );

			return true;
		}

		public override void OnDeactivate() {
		}


		////////////////

		public override int GetRunDuration() {
			return AmbushesMod.Config.BrambleWallTickDurationUntilErosionBegin;
		}

		////////////////

		public override bool Run() {
			return base.Run();
		}


		////////////////

		public override void EditSpawnDataWhileRunning( Player player, ref int spawnRate, ref int maxSpawns ) {
		}

		public override void EditSpawnPoolWhileRunning( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
		}
	}
}
