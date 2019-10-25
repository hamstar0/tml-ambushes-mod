using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.World;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace Ambushes.Ambushes {
	partial class MinibossAmbush : BrambleEnclosureAmbush {
		private int MinibossWho = -1;


		////////////////

		public bool HasCloseEncounterBegun { get; private set; } = false;

		////

		public override float WorldGenWeight => AmbushesMod.Config.MinibossAmbushPriorityWeight;

		public override bool PlaysMusic => this.IsEntrapping && this.HasCloseEncounterBegun;



		////////////////

		private MinibossAmbush() : base( 0, 0, false ) {
		}

		protected MinibossAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}

		////

		protected override Ambush CloneRandomized( int tileX, int tileY ) {
			bool isEntrapping = AmbushesMod.Config.AmbushEntrapmentOdds <= 0
				? false
				: TmlHelpers.SafelyGetRand().Next( AmbushesMod.Config.AmbushEntrapmentOdds ) == 0;
			isEntrapping = isEntrapping && !WorldHelpers.IsWithinUnderworld( new Vector2( tileX << 4, tileY << 4 ) );

			return new MinibossAmbush( tileX, tileY, isEntrapping );
		}


		////////////////

		protected override bool RunUntil() {
			if( this.MinibossWho == -1 ) {
				if( this.ElapsedTicks > 600 ) {
					if( AmbushesMod.Config.DebugModeInfoSpawns ) {
						Main.NewText( "Ambush " + this.GetType().Name + " at " + this.TileX + "," + this.TileY + " aborted: Timed out." );
					}
					return true;
				}

				if( !this.ArePlayersNearby( AmbushesMod.Config.MinibossAmbushPlayerNearbyNeededTileRadius ) ) {
					if( AmbushesMod.Config.DebugModeInfoSpawns ) {
						Main.NewText( "Ambush " + this.GetType().Name + " at " + this.TileX + "," + this.TileY + " aborted: No one nearby." );
					}
					return true;
				}

				// Delay brambles and other run behavior until miniboss "encounter"
				return false;
			}

			NPC npc = Main.npc[this.MinibossWho];
			bool isMinibossDead = this.MinibossWho != -1 && !npc.active;
			bool runBramblesUntil = base.RunUntil();	// Runs brambles also, if any

			if( isMinibossDead && runBramblesUntil ) {
				if( AmbushesMod.Config.DebugModeInfoSpawns ) {
					Main.NewText( "Ambush " + this.GetType().Name + " at " + this.TileX + "," + this.TileY + " ended: Miniboss dead, brambles gone." );
				}
				return true;
			}

			return false;
		}


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			Timers.SetTimer( "SpawnPoolUpdate", 2, () => {
				NPC.SpawnNPC();
				return false;
			} );

			return true;
		}

		protected override void OnDeactivate() {
			this.MinibossWho = -1;
		}
	}
}
