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

		public bool IsEncountered { get; private set; } = false;

		////

		public override float SpawnWeight => AmbushesMod.Config.MinibossAmbushPriorityWeight;

		public override bool PlaysMusic => this.IsEncountered && this.IsEntrapping;



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
			bool runBramblesUntil = base.RunUntil();
			int nearbyRadius = AmbushesMod.Config.MinibossAmbushPlayerNearbyNeededTileRadius;

			if( this.MinibossWho == -1 && !this.ArePlayersNearby(nearbyRadius) ) {
				return true;
			}

			NPC npc = null;
			if( this.MinibossWho != -1 ) {
				npc = Main.npc[ this.MinibossWho ];
			}

			return (this.MinibossWho != -1 && !npc.active) && runBramblesUntil;
		}


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			Main.NewText( "An imposing presence lurks somewhere nearby...", Color.DarkOrange );

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
