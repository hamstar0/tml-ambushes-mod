using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class MinibossAmbush : BrambleEnclosureAmbush {
		public MinibossAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}


		////////////////

		public override void OnDeactivate() {
		}

		////////////////

		public override void EditSpawnData( Player player, ref int spawnRate, ref int maxSpawns ) {
		}

		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
		}
	}
}
