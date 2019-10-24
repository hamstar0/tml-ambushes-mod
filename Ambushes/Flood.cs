using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.World;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace Ambushes.Ambushes {
	class FloodAmbush : MobAmbush {
		public override float SpawnWeight => AmbushesMod.Config.FloodAmbushPriorityWeight;



		////////////////

		private FloodAmbush() : base( 0, 0, false ) {
		}

		protected FloodAmbush( int tileX, int tileY, bool isEntrapping ) : base( tileX, tileY, isEntrapping ) {
		}

		////

		protected override Ambush CloneRandomized( int tileX, int tileY ) {
			bool isEntrapping = true;//TmlHelpers.SafelyGetRand().Next( 4 ) == 0;
			isEntrapping = isEntrapping && !WorldHelpers.IsWithinUnderworld( new Vector2(tileX<<4, tileY<<4) );

			return new FloodAmbush( tileX, tileY, isEntrapping );
		}


		////////////////

		public override int GetSpawnsDuration() {
			return this.GetBrambleDuration();
		}


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			Main.NewText( "The locals are alerted to your presence.", Color.DarkOrange );

			return base.OnActivate( clearTileX, clearTileY );
		}

		protected override void OnDeactivate() {
		}


		////////////////

		public override void EditNPCSpawnDataForMobs( Player player, ref int spawnRate, ref int maxSpawns ) {
			spawnRate = (int)( (float)spawnRate / AmbushesMod.Config.FloodAmbushSpawnWeight );
			maxSpawns = (int)( (float)maxSpawns * AmbushesMod.Config.FloodAmbushSpawnWeight );
		}
	}
}
