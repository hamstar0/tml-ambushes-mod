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

		public bool HasEncounterBegun { get; private set; } = false;

		////

		public override float WorldGenWeight => AmbushesMod.Config.MinibossAmbushPriorityWeight;

		public override bool PlaysMusic => this.IsEntrapping && this.HasEncounterBegun;



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
			int nearbyRadius = AmbushesMod.Config.MinibossAmbushPlayerNearbyNeededTileRadius;

			if( this.MinibossWho == -1 ) {
				if( !this.ArePlayersNearby(nearbyRadius) ) {
					return true;
				}
				if( this.ElapsedTicks > 600 ) {
					return true;
				}

				// Delay brambles and other run behavior until miniboss "encounter"
				return false;
			}

			bool runBramblesUntil = base.RunUntil();
			NPC npc = Main.npc[ this.MinibossWho ];

			return (this.MinibossWho != -1 && !npc.active) && runBramblesUntil;
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
		}
	}
}
