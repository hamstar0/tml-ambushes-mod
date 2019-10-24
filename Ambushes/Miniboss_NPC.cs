using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	partial class MinibossAmbush : BrambleEnclosureAmbush {
		public static void SetAsMiniboss( NPC npc ) {
			var mynpc = npc.GetGlobalNPC<AmbushesNPC>();
			if( mynpc.IsMiniboss ) {
				LogHelpers.Warn( "NPC " + npc.TypeName + " is already a miniboss." );
				return;
			}

			mynpc.IsMiniboss = true;

			float sizeScale = 3f;
			float lifeScale = 6f;
			float damageScale = 3f;

			npc.scale *= sizeScale;
			npc.lifeMax = (int)( (float)npc.lifeMax * lifeScale );
			npc.life = (int)( (float)npc.life * lifeScale );
			npc.damage = (int)( (float)npc.damage * damageScale );
		}



		////////////////

		public override void EditNPCSpawnData( Player player, ref int spawnRate, ref int maxSpawns ) {
			//if( this.MinibossWho == -1 ) {
			//	spawnRate = (int)( (float)spawnRate / 30f );
			//}
		}

		public override void EditNPCSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			if( this.MinibossWho == -1 ) {
				return;
			}

			NPC npc = Main.npc[this.MinibossWho];

			if( npc != null && npc.active && npc.GetGlobalNPC<AmbushesNPC>().IsMiniboss ) {
				pool.Clear();
				pool[npc.type] = 1f;
			}
		}

		////

		protected override void OnClaimNPC( NPC npc ) {
			if( this.MinibossWho == -1 && npc.damage > 0 && !npc.friendly && !npc.immortal ) {
				this.MinibossWho = npc.whoAmI;
				MinibossAmbush.SetAsMiniboss( npc );
			}
		}

		protected override void UpdateNPCForAmbush( NPC npc ) {
			if( this.MinibossWho == npc.whoAmI ) {
				this.UpdatePlayerEncounter( npc );
			}
		}


		////////////////

		private void UpdatePlayerEncounter( NPC npc ) {
			if( this.IsEncountered ) {
				return;
			}

			Player player = Main.player[ this.TriggeringPlayer ];
			if( player == null || !player.active || player.dead ) {
				return;
			}

			int maxDistSqr = 32 * 16;
			maxDistSqr *= maxDistSqr;

			if( Vector2.DistanceSquared(npc.Center, player.Center) < maxDistSqr ) {
				this.IsEncountered = true;
			}
		}
	}
}
