using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	abstract class MobAmbush : BrambleEnclosureAmbush {
		public override bool PlaysMusic => this.IsEntrapping;



		////////////////

		protected MobAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}


		////////////////

		public abstract int GetSpawnsDuration();


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			Timers.SetTimer( "SpawnPoolUpdate", 2, () => {
				NPC.SpawnNPC();
				return false;
			} );

			return base.OnActivate( clearTileX, clearTileY );
		}

		////////////////

		protected override bool RunUntil() {
			if( !base.RunUntil() ) {
				return false;
			}

			bool spawnsEnded = (this.ElapsedTicks >= this.GetSpawnsDuration()) || !this.ArePlayersNearby();
			if( spawnsEnded ) {
				if( AmbushesMod.Config.DebugModeInfoSpawns ) {
					Main.NewText( "Ambush "+this.GetType().Name+" at "+this.TileX+","+this.TileY+" ended." );
				}
			}

			return spawnsEnded;
		}


		////

		private bool ArePlayersNearby() {
			int minDistSqr = AmbushesMod.Config.AmbushPlayerNearbyNeededTileRadius << 4;
			minDistSqr *= minDistSqr;

			int plrMax = Main.player.Length - 1;
			for( int i=0; i<plrMax; i++ ) {
				Player plr = Main.player[i];
				if( plr==null || !plr.active || plr.dead ) {
					continue;
				}

				if( Vector2.DistanceSquared(plr.Center, this.WorldPosition) < minDistSqr ) {
					return true;
				}
			}

			return false;
		}


		////////////////

		public sealed override void EditNPCSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			if( this.ElapsedTicks < this.GetSpawnsDuration() ) {
				this.EditNPCSpawnPoolForMobs( pool, spawnInfo );
			}
		}

		public sealed override void EditNPCSpawnData( Player player, ref int spawnRate, ref int maxSpawns ) {
			if( this.ElapsedTicks < this.GetSpawnsDuration() ) {
				this.EditNPCSpawnDataForMobs( player, ref spawnRate, ref maxSpawns );
			}
		}

		protected sealed override void NPCPreAI( NPC npc ) {
			if( this.ElapsedTicks < this.GetSpawnsDuration() ) {
				this.NPCPreAIForMobs( npc );
			}
		}


		////////////////

		public virtual void EditNPCSpawnPoolForMobs( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
		}

		public virtual void EditNPCSpawnDataForMobs( Player player, ref int spawnRate, ref int maxSpawns ) {
		}

		protected virtual void NPCPreAIForMobs( NPC npc ) {
		}
	}
}
