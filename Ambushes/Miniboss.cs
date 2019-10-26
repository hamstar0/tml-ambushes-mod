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

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			Timers.SetTimer( "SpawnPoolUpdate", 2, () => {
				NPC.SpawnNPC();
				return false;
			} );

			return true;	// <- Does not invoke base class
		}

		protected override void OnDeactivate() {
			this.MinibossWho = -1;
		}


		////////////////

		protected override bool RunUntil() {
			if( this.MinibossWho == -1 ) {
				Player player = Main.player[this.TriggeringPlayer];
				bool isPlayerActive = player != null && player.active && !player.dead;
				bool isPlayerUnderground = isPlayerActive && player.Center.Y >= WorldHelpers.DirtLayerTopTileY;

				if( !isPlayerActive || !isPlayerUnderground ) {
					return true;
				}

				// Time out after 10 seconds of no encounter
				if( this.ElapsedTicks > 600 ) {
					if( AmbushesMod.Config.DebugModeInfoSpawns ) {
						Main.NewText( "Ambush " + this.GetType().Name + " at " + this.TileX + "," + this.TileY + " aborted: Timed out." );
					}
					return true;
				}

				// Ambush follows player until "encountered"
				this.TileX = (int)player.Center.X >> 4;
				this.TileY = (int)player.Center.Y >> 4;

				// Delay brambles and other run behavior until miniboss "encounter"
				return false;
			}

			NPC npc = Main.npc[this.MinibossWho];
			bool isMinibossDead = this.MinibossWho != -1 && !npc.active && npc.life > 0;
			bool runBramblesUntil = base.RunUntil();	// Runs brambles also, if any

			if( isMinibossDead && runBramblesUntil ) {
				if( AmbushesMod.Config.DebugModeInfoSpawns ) {
					Main.NewText( "Ambush "+this.GetType().Name+" at "+this.TileX+","+this.TileY+" ended: Miniboss dead, brambles gone." );
				}
			}

			return isMinibossDead && runBramblesUntil;
		}


		////////////////

		protected void BeginEncounter( NPC npc ) {
			this.MinibossWho = npc.whoAmI;
			MinibossAmbush.SetAsMiniboss( npc );

			this.CreateBrambleEnclosureIfEntrapping();

			Main.NewText( "An imposing presence nears...", Color.DarkOrange );

			// Reset ticks
			this.ElapsedTicks = 1;

			// Refresh spawns so only to the miniboss's type now appears
			Timers.SetTimer( "SpawnPoolUpdate", 2, () => {
				NPC.SpawnNPC();
				return false;
			} );
		}
	}
}
