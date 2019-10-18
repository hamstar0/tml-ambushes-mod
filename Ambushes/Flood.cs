using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class FloodAmbush : BrambleEnclosureAmbush {
		public FloodAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}


		////////////////

		public override void OnDeactivate() {
		}

		////////////////

		public override void EditSpawnDataWhileRunning( Player player, ref int spawnRate, ref int maxSpawns ) {
			spawnRate /= 15;
			maxSpawns *= 15;
		}

		public override void EditSpawnPoolWhileRunning( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
		}
	}
}
