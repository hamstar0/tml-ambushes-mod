using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Timers;
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
			
			float sizeScale = AmbushesMod.Config.MinibossSizeScale;//3f;
			float lifeScale = AmbushesMod.Config.MinibossLifeScale;//6f;
			float damageScale = AmbushesMod.Config.MinibossDamageScale;//3f;

			npc.scale *= sizeScale;
			npc.life = npc.lifeMax = (int)( (float)npc.lifeMax * lifeScale );
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

		protected override bool PreClaimNPC( NPC npc ) {
			// Not currently implemented for any use:
			if( this.MinibossWho != -1 ) {
				if( Main.npc[this.MinibossWho].active ) {
					return Main.npc[this.MinibossWho].type == npc.type;
				}
			}
			return false;
		}

		protected override void OnClaimNPC( NPC npc ) {
			if( this.MinibossWho == -1 ) {
				if( npc.damage == 0 || npc.friendly || npc.immortal ) {
					return;
				}

				this.BeginEncounter( npc );
			}
		}

		protected override void UpdateNPCForAmbush( NPC npc ) {
			if( this.MinibossWho != npc.whoAmI ) {
				return;
			}

			var mynpc = npc.GetGlobalNPC<AmbushesNPC>();

			// Account for mismatched minibosses
			if( !mynpc.IsMiniboss ) {
				for( int i=Main.npc.Length-1; i>0; i++ ) {
					if( Main.npc[i] == null || !Main.npc[i].active ) {
						this.MinibossWho = i;
						break;
					}
				}
			}

			this.UpdatePlayerEncounter( npc );
		}


		////////////////

		private void UpdatePlayerEncounter( NPC npc ) {
			if( this.HasCloseEncounterBegun ) {
				return;
			}

			Player player = Main.player[ this.TriggeringPlayer ];
			if( player == null || !player.active || player.dead ) {
				return;
			}

			int maxDistSqr = 32 * 16;
			maxDistSqr *= maxDistSqr;

			if( Vector2.DistanceSquared(npc.Center, player.Center) < maxDistSqr ) {
				this.HasCloseEncounterBegun = true;
			}
		}
	}
}
