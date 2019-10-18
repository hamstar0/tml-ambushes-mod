using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	class AmbushesNPC : GlobalNPC {
		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			var myworld = ModContent.GetInstance<AmbushesWorld>();
			myworld.AmbushMngr.EditSpawnPool( pool, spawnInfo );
		}


		public override void EditSpawnRate( Player player, ref int spawnRate, ref int maxSpawns ) {
			var myworld = ModContent.GetInstance<AmbushesWorld>();
			myworld.AmbushMngr.EditSpawnData( player, ref spawnRate, ref maxSpawns );
		}
	}
}
