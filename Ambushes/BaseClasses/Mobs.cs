using HamstarHelpers.Helpers.World;
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

		public abstract int GetNPCSpawnsDuration();

		public abstract void ShowMessage();


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			Timers.SetTimer( "SpawnPoolUpdate", 2, () => {
				NPC.SpawnNPC();
				return false;
			} );

			return true;//base.OnActivate( clearTileX, clearTileY );	<- overrides brambles; begin when spawns do
		}

		protected override void OnDeactivate() {
			this.HasEncounterBegun = false;
		}


		////////////////

		protected override bool RunUntil() {
			Player player = Main.player[this.TriggeringPlayer];
			bool isPlayerActive = player != null && player.active && !player.dead;
			bool isPlayerUnderground = isPlayerActive && player.Center.Y >= WorldHelpers.DirtLayerTopTileY;
			int spawnDurationMax = this.GetNPCSpawnsDuration();

			// Ambush follows player until "encountered"
			if( !this.HasEncounterBegun ) {
				if( !isPlayerActive || !isPlayerUnderground ) {
					return true;
				}

				this.TileX = (int)player.Center.X >> 4;
				this.TileY = (int)player.Center.Y >> 4;

				// Minimum nearby NPCs? Begin "encounter"
				if( this.ClaimedNpcWhos.Count >= 4 ) {
					this.HasEncounterBegun = true;

					this.ShowMessage();

					if( this.ElapsedTicks < spawnDurationMax ) {
						this.CreateBrambleEnclosureIfEntrapping();
					}
				}
			}

			// No encounter? Hold everything
			if( !this.HasEncounterBegun ) {
				return false;
			}

			return this.RunEncounterUntil();
		}

		private bool RunEncounterUntil() {
			// Give priority to bramble cleanup
			if( !base.RunUntil() ) {
				return false;
			}

			bool spawnsEnded = this.ElapsedTicks >= this.GetNPCSpawnsDuration();
			bool alone = !this.ArePlayersNearby( AmbushesMod.Config.AmbushPlayerNearbyNeededTileRadius );

			if( AmbushesMod.Config.DebugModeInfoSpawns ) {
				if( spawnsEnded ) {
					Main.NewText( "Ambush " + this.GetType().Name + " at " + this.TileX + "," + this.TileY + " ended: Time's up." );
				} else if( alone ) {
					Main.NewText( "Ambush " + this.GetType().Name + " at " + this.TileX + "," + this.TileY + " ended: No one nearby." );
				}
			}

			return spawnsEnded || alone;
		}


		////////////////

		public sealed override void EditNPCSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			if( this.ElapsedTicks < this.GetNPCSpawnsDuration() ) {
				this.EditNPCSpawnPoolForMobs( pool, spawnInfo );
			}
		}

		public sealed override void EditNPCSpawnData( Player player, ref int spawnRate, ref int maxSpawns ) {
			if( this.ElapsedTicks >= this.GetNPCSpawnsDuration() ) {
				return;
			}

			float weight = this.GetNPCSpawnWeight();

			spawnRate = (int)( (float)spawnRate / weight );
			maxSpawns = (int)( (float)maxSpawns * weight );

			this.EditNPCSpawnDataForMobs( player, ref spawnRate, ref maxSpawns );
		}

		////

		protected sealed override void OnClaimNPC( NPC npc ) {
			if( this.ElapsedTicks < this.GetNPCSpawnsDuration() ) {
				this.OnClaimNPCForMobs( npc );
			}
		}

		protected sealed override void UpdateNPCForAmbush( NPC npc ) {
			if( this.ElapsedTicks < this.GetNPCSpawnsDuration() ) {
				this.UpdateNPCForAmbushForMobs( npc );
			}
		}


		////////////////

		public virtual void EditNPCSpawnPoolForMobs( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
		}

		public virtual void EditNPCSpawnDataForMobs( Player player, ref int spawnRate, ref int maxSpawns ) {
		}

		////

		protected virtual void OnClaimNPCForMobs( NPC npc ) {
			npc.life = (int)((float)npc.life * AmbushesMod.Config.MobAmbushLifeScale);
			npc.lifeMax = (int)((float)npc.lifeMax * AmbushesMod.Config.MobAmbushLifeScale);
			npc.value = npc.value * AmbushesMod.Config.MobAmbushLifeScale;
		}

		protected virtual void UpdateNPCForAmbushForMobs( NPC npc ) {
		}
	}
}
