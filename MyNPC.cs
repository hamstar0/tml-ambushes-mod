using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	class AmbushesNPC : GlobalNPC {
		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			base.EditSpawnPool( pool, spawnInfo );
		}


		public override void EditSpawnRate( Player player, ref int spawnRate, ref int maxSpawns ) {
			ModContent.GetInstance<AmbushesWorld>().AmbushMngr.GetSpawnData( ref spawnRate, ref maxSpawns );
		}
	}
}
