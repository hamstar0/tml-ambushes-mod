using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class BrambleCleanupOnlyAmbush : BrambleAmbush {
		public BrambleCleanupOnlyAmbush( int tileX, int tileY ) : base( tileX, tileY ) {
		}


		////////////////

		public override bool OnActivate( int clearTileX, int clearTileY ) {
			return true;
		}

		public override void OnDeactivate() {
		}


		////////////////

		public override int GetRunDuration() {
			return 0;
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
