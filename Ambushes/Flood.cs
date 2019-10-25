using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.World;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace Ambushes.Ambushes {
	class FloodAmbush : MobAmbush {
		public override float WorldGenWeight => AmbushesMod.Config.FloodAmbushPriorityWeight;



		////////////////

		private FloodAmbush() : base( 0, 0, false ) {
		}

		protected FloodAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}

		////

		protected override Ambush CloneRandomized( int tileX, int tileY ) {
			bool isEntrapping = AmbushesMod.Config.AmbushEntrapmentOdds <= 0
				? false
				: TmlHelpers.SafelyGetRand().Next( AmbushesMod.Config.AmbushEntrapmentOdds ) == 0;
			isEntrapping = isEntrapping && !WorldHelpers.IsWithinUnderworld( new Vector2(tileX<<4, tileY<<4) );

			return new FloodAmbush( tileX, tileY, isEntrapping );
		}


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {

			return base.OnActivate( clearTileX, clearTileY );
		}

		protected override void OnDeactivate() {
		}


		////////////////

		public override int GetNPCSpawnsDuration() {
			return this.GetBrambleDuration();
		}

		public override float GetNPCSpawnWeight() {
			return base.GetNPCSpawnWeight() * AmbushesMod.Config.FloodAmbushSpawnWeight;
		}

		public override void ShowMessage() {
			Main.NewText( "The locals are alerted to your presence.", Color.DarkOrange );
		}
	}
}
