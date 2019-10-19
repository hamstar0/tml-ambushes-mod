using HamstarHelpers.Helpers.TModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Ambushes {
	class FloodAmbush : BrambleEnclosureAmbush {
		public override float SpawnWeight => 2f;



		////////////////

		private FloodAmbush() : base( 0, 0, false ) {
		}

		protected FloodAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}


		////////////////

		protected override Ambush Clone( int tileX, int tileY ) {
			bool isEntrapping = TmlHelpers.SafelyGetRand().Next( 4 ) == 0;
			return new FloodAmbush( tileX, tileY, isEntrapping );
		}


		////////////////

		public override bool OnActivate( int clearTileX, int clearTileY ) {
			Main.NewText( "The locals are alerted to your presence.", Color.DarkOrange );

			return base.OnActivate( clearTileX, clearTileY );
		}

		public override void OnDeactivate() {
		}


		////////////////

		public override void EditSpawnDataWhileRunning( Player player, ref int spawnRate, ref int maxSpawns ) {
			spawnRate /= 15;
			maxSpawns *= 15;
		}
	}
}
