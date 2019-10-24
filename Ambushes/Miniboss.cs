using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.World;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class MinibossAmbush : BrambleEnclosureAmbush {
		private int MinibossWho = -1;


		////////////////

		public bool IsEncountered { get; private set; } = false;

		////

		public override float SpawnWeight => AmbushesMod.Config.MinibossAmbushPriorityWeight;
		public override bool PlaysMusic => this.IsEncountered;



		////////////////

		private MinibossAmbush() : base( 0, 0, false ) {
		}

		protected MinibossAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}

		////

		protected override Ambush CloneRandomized( int tileX, int tileY ) {
			bool isEntrapping = TmlHelpers.SafelyGetRand().Next( 4 ) == 0;
			isEntrapping = isEntrapping && !WorldHelpers.IsWithinUnderworld( new Vector2( tileX << 4, tileY << 4 ) );

			return new MinibossAmbush( tileX, tileY, isEntrapping );
		}


		////////////////

		protected override bool RunUntil() {
			NPC npc = null;
			if( this.MinibossWho != -1 ) {
				npc = Main.npc[ this.MinibossWho ];
			}

			return (this.MinibossWho != -1 && !npc.active) && base.RunUntil();
		}


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			Main.NewText( "An imposing presence lurks somewhere nearby...", Color.DarkOrange );

			Timers.SetTimer( "SpawnPoolUpdate", 2, () => {
				NPC.SpawnNPC();
				return false;
			} );

			return base.OnActivate( clearTileX, clearTileY );
		}

		protected override void OnDeactivate() {
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

		protected override void NPCPreAI( NPC npc ) {
			if( this.MinibossWho == -1 ) {
				this.MinibossWho = npc.whoAmI;
				npc.GetGlobalNPC<AmbushesNPC>().SetAsMiniboss( npc );
			}

			if( this.MinibossWho == npc.whoAmI ) {
				this.CheckPlayerEncounter( npc );
			}
		}


		////

		private void CheckPlayerEncounter( NPC npc ) {
			if( this.IsEncountered ) {
				return;
			}

			Player player = Main.player[this.TriggeringPlayer];
			if( player == null || !player.active || player.dead ) {
				return;
			}

			int maxDistSqr = 24 * 16;
			maxDistSqr *= maxDistSqr;

			if( Vector2.DistanceSquared(npc.Center, player.Center) < maxDistSqr ) {
				this.IsEncountered = true;
			}
		}
	}
}
