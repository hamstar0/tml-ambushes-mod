using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	class AmbushesNPC : GlobalNPC {
		private bool IsAmbushChecked = false;


		////////////////

		public bool IsMiniboss { get; private set; }

		////

		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => false;



		////////////////

		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			var myworld = ModContent.GetInstance<AmbushesWorld>();
			myworld.AmbushMngr.EditSpawnPool( pool, spawnInfo );

			if( AmbushesMod.Config.DebugModeInfoSpawns ) {
				DebugHelpers.Print(
					"SpawnsInfo_"+spawnInfo.player.whoAmI+" ("+spawnInfo.player.name+")",
					string.Join(", ", pool.Select( kv=>kv.Key+": "+kv.Value.ToString("N3") ) ),
					20 );
			}
		}


		public override void EditSpawnRate( Player player, ref int spawnRate, ref int maxSpawns ) {
			var myworld = ModContent.GetInstance<AmbushesWorld>();
			myworld.AmbushMngr.EditSpawnData( player, ref spawnRate, ref maxSpawns );

			if( AmbushesMod.Config.DebugModeInfoSpawns ) {
				DebugHelpers.Print(
					"SpawnsRates_"+player.whoAmI+" ("+player.name+")",
					"Rate: "+spawnRate+", max: "+maxSpawns,
					20 );
			}
		}

		////

		public override bool PreAI( NPC npc ) {
			if( !this.IsAmbushChecked ) {
				this.IsAmbushChecked = true;

				var myworld = ModContent.GetInstance<AmbushesWorld>();
				myworld.AmbushMngr.PreAI( npc );
			}

			if( AmbushesMod.Config.DebugModeMapCheat ) {
				if( this.IsMiniboss ) {
					AmbushesMod.Instance.LatestMinibossWho = npc.whoAmI;
				}
			}

			return true;
		}


		////////////////

		public void SetAsMiniboss( NPC npc ) {
			if( this.IsMiniboss ) {
				LogHelpers.Warn( "NPC "+npc.TypeName+" is already a miniboss." );
				return;
			}
			this.IsMiniboss = true;

			float sizeScale = 3f;
			float lifeScale = 6f;
			float damageScale = 3f;

			npc.scale *= sizeScale;
			npc.lifeMax = (int)( (float)npc.lifeMax * lifeScale );
			npc.life = (int)( (float)npc.life * lifeScale );
			npc.damage = (int)( (float)npc.damage * damageScale );
		}
	}
}
