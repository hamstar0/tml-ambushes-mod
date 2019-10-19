using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	class AmbushesNPC : GlobalNPC {
		private bool IsAmbushChecked = false;


		////////////////

		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => false;



		////////////////

		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			var myworld = ModContent.GetInstance<AmbushesWorld>();
			myworld.AmbushMngr.EditSpawnPool( pool, spawnInfo );
		}


		public override void EditSpawnRate( Player player, ref int spawnRate, ref int maxSpawns ) {
			var myworld = ModContent.GetInstance<AmbushesWorld>();
			myworld.AmbushMngr.EditSpawnData( player, ref spawnRate, ref maxSpawns );
		}

		////

		public override bool PreAI( NPC npc ) {
			if( !this.IsAmbushChecked ) {
				this.IsAmbushChecked = true;

				var myworld = ModContent.GetInstance<AmbushesWorld>();
				myworld.AmbushMngr.PreAI( npc );
			}

			return true;
		}
	}
}
