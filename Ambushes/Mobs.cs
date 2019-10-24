using HamstarHelpers.Services.Timers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	abstract class MobAmbush : BrambleEnclosureAmbush {
		private bool HasEncounterBegun = false;


		////////////////

		public override bool PlaysMusic => this.IsEntrapping && this.HasEncounterBegun;



		////////////////

		protected MobAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}


		////////////////

		public abstract int GetSpawnsDuration();

		public abstract void ShowMessage();


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
			if( this.ClaimedNpcWhos.Count > 5 ) {
				if( !this.HasEncounterBegun ) {
					this.HasEncounterBegun = true;
					this.ShowMessage();
				}
			}

			if( !base.RunUntil() ) {
				return false;
			}

			int nearbyRadius = AmbushesMod.Config.AmbushPlayerNearbyNeededTileRadius;
			bool spawnsEnded = this.ElapsedTicks >= this.GetSpawnsDuration()
				|| !this.ArePlayersNearby( nearbyRadius );

			if( spawnsEnded ) {
				if( AmbushesMod.Config.DebugModeInfoSpawns ) {
					Main.NewText( "Ambush "+this.GetType().Name+" at "+this.TileX+","+this.TileY+" ended." );
				}
			}

			return spawnsEnded;
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

		protected sealed override void OnClaimNPC( NPC npc ) {
			if( this.ElapsedTicks < this.GetSpawnsDuration() ) {
				this.OnClaimNPCForMobs( npc );
			}
		}

		protected sealed override void UpdateNPCForAmbush( NPC npc ) {
			if( this.ElapsedTicks < this.GetSpawnsDuration() ) {
				this.UpdateNPCForAmbushForMobs( npc );
			}
		}


		////////////////

		public virtual void EditNPCSpawnPoolForMobs( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
		}

		public virtual void EditNPCSpawnDataForMobs( Player player, ref int spawnRate, ref int maxSpawns ) {
		}

		protected virtual void OnClaimNPCForMobs( NPC npc ) {
			npc.life = (int)((float)npc.life * AmbushesMod.Config.MobAmbushLifeScale);
			npc.lifeMax = (int)((float)npc.lifeMax * AmbushesMod.Config.MobAmbushLifeScale);
		}

		protected virtual void UpdateNPCForAmbushForMobs( NPC npc ) {
		}
	}
}
